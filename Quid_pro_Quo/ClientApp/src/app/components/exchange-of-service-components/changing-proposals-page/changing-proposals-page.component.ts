import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { AuthorizationService } from '../../../services/authorization.service';
import { RequestService } from '../../../services/request.service';

enum DirectionEnum {
  In = "In",
  Out = "Out",
  InProcess = "InProcess",
};

@Component({
  selector: 'changing-proposals-page',
  templateUrl: './changing-proposals-page.component.html',
  styleUrls: ['./changing-proposals-page.component.css']
})
export class ChangingProposalsPageComponent implements OnInit {

  titleStr: string = "";

  direction: DirectionEnum = DirectionEnum.In;

  subscription: Subscription;

  constructor(
    public activateRoute: ActivatedRoute,
    public router: Router,
    public authorizationService: AuthorizationService,
    public requestService: RequestService,
  ) {
    this.subscription = activateRoute.params.subscribe(params => {
      this.direction = (<any>DirectionEnum)[params['direction']];

      authorizationService
        .loginByLocalStorageData();
    });
  }

  ngOnInit(): void {

  }

}
