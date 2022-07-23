import { Component, OnInit, OnDestroy } from '@angular/core';
import { SignalRService } from '../../services/signalR.service'

@Component({
  selector: 'messagings-page',
  templateUrl: './messagings-page.component.html',
  styleUrls: ['./messagings-page.component.scss'],
  providers: [ SignalRService ],
})
export class MessagingsPageComponent implements OnInit, OnDestroy {

  constructor(
    public signalRService: SignalRService
  ) {
    signalRService.startConnection()
  }

  intervalId: number = -1;

  ngOnInit(): void {
    let i: number = 0;
    this.intervalId = window.setInterval(() => {
      this.signalRService.sendToClientListener();
      this.signalRService.sendToServer(i);
      i++;
    }, 2000);
  }

  ngOnDestroy(): void {
    window.clearInterval(this.intervalId);
  }
}
