import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';

import { AuthorizationService } from '../../services/authorization.service';
import { RequestService } from '../../services/request.service';

import { PostResponseModel } from '../../models/post-response.model'

@Component({
  selector: 'post-page',
  templateUrl: './post-page.component.html',
})
export class PostPageComponent implements OnInit {

  title: string = "Сторінка заметки";
  idStr: string = "ID заметки: ";
  authorLabel: string = "Автор заметки: ";
  deleteStr: string = "Удалить заметку";


  postModel: PostResponseModel = {
    id: "",
    title: "",
    text: "",
    userName: "",
    lastUpdate: new Date(),
  };

  subscription: Subscription;

  get myUsername(): string | undefined {
    return this.authorizationService.userName;
  }

  get isMyPage(): boolean {
    return this.postModel.userName === this.myUsername;
  }

  constructor(
    public activateRoute: ActivatedRoute,
    public router: Router,
    public authorizationService: AuthorizationService,
    public requestService: RequestService,
  ) {
    this.subscription = activateRoute.params.subscribe(params => this.postModel.id = params['id'])!;

    this.authorizationService.loginByLocalStorageData();
  }

  ngOnInit(): void {
    this.downloadPost()
  }

  downloadPost(): void {

    this.requestService
      .get(`/api/post/get/${this.postModel.id}`)
      .then(respObj => this.postModel = respObj);
  }

  deletePost(): void {

    this.requestService
      .delete(
        `/api/post/delete/${this.postModel.id}`,
        undefined,
        this.authorizationService.jwtString)
      .catch(() => { })
      .then(() => this.router.navigateByUrl(`/accountPage/${this.postModel.userName}`));
  }
}
