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

  postModel: PostFormApiModel = {
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
  imageFilesStr: string = "Файли зображень";

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

  get addPostFormData(): FormData {
    let formData: FormData = new FormData();

    formData.append('title', this.postModel.title);
    formData.append('text', this.postModel.text);
    formData.append('performServiceInPlace', this.postModel.performServiceInPlace);
    formData.append('performServiceOnDatesList', new Date(0).toISOString());

    let imageInput: HTMLInputElement = <HTMLInputElement>document.getElementsByName('imageFiles')[0];
    if (imageInput?.files) {
      for (let i in imageInput.files) {
        formData.append('imageFiles', imageInput.files[i]);
      }
    }

    return formData;
  }

  submitForm(): void {
    this.requestService
      .postMultipartForm('/api/post/publish/', this.addPostFormData, this.authorizationService.jwtString)
      .then(() => this.router.navigateByUrl('/'))
      .catch((err: Error) => this.serverError = err);
  }

  resetServerError(): void {
    this.serverError = null;
  }
}
