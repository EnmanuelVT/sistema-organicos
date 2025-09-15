import api from "@/api/client";

export interface HistorialDto {
  idBitacora: number;
  idMuestra: string;
  idAnalista: string;
  estado: number;
  fechaCambio: string;
  observaciones?: string;
}

export async function getAllTrazabilidad(): Promise<HistorialDto[]> {
  const { data } = await api.get<HistorialDto[]>("/api/trazabilidad");
  return data;
}

export async function getTrazabilidadByUserId(userId: string): Promise<HistorialDto[]> {
  const { data } = await api.get<HistorialDto[]>(`/api/trazabilidad/usuario/${userId}`);
  return data;
}

export async function getMyTrazabilidad(): Promise<HistorialDto[]> {
  const { data } = await api.get<HistorialDto[]>("/api/trazabilidad/me");
  return data;
}