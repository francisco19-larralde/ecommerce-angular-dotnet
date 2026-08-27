export interface OrdenItem {
  productoId: number;
  productoNombre: string;
  talle: string | null;
  precioUnitario: number;
  cantidad: number;
  subtotal: number;
}

export interface Orden {
  id: number;
  fecha: string;
  estado: string;
  subtotal: number;
  descuento: number;
  total: number;
  cuponCodigo: string | null;
  ultimosDigitosTarjeta: string | null;
  items: OrdenItem[];
}
