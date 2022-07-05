import { Component, OnInit } from '@angular/core';

import { UserApiModel } from '../../models/user-api.model';
import { UsersPageApiModel } from '../../models/users-page-api.model';

import { DictionaryService } from '../../services/dictionary.service';
import { RequestService } from '../../services/request.service';
import { AuthorizationService } from '../../services/authorization.service';

@Component({
  selector: 'users-page',
  templateUrl: './users-page.component.html',
  styleUrls: [ './user-page.component.scss' ],
  providers: [ DictionaryService ],
})
export class UsersPageComponent implements OnInit {

  title: string = "Користувачі:";

  usersData: UsersPageApiModel = {
    users: [],
    usersCount: 0,
  };

  user: UserApiModel = {
    id: 0,
    userName: "",
    avatarFileName: undefined,
    biographi: "",
    role: "",
  };

  getInnerHtmlByString(str: string) {
    return str.split('  ').join(' &nbsp;').split('\n').join('<br>');
  }

  constructor(
    public dictionaryService: DictionaryService,
    public requestService: RequestService,
    public authorizationService: AuthorizationService) { }

  ngOnInit(): void {
    this.requestService
      .get('/api/user/getByFilter?pageNumber=0&pageSize=10', this.authorizationService.jwtString)
      .then((respObj: UsersPageApiModel) => this.usersData = respObj);
  }

}
