import api from './apiClient';
import { ParametroDto, PruebaDto, CreatePruebaDto, ResultadoPruebaDto, CreateResultadoPruebaDto, ValidarResultadoDto } from '../types/domain';

// Get parameters by sample type (analyst access)
export async function getParametrosByTipoMuestra(tipoMuestraId: number) {
  const { data } = await api.get(`/api/parametro/tipo-muestra/${tipoMuestraId}`);
  return data as ParametroDto[];
}

// Add parameter to sample type
export async function addParametroToTipoMuestra(payload: {
  tpmstId?: number;
  nombreParametro?: string;
  valorMin?: number;
  valorMax?: number;
  unidad?: string;
}) {
  const { data } = await api.post('/api/parametro/tipo-muestra', payload);
  return data;
}

// Get tests by sample ID (analyst access)
export async function getPruebasByMuestra(idMuestra: string) {
  const { data } = await api.get(`/api/pruebas/muestra/${idMuestra}`);
  return data as PruebaDto[];
}

// Create a new test (analyst access)
export async function createPrueba(payload: CreatePruebaDto) {
  const { data } = await api.post('/api/pruebas', payload);
  return data;
}

// Get test results by sample ID (analyst access)
export async function getResultadosByMuestra(idMuestra: string) {
  const { data } = await api.get(`/api/resultados/muestra/${idMuestra}`);
  return data as ResultadoPruebaDto[];
}

// Get test result by ID (analyst access)
export async function getResultadoById(idResultado: number) {
  const { data } = await api.get(`/api/resultados/${idResultado}`);
  return data as ResultadoPruebaDto;
}

// Register a new test result (analyst access)
export async function createResultado(payload: CreateResultadoPruebaDto) {
  const { data } = await api.post('/api/resultados', payload);
  return data;
}

// Validate a test result (evaluator access)
export async function validateResultado(payload: ValidarResultadoDto) {
  const { data } = await api.patch('/api/resultados/validar', payload);
  return data;
}

// Legacy compatibility functions
export interface RegistrarEnsayoInput {
  nombrePrueba: string;
  tipoMuestraAsociada: number;
  parametros: Array<{ idParametro: number; valorObtenido: number }>;
}

export async function registrarEnsayo(mstCodigo: string, input: RegistrarEnsayoInput) {
  // Create the test first
  const prueba = await createPrueba({
    idMuestra: mstCodigo,
    nombrePrueba: input.nombrePrueba,
    tipoMuestraAsociada: input.tipoMuestraAsociada
  });

  // Then create results for each parameter
  const resultados = [];
  for (const param of input.parametros) {
    const resultado = await createResultado({
      idMuestra: mstCodigo,
      idPrueba: prueba.idPrueba,
      idParametro: param.idParametro,
      valorObtenido: param.valorObtenido
    });
    resultados.push(resultado);
  }

  return { idPrueba: prueba.idPrueba, resultados };
}

export async function validarEnsayo(idResultado: number, estado: 'APROBADO' | 'RECHAZADO') {
  return validateResultado({
    idResultado,
    accion: estado
  });
}

export async function obtenerParametrosNorma(tpmstId: number) {
  return getParametrosByTipoMuestra(tpmstId);
}