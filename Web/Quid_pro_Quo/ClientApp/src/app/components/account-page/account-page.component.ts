import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription, timeInterval } from 'rxjs';

import { AuthorizationService } from '../../services/authorization.service';
import { RequestService } from '../../services/request.service';
import { DictionaryService } from '../../services/dictionary.service';
import { MessengerSignalRService } from '../../services/messenger-signalR.service';
import { IoTSignalRService } from '../../services/IoT-signalR.service';

import { UserApiModel } from '../../models/user-api.model';



export enum HasIoT {
  No = 'No',
  Yes = 'Yes',
  Attaching = 'Attaching',
}

@Component({
  selector: 'account-page',
  templateUrl: './account-page.component.html',
  styleUrls: ['./account-page.component.scss'],
  providers: [ DictionaryService ],
})
export class AccountPageComponent implements OnInit {

  title: string = "Аккаунт користувача:";
  logoutStr: string = "Вийти";
  saveChangesStr: string = "Зберегти зміни";
  changePasswordStr: string = "Змінити пароль";



  idStr: string = "ID користувача:";
  usernameStr: string = "Ім'я користувача";
  biographiStr: string = "Боіграфія";
  oldPasswordStr: string = "Старий пароль";
  newPasswordStr: string = "Новий пароль";
  confirmNewPasswordStr: string = "Підтвердіть пароль";
  iotCodeStr: string = "Код IoT-пристрою";
  attachStr: string = "Прикріпити IoT-пристрій";
  dettachStr: string = "Відкріпити IoT-пристрій";
  pleaseConfirmAttachStr: string = "Підтведіть прикріплення на IoT-пристрої";
  iotIsAttachedStr: string = "IoT-пристрій прикріплено";

  changesIsSavedStr: string = "Зміни збережені";
  passwordIsChangedStr: string = "Пароль змінено";
  uncorrectUsernameStr: string = "Некоректне ім'я користувача. Не менше 4 буквено-цифрових символів";
  uncorrectNewPasswordStr: string = "Некоректний пароль. Не менше 4 буквено-цифрових символів";
  uncorrectConfirmPasswordStr: string = "Паролі не співпадають";

  isChangesSaved: boolean = false;
  serverError: Error | null = null;
  get firstSpanError(): string {

    return this.serverError ?
      this.dictionaryService.get(this.serverError.message)
      : this.uncorrectUsernameStr;
  }

  isPasswordChanged: boolean = false;
  serverChangePassError: Error | null = null;
  get firstChangePassSpanError(): string {

    return this.serverChangePassError ?
      this.dictionaryService.get(this.serverChangePassError.message)
      : "";
  }

  resetServerError() {
    this.serverError = this.serverChangePassError = null;
  }



  clickOnAvatar(): void {
    let avatarInput: HTMLInputElement = <HTMLInputElement>document.getElementById("avatar-input");
    avatarInput.click();
  }

  uploadedAvatarOnProfileSrc?: string = undefined;
  get avatarOnProfileSrc(): string {
    if (this.uploadedAvatarOnProfileSrc) {
      return this.uploadedAvatarOnProfileSrc;
    }
    return `/api/file/avatar/${this.userModel.avatarFileName ?? 'default_avatar.png'}`
  }

  onSelectFile(event: any): void { // called each time file input changes
    if (event.target.files && event.target.files[0]) {
      let reader = new FileReader();

      reader.readAsDataURL(event.target.files[0]); // read file as data url

      reader.onload = (event) => { // called once readAsDataURL is completed
        this.uploadedAvatarOnProfileSrc = <any>event?.target?.result;
      }
    }
  }

  

  userModel: UserApiModel = {
    id: 0,
    userName: "",
    avatarFileName: undefined,
    biographi: "",
    role: "",
  };

  changePasswordModel = {
    oldPassword: "",
    newPassword: "",
    confirmNewPassword: "",
  };

  iotCode: string = "—";
  hasIoT: HasIoT = HasIoT.No;
  isAttached: boolean = false;

  timerId: number = -1;
  timeLeft: Date = new Date(0);


  //subscription: Subscription;

  constructor(
    public activateRoute: ActivatedRoute,
    public router: Router,
    public authorizationService: AuthorizationService,
    public requestService: RequestService,
    public dictionaryService: DictionaryService,
    public messagingsService: MessengerSignalRService,
    public iotService: IoTSignalRService,
  ) {
    dictionaryService.dictionary = new Map<string, string>([
      //["User already exist", "Такий користувач вже існує"],
      //["Failed to fetch", "Ошибка соединения с сервером"],
      //["User not exists", "Користувач не знайдений"],
    ]);
  }

  ngOnInit(): void {
    this.authorizationService.loginByLocalStorageData()
      .then(() =>
        this.requestService
          .get(`/api/user/get/${this.authorizationService.userName}`)
          .then(respObj => this.userModel = <UserApiModel>respObj)
      )
      .then(() =>
        this.requestService
          .get(`/api/IoT/getMyIoTCode`, this.authorizationService.jwtString)
          .then(respObj => {
            this.iotCode = "" + <number>respObj;
            this.hasIoT = HasIoT.Yes;
          })
          .catch(respObj => {
            this.iotCode = "—";
            this.hasIoT = HasIoT.No;
          })
      );
  }

  logout(): void {
    this.authorizationService.logout();
    this.messagingsService.abortConnection();
    this.router.navigateByUrl('/');
  }

  get editAccountFormData(): FormData {
    let formData: FormData = new FormData();

    formData.append('userName', this.userModel.userName);
    formData.append('biographi', this.userModel.biographi);

    let avatarInput: HTMLInputElement = <HTMLInputElement>document.getElementsByName('avatarFile')[0];
    if (avatarInput?.files) {
      formData.append('avatarFile', avatarInput.files[0]);
    }

    return formData;
  }

  saveChanges(): void {
    this.requestService
      .putMultipartForm('/api/account/edit', this.editAccountFormData, this.authorizationService.jwtString)
      .then(() => this.isChangesSaved = true)
      .then(() => this.authorizationService.login({
        userName: this.userModel.userName,
        password: this.authorizationService.password!,
      }))
      .then(() => window.location.reload())
      .catch((err: Error) => this.serverError = err);
  }

  changePassword(): void {
    this.authorizationService
      .changePassword(this.changePasswordModel)
      .then(() => this.isPasswordChanged = true)
      .then(() => window.location.reload())
      .catch((err: Error) => this.serverChangePassError = err);
  }

  sendRequestToAttachIoT(): void {
    this.requestService
      .post('/api/IoT/addRequestToAttach', { ioTCode: Number(this.iotCode) }, this.authorizationService.jwtString)
      .then(() => {
        this.hasIoT = HasIoT.Attaching;
        this.timeLeft = new Date(0, 0, 0, 0, 5, 0);
        this.timerId = window.setInterval(() => this.decrementSecondsVariable(), 1000);

        this.iotService.isAttachedCallback =
          () => this.hasBeenAttached();
        this.iotService.startConnection();
      })
  }

  decrementSecondsVariable(): void {
    this.timeLeft.setTime(this.timeLeft.getTime() - 1000);
    this.timeLeft = new Date(this.timeLeft);

    if (this.timeLeft.getTime() - new Date(0, 0, 0).getTime() <= 0) {
      window.clearInterval(this.timerId);
      this.hasIoT = HasIoT.No;
    }
  }

  hasBeenAttached(): void {
    this.hasIoT = HasIoT.Yes;
    this.isAttached = true;
  }

  sendRequestToDettachIoT(): void {
    this.requestService
      .post('/api/IoT/dettach', { }, this.authorizationService.jwtString)
      .then(() => {
        this.iotCode = "—";
        this.hasIoT = HasIoT.No;
        this.isAttached = false;
      })
  }
}
