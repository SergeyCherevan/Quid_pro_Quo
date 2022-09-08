import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MessagingCardComponent } from './messaging-card.component';

describe('MessagingCardComponent', () => {
  let component: MessagingCardComponent;
  let fixture: ComponentFixture<MessagingCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MessagingCardComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MessagingCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
