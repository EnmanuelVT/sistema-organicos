import api from './apiClient';
import { Documento } from '../types/domain';

export async function generarCertificado(mstCodigo: string) {
  const { data } = await api.post(`/api/muestras/${mstCodigo}/certificado`, {});
  return data as { url?: string; documento?: Documento };
}
export async function obtenerDocumentos(mstCodigo: string) {
  const { data } = await api.get(`/api/muestras/${mstCodigo}/documentos`);
  return data as Documento[];
}