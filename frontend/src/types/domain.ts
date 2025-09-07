export type Rol = 'ADMINISTRADOR' | 'ANALISTA' | 'EVALUADOR' | 'SOLICITANTE';

export interface Usuario {
  id: string;              // IdentityUser.Id
  email?: string;
  nombre?: string;         // Nombre
  apellido?: string;       // Apellido
  razonSocial?: string;
  direccion?: string;
  telefono?: string;
  contacto?: string;
  role: Rol;               // viene del token/endpoint /auth/me
}

export type EstadoMuestraCod = number; // byte en DB
export type TipoMuestraCod = number;   // byte en DB

export interface Muestra {
  mstCodigo: string;               // MstCodigo
  tpmstId: TipoMuestraCod;         // Tipo de muestra (byte)
  nombre?: string;
  fechaRecepcion: string;          // ISO
  origen: string;
  fechaSalidaEstimada?: string;
  condicionesAlmacenamiento?: string;
  condicionesTransporte?: string;
  idUsuarioSolicitante: string;    // FK a Usuario
  idAnalista?: string;             // FK a Usuario
  estadoActual: EstadoMuestraCod;  // FK a EstadoMuestra
}

export interface BitacoraMuestra {
  idBitacora: number;
  idMuestra: string;
  idAnalista: string;
  fechaAsignacion: string; // ISO
  observaciones?: string;
}

export interface ParametroNorma {
  idParametro: number;
  tpmstId?: TipoMuestraCod;
  nombreParametro: string;
  valorMin?: number;
  valorMax?: number;
  unidad?: string;
}

export interface Prueba {
  idPrueba: number;
  idMuestra: string;
  nombrePrueba: string;
  tipoMuestraAsociada: TipoMuestraCod;
}

export interface ResultadoPrueba {
  idResultado: number;
  idPrueba: number;
  idMuestra: string;
  idParametro: number;
  valorObtenido?: number;
  cumpleNorma?: boolean;
  fechaRegistro: string;   // ISO
  validadoPor?: string;    // userId
  estadoValidacion?: string;
}

export interface Documento {
  idDocumento: number;
  idMuestra: string;
  idTipoDoc: number;        // byte
  idEstadoDocumento: number;// int
  version: number;
  rutaArchivo?: string;
  fechaCreacion: string;
  // docPdf? → no lo tipamos si no lo pedimos
}

// Para vistas enriquecidas que devuelve tu API
export interface MuestraDetalle extends Muestra {
  bitacora?: BitacoraMuestra[];
  pruebas?: Prueba[];
  resultados?: ResultadoPrueba[];
  documentos?: Documento[];
}
