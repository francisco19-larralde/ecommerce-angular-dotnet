import { Component, inject, input, output, signal } from '@angular/core';
import { ImagenService } from '../../Services/imagen.service';

@Component({
  selector: 'app-admin-imagen-producto',
  imports: [],
  templateUrl: './admin-imagen-producto.html',
  styleUrl: './admin-imagen-producto.css'
})
export class AdminImagenProducto {
  private imagenService = inject(ImagenService);

  productoId = input.required<number>();
  imagenUrlActual = input<string | null>(null);

  imagenCambio = output<string | null>();

  subiendo = signal(false);
  error = signal<string | null>(null);
  previewLocal = signal<string | null>(null);

  onArchivoSeleccionado(event: Event): void {
    const input = event.target as HTMLInputElement;
    const archivo = input.files?.[0];
    if (!archivo) return;

    this.error.set(null);

    const lector = new FileReader();
    lector.onload = () => this.previewLocal.set(lector.result as string);
    lector.readAsDataURL(archivo);

    this.subiendo.set(true);

    this.imagenService.subir(this.productoId(), archivo).subscribe({
      next: (respuesta) => {
        this.subiendo.set(false);
        this.previewLocal.set(null);
        this.imagenCambio.emit(respuesta.imagenUrl);
      },
      error: (err) => {
        this.subiendo.set(false);
        this.previewLocal.set(null);
        this.error.set(err.error?.mensaje ?? 'No se pudo subir la imagen');
      },
      complete: () => {
        input.value = '';
      }
    });
  }

  eliminarImagen(): void {
    const confirmado = confirm('¿Eliminar la imagen de este producto?');
    if (!confirmado) return;

    this.subiendo.set(true);

    this.imagenService.eliminar(this.productoId()).subscribe({
      next: () => {
        this.subiendo.set(false);
        this.imagenCambio.emit(null);
      },
      error: () => this.subiendo.set(false)
    });
  }
}
