import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChooseServiceToChangeComponent } from './choose-service-to-change.component';

describe('ChooseServiceToChangeComponent', () => {
  let component: ChooseServiceToChangeComponent;
  let fixture: ComponentFixture<ChooseServiceToChangeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ChooseServiceToChangeComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ChooseServiceToChangeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
