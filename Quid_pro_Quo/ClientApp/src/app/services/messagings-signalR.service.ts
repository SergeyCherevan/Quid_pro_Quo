import { Injectable } from '@angular/core';
import * as signalR from '@aspnet/signalr';

import { AuthorizationService } from './authorization.service';

@Injectable()
export class MessagingsSignalRService {

  constructor(
    public authorizationService: AuthorizationService,
  ) { }

  hubConnection?: signalR.HubConnection;

  startConnection() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('/messagings', {
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
      .catch(err => console.log('Error while starting connection: ' + err))
  }

  abortConnection() {
    this.hubConnection?.stop()
      .then(() => {
        console.log('Hub Connection Stoped!');
      });
  }

  sendToServer(i: number) {
    this.hubConnection?.invoke("SendToServer", `Hello №${i}!`)
      .catch(err => console.error(err));
  }

  sendToClientListener() {
    this.hubConnection?.on("SendToClient", (message) => {
      console.log(message);
    })
  }
}
