export interface CarritoItem {
  id: number;
  productoId: number;
  productoNombre: string;
  productoImagenUrl: string | null;
  precioUnitario: number;
  cantidad: number;
  subtotal: number;
  stockDisponible: number;
  varianteId: number | null;
  talle: string | null;
}

export interface Carrito {
  id: number;
  items: CarritoItem[];
  total: number;
}
