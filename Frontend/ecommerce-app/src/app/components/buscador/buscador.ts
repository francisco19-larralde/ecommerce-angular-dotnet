import { Component, inject, signal, OnDestroy, ElementRef, HostListener } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { Subject, debounceTime, distinctUntilChanged, switchMap, of } from 'rxjs';
import { Producto } from '../../Models/producto.model';
import { ProductoService } from '../../Services/producto.service';
import { DecimalPipe } from '@angular/common';

@Component({
  selector: 'app-buscador',
  imports: [FormsModule, DecimalPipe],
  templateUrl: './buscador.html',
  styleUrl: './buscador.css'
})
export class Buscador implements OnDestroy {
  private productoService = inject(ProductoService);
  private router = inject(Router);
  private elementRef = inject(ElementRef);

  termino = signal('');
  resultados = signal<Producto[]>([]);
  buscando = signal(false);
  mostrarDropdown = signal(false);


  private terminoSubject = new Subject<string>();

  constructor() {
    this.terminoSubject
      .pipe(
        debounceTime(350),
        distinctUntilChanged(),
        switchMap((termino) => {
          if (termino.trim().length < 2) {
            return of([]);
          }
          this.buscando.set(true);
          return this.productoService.buscar(termino);
        })
      )
      .subscribe({
        next: (resultados) => {
          this.resultados.set(resultados);
          this.buscando.set(false);
          this.mostrarDropdown.set(true);
        },
        error: () => this.buscando.set(false)
      });
  }

  ngOnDestroy(): void {
    this.terminoSubject.complete();
  }

  onInput(valor: string): void {
    this.termino.set(valor);
    this.terminoSubject.next(valor);
  }

  irADetalle(producto: Producto): void {
    this.mostrarDropdown.set(false);
    this.termino.set('');
    this.router.navigate(['/productos', producto.id]);
  }

  onSubmit(): void {
    this.mostrarDropdown.set(false);
  }


  @HostListener('document:click', ['$event'])
  onClickFuera(event: MouseEvent): void {
    if (!this.elementRef.nativeElement.contains(event.target)) {
      this.mostrarDropdown.set(false);
    }
  }
}
