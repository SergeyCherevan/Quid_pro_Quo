import { Component, OnInit, Input } from '@angular/core';
import { Router } from '@angular/router';
import { MessagingCardApiModel } from '../../../models/messaging-card-api.model';

@Component({
  selector: 'messaging-card',
  templateUrl: './messaging-card.component.html',
  styleUrls: ['./messaging-card.component.scss']
})
export class MessagingCardComponent implements OnInit {

  @Input() messagingCard: MessagingCardApiModel = {
    userName: "",
    countOfNotViewedMessages: 0,
  };
  @Input() destinationName: string = "";

  constructor(
    public router: Router,
  ) { }

  ngOnInit(): void { }

  goToMessagingWith(userName: string): void {
    this.router.navigateByUrl(`/messengerPage?companionName=${this.messagingCard.userName}`);
  }

}
