import { create } from "zustand";
import * as authApi from "@/api/auth";
import { normalizeRole, type AppRole } from "@/utils/roles";

export interface User {
  id: string;
  email: string;
  userName: string;
  role: AppRole;
  nombre?: string;
  apellido?: string;
}

interface AuthState {
  user: User | null;
  token: string | null;
  loading: boolean;
  login: (email: string, password: string) => Promise<void>;
  logout: () => Promise<void>;
  fetchMe: () => Promise<void>;
}

let refreshTimer: number | null = null;

function scheduleRefresh(expiresIn?: number) {
  if (refreshTimer) window.clearTimeout(refreshTimer);
  if (!expiresIn || expiresIn <= 0) return;
  
  const ms = Math.max(5_000, (expiresIn - 60) * 1000);
  refreshTimer = window.setTimeout(async () => {
    try {
      const refreshToken = localStorage.getItem("auth.refresh");
      if (!refreshToken) throw new Error("No refresh token");
      
      const response = await authApi.refresh(refreshToken);
      if (response?.accessToken) {
        localStorage.setItem("auth.token", response.accessToken);
      }
      if (response?.refreshToken) {
        localStorage.setItem("auth.refresh", response.refreshToken);
      }
      scheduleRefresh(response?.expiresIn);
    } catch {
      localStorage.removeItem("auth.token");
      localStorage.removeItem("auth.refresh");
      localStorage.removeItem("auth.user");
      window.location.href = "/login";
    }
  }, ms);
}

export const useAuthStore = create<AuthState>((set) => ({
  user: JSON.parse(localStorage.getItem("auth.user") || "null"),
  token: localStorage.getItem("auth.token"),
  loading: false,

  login: async (email, password) => {
    set({ loading: true });
    try {
      const response = await authApi.login({ email, password });

      if (response?.accessToken) {
        localStorage.setItem("auth.token", response.accessToken);
      }
      if (response?.refreshToken) {
        localStorage.setItem("auth.refresh", response.refreshToken);
      }
      scheduleRefresh(response?.expiresIn);

      const me = await authApi.me();
      const user: User = {
        id: me.id || "",
        email: me.email,
        userName: me.userName,
        role: normalizeRole(me.role),
        nombre: me.nombre ?? undefined,
        apellido: me.apellido ?? undefined,
      };
      localStorage.setItem("auth.user", JSON.stringify(user));

      set({ user, token: response?.accessToken || null, loading: false });
    } catch (e) {
      set({ loading: false });
      throw e;
    }
  },

  fetchMe: async () => {
    const me = await authApi.me();
    const user: User = {
      id: me.id || "",
      email: me.email,
      userName: me.userName,
      role: normalizeRole(me.role),
      nombre: me.nombre ?? undefined,
      apellido: me.apellido ?? undefined,
    };
    localStorage.setItem("auth.user", JSON.stringify(user));
    set({ user });
  },

  logout: async () => {
    try {
      await authApi.logout();
    } catch {}
    
    localStorage.removeItem("auth.token");
    localStorage.removeItem("auth.refresh");
    localStorage.removeItem("auth.user");
    if (refreshTimer) window.clearTimeout(refreshTimer);
    set({ user: null, token: null });
  },
}));
