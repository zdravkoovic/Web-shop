import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProgressSpinerComponent } from './progress-spiner.component';

describe('ProgressSpinerComponent', () => {
  let component: ProgressSpinerComponent;
  let fixture: ComponentFixture<ProgressSpinerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProgressSpinerComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProgressSpinerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
