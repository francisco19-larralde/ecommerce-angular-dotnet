import { Component, input, viewChild, ElementRef, afterNextRender, effect } from '@angular/core';
import { Chart, ChartConfiguration, registerables } from 'chart.js';

Chart.register(...registerables);

@Component({
  selector: 'app-chart-canvas',
  imports: [],
  template: `<canvas #canvas></canvas>`,
})
export class ChartCanvas {
  config = input.required<ChartConfiguration>();

  private canvasRef = viewChild.required<ElementRef<HTMLCanvasElement>>('canvas');
  private chart?: Chart;

  constructor() {
    afterNextRender(() => this.crearChart());

    effect(() => {
      const nuevaConfig = this.config();
      if (this.chart) {
        this.chart.destroy();
        this.crearChart();
      }
    });
  }

  private crearChart(): void {
    const ctx = this.canvasRef().nativeElement.getContext('2d');
    if (!ctx) return;
    this.chart = new Chart(ctx, this.config());
  }
}
