export type AppRole = "SOLICITANTE" | "ANALISTA" | "EVALUADOR" | "ADMIN";

export interface AccessTokenResponse {
  tokenType?: string | null;
  accessToken: string | null;
  expiresIn: number;
  refreshToken: string | null;
}

export interface UserDto {
  id: string | null;
  userName: string;
  email: string;
  role: string;
  usCedula?: string | null;
  nombre?: string | null;
  apellido?: string | null;
  estado?: boolean | null;
}

export interface CreateUserDto {
  userName: string;
  email: string;
  role: string;
  usCedula?: string | null;
  nombre?: string | null;
  apellido?: string | null;
  password: string;
}

export interface MuestraDto {
  mstCodigo: string | null;
  tpmstId: number;
  nombre: string | null;
  origen: string | null;
  condicionesAlmacenamiento?: string | null;
  condicionesTransporte?: string | null;
  estadoActual: number;
  fechaRecepcion: string;
  fechaSalidaEstimada?: string | null;
}

export interface CreateMuestraDto {
  tpmstId: number;
  nombre?: string | null;
  origen?: string | null;
  condicionesAlmacenamiento?: string | null;
  condicionesTransporte?: string | null;
}

export interface PruebaDto {
  idPrueba: number;
  idMuestra: string | null;
  tipoPruebaId?: number | null;
  nombrePrueba: string;
}

export interface CreatePruebaDto {
  idMuestra: string | null;
  tipoPruebaId: number;
  nombrePrueba: string;
}

export interface TipoPruebaDto {
  idTipoPrueba: number;
  codigo: string;
  nombre: string;
}

export interface ResultadoPruebaDto {
  idResultado: number;
  idPrueba: number;
  idParametro: number;
  idMuestra: string | null;
  valorObtenido?: number | null;
  unidad?: string | null;
  cumpleNorma?: boolean | null;
  fechaRegistro: string;
  validadoPor?: string | null;
  estadoValidacion?: string | null;
}

export interface CreateResultadoPruebaDto {
  idMuestra: string | null;
  idPrueba: number;
  idParametro: number;
  valorObtenido?: number | null;
  unidad?: string | null;
}

export interface ParametroDto {
  idParametro: number;
  tpmstId?: number | null;
  tipoPruebaId?: number | null;
  nombreParametro?: string | null;
  valorMin?: number | null;
  valorMax?: number | null;
  unidad?: string | null;
}

export interface CreateParametroDto {
  tpmstId?: number | null;
  tipoPruebaId?: number | null;
  nombreParametro?: string | null;
  valorMin?: number | null;
  valorMax?: number | null;
  unidad?: string | null;
}

export interface AsignarAnalistaEnMuestraDto {
  mstCodigo?: string | null;
  idAnalista?: string | null;
  observaciones: string;
}

export interface AsignarEstadoMuestraDto {
  mstCodigo?: string | null;
  estadoMuestra?: number;
  observaciones: string;
}

export interface EvaluarPruebaDto {
  idPrueba?: number;
  aprobado: boolean;
  observaciones?: string | null;
}

export interface GenerarPreliminarRequest {
  observaciones?: string | null;
}

export interface CambiarEstadoDocumentoDto {
  idDocumento?: number;
  idEstadoDocumento?: number;
  observaciones?: string | null;
}
