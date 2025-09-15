import api from "@/api/client";
import type { AccessTokenResponse, UserDto } from "@/frontend/src/types/api";

export interface LoginRequest {
  email: string;
  password: string;
  twoFactorCode?: string | null;
  twoFactorRecoveryCode?: string | null;
}

export async function login(req: LoginRequest): Promise<AccessTokenResponse> {
  const { data } = await api.post<AccessTokenResponse>("/login", req);
  return data;
}

export async function refresh(refreshToken: string): Promise<AccessTokenResponse> {
  const { data } = await api.post<AccessTokenResponse>("/refresh", { refreshToken });
  return data;
}

export async function me(): Promise<UserDto> {
  const { data } = await api.get<UserDto>("/api/usuarios/me");
  return data;
}

export async function logout(): Promise<void> {
  try {
    await api.post("/logout", {});
  } catch {}
}