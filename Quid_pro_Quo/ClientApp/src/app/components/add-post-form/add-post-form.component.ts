import { Component, OnInit, Output, EventEmitter } from '@angular/core';

import { PostGetApiModel } from '../../models/post-get-api.model';

import { DictionaryService } from '../../services/dictionary.service';
import { AuthorizationService } from '../../services/authorization.service';
import { RequestService } from '../../services/request.service';

@Component({
  selector: 'add-post-form',
  templateUrl: './add-post-form.component.html',
  providers: [ DictionaryService ],
})
export class AddPostFormComponent implements OnInit {

  //formData: PostGetApiModel = {
  //  title: "",
  //  text: "",
  //};

  //formTitle: string = "Добавление заметки";

  //titleStr: string = "Заголовок заметки";
  //textStr: string = "Текст заметки";

  //formSubmitButton: string = "Добавить";

  //emptyTitleStr: string = "Отсутствует заколовок";
  //emptyTextStr: string = "Отсутствует текст";

  //serverError: Error | null = null;
  //get firstSpanError(): string {

  //  return this.serverError ?
  //      this.dictionaryService.get(this.serverError.message)
  //    : this.emptyTitleStr;
  //}

  //constructor(
  //  public dictionaryService: DictionaryService,
  //  public authorizationService: AuthorizationService,
  //  public requestService: RequestService,
  //) { }

  ngOnInit(): void { }

  //@Output() onPostAdded = new EventEmitter<boolean>();

  //submitForm(): void {
  //  this.requestService
  //    .post('/api/post/add/', this.formData, this.authorizationService.jwtString)
  //    .then(() => this.onPostAdded.emit());
  //}

  //resetServerError(): void {
  //  this.serverError = null;
  //}

}
