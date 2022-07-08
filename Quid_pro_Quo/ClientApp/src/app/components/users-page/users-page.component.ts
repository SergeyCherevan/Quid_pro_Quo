import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';

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

  title: string = "Користувачі";
  searchStr: string = "Шукати";

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

  querySubscription: Subscription;
  keywords: string = '';
  pageNumber: number = 0;
  pageSize: number = 10;

  get pageCount(): number {
    return Math.floor(this.usersData.usersCount / this.pageSize) +
      (this.usersData.usersCount % this.pageSize > 0 ? 1 : 0);
  }

  pageNumbersArr(begin: number, end: number) {
    let arr = [];
    for (let i = begin; i <= end; i++) {
      arr.push(i);
    }
    return arr;
  }

  constructor(
    public activateRoute: ActivatedRoute,
    public router: Router,
    public dictionaryService: DictionaryService,
    public requestService: RequestService,
    public authorizationService: AuthorizationService
  ) {
    this.querySubscription = activateRoute.queryParams.subscribe(
      (queryParam: any) => {
        this.keywords = queryParam['keywords'] ?? '';
        this.pageNumber = Number(queryParam['pageNumber'] ?? 0);

        this.ngOnInit();
      }
    );
  }

  ngOnInit(): void {
    this.requestService
      .get(`/api/user/getByFilter?keywords=${this.keywords}&pageNumber=${this.pageNumber}&pageSize=${this.pageSize}`,
        this.authorizationService.jwtString)
      .then((respObj: UsersPageApiModel) => this.usersData = respObj);
  }

  search(): void {
    this.router.navigateByUrl(`/usersPage?keywords=${this.keywords}&pageNumber=${this.pageNumber}`);
  }
}
