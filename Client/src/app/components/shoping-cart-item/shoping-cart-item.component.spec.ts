import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShopingCartItemComponent } from './shoping-cart-item.component';

describe('ShopingCartItemComponent', () => {
  let component: ShopingCartItemComponent;
  let fixture: ComponentFixture<ShopingCartItemComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ShopingCartItemComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ShopingCartItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
