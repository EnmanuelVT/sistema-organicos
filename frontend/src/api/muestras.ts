// src/api/muestras.ts
import api from "@/api/apiClient";
import type {
  MuestraDto,
  CreateMuestraDto,
  AsignarAnalistaEnMuestraDto,
  AsignarEstadoMuestraDto,
} from "@/types/api";

/** GET /api/muestras */
export async function getAll() {
  const { data } = await api.get<MuestraDto[]>("/api/muestras");
  return data;
}

/** POST /api/muestras */
export async function create(dto: CreateMuestraDto) {
  const { data } = await api.post("/api/muestras", dto);
  return data;
}

/** GET /api/muestras/me */
export async function getMine() {
  const { data } = await api.get<MuestraDto[]>("/api/muestras/me");
  return data;
}

/** GET /api/muestras/usuario/{id} */
export async function getByUser(userId: string) {
  const { data } = await api.get<MuestraDto[]>(`/api/muestras/usuario/${userId}`);
  return data;
}

/** GET /api/muestras/analista/me */
export async function getAssignedToMe() {
  const { data } = await api.get<MuestraDto[]>("/api/muestras/analista/me");
  return data;
}

/** GET /api/muestras/analista/{id} */
export async function getByAnalyst(analystId: string) {
  const { data } = await api.get<MuestraDto[]>(`/api/muestras/analista/${analystId}`);
  return data;
}

/** GET /api/muestras/{id} */
export async function getById(id: string) {
  const { data } = await api.get<MuestraDto>(`/api/muestras/${id}`);
  return data;
}

/** PUT /api/muestras/{id} */
export async function update(id: string, dto: CreateMuestraDto) {
  const { data } = await api.put(`/api/muestras/${id}`, dto);
  return data;
}

/** PATCH /api/muestras/asignar-analista */
export async function assignAnalyst(payload: AsignarAnalistaEnMuestraDto) {
  const { data } = await api.patch("/api/muestras/asignar-analista", payload);
  return data;
}

/** PATCH /api/muestras/cambiar-estado */
export async function changeStatus(payload: AsignarEstadoMuestraDto) {
  const { data } = await api.patch("/api/muestras/cambiar-estado", payload);
  return data;
}

/* ====================== ALIASES DE COMPATIBILIDAD ====================== */
export const getAllMuestras = getAll;
export const getMuestras = getAll;
export const listMuestras = getAll;
export const obtenerMuestras = getAll;

export const crearMuestra = create;
export const nuevaMuestra = create;
export const createMuestra = create;               // <— faltaba

export const getMisMuestras = getMine;
export const misMuestras = getMine;
export const getMyMuestras = getMine;              // <— faltaba

export const getMuestrasAsignadas = getAssignedToMe;
export const muestrasAsignadas = getAssignedToMe;
export const getMyAssignedMuestras = getAssignedToMe; // <— por si lo usas

export const getMuestrasPorUsuario = getByUser;
export const getMuestrasPorAnalista = getByAnalyst;

export const getMuestraById = getById;
export const obtenerMuestra = getById;

export const updateMuestra = update;
export const actualizarMuestra = update;
export const editarMuestra = update;

export const assignAnalystToMuestra = assignAnalyst;
export const asignarAnalista = assignAnalyst;
export const asignarAnalistaEnMuestra = assignAnalyst;

export const cambiarEstadoMuestra = changeStatus;
export const cambiarEstado = changeStatus;
