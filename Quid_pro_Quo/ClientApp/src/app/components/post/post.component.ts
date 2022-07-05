import { Component, OnInit, Input } from '@angular/core';

import { PostResponseModel } from '../../models/post-response.model';

@Component({
  selector: 'post',
  templateUrl: './post.component.html',
  styleUrls: ['./post.component.scss'],
})
export class PostComponent implements OnInit {

  @Input() postModel: PostResponseModel = {
    id: "",
    title: "",
    text: "",
    userName: "",
    lastUpdate: new Date(),
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
