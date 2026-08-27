export interface Variante {
  id: number;
  talle: string;
  stock: number;
  orden: number;
}

export interface Producto {
  id: number;
  nombre: string;
  descripcion: string | null;
  precio: number;
  stock: number;
  imagenUrl: string | null;
  destacado: boolean;
  activo: boolean;
  tieneVariantes: boolean;
  variantes: Variante[];
  categoriaId: number;
  categoriaNombre: string | null;
}
