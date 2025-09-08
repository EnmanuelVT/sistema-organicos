import { ReactNode } from 'react'
import { Navigate, useLocation } from 'react-router-dom'
import { useAuthStore } from '../store/auth'
import type { Role } from '../store/auth'

export default function Protected({ children, roles }: { children: ReactNode, roles?: Role[] }) {
  const { token, role } = useAuthStore()
  const location = useLocation()
  if (!token) return <Navigate to='/login' state={{ from: location }} replace />
  if (roles && !roles.includes(role)) return <div className='rounded border border-red-200 bg-red-50 p-4 text-sm text-red-700'>No tienes permisos para ver esta sección.</div>
  return <>{children}</>
}
