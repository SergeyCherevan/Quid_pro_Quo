import { Injectable } from '@angular/core';
import * as signalR from '@aspnet/signalr';

import { RequestService } from './request.service';
import { AuthorizationService } from './authorization.service';

@Injectable({ providedIn: 'root' })
export class IoTSignalRService {

  constructor(
    public requestService: RequestService,
    public authorizationService: AuthorizationService,
  ) { }

  hubConnection?: signalR.HubConnection;
  isAttachedCallback: () => void
    = () => { };

  startConnection() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('/IoT', {
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets,
        accessTokenFactory: () => this.authorizationService.jwtString!,
      })
      .build();

    this.hubConnection
      .start()
      .then(() => {
        console.log('Hub Connection Started!');

        this.isAttachedListener();
      })
      .catch(err => console.log('Error while starting connection: ' + err))
  }

  abortConnection() {
    this.hubConnection?.stop()
      .then(() => {
        console.log('Hub Connection Stoped!');
      });
  }

  isAttachedListener() {
    this.hubConnection?.on("IoTisAttached",
      () => {
        this.isAttachedCallback();
      }
    );
  }
}
