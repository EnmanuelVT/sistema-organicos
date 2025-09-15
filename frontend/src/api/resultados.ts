// src/api/resultados.ts
import api from "@/api/apiClient";
import type { CreateResultadoPruebaDto, ResultadoPruebaDto } from "@/types/api";

/** GET /api/resultados/muestra/{idMuestra} */
export async function getByMuestra(idMuestra: string) {
  const { data } = await api.get<ResultadoPruebaDto[]>(`/api/resultados/muestra/${idMuestra}`);
  return data;
}

/** GET /api/resultados/{idResultado} */
export async function getById(idResultado: number) {
  const { data } = await api.get<ResultadoPruebaDto>(`/api/resultados/${idResultado}`);
  return data;
}

/** POST /api/resultados */
export async function create(dto: CreateResultadoPruebaDto) {
  const { data } = await api.post("/api/resultados", dto);
  return data;
}
