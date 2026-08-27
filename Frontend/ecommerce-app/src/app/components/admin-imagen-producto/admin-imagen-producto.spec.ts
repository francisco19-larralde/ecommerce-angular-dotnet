import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminImagenProducto } from './admin-imagen-producto';

describe('AdminImagenProducto', () => {
  let component: AdminImagenProducto;
  let fixture: ComponentFixture<AdminImagenProducto>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminImagenProducto]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminImagenProducto);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
