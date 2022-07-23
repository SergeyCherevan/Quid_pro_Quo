import { Injectable } from '@angular/core';
import * as signalR from '@aspnet/signalr';

@Injectable({ providedIn: 'root' })
export class SignalRService {

  constructor() { }

  hubConnection?: signalR.HubConnection;

  startConnection() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('/messaging', {
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets
      })
      .build();

    this.hubConnection
      .start()
      .then(() => {
        console.log('Hub Connection Started!');
      })
      .catch(err => console.log('Error while starting connection: ' + err))
  }

  sendToServer(i: number) {
    this.hubConnection?.invoke("SendToServer", `Hello №${i}!`)
      .catch(err => console.error(err));
  }

  sendToClientListener() {
    this.hubConnection?.on("SendToClient", (message) => {
      console.log(message);
    })
  }
}
