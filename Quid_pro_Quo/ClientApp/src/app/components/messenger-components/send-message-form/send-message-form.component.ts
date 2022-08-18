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
    imageFile: <any>undefined,
    file: <any>undefined,
    destinationName: "",
  };

  attachedFile: string = "Прикріплений файл";
  attachedImage: string = "Прикріплене зображення"

  constructor(
    public messengerService: MessengerSignalRService,
  ) { }

  ngOnInit(): void { }

  attachImageFile(): void {
    let imageInput: HTMLInputElement = <HTMLInputElement>document.getElementById("image-input");

    if (!this.message.imageFile) {
      imageInput.click();
    } else {
      this.message.imageFile = <any>undefined;
      imageInput.value = '';
    }
  }

  onSelectImageFile(event: any): void { // called each time file input changes
    if (event.target.files && event.target.files[0]) {
      let reader = new FileReader();

      reader.readAsDataURL(event.target.files[0]); // read file as data url

      reader.onload = (event) => { // called once readAsDataURL is completed
        this.message.imageFile = <any>event?.target?.result;
      }
    }
  }

  showImage(): void {
    let showModalButton: HTMLInputElement = <HTMLInputElement>document.getElementById("show-modal");
    showModalButton.click()
  }

  attachFile(): void {
    let fileInput: HTMLInputElement = <HTMLInputElement>document.getElementById("file-input");

    if (!this.message.file) {
      fileInput.click();
    } else {
      this.message.file = <any>undefined;
      fileInput.value = '';
    }
  }

  onSelectFile(event: any): void { // called each time file input changes
    if (event.target.files && event.target.files[0]) {
      let reader = new FileReader();

      reader.readAsDataURL(event.target.files[0]); // read file as data url

      reader.onload = (event) => { // called once readAsDataURL is completed
        this.message.file = <any>event?.target?.result;
      }
    }
  }

  nameForDownloadFile(): string {
    let text = <string><any>this.message.file;

    let d = new Date().getTime(); // Timestamp
    let d2 = ((typeof performance !== 'undefined') && performance.now && (performance.now() * 1000)) || 0;//Time in microseconds since page-load or 0 if unsupported
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
      let r = Math.random() * 16; //random number between 0 and 16
      if (d > 0) { // Use timestamp until depleted
        r = (d + r) % 16 | 0;
        d = Math.floor(d / 16);
      } else { // Use microseconds since page-load if supported
        r = (d2 + r) % 16 | 0;
        d2 = Math.floor(d2 / 16);
      }
      return (c === 'x' ? r : (r & 0x3 | 0x8)).toString(16);
    }) + '.' + text.substring(text.indexOf('/') + 1, text.indexOf(';'));
  }

  get sendMessageFormData(): FormData {
    let formData: FormData = new FormData();

    formData.append('text', this.message.text);

    let imageInput: HTMLInputElement = <HTMLInputElement>document.getElementById("image-input");
    if (imageInput?.files) {
      formData.append('imageFile', imageInput.files[0]);
    }

    let fileInput: HTMLInputElement = <HTMLInputElement>document.getElementById("file-input");
    if (fileInput?.files) {
      formData.append('file', fileInput.files[0]);
    }

    formData.append('destinationName', this.messengerService.messaging.user2Name);

    return formData;
  }

  sendMessage(): void {
    this.messengerService.sendMessage(this.sendMessageFormData)
      .then(() => this.resetData());

    setTimeout(() => {
      let messagingArea: HTMLInputElement = <HTMLInputElement>document.getElementById("messaging-area");
      messagingArea.scrollTop = messagingArea.scrollHeight;
    }, 100);
  }

  resetData(): void {
    this.message = {
      text: "",
      imageFile: <any>undefined,
      file: <any>undefined,
      destinationName: "",
    };

    let imageInput: HTMLInputElement = <HTMLInputElement>document.getElementById("image-input");
    imageInput.value = '';

    let fileInput: HTMLInputElement = <HTMLInputElement>document.getElementById("file-input");
    fileInput.value = '';
  }
}
