import { Component, inject, signal, computed, OnInit } from '@angular/core';
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { CarritoService } from '../../Services/carrito.service';
import { OrdenService } from '../../Services/orden.service';
import { DecimalPipe } from '@angular/common';

@Component({
  selector: 'app-checkout',
  imports: [ReactiveFormsModule, DecimalPipe, FormsModule],
  templateUrl: './checkout.html',
  styleUrl: './checkout.css'
})
export class Checkout implements OnInit {
  private fb = inject(FormBuilder);
  private carritoService = inject(CarritoService);
  private ordenService = inject(OrdenService);
  private router = inject(Router);

  carrito = this.carritoService.carrito;

  procesando = signal(false);
  error = signal<string | null>(null);

  cuponTexto = signal('');
  validandoCupon = signal(false);
  cuponAplicado = signal<{ codigo: string; porcentaje: number } | null>(null);
  errorCupon = signal<string | null>(null);

  descuentoCalculado = computed(() => {
    const carrito = this.carrito();
    const cupon = this.cuponAplicado();
    if (!carrito || !cupon) return 0;
    return Math.round((carrito.total * cupon.porcentaje) / 100);
  });

  totalConDescuento = computed(() => {
    const carrito = this.carrito();
    if (!carrito) return 0;
    return carrito.total - this.descuentoCalculado();
  });

  formulario = this.fb.group({
    numeroTarjeta: ['', [Validators.required, Validators.pattern(/^[\d\s]{13,19}$/)]],
    nombreTitular: ['', [Validators.required]],
    vencimiento: ['', [Validators.required, Validators.pattern(/^(0[1-9]|1[0-2])\/\d{2}$/)]],
    cvv: ['', [Validators.required, Validators.pattern(/^\d{3,4}$/)]]
  });

  ngOnInit(): void {
    if (!this.carrito()) {
      this.carritoService.cargarCarrito();
    }
  }

  validarCupon(): void {
    const codigo = this.cuponTexto().trim();
    if (!codigo) return;

    this.validandoCupon.set(true);
    this.errorCupon.set(null);

    this.ordenService.validarCupon(codigo).subscribe({
      next: (res) => {
        this.cuponAplicado.set({ codigo: codigo.toUpperCase(), porcentaje: res.porcentajeDescuento });
        this.validandoCupon.set(false);
      },
      error: (err) => {
        this.cuponAplicado.set(null);
        this.errorCupon.set(err.error?.mensaje ?? 'Cupón inválido');
        this.validandoCupon.set(false);
      }
    });
  }

  quitarCupon(): void {
    this.cuponAplicado.set(null);
    this.cuponTexto.set('');
    this.errorCupon.set(null);
  }

  onSubmit(): void {
    if (this.formulario.invalid) {
      this.formulario.markAllAsTouched();
      return;
    }

    this.procesando.set(true);
    this.error.set(null);

    const valores = this.formulario.getRawValue();

    this.ordenService
      .checkout({
        cuponCodigo: this.cuponAplicado()?.codigo ?? null,
        numeroTarjeta: valores.numeroTarjeta!,
        nombreTitular: valores.nombreTitular!,
        vencimiento: valores.vencimiento!,
        cvv: valores.cvv!
      })
      .subscribe({
        next: (orden) => {
          this.procesando.set(false);
          this.carritoService.cargarCarrito();
          this.router.navigate(['/mis-compras', orden.id]);
        },
        error: (err) => {
          this.procesando.set(false);
          this.error.set(err.error?.mensaje ?? 'No se pudo procesar el pago');
        }
      });
  }
}
