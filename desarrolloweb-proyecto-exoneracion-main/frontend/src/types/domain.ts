// Updated types to match real API schemas
export type Role = 'SOLICITANTE' | 'ANALISTA' | 'EVALUADOR' | 'Admin'

export interface UserDto {
  id: string
  userName: string
  email: string
  role: string
  usCedula?: string
  nombre?: string
  apellido?: string
}

export interface MuestraDto {
  mstCodigo: string
  tpmstId: number
  nombre: string
  origen: string
  condicionesAlmacenamiento?: string
  condicionesTransporte?: string
  estadoActual: number
  fechaRecepcion: string
  fechaSalidaEstimada?: string
}

export interface CreateMuestraDto {
  mstCodigo: string
  tpmstId: number
  nombre: string
  origen: string
  condicionesAlmacenamiento?: string
  condicionesTransporte?: string
  estadoActual: number
}

export interface ParametroDto {
  idParametro: number
  tpmstId?: number
  nombreParametro?: string
  valorMin?: number
  valorMax?: number
  unidad?: string
}

export interface PruebaDto {
  idPrueba: number
  idMuestra: string
  nombrePrueba?: string
  tipoMuestraAsociada: number
}

export interface CreatePruebaDto {
  idMuestra: string
  nombrePrueba?: string
  tipoMuestraAsociada: number
}

export interface ResultadoPruebaDto {
  idResultado: number
  idPrueba: number
  idParametro: number
  idMuestra: string
  valorObtenido?: number
  unidad?: string
  cumpleNorma?: boolean
  fechaRegistro: string
  validadoPor?: string
  estadoValidacion?: string
}

export interface CreateResultadoPruebaDto {
  idMuestra: string
  idPrueba: number
  idParametro: number
  valorObtenido?: number
  unidad?: string
}

export interface AsignarAnalistaEnMuestraDto {
  mstCodigo?: string
  idAnalista?: string
  observaciones?: string
}

export interface AsignarEstadoMuestraDto {
  mstCodigo?: string
  estadoMuestra?: number
  observaciones?: string
}

export interface EvaluarMuestraDto {
  muestraId?: string
  aprobado: boolean
  observaciones?: string
}

export interface ValidarResultadoDto {
  idResultado?: number
  accion?: string
  observaciones?: string
}

// Legacy types for backward compatibility
export type TipoMuestra = 'ALIMENTO' | 'AGUA' | 'BEBIDA_ALCOHOLICA'

export interface Solicitante {
  nombre: string
  razonSocial?: string
  direccion: string
  contacto: string
}

export interface TestRegistro {
  nombre: string
  unidad?: string
  valor: number
}

export interface Muestra {
  id: string
  codigo: string
  tipo: TipoMuestra
  fechaRecepcion: string
  origen: string
  condiciones: string
  solicitante: Solicitante
  solicitanteId?: string
  responsableTecnicoId: string
  estado: 'RECIBIDA'|'EN_ANALISIS'|'EVALUADA'|'CERTIFICADA'
  analistaId?: string
  evaluadorId?: string
  observaciones?: string
  tests: { nombre: string, unidad?: string, valor: number }[]
}

export interface Documento {
  id: string
  muestraId: string
  tipo: 'CERTIFICADO'|'INFORME'
  version: number
  url: string
  creadoPor: string
  creadoEn: string
}
