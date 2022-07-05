import { Component, OnInit, Input } from '@angular/core';

import { AuthorizationService } from '../../services/authorization.service';

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

  constructor(public authorizationService: AuthorizationService) { }

  ngOnInit(): void { }
}
