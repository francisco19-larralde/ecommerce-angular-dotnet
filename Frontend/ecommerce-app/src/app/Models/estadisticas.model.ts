export interface ResumenVentas {
  ingresosTotales: number;
  cantidadOrdenes: number;
  ticketPromedio: number;
  productosVendidos: number;
}

export interface VentaPorDia {
  fecha: string;
  total: number;
  cantidadOrdenes: number;
}

export interface ProductoMasVendido {
  nombre: string;
  cantidadVendida: number;
  ingresosGenerados: number;
}
