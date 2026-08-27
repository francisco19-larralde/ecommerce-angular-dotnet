import { Component, inject, signal, computed, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { Subscription } from 'rxjs';
import { Producto, Variante } from '../../Models/producto.model';
import { ProductoService } from '../../Services/producto.service';
import { CarritoService } from '../../Services/carrito.service';
import { AuthService } from '../../Services/auth.service';
import { DecimalPipe } from '@angular/common';

@Component({
  selector: 'app-producto-detalle',
  imports: [RouterLink, DecimalPipe],
  templateUrl: './producto-detalle.html',
  styleUrl: './producto-detalle.css'
})
export class ProductoDetalle implements OnInit, OnDestroy {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private productoService = inject(ProductoService);
  private carritoService = inject(CarritoService);
  private authService = inject(AuthService);

  producto = signal<Producto | null>(null);
  cargando = signal(true);
  error = signal<string | null>(null);
  cantidad = signal(1);
  agregando = signal(false);

  varianteSeleccionada = signal<Variante | null>(null);


  stockDisponible = computed(() => {
    const producto = this.producto();
    if (!producto) return 0;
    if (producto.tieneVariantes) {
      return this.varianteSeleccionada()?.stock ?? 0;
    }
    return producto.stock;
  });

  hayStockGeneral = computed(() => {
    const producto = this.producto();
    if (!producto) return false;

    if (producto.tieneVariantes) {
      return producto.variantes.some((v) => v.stock > 0);
    }
    return producto.stock > 0;
  });


  puedeAgregar = computed(() => {
    const producto = this.producto();
    if (!producto) return false;
    if (producto.tieneVariantes && !this.varianteSeleccionada()) return false;
    return this.stockDisponible() > 0;
  });

  private subscripcion?: Subscription;

  ngOnInit(): void {
    this.subscripcion = this.route.paramMap.subscribe((params) => {
      const id = Number(params.get('id'));
      this.cargarProducto(id);
    });
  }

  ngOnDestroy(): void {
    this.subscripcion?.unsubscribe();
  }

  private cargarProducto(id: number): void {
    this.cargando.set(true);
    this.error.set(null);
    this.cantidad.set(1);
    this.varianteSeleccionada.set(null);

    this.productoService.obtenerPorId(id).subscribe({
      next: (data) => {
        this.producto.set(data);
        this.cargando.set(false);
      },
      error: () => {
        this.error.set('No se encontró el producto que buscás.');
        this.cargando.set(false);
      }
    });
  }

  seleccionarTalle(variante: Variante): void {
    if (variante.stock === 0) return;
    this.varianteSeleccionada.set(variante);
    this.cantidad.set(1);
  }

  incrementar(): void {
    if (this.cantidad() < this.stockDisponible()) {
      this.cantidad.update((c) => c + 1);
    }
  }

  decrementar(): void {
    if (this.cantidad() > 1) {
      this.cantidad.update((c) => c - 1);
    }
  }

  agregarAlCarrito(): void {
    const producto = this.producto();
    if (!producto || !this.puedeAgregar()) return;

    if (!this.authService.estaLogueado()) {
      this.router.navigateByUrl('/login');
      return;
    }

    this.agregando.set(true);

    this.carritoService
      .agregarItem(producto.id, this.cantidad(), this.varianteSeleccionada()?.id ?? null)
      .subscribe({
        next: () => this.agregando.set(false),
        error: (err) => {
          this.agregando.set(false);
          alert(err.error?.mensaje ?? 'No se pudo agregar el producto al carrito');
        }
      });
  }
}
