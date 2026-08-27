import { Component, inject, input, output, signal, computed, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Variante } from '../../Models/producto.model';
import { VarianteService } from '../../Services/variante.service';

export interface EstadoVariantes {
  tieneVariantes: boolean;
  stockTotal: number;
}

@Component({
  selector: 'app-admin-variantes',
  imports: [FormsModule],
  templateUrl: './admin-variantes.html',
  styleUrl: './admin-variantes.css'
})
export class AdminVariantes implements OnInit {
  private varianteService = inject(VarianteService);

  productoId = input.required<number>();


  variantesCambiaron = output<EstadoVariantes>();

  variantes = signal<Variante[]>([]);
  cargando = signal(true);
  guardandoId = signal<number | null>(null);

  nuevoTalle = signal('');
  nuevoStock = signal(0);
  creando = signal(false);
  error = signal<string | null>(null);

  stockTotal = computed(() => this.variantes().reduce((total, v) => total + v.stock, 0));

  ngOnInit(): void {
    this.cargarVariantes();
  }

  cargarVariantes(): void {
    this.cargando.set(true);
    this.varianteService.obtenerPorProducto(this.productoId()).subscribe({
      next: (data) => {
        this.variantes.set(data);
        this.cargando.set(false);
        this.emitirEstado();
      },
      error: () => {
        this.cargando.set(false);
        this.error.set('No se pudieron cargar los talles');
      }
    });
  }

  private emitirEstado(): void {
    this.variantesCambiaron.emit({
      tieneVariantes: this.variantes().length > 0,
      stockTotal: this.stockTotal()
    });
  }

  agregarTalle(): void {
    const talle = this.nuevoTalle().trim();
    if (!talle) return;

    this.creando.set(true);
    this.error.set(null);

    this.varianteService
      .crear(this.productoId(), { talle, stock: this.nuevoStock(), orden: this.variantes().length })
      .subscribe({
        next: (nueva) => {
          this.variantes.update((lista) => [...lista, nueva]);
          this.nuevoTalle.set('');
          this.nuevoStock.set(0);
          this.creando.set(false);
          this.emitirEstado();
        },
        error: (err) => {
          this.creando.set(false);
          this.error.set(err.error?.mensaje ?? 'No se pudo agregar el talle');
        }
      });
  }

  actualizarStock(variante: Variante, nuevoStock: number): void {
    this.guardandoId.set(variante.id);
    this.error.set(null);

    this.varianteService.actualizarStock(variante.id, nuevoStock).subscribe({
      next: (actualizada) => {
        this.variantes.update((lista) => lista.map((v) => (v.id === actualizada.id ? actualizada : v)));
        this.guardandoId.set(null);
        this.emitirEstado();
      },
      error: (err) => {
        this.guardandoId.set(null);
        this.error.set(err.error?.mensaje ?? 'No se pudo actualizar el stock');
      }
    });
  }

  eliminarTalle(variante: Variante): void {
    const confirmado = confirm(`¿Eliminar el talle "${variante.talle}"?`);
    if (!confirmado) return;

    this.guardandoId.set(variante.id);
    this.error.set(null);

    this.varianteService.eliminar(variante.id).subscribe({
      next: () => {
        this.variantes.update((lista) => lista.filter((v) => v.id !== variante.id));
        this.guardandoId.set(null);
        this.emitirEstado();
      },
      error: (err) => {
        this.guardandoId.set(null);
        this.error.set(err.error?.mensaje ?? 'No se pudo eliminar el talle');
      }
    });
  }
}
