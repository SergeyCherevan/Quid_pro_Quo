import { Component, OnInit, Input } from '@angular/core';

import { PostGetApiModel } from '../../models/post-get-api.model';

@Component({
  selector: 'post',
  templateUrl: './post.component.html',
  styleUrls: ['./post.component.scss'],
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

  get titleInnerHTML() {
    return this.postModel.title.split('  ').join(' &nbsp;');
  }

  get textInnerHTML() {
    return this.postModel.text.split('  ').join(' &nbsp;').split('\n').join('<br>');
  }

  constructor() { }

  ngOnInit(): void { }

}
