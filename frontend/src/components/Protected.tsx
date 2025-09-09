// src/components/ProtectedRoute.tsx (example)
import { Navigate } from 'react-router-dom'
import { useAuthStore } from '../store/auth'
import { hasAnyRole, AppRole } from '../utils/roles'

export default function ProtectedRoute({
  roles,
  children,
}: {
  roles?: AppRole[]
  children: React.ReactNode
}) {
  const { user } = useAuthStore()

  // Not logged in
  if (!user) return <Navigate to="/login" replace />

  if (roles && !hasAnyRole(user.role, roles)) {
    return <div className="p-6 text-center text-sm text-red-600">
      No tienes permisos para ver esta sección.
    </div>
  }

  return <>{children}</>
}
