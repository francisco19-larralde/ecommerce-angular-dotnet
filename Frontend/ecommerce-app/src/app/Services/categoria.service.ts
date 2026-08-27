import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Categoria, CategoriaHome, CategoriaAdmin } from '../Models/categoria.model';

export interface CategoriaRequest {
  nombre: string;
  mostrarEnHome: boolean;
  orden: number;
}

@Injectable({
  providedIn: 'root'
})
export class CategoriaService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/categorias`;

  obtenerTodas(): Observable<Categoria[]> {
    return this.http.get<Categoria[]>(this.apiUrl);
  }

  obtenerParaHome(): Observable<CategoriaHome[]> {
    return this.http.get<CategoriaHome[]>(`${this.apiUrl}/home`);
  }

  obtenerParaAdmin(): Observable<CategoriaAdmin[]> {
    return this.http.get<CategoriaAdmin[]>(`${this.apiUrl}/admin`);
  }

  crear(datos: CategoriaRequest): Observable<CategoriaAdmin> {
    return this.http.post<CategoriaAdmin>(this.apiUrl, datos);
  }

  eliminar(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  actualizar(id: number, datos: CategoriaRequest): Observable<CategoriaAdmin> {
    return this.http.put<CategoriaAdmin>(`${this.apiUrl}/${id}`, datos);
  }
}
