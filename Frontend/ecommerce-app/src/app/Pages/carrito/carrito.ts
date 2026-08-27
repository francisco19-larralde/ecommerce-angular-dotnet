import { Component, inject, signal } from '@angular/core';
import { CarritoService } from '../../Services/carrito.service';
import { CarritoItem } from '../../Models/carrito.model';
import { DecimalPipe } from '@angular/common';
import { RouterLink, RouterLinkActive } from '@angular/router';

@Component({
  selector: 'app-carrito',
  imports: [DecimalPipe, RouterLink],
  templateUrl: './carrito.html',
  styleUrl: './carrito.css'
})
export class CarritoPage {
  carritoService = inject(CarritoService);

  procesandoId = signal<number | null>(null);

  cambiarCantidad(item: CarritoItem, nuevaCantidad: number): void {
    if (nuevaCantidad < 1) return;

    this.procesandoId.set(item.id);

    this.carritoService.actualizarCantidad(item.id, nuevaCantidad).subscribe({
      next: () => this.procesandoId.set(null),
      error: (err) => {
        this.procesandoId.set(null);
        alert(err.error?.mensaje ?? 'No se pudo actualizar la cantidad');
      }
    });
  }

  eliminarItem(item: CarritoItem): void {
    this.procesandoId.set(item.id);

    this.carritoService.eliminarItem(item.id).subscribe({
      next: () => this.procesandoId.set(null),
      error: () => this.procesandoId.set(null)
    });
  }
}
