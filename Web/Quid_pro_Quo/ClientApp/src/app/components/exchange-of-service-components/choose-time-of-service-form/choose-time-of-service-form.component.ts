import { Component, Input, OnInit } from '@angular/core';

import { PostGetApiModel } from '../../../models/post-get-api.model';
import { SendProposalToExchangeApiModel } from '../../../models/send-proposal-to-exchange-api.model';

import { RequestService } from '../../../services/request.service';
import { AuthorizationService } from '../../../services/authorization.service';

@Component({
  selector: 'choose-time-of-service-form',
  templateUrl: './choose-time-of-service-form.component.html',
  styleUrls: [ './choose-time-of-service-form.component.scss', ]
})
export class ChooseTimeOfServiceFormComponent implements OnInit {

  formTitle: string = "Вибір дати та часу для послуг";
  sendProposalButton: string = "Відправити пропозицію на обмін";
  cancelButton: string = "Відміна";

  userServiceStr: string = "Послуга користувача";
  myServiceStr: string = "Моя послуга";
  idServiceTitleStr: string = "(Id) Назва послуги";
  chooseDateTimeStr = "Оберіть час виконання послуги";

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

  proposalModel: SendProposalToExchangeApiModel = {
    requestedPostId: 0,
    requestingPostId: 0,
    dateNumberOfRequestedPost: 0,
    dateNumberOfRequestingPost: 0,
    text: "",
  };

  constructor(
    public requestService: RequestService,
    public authorizationService: AuthorizationService,
  ) {
    
  }

  ngOnInit(): void {
    this.proposalModel = {
      requestedPostId: this.outherPost.id,
      requestingPostId: this.myPost!.id,
      dateNumberOfRequestedPost: 0,
      dateNumberOfRequestingPost: 0,
      text: "",
    }
  }

  close(): void {
    let closeButton = document.getElementById("choose-time-of-service-form-close-button");
    closeButton?.click();
  }

  sendProposal() {
    this.proposalModel.requestingPostId = this.myPost!.id;

    return this.requestService
      .post('/api/ExchangeOfServices/sendProposal',
        <SendProposalToExchangeApiModel>this.proposalModel, this.authorizationService.jwtString)
      .then(responseObject => this.close());
  }

}
