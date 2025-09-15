import api from "@/api/client";
import type { ParametroDto, CreateParametroDto } from "@/types/api";

export async function getParametersBySampleType(sampleTypeId: number): Promise<ParametroDto[]> {
  const { data } = await api.get<ParametroDto[]>(`/api/parametro/tipo-muestra/${sampleTypeId}`);
  return data;
}

export async function addParameterToSampleType(parameter: CreateParametroDto): Promise<ParametroDto> {
  const { data } = await api.post<ParametroDto>("/api/parametro/tipo-muestra", parameter);
  return data;
}