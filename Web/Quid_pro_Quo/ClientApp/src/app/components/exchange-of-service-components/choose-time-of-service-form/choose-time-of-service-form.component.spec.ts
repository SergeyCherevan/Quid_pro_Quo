import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChooseTimeOfServiceFormComponent } from './choose-time-of-service-form.component';

describe('ChooseTimeOfServiceFormComponent', () => {
  let component: ChooseTimeOfServiceFormComponent;
  let fixture: ComponentFixture<ChooseTimeOfServiceFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ChooseTimeOfServiceFormComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ChooseTimeOfServiceFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
