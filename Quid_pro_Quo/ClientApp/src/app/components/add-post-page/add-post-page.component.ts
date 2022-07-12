import { Component, OnInit, ViewChild, NgZone } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { PostFormApiModel } from '../../models/post-form-api.model';

import { DictionaryService } from '../../services/dictionary.service';
import { AuthorizationService } from '../../services/authorization.service';
import { RequestService } from '../../services/request.service';
import { GoogleMap, MapMarker } from '@angular/google-maps';

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
    public ngZone: NgZone,
  ) { }

  ngOnInit() {
    navigator.geolocation.getCurrentPosition((position) => {
      this.center = {
        lat: position.coords.latitude,
        lng: position.coords.longitude,
      };
    });
  }

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

    formData.append('performServiceInPlace', this.postModel.performServiceInPlace);

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

  @ViewChild(GoogleMap, { static: false }) map: GoogleMap = <any>{};
  markers: MapMarker[] = [];

  click(event: google.maps.MapMouseEvent | google.maps.IconMouseEvent) {
    console.log(event)

    let marker: MapMarker = new MapMarker(this.map, this.ngZone);
    marker.position = {
      lat: this.center.lat /*+ ((Math.random() - 0.5) * 2) / 10*/,
      lng: this.center.lng /*+ ((Math.random() - 0.5) * 2) / 10*/,
    };
    marker.label = {
      color: 'red',
      text: 'Marker label ' + (this.markers.length + 1),
    };
    marker.title = 'Marker title ' + (this.markers.length + 1);
    marker.options = { animation: google.maps.Animation.BOUNCE };

    this.markers.push(marker);
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
