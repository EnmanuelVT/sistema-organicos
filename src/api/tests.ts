import api from "@/api/client";
import type { PruebaDto, CreatePruebaDto, EvaluarPruebaDto } from "@/types/api";

export async function getTestsBySample(sampleId: string): Promise<PruebaDto[]> {
  const { data } = await api.get<PruebaDto[]>(`/api/pruebas/muestra/${sampleId}`);
  return data;
}

export async function createTest(test: CreatePruebaDto): Promise<PruebaDto> {
  const { data } = await api.post<PruebaDto>("/api/pruebas", test);
  return data;
}

export async function evaluateTest(testId: number, evaluation: EvaluarPruebaDto): Promise<any> {
  const { data } = await api.post(`/api/muestras/pruebas/${testId}/evaluar`, evaluation);
  return data;
}

export async function generatePreliminaryDocument(testId: number, observaciones?: string): Promise<any> {
  const { data } = await api.post(`/api/muestras/pruebas/${testId}/documentos/preliminar`, {
    observaciones,
  });
  return data;
}