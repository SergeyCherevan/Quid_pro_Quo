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
        this.messengerService.getMessagingCards();
        this.messengerService.getMessaging(this.messengerService.messaging.user2Name);

        //this.messengerService.messagingCards = [{
        //  userName: "Ageris",
        //  countOfNotViewedMessages: 1,
        //}];

        this.ngOnInit();
      }
    );
  }

  ngOnInit(): void { }

  ngOnDestroy(): void { }
}
