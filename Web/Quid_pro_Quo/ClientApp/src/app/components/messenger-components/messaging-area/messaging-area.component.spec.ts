import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MessagingAreaComponent } from './messaging-area.component';

describe('MessagingAreaComponent', () => {
  let component: MessagingAreaComponent;
  let fixture: ComponentFixture<MessagingAreaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MessagingAreaComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MessagingAreaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
