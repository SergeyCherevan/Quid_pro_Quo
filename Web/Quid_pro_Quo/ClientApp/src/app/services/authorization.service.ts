import { Injectable } from "@angular/core";

import { RequestService } from "./request.service";

import { LoginApiModel } from '../models/login-api.model';
import { ChangePasswordApiModel } from '../models/change-password-api.model';

@Injectable()
export class AuthorizationService {

  userName?: string;
  password?: string;

  jwtString?: string;
  jwtObject: any;

  constructor(public requestService: RequestService) { }

  login(formData: LoginApiModel): Promise<any> {
    return this.requestService
      .post('/api/account/login', <LoginApiModel>formData)
      .then(responseObject => this.saveDataInSevice(responseObject.jwtString, formData))
      .then(() => this.saveDataInLocalStorage());
  }

  register(formData: LoginApiModel): Promise<void> {
    return this.requestService
      .post('/api/account/registration', <LoginApiModel>formData)
      .then(() => this.login(formData));
  }

  saveDataInSevice(jwtString: string, formData: LoginApiModel): void {
    this.userName = formData.userName;
    this.password = formData.password;
    this.jwtString = jwtString;
    this.jwtObject = this.decodeJwt(jwtString);
  }

  decodeJwt(jwtString: string): any {
    try {
      return JSON.parse(atob(jwtString.split('.')[1]));
    } catch (err) {
      return null;
    }
  }

  saveDataInLocalStorage(): void {
    localStorage.setItem('userName', this.userName!);
    localStorage.setItem('password', this.password!);
  }

  loginByLocalStorageData(): Promise<any> {
    let formData: LoginApiModel = {
      userName: localStorage.getItem('userName')!,
      password: localStorage.getItem('password')!,
    };

    if (formData.userName == null || formData.userName == null) {
      return new Promise<any>(() => { });
    }

    return this.login(formData);
  }

  idTimeout?: number;

  startRegularLogin(): void {
    this.idTimeout =
      window.setInterval(() => this.login({
        userName: this.userName!,
        password: this.password!,
      }), 10_000);
  }

  logout(): void {
    this.userName = this.password = this.jwtString = undefined
    this.jwtObject = undefined;

    localStorage.removeItem('id');
    localStorage.removeItem('userName');
    localStorage.removeItem('password');
  }

  changePassword(formData: ChangePasswordApiModel): Promise<any> {
    return this.requestService
      .put('/api/account/changePassword', {
        oldPassword: formData.oldPassword,
        newPassword: formData.newPassword,
      }, this.jwtString)
      .then(() => this.login({
        userName: this.userName!,
        password: formData.newPassword,
      }));
  }
}
