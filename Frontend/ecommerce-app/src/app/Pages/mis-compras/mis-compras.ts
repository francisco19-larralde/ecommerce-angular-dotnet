import { Component, inject, signal, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Orden } from '../../Models/orden.model';
import { OrdenService } from '../../Services/orden.service';
import { DatePipe, DecimalPipe } from '@angular/common';

@Component({
  selector: 'app-mis-compras',
  imports: [DecimalPipe, DatePipe],
  templateUrl: './mis-compras.html',
  styleUrl: './mis-compras.css'
})
export class MisCompras implements OnInit {
  private ordenService = inject(OrdenService);
  private route = inject(ActivatedRoute);

  compras = signal<Orden[]>([]);
  cargando = signal(true);
  ordenAbiertaId = signal<number | null>(null);

  ngOnInit(): void {
    this.ordenService.obtenerMisCompras().subscribe({
      next: (data) => {
        this.compras.set(data);
        this.cargando.set(false);

        const idParam = this.route.snapshot.paramMap.get('id');
        if (idParam) {
          this.ordenAbiertaId.set(Number(idParam));
        }
      },
      error: () => this.cargando.set(false)
    });
  }

  toggleOrden(ordenId: number): void {
    this.ordenAbiertaId.set(this.ordenAbiertaId() === ordenId ? null : ordenId);
  }
}
