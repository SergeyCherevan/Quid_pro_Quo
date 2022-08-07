import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';

import { MessengerSignalRService } from '../../../services/messenger-signalR.service'

@Component({
  selector: 'messenger-page',
  templateUrl: './messenger-page.component.html',
  styleUrls: ['./messenger-page.component.scss'],
})
export class MessengerPageComponent implements OnInit, OnDestroy {

  title: string = "Листування з:";
  querySubscription: Subscription;

  constructor(
    public activateRoute: ActivatedRoute,
    public messengerService: MessengerSignalRService,
  ) {
    this.querySubscription = activateRoute.queryParams.subscribe(
      (queryParam: any) => {
        this.messengerService.messaging.user2Name = queryParam['companionName'] ?? '';

        this.ngOnInit();
      }
    );
  }

  ngOnInit(): void {
    this.messengerService.getMessagingCards();
    this.messengerService.getMessaging(this.messengerService.messaging.user2Name);

    setTimeout(() => {
      if (this.messengerService.messaging.user2Name && !this.messengerService.messagingCards.find(
        card => card.userName == this.messengerService.messaging.user2Name
      )) {
        this.messengerService.messagingCards.unshift({
          userName: this.messengerService.messaging.user2Name,
          countOfNotViewedMessages: 0
        })
      }
    }, 100);
  }

  ngOnDestroy(): void { }
}
