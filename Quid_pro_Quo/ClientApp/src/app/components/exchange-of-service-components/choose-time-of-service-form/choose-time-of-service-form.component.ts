import { Component, Input, OnInit } from '@angular/core';
import { PostGetApiModel } from '../../../models/post-get-api.model';

@Component({
  selector: 'choose-time-of-service-form',
  templateUrl: './choose-time-of-service-form.component.html',
  styleUrls: ['./choose-time-of-service-form.component.css']
})
export class ChooseTimeOfServiceComponentForm implements OnInit {

  formTitle: string = "Вибір дати та часу для послуг";
  sendProposalButton: string = "Відправити пропозицію на обмін";
  cancelButton: string = "Відміна";

  @Input() outherPost: PostGetApiModel = {
    id: 0,
    title: "",
    text: "",
    imageFileNames: "",
    authorName: "",
    postedAt: new Date(),
    isActual: false,
    performServiceOnDatesList: [],
    performServiceInPlaceLat: 0,
    performServiceInPlaceLng: 0,
    performServiceInPlaceZoom: 15,
  };
  @Input() myPost?: PostGetApiModel = {
    id: 0,
    title: "",
    text: "",
    imageFileNames: "",
    authorName: "",
    postedAt: new Date(),
    isActual: false,
    performServiceOnDatesList: [],
    performServiceInPlaceLat: 0,
    performServiceInPlaceLng: 0,
    performServiceInPlaceZoom: 15,
  };

  constructor() { }

  ngOnInit(): void {

  }

  close(): void {
    let closeButton = document.getElementById("choose-time-of-service-form-close-button");
    closeButton?.click();
  }

  sendProposal() {

  }

}
