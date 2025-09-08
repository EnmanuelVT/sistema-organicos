// src/api/auth.ts
import api from './apiClient';

export type Role = 'ADMINISTRADOR' | 'ANALISTA' | 'EVALUADOR' | 'SOLICITANTE';
export interface User { id: string; name: string; role: Role; email?: string; }

type LoginResponse =
  | { token: string; user: User }
  | { token: string; userId: string; name: string; role: Role; email?: string };

export async function login(identifier: string, password: string) {
  const payload = { email: identifier, userName: identifier, password };
  const { data } = await api.post('/login', payload);   // <- EXACTO como en Swagger
  return data as LoginResponse;
}

export async function me() {
  const { data } = await api.get('/me'); // ajusta si tu API usa otro path (p. ej. /auth/me)
  return data as User;
}
