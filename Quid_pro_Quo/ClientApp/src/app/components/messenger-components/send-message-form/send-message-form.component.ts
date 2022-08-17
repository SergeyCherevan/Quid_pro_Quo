import { Component, OnInit, Input } from '@angular/core';

import { SendMessageApiModel } from '../../../models/send-message-api.model';

import { MessengerSignalRService } from '../../../services/messenger-signalR.service'

@Component({
  selector: 'send-message-form',
  templateUrl: './send-message-form.component.html',
  styleUrls: ['./send-message-form.component.scss']
})
export class SendMessageFormComponent implements OnInit {

  message: SendMessageApiModel = {
    text: "",
    imageFile: new Blob(),
    file: new Blob(),
    destinationName: "",
  };

  constructor(
    public messengerService: MessengerSignalRService,
  ) { }

  ngOnInit(): void { }

  get sendMessageFormData(): FormData {
    let formData: FormData = new FormData();

    formData.append('text', this.message.text);
    formData.append('imageFile', "");
    formData.append('file', "");
    formData.append('destinationName', this.messengerService.messaging.user2Name);

    return formData;
  }

  sendMessage(): void {
    this.messengerService.sendMessage(this.sendMessageFormData);
  }

}
