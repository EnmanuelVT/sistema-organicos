// src/api/apiClient.ts
import axios from 'axios';

const baseURL = import.meta.env.VITE_API_BASE_URL || 'https://localhost:7232';

// Read the SAME key used in auth.ts
const TOKEN_KEY = import.meta.env.VITE_TOKEN_STORAGE_KEY || 'accessToken';

const api = axios.create({ baseURL });

api.interceptors.request.use((config) => {
  const token = localStorage.getItem(TOKEN_KEY);
  if (token) config.headers.Authorization = `Bearer ${token}`;
  return config;
});

api.interceptors.response.use(
  (res) => res,
  async (err) => {
    if (err.response?.status === 401) {
      // Optional: try refresh here using REFRESH_KEY & /refresh
      localStorage.removeItem(TOKEN_KEY);
      // redirect user to login route
      window.location.href = '/login';
    }
    return Promise.reject(err);
  }
);

export default api;
