// src/store/auth.ts
import { create } from "zustand";
import * as authApi from "@/api/auth";
import { normalizeRole, type AppRole } from "@/utils/roles";

export type User = {
  id: string;
  email: string;
  userName: string;
  role: AppRole; // ADMIN | ANALISTA | EVALUADOR | SOLICITANTE
};

type AuthState = {
  user: User | null;
  token: string | null;           // JWT accessToken
  loading: boolean;
  login: (email: string, password: string) => Promise<void>;
  fetchMe: () => Promise<void>;
  logout: () => Promise<void>;
};

let refreshTimer: number | null = null;

// Programa un refresh ~60s antes de expirar
function scheduleRefresh(expiresIn?: number) {
  if (refreshTimer) window.clearTimeout(refreshTimer);
  if (!expiresIn || expiresIn <= 0) return;
  const ms = Math.max(5_000, (expiresIn - 60) * 1000);
  refreshTimer = window.setTimeout(async () => {
    try {
      const rt = localStorage.getItem("auth.refresh");
      if (!rt) throw new Error("No refresh token");
      const r = await authApi.refresh(rt); // POST /refresh { refreshToken }
      if (r?.accessToken) localStorage.setItem("auth.token", r.accessToken);
      if (r?.refreshToken) localStorage.setItem("auth.refresh", r.refreshToken);
      scheduleRefresh(r?.expiresIn);
      // Opcional: revalidar perfil si lo necesitas
      // await useAuthStore.getState().fetchMe();
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
      // 1) Login: { tokenType?, accessToken, refreshToken, expiresIn }
      const r = await authApi.login({ email, password });

      if (r?.accessToken) localStorage.setItem("auth.token", r.accessToken);
      if (r?.refreshToken) localStorage.setItem("auth.refresh", r.refreshToken);
      scheduleRefresh(r?.expiresIn);

      // 2) Perfil para conocer el rol
      const me = await authApi.me();
      const user: User = {
        id: me.id || "",
        email: me.email,
        userName: me.userName,
        role: normalizeRole(me.role), // <- si viene nulo => SOLICITANTE
      };
      localStorage.setItem("auth.user", JSON.stringify(user));

      set({ user, token: r?.accessToken || null, loading: false });
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
    };
    localStorage.setItem("auth.user", JSON.stringify(user));
    set({ user });
  },

  logout: async () => {
    try { await authApi.logout(); } catch {}
    localStorage.removeItem("auth.token");
    localStorage.removeItem("auth.refresh");
    localStorage.removeItem("auth.user");
    if (refreshTimer) window.clearTimeout(refreshTimer);
    set({ user: null, token: null });
  },
}));
