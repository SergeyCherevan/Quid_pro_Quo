import { Component, OnInit, Output, EventEmitter, ViewChild } from '@angular/core';
import { GoogleMap, MapMarker } from '@angular/google-maps';

@Component({
  selector: 'search-geomarker-form',
  templateUrl: './search-geomarker-form.component.html',
  styleUrls: [ '../add-post-page/add-post-page.component.scss' ]
})
export class SearchGeomarkerFormComponent implements OnInit {

  formTitle: string = "Пошук за геометкою";
  searchServiceInPlaceStr: string = "Шукати послугу у певному місці";
  setGeomarkerButton: string = "Встановити геометку";
  deleteGeomarkerButton: string = "Видалити геометку";

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

  constructor() { }

  ngOnInit(): void {
    navigator.geolocation.getCurrentPosition((position) => {
      this.center = {
        lat: position.coords.latitude,
        lng: position.coords.longitude,
      };
    });
  }

  @Output() onSetGeomarker = new EventEmitter<string>();
  @Output() onDeleteGeomarker = new EventEmitter<void>();

  setGeomarker(): void {
    let mapCenter: google.maps.LatLng = this.map.getCenter()!;
    this.onSetGeomarker.emit(`${mapCenter.toUrlValue()}`);

    this.close();
  }

  deleteGeomarker(): void {
    let mapCenter: google.maps.LatLng = this.map.getCenter()!;
    this.onDeleteGeomarker.emit();

    this.close();
  }

  close(): void {
    let closeButton = document.getElementById("search-geomarker-form-close-button");
    closeButton?.click();
  }
}
