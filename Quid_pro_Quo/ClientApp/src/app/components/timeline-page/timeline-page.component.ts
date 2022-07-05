import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'timeline-page',
  templateUrl: './timeline-page.component.html',
})
export class TimelinePageComponent implements OnInit {

  title: string[] = [ "Стрічка послуг" ];

  constructor() { }

  ngOnInit(): void {
  }

}
