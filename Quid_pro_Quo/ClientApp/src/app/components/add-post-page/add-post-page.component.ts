import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { PostFormApiModel } from '../../models/post-form-api.model';

import { DictionaryService } from '../../services/dictionary.service';
import { AuthorizationService } from '../../services/authorization.service';
import { RequestService } from '../../services/request.service';

@Component({
  selector: 'add-post-page',
  templateUrl: './add-post-page.component.html',
  providers: [ DictionaryService ],
})
export class AddPostPageComponent implements OnInit {

  formData: PostFormApiModel = {
    id: 0,
    title: "",
    text: "",
    imageFiles: [],
    performServiceOnDatesList: [],
    performServiceInPlace: "",
  };


  formTitle: string = "Додавання посту";

  titleStr: string = "Заголовок посту";
  textStr: string = "Текст посту";

  formSubmitButton: string = "Додати";

  emptyTitleStr: string = "Відсутній заколовок";
  emptyTextStr: string = "Відсутній текст";

  serverError: Error | null = null;
  get firstSpanError(): string {

    return this.serverError ?
        this.dictionaryService.get(this.serverError.message)
      : this.emptyTitleStr;
  }

  constructor(
    public router: Router,
    public dictionaryService: DictionaryService,
    public authorizationService: AuthorizationService,
    public requestService: RequestService,
  ) { }

  ngOnInit(): void { }

  submitForm(): void {
    this.requestService
      .post('/api/post/publish/', this.formData, this.authorizationService.jwtString)
      .then(() => this.router.navigateByUrl('/'));
  }

  resetServerError(): void {
    this.serverError = null;
  }

}
