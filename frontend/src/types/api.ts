// src/types/api.ts
export type AppRole = "Solicitante" | "Analista" | "Evaluador" | "Administrador";

export type AccessTokenResponse = {
  tokenType?: string | null;
  accessToken: string | null;
  expiresIn: number; // seconds (int64)
  refreshToken: string | null;
};

export type UserDto = {
  id: string | null;
  userName: string;
  email: string;
  role: string; // lo normalizaremos a AppRole
  usCedula?: string | null;
  nombre?: string | null;
  apellido?: string | null;
  estado?: boolean | null;
};

export type CreateUserDto = {
  userName: string;
  email: string;
  role: string;
  usCedula?: string | null;
  nombre?: string | null;
  apellido?: string | null;
  password: string | null;
};

export type MuestraDto = {
  mstCodigo: string | null;
  tpmstId: number;
  nombre: string | null;
  origen: string | null;
  condicionesAlmacenamiento?: string | null;
  condicionesTransporte?: string | null;
  estadoActual: number;
  fechaRecepcion: string; // ISO
  fechaSalidaEstimada?: string | null; // ISO
};

export type CreateMuestraDto = {
  tpmstId: number;
  nombre?: string | null;
  origen?: string | null;
  condicionesAlmacenamiento?: string | null;
  condicionesTransporte?: string | null;
};

export type CreatePruebaDto = {
  idMuestra: string | null;
  nombrePrueba: string | null;
};

export type PruebaDto = {
  idPrueba: number;
  idMuestra: string | null;
  nombrePrueba: string | null;
};

export type CreateResultadoPruebaDto = {
  idMuestra: string | null;
  idPrueba: number;
  idParametro: number;
  valorObtenido?: number | null;
  unidad?: string | null;
};

export type ResultadoPruebaDto = {
  idResultado: number;
  idPrueba: number;
  idParametro: number;
  idMuestra: string | null;
  valorObtenido?: number | null;
  unidad?: string | null;
  cumpleNorma?: boolean | null;
  fechaRegistro: string; // ISO
  validadoPor?: string | null;
  estadoValidacion?: string | null;
};

export type ParametroDto = {
  idParametro: number;
  tpmstId?: number | null;
  nombreParametro?: string | null;
  valorMin?: number | null;
  valorMax?: number | null;
  unidad?: string | null;
};

export type CreateParametroDto = {
  tpmstId?: number | null;
  nombreParametro?: string | null;
  valorMin?: number | null;
  valorMax?: number | null;
  unidad?: string | null;
};

export type AsignarAnalistaEnMuestraDto = {
  mstCodigo?: string | null;
  idAnalista?: string | null;
  observaciones: string;
};

export type AsignarEstadoMuestraDto = {
  mstCodigo?: string | null;
  estadoMuestra?: number;
  observaciones: string;
};

export type EvaluarPruebaDto = {
  idPrueba?: number;
  aprobado: boolean;
  observaciones?: string | null;
};

export type GenerarPreliminarRequest = {
  observaciones?: string | null;
};

export type CambiarEstadoDocumentoDto = {
  idDocumento?: number;
  idEstadoDocumento?: number;
  observaciones?: string | null;
};
