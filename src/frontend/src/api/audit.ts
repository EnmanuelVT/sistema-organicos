import api from "@/api/client";

export interface AuditoriaDto {
  idAuditoria: string;
  idUsuario: string;
  accion: string;
  fechaAcción: string;
  descripcion: string;
}

export async function getAllAuditorias(): Promise<AuditoriaDto[]> {
  const { data } = await api.get<AuditoriaDto[]>("/api/auditoria");
  return data;
}

export async function getAuditoriaByUserId(userId: string): Promise<AuditoriaDto[]> {
  const { data } = await api.get<AuditoriaDto[]>(`/api/auditoria/usuario/${userId}`);
  return data;
}

export async function getMyAuditoria(): Promise<AuditoriaDto[]> {
  const { data } = await api.get<AuditoriaDto[]>("/api/auditoria/me");
  return data;
}