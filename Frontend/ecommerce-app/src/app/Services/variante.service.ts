import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Variante } from '../Models/producto.model';

export interface CrearVarianteRequest {
  talle: string;
  stock: number;
  orden: number;
}

@Injectable({
  providedIn: 'root'
})
export class VarianteService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/productos`;

  obtenerPorProducto(productoId: number): Observable<Variante[]> {
    return this.http.get<Variante[]>(`${this.apiUrl}/${productoId}/variantes`);
  }

  crear(productoId: number, datos: CrearVarianteRequest): Observable<Variante> {
    return this.http.post<Variante>(`${this.apiUrl}/${productoId}/variantes`, datos);
  }

  actualizarStock(varianteId: number, stock: number): Observable<Variante> {
    return this.http.put<Variante>(`${this.apiUrl}/variantes/${varianteId}/stock`, { stock });
  }

  eliminar(varianteId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/variantes/${varianteId}`);
  }
}
