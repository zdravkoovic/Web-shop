import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NavProductDetailsComponent } from './nav-product-details.component';

describe('NavProductDetailsComponent', () => {
  let component: NavProductDetailsComponent;
  let fixture: ComponentFixture<NavProductDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NavProductDetailsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(NavProductDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
