import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';

import { PostGetApiModel } from '../../../models/post-get-api.model';
import { PostsPageApiModel } from '../../../models/posts-page-api-model';

import { AuthorizationService } from '../../../services/authorization.service';
import { RequestService } from '../../../services/request.service';

@Component({
  selector: 'choose-service-to-change',
  templateUrl: './choose-service-to-change.component.html',
  styleUrls: ['./choose-service-to-change.component.scss']
})
export class ChooseServiceToChangeComponent implements OnInit {

  changeOutherService: string = "Обміняти послугу:";
  changeMyService: string = "На мою послугу:";

  subscription: Subscription;

  myPostsData: PostsPageApiModel = {
    posts: [],
    postsCount: 0,
  };

  outherServicePost: PostGetApiModel = {
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

  choosedNumberId?: number = undefined;
  myServicePost: PostGetApiModel = {
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

  constructor(
    public activateRoute: ActivatedRoute,
    public router: Router,
    public authorizationService: AuthorizationService,
    public requestService: RequestService,
    public ref: ChangeDetectorRef
  ) {
    this.subscription = activateRoute.params.subscribe(params => {
      this.outherServicePost.id = params['id'];

      requestService
        .get(`/api/post/get/${this.outherServicePost.id}`)
        .then(respObj => this.outherServicePost = respObj);

      authorizationService
        .loginByLocalStorageData()
        .then(() => this.downloadPosts());
    });
  }

  ngOnInit(): void { }

  downloadPosts(): void {
    this.requestService
      .get(`/api/post/getByAuthor/${this.authorizationService.userName}?pageNumber=0&pageSize=1000`)
      .then(respObj => this.myPostsData = respObj);
  }

  chooseMyServiceToChange(postId: number): void {
    this.choosedNumberId = postId;
    this.myServicePost = this.myPostsData.posts.find(post => post.id == this.choosedNumberId)!;

    this.ref.markForCheck();

    let showFormButton: HTMLButtonElement = <HTMLButtonElement>document.getElementById("button-show-choose-time-of-service-form");
    showFormButton.click();
  }

}
