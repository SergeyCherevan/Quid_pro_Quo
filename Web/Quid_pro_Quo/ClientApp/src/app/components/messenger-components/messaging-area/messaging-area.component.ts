import { Component, OnInit, Input } from '@angular/core';
import { MessagingApiModel } from '../../../models/messaging-api.model';

@Component({
  selector: 'messaging-area',
  templateUrl: './messaging-area.component.html',
  styleUrls: ['./messaging-area.component.scss']
})
export class MessagingAreaComponent implements OnInit {

  @Input() messaging: MessagingApiModel = {
    user1Name: "",
    user2Name: "",
    messagesList: [],
  };
  @Input() destinationName: string = "";

  constructor() { }

  ngOnInit(): void { }

}
