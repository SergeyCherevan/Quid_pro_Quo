import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';

import { AuthorizationService } from '../../services/authorization.service';
import { RequestService } from '../../services/request.service';

import { UserApiModel } from '../../models/user-api.model'
import { PostGetApiModel } from '../../models/post-get-api.model'

@Component({
  selector: 'user-page',
  templateUrl: './user-page.component.html',
  styleUrls: [ '../account-page/account-page.component.scss' ],
})
export class UserPageComponent implements OnInit {

  title: string = "Сторінка користувача";
  idStr: string = "ID користувача:";
  usernameStr: string = "Ім'я користувача:";
  biographiStr: string = "Біографія:";
  editAccountStr: string = "Редагувати аккаунт";


  subscription: Subscription;

  userModel: UserApiModel = {
    id: 0,
    userName: "",
    avatarFileName: "",
    biographi: "",
    role: "",
  };
  postItems: PostGetApiModel[] = [];

  getInnerHtmlByString(str: string) {
    return str.split('  ').join(' &nbsp;').split('\n').join('<br>');
  }

  get myUsername(): string | undefined {
    return this.authorizationService.userName;
  }

  get isMyPage(): boolean {
    return this.userModel.userName === this.myUsername;
  }

  constructor(
    public activateRoute: ActivatedRoute,
    public router: Router,
    public authorizationService: AuthorizationService,
    public requestService: RequestService,
  ) {
    this.subscription = activateRoute.params.subscribe(params => {
      this.userModel.userName = params['userName'];

      this.requestService
        .get(`/api/user/get/${this.userModel.userName}`)
        .then(respObj => this.userModel = respObj)
        .then(() => this.downloadPosts());

      this.authorizationService.loginByLocalStorageData()
    })!;
  }

  ngOnInit(): void { }

  downloadPosts(): void {
    this.requestService
      .get(`/api/post/getByUserId/${this.userModel.id}`)
      .then(respObj => this.postItems = respObj);
  }

  goPostPage(id: string): void {
    this.router.navigateByUrl(`/postPage/${id}`);
  }
}
