import { Component, OnInit } from '@angular/core';

import { UserApiModel } from '../../models/user-api.model';

import { DictionaryService } from '../../services/dictionary.service';
import { RequestService } from '../../services/request.service';
import { AuthorizationService } from '../../services/authorization.service';

@Component({
  selector: 'users-page',
  templateUrl: './users-page.component.html',
  providers: [ DictionaryService ],
})
export class UsersPageComponent implements OnInit {

  title: string = "Користувачі:";

  usersData: UserApiModel[] = [];

  constructor(
    public dictionaryService: DictionaryService,
    public requestService: RequestService,
    public authorizationService: AuthorizationService) { }

  ngOnInit(): void {
    this.requestService
      .get('/api/user/getAll', this.authorizationService.jwtString)
      .then((respObj: UserApiModel[]) => this.usersData = respObj);
  }

}
