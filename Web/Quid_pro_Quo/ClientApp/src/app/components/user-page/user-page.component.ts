import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';

import { AuthorizationService } from '../../services/authorization.service';
import { RequestService } from '../../services/request.service';

import { UserApiModel } from '../../models/user-api.model'
import { PostGetApiModel } from '../../models/post-get-api.model'
import { PostsPageApiModel } from '../../models/posts-page-api-model';

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
  sendMessageStr: string = "Відправити повідомлення";
  postsTitle: string = "Пости користувача";


  subscription: Subscription;

  userModel: UserApiModel = {
    id: 0,
    userName: "",
    avatarFileName: "",
    biographi: "",
    role: "",
  };

  getInnerHtmlByString(str: string) {
    return str.split('  ').join(' &nbsp;').split('\n').join('<br>');
  }

  get myUsername(): string | undefined {
    return this.authorizationService.userName;
  }

  get isMyPage(): boolean {
    return this.userModel.userName === this.myUsername;
  }



  postsData: PostsPageApiModel = {
    posts: [],
    postsCount: 0,
  };
  post: PostGetApiModel = {
    id: 0,
    title: "",
    text: "",
    imageFileNames: "",
    authorName: "",
    postedAt: new Date(),
    isActual: false,
    performServiceOnDatesList: [],
    performServiceInPlaceLat: 0,
    performServiceInPlaceLng: 0,
    performServiceInPlaceZoom: 15,
  };
  i: number = 0;

  pageNumber: number = 0;
  pageSize: number = 2;

  get pageCount(): number {
    return Math.floor(this.postsData.postsCount / this.pageSize) +
      (this.postsData.postsCount % this.pageSize > 0 ? 1 : 0);
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
      .get(`/api/post/getByAuthor/${this.userModel.userName}?pageNumber=${this.pageNumber}&pageSize=${this.pageSize}`)
      .then(respObj => this.postsData = respObj);
  }

  goPostPage(id: string): void {
    this.router.navigateByUrl(`/postPage/${id}`);
  }
}
