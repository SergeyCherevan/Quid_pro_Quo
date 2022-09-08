import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChooseTimeOfServiceComponent } from './choose-time-of-service-form.component.spec';

describe('ChooseTimeOfServiceComponent', () => {
  let component: ChooseTimeOfServiceComponent;
  let fixture: ComponentFixture<ChooseTimeOfServiceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ChooseTimeOfServiceComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ChooseTimeOfServiceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
