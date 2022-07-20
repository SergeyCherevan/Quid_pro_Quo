import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { PostFormApiModel } from '../../models/post-form-api.model';

import { DictionaryService } from '../../services/dictionary.service';
import { AuthorizationService } from '../../services/authorization.service';
import { RequestService } from '../../services/request.service';
import { GoogleMap, MapMarker } from '@angular/google-maps';

@Component({
  selector: 'add-post-page',
  templateUrl: './add-post-page.component.html',
  styleUrls: [ './add-post-page.component.scss' ],
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
  performServiceOnDatesListStr: string = "Надати послугу у певний час";
  performServiceInPlaceStr: string = "Надати послугу у певному місці";

  addDateTimeItemButton: string = "Додати час/дату виконання";
  formSubmitButton: string = "Додати";

  emptyTitleStr: string = "Відсутній заколовок";
  emptyTextStr: string = "Відсутній текст";

  serverError: Error | null = null;
  get firstSpanError(): string {

    return this.serverError ?
        this.dictionaryService.get(this.serverError.message)
      : this.emptyTitleStr;
  }

  @ViewChild(GoogleMap, { static: false }) map: GoogleMap = <any>{};
  zoom = 12
  center: google.maps.LatLngLiteral = <any>{};
  options: google.maps.MapOptions = {
    mapTypeId: 'terrain',
    //zoomControl: false,
    scrollwheel: true,
    disableDoubleClickZoom: true,
    maxZoom: 15,
    minZoom: 8,
  };

  constructor(
    public router: Router,
    public dictionaryService: DictionaryService,
    public authorizationService: AuthorizationService,
    public requestService: RequestService,
  ) { }

  ngOnInit() {
    navigator.geolocation.getCurrentPosition((position) => {
      this.center = {
        lat: position.coords.latitude,
        lng: position.coords.longitude,
      };
    });
  }

  mapCenter = "";
  get addPostFormData(): FormData {
    let formData: FormData = new FormData();

    formData.append('title', this.postModel.title);
    formData.append('text', this.postModel.text);

    let imageInput: HTMLInputElement = <HTMLInputElement>document.getElementsByName('imageFiles')[0];
    if (imageInput?.files) {
      for (let i in imageInput.files) {
        formData.append('imageFiles', imageInput.files[i]);
      }
    }

    let mapCenter: google.maps.LatLng = this.map.getCenter()!;
    let mapZoom: number = this.map.getZoom()!;
    formData.append('performServiceInPlace', `@${mapCenter.toUrlValue()},${mapZoom}z`);

    for (let date of this.postModel.performServiceOnDatesList) {
      formData.append('performServiceOnDatesList', <any>date);
    }

    return formData;
  }

  addDataTimeItem(): void {
    this.postModel.performServiceOnDatesList.push(new Date());
  }

  deleteDataTimeItem(index: number): void {
    this.postModel.performServiceOnDatesList.splice(index, 1);
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
