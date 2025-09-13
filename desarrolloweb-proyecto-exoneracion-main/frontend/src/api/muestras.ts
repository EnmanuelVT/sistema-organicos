// src/api/apiMuestras.ts  (o muestras.ts)
import api from './apiClient';
import type { MuestraDto, CreateMuestraDto, AsignarAnalistaEnMuestraDto, AsignarEstadoMuestraDto, EvaluarMuestraDto } from '../types/domain';

// Get all samples (admin/analyst access)
export async function getAllMuestras() {
  const { data } = await api.get('/api/muestras');
  return data as MuestraDto[];
}

// Get current user's samples  
export async function getMyMuestras() {
  const { data } = await api.get('/api/muestras/me');
  return data as MuestraDto[];
}

// Get samples by user ID (analyst access)
export async function getMuestrasByUser(userId: string) {
  const { data } = await api.get(`/api/muestras/usuario/${userId}`);
  return data as MuestraDto[];
}

// Get samples assigned to current analyst
export async function getMyAssignedMuestras() {
  const { data } = await api.get('/api/muestras/analista/me');
  return data as MuestraDto[];
}

// Get samples assigned to specific analyst (admin access)
export async function getMuestrasByAnalyst(analystId: string) {
  const { data } = await api.get(`/api/muestras/analista/${analystId}`);
  return data as MuestraDto[];
}

// Get sample by ID
export async function getMuestraById(id: string) {
  const { data } = await api.get(`/api/muestras/${id}`);
  return data as MuestraDto;
}

// Create new sample
export async function createMuestra(payload: CreateMuestraDto) {
  const { data } = await api.post('/api/muestras', payload);
  return data;
}

// Update existing sample (analyst access)
export async function updateMuestra(id: string, payload: CreateMuestraDto) {
  const { data } = await api.put(`/api/muestras/${id}`, payload);
  return data;
}

// Assign analyst to sample
export async function assignAnalystToMuestra(payload: AsignarAnalistaEnMuestraDto) {
  const { data } = await api.patch('/api/muestras/asignar-analista', payload);
  return data;
}

// Change sample status
export async function changeMuestraStatus(payload: AsignarEstadoMuestraDto) {
  const { data } = await api.patch('/api/muestras/cambiar-estado', payload);
  return data;
}

// Evaluate sample (evaluator access)
export async function evaluateMuestra(id: string, payload: EvaluarMuestraDto) {
  const { data } = await api.post(`/api/muestras/${id}/evaluar`, payload);
  return data;
}

// Get sample audit trail (admin access)
export async function getMuestraAudit(id: string) {
  const { data } = await api.get(`/api/muestras/Auditoria${id}`);
  return data;
}

// Legacy compatibility functions
export async function listMuestras() {
  return getAllMuestras();
}

export async function getMuestra(id: string) {
  return getMuestraById(id);
}

export async function listMuestrasByAnalista(analistaId: string) {
  return getMuestrasByAnalyst(analistaId);
}

export async function listMuestrasByEvaluador(evaluadorId: string) {
  // This would need to be implemented based on business logic
  // For now, return samples assigned to evaluator
  return getMyAssignedMuestras();
}

export async function listMuestrasBySolicitante(solicitanteId: string) {
  return getMuestrasByUser(solicitanteId);
}
