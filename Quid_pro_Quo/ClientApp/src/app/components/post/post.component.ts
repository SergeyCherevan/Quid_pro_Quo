import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

import { PostGetApiModel } from '../../models/post-get-api.model';

@Component({
  selector: 'post',
  templateUrl: './post.component.html',
  styleUrls: ['./post.component.scss', '../users-page/users-page.component.scss' ],
})
export class PostComponent implements OnInit {

  @Input() postModel: PostGetApiModel = {
    id: 0,
    title: "",
    text: "",
    imageFileNames: "",
    authorName: "",
    postedAt: new Date(),
    isActual: false,
    performServiceOnDatesList: [],
    performServiceInPlaceLat: 0,
    performServiceInPlaceLng: 0,
    performServiceInPlaceZoom: 15,
  };
  currentImageNumber: number = 0;

  @Input() symbolOnCenter: string = "change";
  @Input() isChoosed: boolean = false;

  get imageFileNamesArr(): string[] {
    if (this.postModel.imageFileNames == "" || this.postModel.imageFileNames == null) {
      return [];
    }
    return this.postModel.imageFileNames.split(';');
  }
  get imageNumbersArr(): number[] {
    let countImages: number = this.imageFileNamesArr.length;
    let arr: number[] = [];
    for (let i = 0; i < countImages; i++) {
      arr.push(i);
    }

    return arr;
  }

  get titleInnerHTML() {
    return this.postModel.title.split('  ').join(' &nbsp;');
  }

  get textInnerHTML() {
    return this.postModel.text.split('  ').join(' &nbsp;').split('\n').join('<br>');
  }

  datesStr: string = "Дати і час послуги";
  placeStr: string = "Переглянути місце у Google.Maps";
  prevStr: string = "Попередній";
  nextStr: string = "Наступний";

  constructor() { }

  ngOnInit(): void { }

  @Output() onCircleButtonClick = new EventEmitter<number>();

  clickCircleButton() {
    this.onCircleButtonClick.emit(this.postModel.id);
  }

}
