import { Injectable } from '@angular/core';
import { AuthResponse } from '../Models/auth.model';

const TOKEN_KEY = 'ecommerce_token';
const USER_KEY = 'ecommerce_user';

@Injectable({
  providedIn: 'root'
})
export class TokenStorageService {
  guardar(respuesta: AuthResponse): void {
    localStorage.setItem(TOKEN_KEY, respuesta.token);
    localStorage.setItem(USER_KEY, JSON.stringify(respuesta));
  }

  limpiar(): void {
    localStorage.removeItem(TOKEN_KEY);
    localStorage.removeItem(USER_KEY);
  }

  obtenerToken(): string | null {
    return localStorage.getItem(TOKEN_KEY);
  }

  obtenerUsuarioGuardado(): AuthResponse | null {
    const guardado = localStorage.getItem(USER_KEY);
    return guardado ? JSON.parse(guardado) : null;
  }
}
