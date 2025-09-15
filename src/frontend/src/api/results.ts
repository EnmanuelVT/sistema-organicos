import api from "@/api/client";
import type { ResultadoPruebaDto, CreateResultadoPruebaDto } from "@/frontend/src/types/api";

export async function getResultsBySample(sampleId: string): Promise<ResultadoPruebaDto[]> {
  const { data } = await api.get<ResultadoPruebaDto[]>(`/api/resultados/muestra/${sampleId}`);
  return data;
}

export async function getResultById(resultId: number): Promise<ResultadoPruebaDto> {
  const { data } = await api.get<ResultadoPruebaDto>(`/api/resultados/${resultId}`);
  return data;
}

export async function createResult(result: CreateResultadoPruebaDto): Promise<ResultadoPruebaDto> {
  const { data } = await api.post<ResultadoPruebaDto>("/api/resultados", result);
  return data;
}