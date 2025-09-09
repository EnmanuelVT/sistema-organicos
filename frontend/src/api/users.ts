import api from './apiClient';
import { UserDto } from '../types/domain';

interface CreateUserDto {
  userName: string;
  email: string;
  role: string;
  usCedula?: string;
  nombre?: string;
  apellido?: string;
  password?: string;
}

// Get all users (admin access)
export async function getAllUsers() {
  const { data } = await api.get('/api/admin/users');
  return data as UserDto[];
}

// Create a new user (admin access)
export async function createUser(payload: CreateUserDto) {
  const { data } = await api.post('/api/admin/users', payload);
  return data as UserDto;
}

// Update an existing user (admin access)
export async function updateUser(payload: UserDto) {
  const { data } = await api.put(`/api/admin/users/${payload.id}`, payload);
  return data;
}

// Delete a user (admin access)
export async function deleteUser(id: string) {
  const { data } = await api.delete(`/api/admin/users/${id}`);
  return data;
}

// Get current user info
export async function getCurrentUser() {
  const { data } = await api.get('/api/usuarios/me');
  return data as UserDto;
}
