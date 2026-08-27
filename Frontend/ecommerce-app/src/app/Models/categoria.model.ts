export interface Categoria {
  id: number;
  nombre: string;
}

export interface CategoriaHome {
  id: number;
  nombre: string;
}

export interface CategoriaAdmin {
  id: number;
  nombre: string;
  mostrarEnHome: boolean;
  orden: number;
  cantidadProductos: number;
}
