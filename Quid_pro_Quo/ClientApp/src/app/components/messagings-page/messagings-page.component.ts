import { Component, OnInit, OnDestroy } from '@angular/core';
import { MessagingsSignalRService } from '../../services/messagings-signalR.service'

@Component({
  selector: 'messagings-page',
  templateUrl: './messagings-page.component.html',
  styleUrls: ['./messagings-page.component.scss'],
  providers: [ MessagingsSignalRService ],
})
export class MessagingsPageComponent implements OnInit, OnDestroy {

  constructor(
    public messagingsService: MessagingsSignalRService
  ) {
    messagingsService.startConnection()
  }

  intervalId: number = -1;

  ngOnInit(): void {
    let i: number = 0;
    this.intervalId = window.setInterval(() => {
      this.messagingsService.sendToClientListener();
      this.messagingsService.sendToServer(i);
      i++;
    }, 2000);
  }

  ngOnDestroy(): void {
    window.clearInterval(this.intervalId);
  }
}
