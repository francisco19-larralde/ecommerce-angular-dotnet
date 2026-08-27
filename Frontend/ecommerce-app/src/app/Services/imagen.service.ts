import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ImagenService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/productos`;

  subir(productoId: number, archivo: File): Observable<{ imagenUrl: string }> {
    const formData = new FormData();
    formData.append('archivo', archivo);
    return this.http.post<{ imagenUrl: string }>(`${this.apiUrl}/${productoId}/imagen`, formData);
  }

  eliminar(productoId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${productoId}/imagen`);
  }
}
