import api from "@/api/client";
import type { UserDto, CreateUserDto } from "@/frontend/src/types/api";

export async function getAllUsers(): Promise<UserDto[]> {
  const { data } = await api.get<UserDto[]>("/api/admin/users");
  return data;
}

export async function createUser(user: CreateUserDto): Promise<UserDto> {
  const { data } = await api.post<UserDto>("/api/admin/users", user);
  return data;
}

export async function updateUser(id: string, user: UserDto): Promise<void> {
  await api.put(`/api/admin/users/${id}`, user);
}

export async function deleteUser(id: string): Promise<void> {
  await api.delete(`/api/admin/users/${id}`);
}