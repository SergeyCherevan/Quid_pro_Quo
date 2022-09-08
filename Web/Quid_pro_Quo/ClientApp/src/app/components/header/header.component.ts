import { Component, OnInit, Input } from '@angular/core';

import { AuthorizationService } from '../../services/authorization.service';
import { MessengerSignalRService } from '../../services/messenger-signalR.service';


@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {

  @Input() title: string = "";

  publishPageStr: string = "Опублікувати";
  usersPageStr: string = "Користувачі";
  proposalsPageStr: string = "Пропозиції";
  messagingsPageStr: string = "Листування";

  loginStr: string = "Вхід";

  get userNameStr(): string | undefined {
    return this.authorizationService.userName;
  }

  get countOfUnreadMessagings(): number {
    let count: number = 0

    this.messengerService.messagingCards.forEach(card => {
      if (card.countOfNotViewedMessages > 0) {
        count++;
      }
    });

    return count;
  }

  constructor(
    public authorizationService: AuthorizationService,
    public messengerService: MessengerSignalRService,
  ) { }

  ngOnInit(): void { }
}
