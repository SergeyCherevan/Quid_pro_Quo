import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { first, Subscription } from 'rxjs';

import { AuthorizationService } from '../../../services/authorization.service';
import { RequestService } from '../../../services/request.service';

import { StatusEnum } from '../../../models/exchange-of-services-api.model';
import { ExchangeOfServicesClientModel } from '../../../models/exchange-of-services-client.model';
import { PostGetApiModel } from '../../../models/post-get-api.model';

enum DirectionEnum {
  In = "In",
  Out = "Out",
  Confirmed = "Confirmed",
};

@Component({
  selector: 'changing-proposals-page',
  templateUrl: './changing-proposals-page.component.html',
  styleUrls: ['./changing-proposals-page.component.scss']
})
export class ChangingProposalsPageComponent implements OnInit {

  titleStrs: string[] = [
    "Пропозиції на обмін послугами (до мене)",
    "Пропозиції на обмін послугами (від мене)",
    "Підтверджені обміни послугами",
  ];
  firstSubtitleStrs: string[] = [
    "Вхідні",
    "Вихідні",
    "В процесі",
  ];
  secondSubtitleStrs: string[] = [
    "Відхилені",
    "Відхилені",
    "Виконані",
  ];
  thirdSubtitleStrs: string[] = [
    "",
    "",
    "Відхилені",
  ];

  get firstHttpRoute(): string {
    switch (this.direction) {
      case DirectionEnum.In:
        return `/api/exchangeOfServices/getByDestination/${this.authorizationService.userName}/${StatusEnum.noInfo}`;
      case DirectionEnum.Out:
        return `/api/exchangeOfServices/getBySender/${this.authorizationService.userName}/${StatusEnum.noInfo}`;
      case DirectionEnum.Confirmed:
        return `/api/exchangeOfServices/getConfirmed/${this.authorizationService.userName}/${StatusEnum.noInfo}`;
    }
  }
  get secondHttpRoute(): string {
    switch (this.direction) {
      case DirectionEnum.In:
        return `/api/exchangeOfServices/getByDestination/${this.authorizationService.userName}/${StatusEnum.no}`;
      case DirectionEnum.Out:
        return `/api/exchangeOfServices/getBySender/${this.authorizationService.userName}/${StatusEnum.no}`;
      case DirectionEnum.Confirmed:
        return `/api/exchangeOfServices/getConfirmed/${this.authorizationService.userName}/${StatusEnum.yes}`;
    }
  }
  get thirdHttpRoute(): string {
    switch (this.direction) {
      case DirectionEnum.In:
        return '';
      case DirectionEnum.Out:
        return '';
      case DirectionEnum.Confirmed:
        return `/api/exchangeOfServices/getConfirmed/${this.authorizationService.userName}/${StatusEnum.no}`;
    }
  }

  direction: DirectionEnum = DirectionEnum.In;
  subscription: Subscription;

  directionArray: string[] = [
    "In",
    "Out",
    "Confirmed",
  ];

  get directionNumber(): number {
    return this.directionArray.indexOf(<string>this.direction);
  }

  firstExchanges: ExchangeOfServicesClientModel[] = [];
  secondExchanges: ExchangeOfServicesClientModel[] = [];
  thirdExchanges: ExchangeOfServicesClientModel[] = [];

  constructor(
    public activateRoute: ActivatedRoute,
    public router: Router,
    public authorizationService: AuthorizationService,
    public requestService: RequestService,
  ) {
    this.subscription = activateRoute.params.subscribe(params => {
      this.direction = (<any>DirectionEnum)[params['direction']];

      let promise: Promise<any> =
        authorizationService
          .loginByLocalStorageData()
          .then(() => this.requestService
            .get(this.firstHttpRoute))
          .then(response => this.firstExchanges = response)
          .then(() => this.firstExchanges.forEach(e => {
            this.requestService.get(`/api/post/get/${e.requestedPostId}`)
              .then(response => e.requestedPost = response);

            this.requestService.get(`/api/post/get/${e.requestingPostId}`)
              .then(response => e.requestingPost = response);
          }))
          .then(() => this.requestService
            .get(this.secondHttpRoute))
          .then(response => this.secondExchanges = response)
          .then(() => this.secondExchanges.forEach(e => {
            this.requestService.get(`/api/post/get/${e.requestedPostId}`)
              .then(response => e.requestedPost = response);

            this.requestService.get(`/api/post/get/${e.requestingPostId}`)
              .then(response => e.requestingPost = response);
          }));

      if (this.direction == DirectionEnum.Confirmed) {
        promise
          .then(() => this.requestService
            .get(this.thirdHttpRoute))
          .then(response => this.thirdExchanges = response)
          .then(() => this.thirdExchanges.forEach(e => {
            this.requestService.get(`/api/post/get/${e.requestedPostId}`)
              .then(response => e.requestedPost = response);

            this.requestService.get(`/api/post/get/${e.requestingPostId}`)
              .then(response => e.requestingPost = response);
          }));
      }
    });
  }

  ngOnInit(): void {

  }

  clickOkButton(exchange: ExchangeOfServicesClientModel) {
    this.requestService
      .post('/api/exchangeOfServices/confirmProposal', {
        exchangeId: exchange.id,
      }, this.authorizationService.jwtString)
      .catch(err => console.log(err));
  }

  clickCancelButton(exchange: ExchangeOfServicesClientModel) {
    if (this.direction != DirectionEnum.Confirmed) {
      this.requestService
        .post('/api/exchangeOfServices/cancelProposal', {
          exchangeId: exchange.id,
        }, this.authorizationService.jwtString)
        .catch(err => console.log(err));
    } else {
      let myPost: PostGetApiModel;

      if (exchange.requestingPost.authorName == this.authorizationService.userName) {
        myPost = exchange.requestingPost;
      } else {
        myPost = exchange.requestedPost;
      }

      this.requestService
        .post('/api/exchangeOfServices/cancelExchange', {
          exchangeId: exchange.id,
          postId: myPost.id,
        }, this.authorizationService.jwtString)
        .catch(err => console.log(err));
    }
  }

}
