// src/api/apiClient.ts
import axios from "axios";

const baseURL = (import.meta.env.VITE_API_BASE_URL || "").replace(/\/+$/, "");
const useCookies =
  String(import.meta.env.VITE_USE_COOKIES).toLowerCase() === "true";

export const api = axios.create({
  baseURL,
  withCredentials: useCookies,
  headers: { "Content-Type": "application/json", Accept: "application/json" },
  timeout: 20000,
});

api.interceptors.request.use((config) => {
  if (!useCookies) {
    const token = localStorage.getItem("auth.token");
    if (token) config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

api.interceptors.response.use(
  (res) => res,
  (err) => {
    const status = err?.response?.status;
    if (status === 401 || status === 403) {
      localStorage.removeItem("auth.token");
      localStorage.removeItem("auth.user");
      if (typeof window !== "undefined") window.location.href = "/login";
    }
    return Promise.reject(err);
  }
);

// Export default para imports existentes
export default api;
