import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'not-found-page',
  templateUrl: './not-found-page.component.html',
})
export class NotFoundPageComponent implements OnInit {

  title: string = "Сторінка не знайдена";

  constructor() { }

  ngOnInit(): void {
  }

}
