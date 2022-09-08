import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

import { EditPostModel } from '../../models/edit-post.model';

import { DictionaryService } from '../../services/dictionary.service';
import { AuthorizationService } from '../../services/authorization.service';
import { RequestService } from '../../services/request.service';

@Component({
  selector: 'edit-post-form',
  templateUrl: './edit-post-form.component.html',
  providers: [ DictionaryService ],
})
export class EditPostFormComponent implements OnInit {

  @Input() @Output()
  formData: EditPostModel = {
    id: "",
    title: "",
    text: "",
  };

  formTitle: string = "Редагування посту";

  titleStr: string = "Заголовок посту";
  textStr: string = "Текст посту";

  formSubmitButton: string = "Редагувати";

  emptyTitleStr: string = "Відсутній заколовок";
  emptyTextStr: string = "Відсутній текст";

  serverError: Error | null = null;
  get firstSpanError(): string {

    return this.serverError ?
      this.dictionaryService.get(this.serverError.message)
      : this.emptyTitleStr;
  }

  constructor(
    public dictionaryService: DictionaryService,
    public authorizationService: AuthorizationService,
    public requestService: RequestService,
  ) { }

  ngOnInit(): void {

  }

  @Output() onPostEdited = new EventEmitter<boolean>();

  submitForm(): void {
    this.requestService
      .put('/api/post/edit/', this.formData, this.authorizationService.jwtString)
      .then(() => this.onPostEdited.emit());
  }

  resetServerError(): void {
    this.serverError = null;
  }

}
