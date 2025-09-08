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
