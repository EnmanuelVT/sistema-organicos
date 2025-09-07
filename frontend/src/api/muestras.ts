// src/api/apiMuestras.ts  (o muestras.ts)
import api from './apiClient';
import type { Muestra, MuestraDetalle } from '@/types/domain';

export async function listarMuestras(params?: { q?: string; estado?: number | '' }) {
  const { data } = await api.get('/muestras', { params });
  const items: Muestra[] = Array.isArray(data) ? data : data.items;
  const total = Array.isArray(data) ? items.length : data.total ?? items.length;
  return { items, total };
}

export async function detalleMuestra(mstCodigo: string) {
  const { data } = await api.get(`/muestras/${mstCodigo}`);
  return data as MuestraDetalle;
}

export async function crearMuestra(payload: Partial<Muestra> & {
  mstCodigo: string;
  tpmstId: number;
  nombre: string;
  origen: string;
  fechaRecepcion: string;
  idUsuarioSolicitante: string;
  estadoActual: number;
}) {
  const { data } = await api.post('/muestras', payload);
  return data;
}
