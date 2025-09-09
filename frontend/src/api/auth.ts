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

// Use the same base URL as apiClient; you can override specifically for auth if needed.
const AUTH_BASE =
  import.meta.env.VITE_AUTH_BASE_URL ||
  import.meta.env.VITE_API_BASE_URL ||
  'http://localhost:5062';

// Storage key must match apiClient
const TOKEN_KEY = import.meta.env.VITE_TOKEN_STORAGE_KEY || 'accessToken';
const REFRESH_KEY = import.meta.env.VITE_REFRESH_TOKEN_STORAGE_KEY || 'refreshToken';

export async function login(email: string, password: string) {
  const res = await fetch(`${AUTH_BASE}/login`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json', accept: 'application/json' },
    body: JSON.stringify({ email, password }),
  });
  if (!res.ok) throw new Error(`Login failed: ${res.status}`);
  const data = (await res.json()) as LoginResponse;

  // Persist using the SAME key apiClient reads
  localStorage.setItem(TOKEN_KEY, data.accessToken);
  localStorage.setItem(REFRESH_KEY, data.refreshToken);

  return data;
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
