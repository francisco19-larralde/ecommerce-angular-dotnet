export interface Paginacion<T> {
  items: T[];
  paginaActual: number;
  totalPaginas: number;
  totalRegistros: number;
}
