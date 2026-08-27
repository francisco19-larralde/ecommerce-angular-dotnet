export interface FiltroCatalogo {
  pagina: number;
  tamanioPagina: number;
  categoriaId: number | null;
  precioMin: number | null;
  precioMax: number | null;
  talle: string | null;
  busqueda: string | null;
  ordenarPor: string;
}

export const FILTRO_INICIAL: FiltroCatalogo = {
  pagina: 1,
  tamanioPagina: 12,
  categoriaId: null,
  precioMin: null,
  precioMax: null,
  talle: null,
  busqueda: null,
  ordenarPor: 'recientes'
};
