import api from './apiClient';
import { ParametroNorma, ResultadoPrueba } from '../types/domain';

export interface RegistrarEnsayoInput {
  nombrePrueba: string;         // requerido por Prueba
  tipoMuestraAsociada: number;  // byte
  parametros: Array<{ idParametro: number; valorObtenido: number }>;
}

export async function registrarEnsayo(mstCodigo: string, input: RegistrarEnsayoInput) {
  // Back podría crear Prueba y Resultados de una vez
  const { data } = await api.post(`/api/muestras/${mstCodigo}/ensayos`, input);
  return data as { idPrueba: number; resultados: ResultadoPrueba[] };
}

export async function validarEnsayo(idResultado: number, estado: 'APROBADO' | 'RECHAZADO') {
  const { data } = await api.put(`/api/resultados/${idResultado}/validar`, { estado });
  return data as ResultadoPrueba;
}
export async function obtenerParametrosNorma(tpmstId: number) {
  const { data } = await api.get('/api/parametros-norma', { params: { tpmstId } });
  return data as ParametroNorma[];
}