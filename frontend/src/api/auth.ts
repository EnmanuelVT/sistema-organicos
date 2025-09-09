// src/api/auth.ts
import api from './apiClient';
import { UserDto } from '../types/domain';

export type Role = 'Admin' | 'ANALISTA' | 'EVALUADOR' | 'SOLICITANTE';

interface LoginResponse {
  tokenType: string;
  accessToken: string;
  expiresIn: number;
  refreshToken: string;
}

export async function login(email: string, password: string) {
  const { data } = await api.post('/login', { email, password });
  return data as LoginResponse;
}

export async function register(email: string, password: string) {
  const { data } = await api.post('/register', { email, password });
  return data;
}

export async function getCurrentUser() {
  const { data } = await api.get('/api/usuarios/me');
  return data as UserDto;
}

export async function refreshToken(refreshToken: string) {
  const { data } = await api.post('/refresh', { refreshToken });
  return data as LoginResponse;
}

export async function forgotPassword(email: string) {
  const { data } = await api.post('/forgotPassword', { email });
  return data;
}

export async function resetPassword(email: string, resetCode: string, newPassword: string) {
  const { data } = await api.post('/resetPassword', { email, resetCode, newPassword });
  return data;
}

// Legacy compatibility
export async function me() {
  return getCurrentUser();
}
