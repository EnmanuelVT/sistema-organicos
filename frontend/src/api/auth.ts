// src/api/auth.ts
import api from "@/api/apiClient";
import type { AccessTokenResponse, UserDto } from "@/types/api";

export type LoginRequest = {
  email: string;
  password: string;
  twoFactorCode?: string | null;
  twoFactorRecoveryCode?: string | null;
};

const LOGIN_URL   = "/login";
const REFRESH_URL = "/refresh";
const ME_URL      = "/api/usuarios/me";
const LOGOUT_URL  = "/logout"; // si no existe en tu back, no pasa nada: lo ignoramos en try/catch

/** POST /login -> { accessToken, refreshToken, expiresIn, tokenType? } */
export async function login(req: LoginRequest) {
  const { data } = await api.post<AccessTokenResponse>(LOGIN_URL, req);
  return data;
}

/** POST /refresh -> { accessToken, refreshToken, expiresIn } */
export async function refresh(refreshToken: string) {
  const { data } = await api.post<AccessTokenResponse>(REFRESH_URL, { refreshToken });
  return data;
}

/** GET /api/usuarios/me -> UserDto */
export async function me() {
  const { data } = await api.get<UserDto>(ME_URL);
  return data;
}

/** POST /logout (opcional) */
export async function logout() {
  try { await api.post(LOGOUT_URL, {}); } catch {}
}
