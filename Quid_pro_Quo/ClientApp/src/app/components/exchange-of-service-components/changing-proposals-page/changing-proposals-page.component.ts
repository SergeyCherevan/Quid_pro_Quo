import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';

import { AuthorizationService } from '../../../services/authorization.service';
import { RequestService } from '../../../services/request.service';

import { StatusEnum } from '../../../models/exchange-of-services-api.model';
import { ExchangeOfServicesClientModel } from '../../../models/exchange-of-services-client.model';

enum DirectionEnum {
  In = "In",
  Out = "Out",
  InProcess = "InProcess",
};

@Component({
  selector: 'changing-proposals-page',
  templateUrl: './changing-proposals-page.component.html',
  styleUrls: ['./changing-proposals-page.component.scss']
})
export class ChangingProposalsPageComponent implements OnInit {

  titleStr: string = "Пропозиції на обмін послугами (до мене)";
  firstSubtitleStr: string = "Вхідні";
  secondSubtitleStr: string = "Відхилені";

  direction: DirectionEnum = DirectionEnum.In;
  subscription: Subscription;

  inputExchanges: ExchangeOfServicesClientModel[] = [];
  cancelExchanges: ExchangeOfServicesClientModel[] = [];

  constructor(
    public activateRoute: ActivatedRoute,
    public router: Router,
    public authorizationService: AuthorizationService,
    public requestService: RequestService,
  ) {
    this.subscription = activateRoute.params.subscribe(params => {
      this.direction = (<any>DirectionEnum)[params['direction']];

      authorizationService
        .loginByLocalStorageData()
        .then(() => this.requestService
          .get(`/api/exchangeOfServices/getByDestination/${this.authorizationService.userName}/${StatusEnum.noInfo}`))
        .then(response => this.inputExchanges = response)
        .then(() => this.inputExchanges.forEach(e => {
          this.requestService.get(`/api/post/get/${e.requestedPostId}`)
            .then(response => e.requestedPost = response);

          this.requestService.get(`/api/post/get/${e.requestingPostId}`)
            .then(response => e.requestingPost = response);
        }))
        .then(() => this.requestService
          .get(`/api/exchangeOfServices/getByDestination/${this.authorizationService.userName}/${StatusEnum.no}`))
        .then(response => this.cancelExchanges = response)
        .then(() => this.cancelExchanges.forEach(e => {
          this.requestService.get(`/api/post/get/${e.requestedPostId}`)
            .then(response => e.requestedPost = response);

          this.requestService.get(`/api/post/get/${e.requestingPostId}`)
            .then(response => e.requestingPost = response);
        }));
    });
  }

  ngOnInit(): void {

  }

  clickOkButton() {

  }

  clickCancelButton() {

  }

}
