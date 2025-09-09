import { create } from 'zustand'
import { UserDto } from '../types/domain'
import { login as apiLogin, getCurrentUser as apiGetCurrentUser } from '../api/auth'
import { normalizeRole } from '../utils/roles'

export type Role = 'SOLICITANTE' | 'ANALISTA' | 'EVALUADOR' | 'ADMIN'

type User = {
  id: string
  email: string
  role: string // keep original for debugging
  roleNorm?: ReturnType<typeof normalizeRole> // normalized
}

interface AuthState {
  token: string | null
  user: User,
  isAuthenticated: boolean
  login: (email: string, password: string) => Promise<void>
  logout: () => void
  getCurrentUser: () => Promise<void>
  setUser: (u: User) => void
}

export const useAuthStore = create<AuthState>((set, get) => ({
  token: localStorage.getItem('token'),
  user: null,
  isAuthenticated: !!localStorage.getItem('token'),

  setUser: (u) => set({ user: { ...u, roleNorm: normalizeRole(u.role) } }),

  login: async (email: string, password: string) => {
    try {
      const response = await apiLogin(email, password)
      const token = response.accessToken

      localStorage.setItem('token', token)
      if (response.refreshToken) {
        localStorage.setItem('refreshToken', response.refreshToken)
      }

      set({ token, isAuthenticated: true })

      // Get user info
      await get().getCurrentUser()
    } catch (error) {
      throw error
    }
  },

  getCurrentUser: async () => {
    try {
      const token = get().token
      if (!token) return

      const user = await apiGetCurrentUser()
      set({ user })
    } catch (error) {
      console.error('Failed to get current user:', error)
      // If getting user fails, logout
      get().logout()
    }
  },

  logout: () => {
    localStorage.removeItem('token')
    localStorage.removeItem('refreshToken')
    set({ token: null, user: null, isAuthenticated: false })
  },
}))

// Initialize user on app start
const token = localStorage.getItem('token')
if (token) {
  useAuthStore.getState().getCurrentUser()
}

