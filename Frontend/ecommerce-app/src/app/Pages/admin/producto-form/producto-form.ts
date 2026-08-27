import { Component, inject, signal, OnInit } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { ProductoService } from '../../../Services/producto.service';
import { CategoriaService } from '../../../Services/categoria.service';
import { Categoria } from '../../../Models/categoria.model';
import { AdminVariantes, EstadoVariantes } from '../../../components/admin-variantes/admin-variantes';
import { AdminImagenProducto } from '../../../components/admin-imagen-producto/admin-imagen-producto';

@Component({
  selector: 'app-producto-form',
  imports: [ReactiveFormsModule, AdminVariantes, AdminImagenProducto, RouterLink],
  templateUrl: './producto-form.html',
  styleUrl: './producto-form.css'
})
export class ProductoForm implements OnInit {
  private fb = inject(FormBuilder);
  private productoService = inject(ProductoService);
  private categoriaService = inject(CategoriaService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  categorias = signal<Categoria[]>([]);
  cargando = signal(false);
  errorMensaje = signal<string | null>(null);
  imagenUrlActual = signal<string | null>(null);

  productoId = signal<number | null>(null);
  esEdicion = () => this.productoId() !== null;

  tieneVariantes = signal(false);

  formulario = this.fb.group({
    nombre: ['', [Validators.required, Validators.maxLength(150)]],
    descripcion: [''],
    precio: [0, [Validators.required, Validators.min(0.01)]],
    stock: [0, [Validators.required, Validators.min(0)]],
    imagenUrl: [''],
    destacado: [false],
    tieneVariantes: [false],
    categoriaId: [0, [Validators.required, Validators.min(1)]]
  });

  ngOnInit(): void {
    this.categoriaService.obtenerTodas().subscribe({ next: (data) => this.categorias.set(data) });


    this.formulario.get('tieneVariantes')?.valueChanges.subscribe((tieneVariantes) => {
      const controlStock = this.formulario.get('stock');
      if (tieneVariantes) {
        controlStock?.disable();
      } else {
        controlStock?.enable();
      }
    });

    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      const id = Number(idParam);
      this.productoId.set(id);
      this.cargarProducto(id);
    }
  }

  private cargarProducto(id: number): void {
    this.cargando.set(true);
    this.productoService.obtenerPorId(id).subscribe({
      next: (producto) => {
        this.imagenUrlActual.set(producto.imagenUrl);
        this.formulario.patchValue({
          nombre: producto.nombre,
          descripcion: producto.descripcion ?? '',
          precio: producto.precio,
          stock: producto.stock,
          imagenUrl: producto.imagenUrl ?? '',
          destacado: producto.destacado,
          tieneVariantes: producto.tieneVariantes,
          categoriaId: producto.categoriaId
        });
        this.cargando.set(false);
      },
      error: () => {
        this.cargando.set(false);
        this.errorMensaje.set('No se pudo cargar el producto');
      }
    });
  }

  onImagenCambio(nuevaUrl: string | null): void {
    this.imagenUrlActual.set(nuevaUrl);
    this.formulario.patchValue({
      imagenUrl: nuevaUrl ?? ''
    });
  }


  onVariantesCambiaron(estado: EstadoVariantes): void {
    this.aplicarEstadoVariantes(estado);
  }

  private aplicarEstadoVariantes(estado: EstadoVariantes): void {
    this.tieneVariantes.set(estado.tieneVariantes);

    const controlStock = this.formulario.get('stock')!;

    if (estado.tieneVariantes) {
      controlStock.setValue(estado.stockTotal);
      controlStock.disable();
    } else {
      controlStock.enable();
    }
  }

  onSubmit(): void {
    if (this.formulario.invalid) {
      this.formulario.markAllAsTouched();
      return;
    }

    this.cargando.set(true);
    this.errorMensaje.set(null);

    const valores = this.formulario.getRawValue();
    const datos = {
      nombre: valores.nombre!,
      descripcion: valores.descripcion || null,
      precio: valores.precio!,
      stock: valores.stock!,
      imagenUrl: valores.imagenUrl || null,
      destacado: valores.destacado!,
      tieneVariantes: valores.tieneVariantes!,
      categoriaId: valores.categoriaId!
    };

    if (this.esEdicion()) {
      this.productoService.actualizar(this.productoId()!, datos).subscribe({
        next: () => this.onGuardadoExitoso(),
        error: (err) => this.onError(err)
      });
    } else {
      this.productoService.crear(datos).subscribe({
        next: (creado) => {
          this.cargando.set(false);
          // Después de crear, navegamos a EDITAR ese producto (no a la lista),
          // así el admin puede cargar los talles inmediatamente
          this.router.navigate(['/admin/productos/editar', creado.id]);
        },
        error: (err) => this.onError(err)
      });
    }
  }

  private onGuardadoExitoso(): void {
    this.cargando.set(false);
    this.router.navigateByUrl('/admin/productos');
  }

  private onError(err: any): void {
    this.cargando.set(false);
    this.errorMensaje.set(err.error?.mensaje ?? 'Ocurrió un error al guardar');
  }
}
