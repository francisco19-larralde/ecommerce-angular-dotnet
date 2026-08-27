import { Component, inject, signal, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { Producto } from '../../../Models/producto.model';
import { Categoria } from '../../../Models/categoria.model';
import { ProductoService } from '../../../Services/producto.service';
import { CategoriaService } from '../../../Services/categoria.service';
import { DecimalPipe } from '@angular/common';

@Component({
  selector: 'app-productos-admin',
  imports: [RouterLink, FormsModule, DecimalPipe],
  templateUrl: './productos-admin.html',
  styleUrl: './productos-admin.css'
})
export class ProductosAdmin implements OnInit {
  private productoService = inject(ProductoService);
  private categoriaService = inject(CategoriaService);

  productos = signal<Producto[]>([]);
  categorias = signal<Categoria[]>([]);
  cargando = signal(true);
  eliminandoId = signal<number | null>(null);
  actualizandoId = signal<number | null>(null);

  // Estado de filtros y paginación
  paginaActual = signal(1);
  totalPaginas = signal(1);
  totalRegistros = signal(0);
  tamanioPagina = 10;
  categoriaFiltro = signal<number | null>(null);
  busqueda = signal('');

  ngOnInit(): void {
    this.categoriaService.obtenerTodas().subscribe({
      next: (data) => this.categorias.set(data)
    });

    this.cargarProductos();
  }

  cargarProductos(): void {
    this.cargando.set(true);

    this.productoService
      .obtenerPaginado(this.paginaActual(), this.tamanioPagina, this.categoriaFiltro(), this.busqueda())
      .subscribe({
        next: (resultado) => {
          this.productos.set(resultado.items);
          this.totalPaginas.set(resultado.totalPaginas);
          this.totalRegistros.set(resultado.totalRegistros);
          this.cargando.set(false);
        },
        error: () => this.cargando.set(false)
      });
  }

  // Cada vez que cambia un filtro, volvemos a la página 1 y recargamos
  onCambiarCategoria(): void {
    this.paginaActual.set(1);
    this.cargarProductos();
  }

  onBuscar(): void {
    this.paginaActual.set(1);
    this.cargarProductos();
  }

  irAPagina(pagina: number): void {
    if (pagina < 1 || pagina > this.totalPaginas()) return;
    this.paginaActual.set(pagina);
    this.cargarProductos();
  }

  toggleDestacado(producto: Producto): void {
    this.actualizandoId.set(producto.id);

    this.productoService.actualizarEstado(producto.id, { destacado: !producto.destacado }).subscribe({
      next: (actualizado) => {
        this.productos.update((lista) =>
          lista.map((p) => (p.id === actualizado.id ? actualizado : p))
        );
        this.actualizandoId.set(null);
      },
      error: () => this.actualizandoId.set(null)
    });
  }

  toggleActivo(producto: Producto): void {
    this.actualizandoId.set(producto.id);

    this.productoService.actualizarEstado(producto.id, { activo: !producto.activo }).subscribe({
      next: (actualizado) => {
        this.productos.update((lista) =>
          lista.map((p) => (p.id === actualizado.id ? actualizado : p))
        );
        this.actualizandoId.set(null);
      },
      error: () => this.actualizandoId.set(null)
    });
  }

  eliminar(producto: Producto): void {
    const confirmado = confirm(`¿Seguro que querés eliminar "${producto.nombre}"?`);
    if (!confirmado) return;

    this.eliminandoId.set(producto.id);

    this.productoService.eliminar(producto.id).subscribe({
      next: () => {
        this.eliminandoId.set(null);
        this.cargarProductos();
      },
      error: (err) => {
        this.eliminandoId.set(null);
        alert(err.error?.mensaje ?? 'No se pudo eliminar el producto');
      }
    });
  }
}
