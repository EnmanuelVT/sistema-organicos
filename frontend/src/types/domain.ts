// src/types/domain.ts

// Roles que usa el front; incluimos ADMIN y Admin por compatibilidad con normalización
export type Role = 'SOLICITANTE' | 'ANALISTA' | 'EVALUADOR' | 'ADMIN' | 'Admin';

// ===== Usuario =====
export interface UserDto {
  id: string;
  userName?: string;
  email: string;
  role: string; // el backend puede devolver variantes; el front lo normaliza
  usCedula?: string;
  nombre?: string;
  apellido?: string;
}

// ===== Muestras =====
export interface MuestraDto {
  // Importante: usar el mismo shape en TODO el archivo (mismo tipo, no opcional)
  mstCodigo: string;
  tpmstId: number;
  nombre: string;
  origen: string;
  condicionesAlmacenamiento?: string;
  condicionesTransporte?: string;
  fechaRecepcion?: string; // ISO string
  estado?: string | boolean; // por si el backend expone string o bool
  solicitanteId?: string;
  analistaId?: string;
  evaluadorId?: string;
  estadoActual?: string;

}

export interface CreateMuestraDto {
  // Mantener idéntico modificador que en MuestraDto si coincide por nombre
  mstCodigo: string;
  tpmstId: number;
  nombre: string;
  origen: string;
  condicionesAlmacenamiento?: string;
  condicionesTransporte?: string;
  fechaRecepcion?: string; // ISO string
}

// Asignaciones y cambios de estado
export interface AsignarAnalistaEnMuestraDto {
  mstCodigo: string;
  analistaId: string;
}

export interface AsignarEvaluadorEnMuestraDto {
  mstCodigo: string;
  evaluadorId: string;
}

export interface AsignarEstadoMuestraDto {
  mstCodigo: string;
  estado: string; // 'Pendiente' | 'EnProceso' | 'Aprobada' | 'Rechazada' (ajusta si tienes enum)
}

// Alias de compatibilidad: si el front importa CambiarEstadoMuestraDto, que funcione igual
export type CambiarEstadoMuestraDto = AsignarEstadoMuestraDto;

export interface EvaluarMuestraDto {
  mstCodigo: string;
  aprobado: boolean;
  observaciones?: string;
}

// ===== Parámetros / Ensayos =====
export interface ParametroDto {
  idParametro: number;
  nombreParametro: string;
  valorMin?: number;
  valorMax?: number;
  unidad?: string;
  tpmstId?: number; // id del tipo de muestra al que pertenece, si aplica
}

// Estructura con la que se envían parámetros en CreatePruebaDto
export interface PruebaParametroInput {
  idParametro: number;
  valorObtenido: number;
}

export interface CreatePruebaDto {
  mstCodigo: string;
  nombrePrueba: string;
  tipoMuestraAsociada: number;
  parametros: PruebaParametroInput[];
}

// Respuesta típica de creación/consulta de prueba (mínimo necesario para el front)
export interface PruebaDto {
  id?: number;
  mstCodigo: string;
  nombrePrueba: string;
  tipoMuestraAsociada: number;
  resultados?: Array<{
    idParametro: number;
    valorObtenido: number;
    unidad?: string;
  }>;
  creadoEn?: string; // ISO
}

// Si lo usas en alguna parte, lo dejamos minimalista
export interface ResultadoPruebaDto {
  mstCodigo: string;
  nombrePrueba: string;
  resultados: Array<{
    idParametro: number;
    valorObtenido: number;
    unidad?: string;
  }>;
}

// ===== Documentos =====
export interface Documento {
  id?: string;
  muestraId?: string;
  mstCodigo?: string;  // por si tu backend identifica por código
  tipo: 'CERTIFICADO' | 'INFORME';
  version: number;
  url?: string;
  creadoPor?: string;
  creadoEn?: string; // ISO
}
