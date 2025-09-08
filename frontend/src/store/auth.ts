import { create } from 'zustand'

export type Role = 'SOLICITANTE' | 'ANALISTA' | 'EVALUADOR' | 'ADMIN'

interface AuthState {
  token: string | null
  role: Role
  userId: string | null
  login: (role: Role, userId: string) => void
  logout: () => void
}

export const useAuthStore = create<AuthState>((set) => ({
  token: localStorage.getItem('token'),
  role: (localStorage.getItem('role') as Role) || 'SOLICITANTE',
  userId: localStorage.getItem('userId') || null,
  login: (role: Role, userId: string) => {
    localStorage.setItem('token', 'demo-token')
    localStorage.setItem('role', role)
    localStorage.setItem('userId', userId)
    set({ token: 'demo-token', role, userId })
  },
  logout: () => {
    localStorage.removeItem('token')
    localStorage.removeItem('role')
    localStorage.removeItem('userId')
    set({ token: null, role: 'SOLICITANTE', userId: null })
  },
}))
