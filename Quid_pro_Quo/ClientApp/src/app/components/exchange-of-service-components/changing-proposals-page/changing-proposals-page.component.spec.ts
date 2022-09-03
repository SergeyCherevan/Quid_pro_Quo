import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChangingProposalsPageComponent } from './changing-proposals-page.component';

describe('ChangingProposalsPageComponent', () => {
  let component: ChangingProposalsPageComponent;
  let fixture: ComponentFixture<ChangingProposalsPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ChangingProposalsPageComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ChangingProposalsPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
