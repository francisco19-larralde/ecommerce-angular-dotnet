export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegistroRequest {
  nombre: string;
  apellido: string;
  email: string;
  password: string;
}

export interface AuthResponse {
  token: string;
  expiracion: string;
  nombre: string;
  email: string;
  roles: string[];
}
