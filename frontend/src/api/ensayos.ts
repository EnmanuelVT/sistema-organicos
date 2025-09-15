// src/api/ensayos.ts
import api from "@/api/apiClient";
import type { CreatePruebaDto, PruebaDto, EvaluarPruebaDto, GenerarPreliminarRequest } from "@/types/api";

/** GET /api/pruebas/muestra/{idMuestra} */
export async function getByMuestra(idMuestra: string) {
  const { data } = await api.get<PruebaDto[]>(`/api/pruebas/muestra/${idMuestra}`);
  return data;
}

/** POST /api/pruebas */
export async function create(dto: CreatePruebaDto) {
  const { data } = await api.post("/api/pruebas", dto);
  return data;
}

/** POST /api/muestras/pruebas/{id}/evaluar */
export async function evaluar(idPrueba: number, dto: EvaluarPruebaDto) {
  const { data } = await api.post(`/api/muestras/pruebas/${idPrueba}/evaluar`, dto);
  return data;
}

/** POST /api/muestras/pruebas/{id}/documentos/preliminar */
export async function generarPreliminar(idPrueba: number, dto: GenerarPreliminarRequest) {
  const { data } = await api.post(`/api/muestras/pruebas/${idPrueba}/documentos/preliminar`, dto);
  return data;
}

/* ===== Aliases de compatibilidad ===== */
export const registrarPrueba = create; // <— faltaba

/* Re-export para imports viejos (mal ubicados) */
export { getByTipoMuestra as getParametrosByTipoMuestra } from "./parametros"; // <— para el import en analista-registrar-prueba
