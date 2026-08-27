import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminVariantes } from './admin-variantes';

describe('AdminVariantes', () => {
  let component: AdminVariantes;
  let fixture: ComponentFixture<AdminVariantes>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminVariantes]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminVariantes);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
