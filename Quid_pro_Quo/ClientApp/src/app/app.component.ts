import { Component, OnInit } from '@angular/core';

import { AuthorizationService } from './services/authorization.service';
import { MessagingsSignalRService } from './services/messagings-signalR.service'

@Component({
  selector: 'app',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

  public title: string = 'Quid pro Quo';

  constructor(
    public authorizationService: AuthorizationService,
    public messagingsService: MessagingsSignalRService,
  ) { }

  ngOnInit(): void {
    this.authorizationService.loginByLocalStorageData()
      .then(() => {
        if (this.authorizationService.userName) {
          this.messagingsService.startConnection();
        }
      });

    //this.authorizationService.startRegularLogin();
  }
}
