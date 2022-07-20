import { Component, OnInit } from '@angular/core';

import { LoginApiModel } from '../../models/login-api.model';

import { DictionaryService } from '../../services/dictionary.service';
import { AuthorizationService } from '../../services/authorization.service';



@Component({
  selector: 'authorization-form',
  templateUrl: './authorization-form.component.html',
  providers: [ DictionaryService ],
})
export class AuthorizationFormComponent implements OnInit {

  formData = {
    userName: "",
    password: "",
    confirmPassword: "",
  };



  readonly isLogin = 0; readonly isReg = 1;
  isLoginOrReg: number = this.isLogin;
  loginOrRegTitle: string[] = [ "Вхід", "Реєстрація" ];
  loginOrRegSubmitButton: string[] = [ "Увійти", "Зареєструватися" ];

  get formTitle(): string {
    return this.loginOrRegTitle[this.isLoginOrReg];
  }

  get formSubmitButton(): string {
    return this.loginOrRegSubmitButton[this.isLoginOrReg];
  }

  get formSwitchingButton(): string {
    return this.loginOrRegTitle[(this.isLoginOrReg + 1) % 2];
  }



  usernameStr: string = "Ім'я користувача";
  passwordStr: string = "Пароль";
  confirmPasswordStr: string = "Підтвердіть пароль";

  uncorrectUsernameStr: string = "Некоректне ім'я користувача. Не менше 4 буквено-цифрових символів";
  uncorrectPasswordStr: string = "Некоректний пароль. Не менше 4 буквено-цифрових символів";
  uncorrectConfirmPasswordStr: string = "Паролі не співпадають";

  serverError: Error | null = null;
  get firstSpanError(): string {

    return this.serverError ?
        this.dictionaryService.get(this.serverError.message)
      : this.uncorrectUsernameStr;
  }

  resetServerError() {
    this.serverError = null;
  }



  constructor(public dictionaryService: DictionaryService, public authorizationService: AuthorizationService) {

    dictionaryService.dictionary = new Map<string, string>([
      [ "User already exist", "Такий користувач вже існує" ],
      [ "Failed to fetch", "Ошибка соединения с сервером" ],
      [ "User not exists", "Користувач не знайдений" ],
    ]);
  }

  ngOnInit(): void { }

  switchForm(): void {
    this.isLoginOrReg = (this.isLoginOrReg + 1) % 2;
  }

  submitForm(): void {
    let promise: Promise<any>;

    if (this.isLoginOrReg == this.isLogin) {
      promise = this.authorizationService.login(<LoginApiModel>this.formData);
    } else /*if (this.isLoginOrReg == this.isReg)*/ {
      promise = this.authorizationService.register(<LoginApiModel>this.formData);
    }

    promise
      .then(() => this.close())
      .catch((err: Error) => this.serverError = err);
  }

  close(): void {
    let closeButton = document.getElementById("authorization-form-close-button");
    closeButton?.click();
  }
}
