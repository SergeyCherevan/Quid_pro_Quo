import { Component, OnInit } from '@angular/core';

import { AuthorizationService } from './services/authorization.service';

@Component({
  selector: 'app',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

  public title: string = 'Quid pro Quo';

  constructor(public authorizationService: AuthorizationService) { }

  ngOnInit(): void {
    this.authorizationService.loginByLocalStorageData();
    //this.authorizationService.startRegularLogin();
  }
}
