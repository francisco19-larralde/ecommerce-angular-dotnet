import { Injectable, inject, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { environment } from '../../environments/environment';
import { LoginRequest, RegistroRequest, AuthResponse } from '../Models/auth.model';
import { CarritoService } from './carrito.service';
import { TokenStorageService } from './tokenStorage.service';


@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private http = inject(HttpClient);
  private carritoService = inject(CarritoService);
  private tokenStorage = inject(TokenStorageService);
  private apiUrl = `${environment.apiUrl}/auth`;

  private usuarioActual = signal<AuthResponse | null>(this.tokenStorage.obtenerUsuarioGuardado());

  usuario = this.usuarioActual.asReadonly();
  estaLogueado = computed(() => this.usuarioActual() !== null);
  esAdmin = computed(() => this.usuarioActual()?.roles.includes('Admin') ?? false);

  constructor() {
    if (this.estaLogueado()) {
      this.carritoService.cargarCarrito();
    }
  }

  login(datos: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/login`, datos).pipe(
      tap((respuesta) => this.guardarSesion(respuesta))
    );
  }

  registro(datos: RegistroRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/registro`, datos).pipe(
      tap((respuesta) => this.guardarSesion(respuesta))
    );
  }

  logout(): void {
    this.tokenStorage.limpiar();
    this.usuarioActual.set(null);
  }

  private guardarSesion(respuesta: AuthResponse): void {
    this.tokenStorage.guardar(respuesta);
    this.usuarioActual.set(respuesta);
    this.carritoService.cargarCarrito();
  }
}
