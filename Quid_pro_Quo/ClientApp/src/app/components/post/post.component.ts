import { Component, OnInit, Input } from '@angular/core';

import { PostGetApiModel } from '../../models/post-get-api.model';

@Component({
  selector: 'post',
  templateUrl: './post.component.html',
  styleUrls: ['./post.component.scss', '../users-page/users-page.component.scss' ],
})
export class PostComponent implements OnInit {

  @Input() postModel: PostGetApiModel = {
    id: "",
    title: "",
    text: "",
    imageFileNames: "",
    authorName: "",
    postedAt: new Date(),
    isActual: false,
    performServiceOnDatesList: [],
    performServiceInPlace: ""
  };
  currentImageNumber: number = 0;

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

}
