import { ReactNode } from 'react'
import { Navigate, useLocation } from 'react-router-dom'
import { useAuthStore } from '../store/auth'
import type { Role } from '../store/auth'

export default function Protected({ children, roles }: { children: ReactNode, roles?: Role[] }) {
  const { isAuthenticated, user } = useAuthStore()
  const location = useLocation()
  
  if (!isAuthenticated) {
    return <Navigate to='/login' state={{ from: location }} replace />
  }
  
  if (roles && user && !roles.includes(user.role as Role)) {
    return (
      <div className='rounded border border-red-200 bg-red-50 p-4 text-sm text-red-700'>
        No tienes permisos para ver esta sección.
      </div>
    )
  }
  
  return <>{children}</>
}
