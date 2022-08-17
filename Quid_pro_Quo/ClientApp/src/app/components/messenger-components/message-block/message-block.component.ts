import { Component, OnInit, Input } from '@angular/core';
import { MessageApiModel } from '../../../models/message-api.model';

@Component({
  selector: 'message-block',
  templateUrl: './message-block.component.html',
  styleUrls: ['./message-block.component.scss']
})
export class MessageBlockComponent implements OnInit {

  @Input() message: MessageApiModel = {
    id: 0,
    authorName: "",
    text: "",
    imageFileName: "",
    fileName: "",
    postedAt: new Date(),
    notViewed: false,
  };
  @Input() companionName: string = "";

  constructor() { }

  ngOnInit(): void {
  }

}
