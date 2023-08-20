import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BasketCreateOrderDialogComponent } from './basket-create-order-dialog.component';

describe('BasketCreateOrderDialogComponent', () => {
  let component: BasketCreateOrderDialogComponent;
  let fixture: ComponentFixture<BasketCreateOrderDialogComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [BasketCreateOrderDialogComponent]
    });
    fixture = TestBed.createComponent(BasketCreateOrderDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
