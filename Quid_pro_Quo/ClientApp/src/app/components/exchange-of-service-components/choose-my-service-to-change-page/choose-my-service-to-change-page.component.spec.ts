import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChooseMyServiceToChangePageComponent } from './choose-my-service-to-change-page.component';

describe('ChooseMyServiceToChangePageComponent', () => {
  let component: ChooseMyServiceToChangePageComponent;
  let fixture: ComponentFixture<ChooseMyServiceToChangePageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ChooseMyServiceToChangePageComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ChooseMyServiceToChangePageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
