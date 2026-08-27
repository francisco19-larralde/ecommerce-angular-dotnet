import { Component, inject, signal, computed, OnInit } from '@angular/core';
import { ChartConfiguration } from 'chart.js';
import { ResumenVentas, VentaPorDia, ProductoMasVendido } from '../../../Models/estadisticas.model';
import { EstadisticaService } from '../../../Services/estadistica.service';
import { ChartCanvas } from '../../../components/chart-canvas/chart-canvas';
import { DecimalPipe } from '@angular/common';

@Component({
  selector: 'app-dashboard-admin',
  imports: [ChartCanvas, DecimalPipe],
  templateUrl: './dashboard-admin.html',
  styleUrl: './dashboard-admin.css'
})
export class DashboardAdmin implements OnInit {
  private estadisticaService = inject(EstadisticaService);

  resumen = signal<ResumenVentas | null>(null);
  ventasPorDia = signal<VentaPorDia[]>([]);
  productosMasVendidos = signal<ProductoMasVendido[]>([]);
  cargando = signal(true);

  ngOnInit(): void {
    this.estadisticaService.obtenerResumen().subscribe({ next: (data) => this.resumen.set(data) });
    this.estadisticaService.obtenerVentasPorDia(30).subscribe({
      next: (data) => {
        this.ventasPorDia.set(data);
        this.cargando.set(false);
      },
      error: () => this.cargando.set(false)
    });
    this.estadisticaService.obtenerProductosMasVendidos(5).subscribe({
      next: (data) => this.productosMasVendidos.set(data)
    });
  }

  // Configuración del gráfico de línea (ventas por día), recalculada cada vez que llegan datos nuevos
  configVentasPorDia = computed<ChartConfiguration>(() => ({
    type: 'line',
    data: {
      labels: this.ventasPorDia().map((v) => v.fecha.slice(5)), // "MM-dd"
      datasets: [
        {
          label: 'Ventas ($)',
          data: this.ventasPorDia().map((v) => v.total),
          borderColor: '#6366f1',
          backgroundColor: '#6366f133',
          fill: true,
          tension: 0.3
        }
      ]
    },
    options: {
      responsive: true,
      plugins: { legend: { display: false } },
      scales: { y: { beginAtZero: true } }
    }
  }));

  // Configuración del gráfico de barras (top 5 productos)
  configProductosMasVendidos = computed<ChartConfiguration>(() => ({
    type: 'bar',
    data: {
      labels: this.productosMasVendidos().map((p) => p.nombre),
      datasets: [
        {
          label: 'Unidades vendidas',
          data: this.productosMasVendidos().map((p) => p.cantidadVendida),
          backgroundColor: '#6366f1'
        }
      ]
    },
    options: {
      indexAxis: 'y',
      responsive: true,
      plugins: { legend: { display: false } },
      scales: { x: { beginAtZero: true } }
    }
  }));
}
