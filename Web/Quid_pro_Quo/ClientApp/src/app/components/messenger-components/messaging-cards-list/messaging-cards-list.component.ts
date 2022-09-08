import { Component, OnInit, Input } from '@angular/core';
import { MessagingCardApiModel } from '../../../models/messaging-card-api.model';

@Component({
  selector: 'messaging-cards-list',
  templateUrl: './messaging-cards-list.component.html',
  styleUrls: ['./messaging-cards-list.component.scss']
})
export class MessagingCardsListComponent implements OnInit {

  @Input() messagingCards: MessagingCardApiModel[] = [];
  @Input() destinationName: string = "";

  constructor() { }

  ngOnInit(): void { }

}
