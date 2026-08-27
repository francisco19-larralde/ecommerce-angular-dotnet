import { Component, inject, signal, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Producto } from '../../Models/producto.model';
import { Categoria } from '../../Models/categoria.model';
import { FiltroCatalogo, FILTRO_INICIAL } from '../../Models/filtro-catalogo.model';
import { ProductoService } from '../../Services/producto.service';
import { CategoriaService } from '../../Services/categoria.service';
import { CarritoService } from '../../Services/carrito.service';
import { AuthService } from '../../Services/auth.service';
import { ProductoCard } from '../../components/producto-card/producto-card';

@Component({
  selector: 'app-catalogo',
  imports: [FormsModule, ProductoCard],
  templateUrl: './catalogo.html',
  styleUrl: './catalogo.css'
})
export class Catalogo implements OnInit {
  private productoService = inject(ProductoService);
  private categoriaService = inject(CategoriaService);
  private carritoService = inject(CarritoService);
  private authService = inject(AuthService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  productos = signal<Producto[]>([]);
  categorias = signal<Categoria[]>([]);
  tallesDisponibles = signal<string[]>([]);

  cargando = signal(true);
  agregandoId = signal<number | null>(null);

  totalPaginas = signal(1);
  totalRegistros = signal(0);

  filtros = signal<FiltroCatalogo>({ ...FILTRO_INICIAL });


  precioMinBorrador = signal<number | null>(null);
  precioMaxBorrador = signal<number | null>(null);

  ngOnInit(): void {
    this.categoriaService.obtenerTodas().subscribe({ next: (data) => this.categorias.set(data) });
    this.productoService.obtenerTallesDisponibles().subscribe({ next: (data) => this.tallesDisponibles.set(data) });

    const categoriaParam = this.route.snapshot.queryParamMap.get('categoriaId');
    if (categoriaParam) {
      this.filtros.update((f) => ({ ...f, categoriaId: Number(categoriaParam) }));
    }

    this.buscar();
  }

  buscar(): void {
    this.cargando.set(true);

    this.productoService.obtenerCatalogo(this.filtros()).subscribe({
      next: (resultado) => {
        this.productos.set(resultado.items);
        this.totalPaginas.set(resultado.totalPaginas);
        this.totalRegistros.set(resultado.totalRegistros);
        this.cargando.set(false);
      },
      error: () => this.cargando.set(false)
    });
  }


  private aplicarFiltro(cambios: Partial<FiltroCatalogo>): void {
    this.filtros.update((f) => ({ ...f, ...cambios, pagina: 1 }));
    this.buscar();
  }

  onCambiarCategoria(categoriaId: number | null): void {
    this.aplicarFiltro({ categoriaId });
  }

  onCambiarTalle(talle: string | null): void {
    this.aplicarFiltro({ talle });
  }

  onCambiarOrden(orden: string): void {
    this.aplicarFiltro({ ordenarPor: orden });
  }

  onBuscarTexto(texto: string): void {
    this.aplicarFiltro({ busqueda: texto || null });
  }

  aplicarPrecio(): void {
    this.aplicarFiltro({
      precioMin: this.precioMinBorrador(),
      precioMax: this.precioMaxBorrador()
    });
  }

  limpiarFiltros(): void {
    this.filtros.set({ ...FILTRO_INICIAL });
    this.precioMinBorrador.set(null);
    this.precioMaxBorrador.set(null);
    this.buscar();
  }

  irAPagina(pagina: number): void {
    if (pagina < 1 || pagina > this.totalPaginas()) return;
    this.filtros.update((f) => ({ ...f, pagina }));
    this.buscar();
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }

  agregarAlCarrito(producto: Producto): void {
    if (!this.authService.estaLogueado()) {
      this.router.navigateByUrl('/login');
      return;
    }

    this.agregandoId.set(producto.id);

    this.carritoService.agregarItem(producto.id, 1).subscribe({
      next: () => this.agregandoId.set(null),
      error: (err) => {
        this.agregandoId.set(null);
        alert(err.error?.mensaje ?? 'No se pudo agregar el producto al carrito');
      }
    });
  }
}
