import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';

import { PostGetApiModel } from '../../models/post-get-api.model';
import { PostsPageApiModel } from '../../models/posts-page-api-model';

import { DictionaryService } from '../../services/dictionary.service';
import { RequestService } from '../../services/request.service';
import { AuthorizationService } from '../../services/authorization.service';

@Component({
  selector: 'timeline-page',
  templateUrl: './timeline-page.component.html',
  providers: [DictionaryService],
})
export class TimelinePageComponent implements OnInit {

  title: string = "Стрічка послуг";
  searchStr: string = "Шукати";

  postsData: PostsPageApiModel = {
    posts: [],
    postsCount: 0,
  };
  post: PostGetApiModel = {
    id: "",
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

  querySubscription: Subscription;
  keywords: string = '';
  geomarker?: string = undefined;
  date?: string = undefined;
  pageNumber: number = 0;
  pageSize: number = 3;

  get pageCount(): number {
    return Math.floor(this.postsData.postsCount / this.pageSize) +
      (this.postsData.postsCount % this.pageSize > 0 ? 1 : 0);
  }

  pageNumbersArr(begin: number, end: number) {
    let arr = [];
    for (let i = begin; i <= end; i++) {
      arr.push(i);
    }
    return arr;
  }

  constructor(
    public activateRoute: ActivatedRoute,
    public router: Router,
    public dictionaryService: DictionaryService,
    public requestService: RequestService,
    public authorizationService: AuthorizationService
  ) {
    this.querySubscription = activateRoute.queryParams.subscribe(
      (queryParam: any) => {
        this.keywords = queryParam['keywords'] ?? '';
        this.date = queryParam['date'] ?? '';
        this.geomarker = queryParam['geomarker'] ?? undefined;
        this.pageNumber = Number(queryParam['pageNumber'] ?? 0);

        this.ngOnInit();
      }
    );
  }

  ngOnInit(): void {
    this.requestService
      .get(`/api/post/getByFilter?keywords=${this.keywords}&date=${this.date ?? ''}&geomarker=${this.geomarker ?? ''}&pageNumber=${this.pageNumber}&pageSize=${this.pageSize}`,
        this.authorizationService.jwtString)
      .then((respObj: PostsPageApiModel) => this.postsData = respObj);
  }

  setGeomarker(geomarker: string): void {
    this.geomarker = geomarker;
    this.search();
  }

  deleteGeomarker(): void {
    this.geomarker = undefined;
    this.search();
  }

  search(): void {
    console.log(this.date);
    this.router.navigateByUrl(`?keywords=${this.keywords}&date=${this.date ?? ''}&geomarker=${this.geomarker ?? ''}&pageNumber=0`);
  }
}
