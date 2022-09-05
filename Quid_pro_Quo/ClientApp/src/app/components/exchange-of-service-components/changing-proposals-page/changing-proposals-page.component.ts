import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { first, Subscription } from 'rxjs';

import { AuthorizationService } from '../../../services/authorization.service';
import { RequestService } from '../../../services/request.service';

import { StatusEnum } from '../../../models/exchange-of-services-api.model';
import { ExchangeOfServicesClientModel } from '../../../models/exchange-of-services-client.model';

enum DirectionEnum {
  In = "In",
  Out = "Out",
};
let directionArray: string[] = [
  "In",
  "Out",
];

@Component({
  selector: 'changing-proposals-page',
  templateUrl: './changing-proposals-page.component.html',
  styleUrls: ['./changing-proposals-page.component.scss']
})
export class ChangingProposalsPageComponent implements OnInit {

  titleStrs: string[] = [
    "Пропозиції на обмін послугами (до мене)",
    "Пропозиції на обмін послугами (від мене)",
  ];
  firstSubtitleStrs: string[] = [
    "Вхідні",
    "Вихідні",
  ];
  secondSubtitleStrs: string[] = [
    "Відхилені",
    "Відхилені",
  ];

  get firstHttpRoute(): string {
    if (this.direction == DirectionEnum.In) {
      return `/api/exchangeOfServices/getByDestination/${this.authorizationService.userName}/${StatusEnum.noInfo}`;
    } else {
      return `/api/exchangeOfServices/getBySender/${this.authorizationService.userName}/${StatusEnum.noInfo}`;
    }
  }
  get secondHttpRoute(): string {
    if (this.direction == DirectionEnum.In) {
      return `/api/exchangeOfServices/getByDestination/${this.authorizationService.userName}/${StatusEnum.no}`;
    } else {
      return `/api/exchangeOfServices/getBySender/${this.authorizationService.userName}/${StatusEnum.no}`;
    }
  }

  direction: DirectionEnum = DirectionEnum.In;
  subscription: Subscription;

  get directionNumber(): number {
    return directionArray.indexOf(<string>this.direction);
  }

  firstExchanges: ExchangeOfServicesClientModel[] = [];
  secondExchanges: ExchangeOfServicesClientModel[] = [];

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
    });
  }

  ngOnInit(): void {

  }

  clickOkButton(id: number) {
    this.requestService
      .post('/api/exchangeOfServices/confirmProposal', {
        exchangeId: id,
      }, this.authorizationService.jwtString)
      .catch(err => console.log(err));
  }

  clickCancelButton(id: number) {
    this.requestService
      .post('/api/exchangeOfServices/cancelProposal', {
        exchangeId: id,
      }, this.authorizationService.jwtString)
      .catch(err => console.log(err));
  }

}
