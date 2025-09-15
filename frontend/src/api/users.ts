// src/api/users.ts
import api from "@/api/apiClient";
import type { UserDto, CreateUserDto } from "@/types/api";
// Si tus tipos viven en otro archivo, cambia el import a '../types/domain'.

/** GET /api/admin/users */
export async function getAllUsers(): Promise<UserDto[]> {
  const { data } = await api.get<UserDto[]>("/api/admin/users");
  return data;
}

/** POST /api/admin/users
 * Swagger: 201 -> UserDto, 200 -> {} (algunos backends)
 * Devolvemos UserDto | {} para tolerar ambas respuestas.
 */
export async function createUser(payload: CreateUserDto): Promise<UserDto | {}> {
  const { data } = await api.post<UserDto | {}>("/api/admin/users", payload);
  return data;
}

/** PUT /api/admin/users/{id} */
export async function updateUser(id: string, payload: UserDto): Promise<{} | void> {
  const { data } = await api.put<{}>(`/api/admin/users/${id}`, payload);
  return data;
}

/** DELETE /api/admin/users/{id} */
export async function deleteUser(id: string): Promise<{} | void> {
  const { data } = await api.delete<{}>(`/api/admin/users/${id}`);
  return data;
}

/** GET /api/usuarios/me (perfil del autenticado) */
export async function getCurrentUser(): Promise<UserDto> {
  const { data } = await api.get<UserDto>("/api/usuarios/me");
  return data;
}

/* Compat si usabas nombres alternativos */
export const getUsers = getAllUsers;
export const listUsers = getAllUsers;
