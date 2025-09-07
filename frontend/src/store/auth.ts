// src/store/auth.ts
import { create } from "zustand";
import { login as apiLogin, me as apiMe } from "@/api/auth";
import type { User } from "@/api/auth";

interface AuthState {
  user: User | null;
  loading: boolean;
  error: string | null;
  login: (email: string, password: string) => Promise<void>;
  logout: () => void;
  hydrate: () => Promise<void>;
}

export const useAuth = create<AuthState>((set) => ({
  user: null,
  loading: false,
  error: null,

  async login(email, password) {
try {
  const resp: any = await apiLogin(email, password);
  const token = resp.token;
  localStorage.setItem(import.meta.env.VITE_TOKEN_STORAGE_KEY || "token", token);

  const normalizedUser = resp.user ?? {
    id: resp.userId,
    name: resp.name,
    role: resp.role,
    email: resp.email,
  };

  set({ user: normalizedUser });
} catch (e: any) {
  console.error('LOGIN ERROR', e?.response?.data ?? e);
  set({ error: e?.response?.data?.message ?? 'Credenciales inválidas' });
}

},

  logout() {
    localStorage.removeItem(import.meta.env.VITE_TOKEN_STORAGE_KEY || "token");
    set({ user: null });
  },

  async hydrate() {
    const token = localStorage.getItem(import.meta.env.VITE_TOKEN_STORAGE_KEY || "token");
    if (!token) return;
    try {
      const me = await apiMe();
      set({ user: me });
    } catch {
      localStorage.removeItem(import.meta.env.VITE_TOKEN_STORAGE_KEY || "token");
      set({ user: null });
    }
  },
}));

// 🔁 Alias para compatibilidad con código antiguo:
export { useAuth as useAuthStore };
