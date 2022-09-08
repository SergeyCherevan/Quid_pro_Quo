import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchGeomarkerFormComponent } from './search-geomarker-form.component';

describe('SearchGeomarkerFormComponent', () => {
  let component: SearchGeomarkerFormComponent;
  let fixture: ComponentFixture<SearchGeomarkerFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SearchGeomarkerFormComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SearchGeomarkerFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
