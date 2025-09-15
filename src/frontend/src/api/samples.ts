import api from "@/api/client";
import type {
  MuestraDto,
  CreateMuestraDto,
  AsignarAnalistaEnMuestraDto,
  AsignarEstadoMuestraDto,
} from "@/frontend/src/types/api";

export async function getAllSamples(): Promise<MuestraDto[]> {
  const { data } = await api.get<MuestraDto[]>("/api/muestras");
  return data;
}

export async function getMySamples(): Promise<MuestraDto[]> {
  const { data } = await api.get<MuestraDto[]>("/api/muestras/me");
  return data;
}

export async function getAssignedSamples(): Promise<MuestraDto[]> {
  const { data } = await api.get<MuestraDto[]>("/api/muestras/analista/me");
  return data;
}

export async function getSampleById(id: string): Promise<MuestraDto> {
  const { data } = await api.get<MuestraDto>(`/api/muestras/${id}`);
  return data;
}

export async function createSample(sample: CreateMuestraDto): Promise<MuestraDto> {
  const { data } = await api.post<MuestraDto>("/api/muestras", sample);
  return data;
}

export async function updateSample(id: string, sample: CreateMuestraDto): Promise<MuestraDto> {
  const { data } = await api.put<MuestraDto>(`/api/muestras/${id}`, sample);
  return data;
}

export async function assignAnalyst(payload: AsignarAnalistaEnMuestraDto): Promise<any> {
  const { data } = await api.patch("/api/muestras/asignar-analista", payload);
  return data;
}

export async function changeStatus(payload: AsignarEstadoMuestraDto): Promise<any> {
  const { data } = await api.patch("/api/muestras/cambiar-estado", payload);
  return data;
}

export async function getDocumentsBySample(sampleId: string): Promise<any[]> {
  const { data } = await api.get(`/api/muestras/${sampleId}/documentos`);
  return data;
}