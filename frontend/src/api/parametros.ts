// src/api/parametros.ts
import api from "@/api/apiClient";
import type { ParametroDto, CreateParametroDto } from "@/types/api";

/** GET /api/parametro/tipo-muestra/{tipoMuestraId} */
export async function getByTipoMuestra(tipoMuestraId: number) {
  const { data } = await api.get<ParametroDto[]>(`/api/parametro/tipo-muestra/${tipoMuestraId}`);
  return data;
}

/** POST /api/parametro/tipo-muestra */
export async function addToTipoMuestra(dto: CreateParametroDto) {
  const { data } = await api.post("/api/parametro/tipo-muestra", dto);
  return data;
}
