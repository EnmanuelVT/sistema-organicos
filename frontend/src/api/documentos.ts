// src/api/documentos.ts
import api from "@/api/apiClient";
import type { CambiarEstadoDocumentoDto } from "@/types/api";

export async function formularioAguaRegistroGet() {
  const { data } = await api.get("/api/documentos/formulario-agua-registro");
  return data;
}
export async function formularioAguaRegistroPost(body: any) {
  const { data } = await api.post("/api/documentos/formulario-agua-registro", body);
  return data;
}
export async function cambiarEstado(dto: CambiarEstadoDocumentoDto) {
  const { data } = await api.patch("/api/documentos/cambiar-estado", dto);
  return data;
}
