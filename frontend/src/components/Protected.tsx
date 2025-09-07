// src/components/Protected.tsx
import { Navigate, useLocation } from "react-router-dom";
import { ReactNode } from "react";
import { useAuth } from "@/store/auth";
import type { Role } from "@/api/auth";

type Props = {
  children: JSX.Element;
  roles?: Role[]; // si se omite, cualquier usuario logueado puede entrar
};

/**
 * Componente de protección de rutas:
 * - Si no hay sesión -> /login
 * - Si hay sesión pero rol no permitido -> /
 * - Si está hidratando la sesión -> loader
 */
export default function Protected({ children, roles }: Props) {
  const { user, loading } = useAuth();
  const location = useLocation();

  if (loading) {
    return (
      <div className="min-h-screen grid place-items-center">
        <div className="animate-pulse text-sm opacity-70">Cargando sesión…</div>
      </div>
    );
  }

  if (!user) {
    return <Navigate to="/login" replace state={{ from: location }} />;
  }

  if (roles && !roles.includes(user.role)) {
    return <Navigate to="/" replace />;
  }

  return children;
}

/** Extra: Evita que un usuario logueado visite /login */
export function RequireNoAuth({ children }: { children: ReactNode }) {
  const { user, loading } = useAuth();
  if (loading) return null;
  if (user) return <Navigate to="/" replace />;
  return <>{children}</>;
}
