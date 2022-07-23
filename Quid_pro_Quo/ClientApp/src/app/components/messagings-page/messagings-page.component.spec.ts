import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MessagingsPageComponent } from './messagings-page.component';

describe('MessagingsPageComponent', () => {
  let component: MessagingsPageComponent;
  let fixture: ComponentFixture<MessagingsPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MessagingsPageComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MessagingsPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
