import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CarruselProductos } from './carrusel-productos';

describe('CarruselProductos', () => {
  let component: CarruselProductos;
  let fixture: ComponentFixture<CarruselProductos>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CarruselProductos]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CarruselProductos);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
