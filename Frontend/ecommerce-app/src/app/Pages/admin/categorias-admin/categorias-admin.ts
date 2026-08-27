import { Component, inject, signal, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CategoriaAdmin } from '../../../Models/categoria.model';
import { CategoriaService } from '../../../Services/categoria.service';

@Component({
  selector: 'app-categorias-admin',
  imports: [FormsModule],
  templateUrl: './categorias-admin.html',
  styleUrl: './categorias-admin.css'
})
export class CategoriasAdmin implements OnInit {
  private categoriaService = inject(CategoriaService);

  categorias = signal<CategoriaAdmin[]>([]);
  cargando = signal(true);
  guardandoId = signal<number | null>(null);

  nombreNuevo = signal('');
  creando = signal(false);

  ngOnInit(): void {
    this.cargarCategorias();
  }

  cargarCategorias(): void {
    this.cargando.set(true);
    this.categoriaService.obtenerParaAdmin().subscribe({
      next: (data) => {
        this.categorias.set(data);
        this.cargando.set(false);
      },
      error: () => this.cargando.set(false)
    });
  }

  crearCategoria(): void {
    const nombre = this.nombreNuevo().trim();
    if (!nombre) return;

    this.creando.set(true);

    this.categoriaService
      .crear({ nombre, mostrarEnHome: true, orden: this.categorias().length })
      .subscribe({
        next: () => {
          this.nombreNuevo.set('');
          this.creando.set(false);
          this.cargarCategorias();
        },
        error: (err) => {
          this.creando.set(false);
          alert(err.error?.mensaje ?? 'No se pudo crear la categoría');
        }
      });
  }

  toggleMostrarEnHome(categoria: CategoriaAdmin): void {
    this.guardarCambio(categoria, { mostrarEnHome: !categoria.mostrarEnHome });
  }

  cambiarOrden(categoria: CategoriaAdmin, nuevoOrden: number): void {
    this.guardarCambio(categoria, { orden: nuevoOrden });
  }

  eliminarCategoria(categoria: CategoriaAdmin): void {
    this.categoriaService.eliminar(categoria.id).subscribe({
      next: () => {
        console.log('Categoría eliminada correctamente');
        this.cargarCategorias();
      },
      error: (error) => {
        console.error('Error al eliminar la categoría', error);
      }
    });
  }


  private guardarCambio(categoria: CategoriaAdmin, cambios: Partial<CategoriaAdmin>): void {
    this.guardandoId.set(categoria.id);

    const datos = {
      nombre: categoria.nombre,
      mostrarEnHome: cambios.mostrarEnHome ?? categoria.mostrarEnHome,
      orden: cambios.orden ?? categoria.orden
    };

    this.categoriaService.actualizar(categoria.id, datos).subscribe({
      next: (actualizada) => {
        this.categorias.update((lista) =>
          lista.map((c) => (c.id === actualizada.id ? actualizada : c))
        );
        this.guardandoId.set(null);
      },
      error: () => this.guardandoId.set(null)
    });
  }

}
