import { Injectable } from '@angular/core';
import * as signalR from '@aspnet/signalr';
import { MessageApiModel } from '../models/message-api.model';
import { MessagingApiModel } from '../models/messaging-api.model';
import { MessagingCardApiModel } from '../models/messaging-card-api.model';

import { RequestService } from './request.service';
import { AuthorizationService } from './authorization.service';

@Injectable({ providedIn: 'root' })
export class MessengerSignalRService {

  constructor(
    public requestService: RequestService,
    public authorizationService: AuthorizationService,
  ) { }

  hubConnection?: signalR.HubConnection;
  messagingCards: MessagingCardApiModel[] = [];
  messaging: MessagingApiModel = {
    user1Name: "",
    user2Name: "",
    messagesList: [],
  };

  startConnection() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('/messenger', {
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets,
        accessTokenFactory: () => this.authorizationService.jwtString!,
      })
      .build();

    this.hubConnection
      .start()
      .then(() => {
        console.log('Hub Connection Started!');
      })
      .then(() => {
        this.messagingCards = [];
        this.messaging = {
          user1Name: this.authorizationService.userName!,
          user2Name: "",
          messagesList: [],
        };
      })
      .then(() => {
        this.getMessagingCardsListener();
        this.getMessagingListener();
        this.messagingsIsUpdatedListener();
        this.getNewMessagesListener();
      })
      .then(() => {
        this.getMessagingCards();
      })
      .catch(err => console.log('Error while starting connection: ' + err))
  }

  abortConnection() {
    this.hubConnection?.stop()
      .then(() => {
        console.log('Hub Connection Stoped!');
      });
  }

  getMessagingCards() {
    this.hubConnection?.invoke("GetMessagingCardsRequest")
      .catch(err => console.error(err));
  }

  getMessagingCardsListener() {
    this.hubConnection?.on("GetMessagingCardsResponse",
      (cards: MessagingCardApiModel[]) => {
        this.messagingCards = cards;

        if (this.messaging.user2Name && !this.messagingCards.find(
          card => card.userName == this.messaging.user2Name
        )) {
          this.messagingCards.unshift({
            userName: this.messaging.user2Name,
            countOfNotViewedMessages: 0
          })
        }
      }
    );
  }

  getMessaging(userName: string) {
    this.hubConnection?.invoke("GetMessagingRequest", userName)
      .catch(err => console.error(err));
  }

  getMessagingListener() {
    this.hubConnection?.on("GetMessagingResponse",
      (messaging: MessagingApiModel) => {
        this.messaging = messaging;

        let viewedMessageIDs: number[] = [];
        for (let mes of this.messaging.messagesList) {
          if (mes.authorNumber == true && mes.notViewed) {
            viewedMessageIDs.push(mes.id);
          }
        }

        if (viewedMessageIDs.length > 0 || this.messaging.user1Name == this.messaging.user2Name) {
          this.messagesIsViewed(this.messaging.user2Name, viewedMessageIDs);
        }
      }
    );
  }

  sendMessage(message: FormData) {
    this.requestService
      .postMultipartForm('/api/messenger/sendMessage', message, this.authorizationService.jwtString)
      .catch(err => console.error(err));
  }

  messagingsIsUpdatedListener() {
    this.hubConnection?.on("MessagingsIsUpdated",
      (cards: MessagingCardApiModel[]) => {
        this.messagingCards = cards;

        if (this.messagingCards.find(
          card => card.countOfNotViewedMessages > 0 && card.userName == this.messaging.user2Name
        )) {
          this.getNewMessages(this.messaging.user2Name);
        }
      }
    );
  }

  getNewMessages(userName: string) {
    this.hubConnection?.invoke("GetNewMessagesRequest", userName)
      .catch(err => console.error(err));
  }

  getNewMessagesListener() {
    this.hubConnection?.on("GetNewMessagesResponse",
      (messages: MessageApiModel[]) => {
        this.messaging.messagesList.push(...messages);

        let viewedMessageIDs: number[] = [];
        for (let mes of messages) {
          if (mes.authorNumber == true) {
            viewedMessageIDs.push(mes.id);
          }
        }

        if (viewedMessageIDs.length > 0 || this.messaging.user1Name == this.messaging.user2Name) {
          this.messagesIsViewed(this.messaging.user2Name, viewedMessageIDs);
        }
      }
    );
  }

  messagesIsViewed(userName: string, messagesIDs: number[]) {
    this.hubConnection?.invoke("MessagesIsViewed", userName, messagesIDs)
      .catch(err => console.error(err));
  }
}
