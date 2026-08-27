import {
  Component, ElementRef, input, output, viewChild,
  signal, afterNextRender, HostListener
} from '@angular/core';
import { Producto } from '../../Models/producto.model';
import { ProductoCard } from '../producto-card/producto-card';

@Component({
  selector: 'app-carrusel-productos',
  imports: [ProductoCard],
  templateUrl: './carrusel-productos.html',
  styleUrl: './carrusel-productos.css'
})
export class CarruselProductos {
  titulo = input.required<string>();
  productos = input.required<Producto[]>();
  agregandoId = input<number | null>(null);

  agregarClick = output<Producto>();

  private contenedor = viewChild.required<ElementRef<HTMLDivElement>>('scrollContainer');


  necesitaScroll = signal(false);
  puedeIrIzquierda = signal(false);
  puedeIrDerecha = signal(false);

  constructor() {

    afterNextRender(() => {
      this.recalcularEstado();
      const observer = new ResizeObserver(() => this.recalcularEstado());
      observer.observe(this.contenedor().nativeElement);
    });
  }

  @HostListener('window:resize')
  onResize(): void {
    this.recalcularEstado();
  }

  onScroll(): void {
    this.recalcularEstado();
  }

  private recalcularEstado(): void {
    const el = this.contenedor().nativeElement;
    const desborda = el.scrollWidth > el.clientWidth + 4;

    this.necesitaScroll.set(desborda);
    this.puedeIrIzquierda.set(desborda && el.scrollLeft > 4);
    this.puedeIrDerecha.set(desborda && el.scrollLeft < el.scrollWidth - el.clientWidth - 4);
  }

  onAgregarClick(producto: Producto): void {
    this.agregarClick.emit(producto);
  }

  scrollIzquierda(): void {
    this.contenedor().nativeElement.scrollBy({ left: -this.getScrollAmount(), behavior: 'smooth' });
  }

  scrollDerecha(): void {
    this.contenedor().nativeElement.scrollBy({ left: this.getScrollAmount(), behavior: 'smooth' });
  }

  private getScrollAmount(): number {
    const container = this.contenedor().nativeElement;
    const card = container.querySelector(':scope > div') as HTMLElement | null;
    if (!card) return 272;

    const styles = getComputedStyle(container);
    const gap = parseFloat(styles.columnGap) || parseFloat(styles.gap) || 0;
    return card.offsetWidth + gap;
  }
}
