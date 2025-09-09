// src/api/apiClient.ts
import axios from 'axios';

const baseURL = import.meta.env.VITE_API_BASE_URL || 'https://localhost:7232';

const api = axios.create({ baseURL });

api.interceptors.request.use((config) => {
  const key = import.meta.env.VITE_TOKEN_STORAGE_KEY || 'token';
  const token = localStorage.getItem(key);
  if (token) config.headers.Authorization = `Bearer ${token}`;
  return config;
});

api.interceptors.response.use(
  (res) => res,
  (err) => {
    if (err.response?.status === 401) {
      localStorage.removeItem(import.meta.env.VITE_TOKEN_STORAGE_KEY || 'token');
      window.location.href = '/login';
    }
    return Promise.reject(err);
  }
);

export default api;
