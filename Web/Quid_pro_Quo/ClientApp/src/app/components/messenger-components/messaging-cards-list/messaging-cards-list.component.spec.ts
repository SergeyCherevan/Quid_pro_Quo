import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MessagingCardsListComponent } from './messaging-cards-list.component';

describe('MessagingCardsListComponent', () => {
  let component: MessagingCardsListComponent;
  let fixture: ComponentFixture<MessagingCardsListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MessagingCardsListComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MessagingCardsListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
