// src/routes/RoleRoute.tsx
import { Navigate } from "react-router-dom";
import { useAuthStore } from "@/store/auth";
import { normalizeRole, type AppRole } from "@/utils/roles";

export default function RoleRoute({
  role,
  children,
}: {
  role: AppRole;
  children: JSX.Element;
}) {
  const user = useAuthStore((s) => s.user);
  if (!user) return <Navigate to="/login" replace />;

  const myRole = normalizeRole(user.role);

  // Evita comparar literales directamente (causa ts2367)
  const allowed: AppRole[] = ['ADMIN', role];
  if (allowed.includes(myRole)) return children;

  const redirectByRole: Record<AppRole, string> = {
    ADMIN: '/admin',
    ANALISTA: '/analista',
    EVALUADOR: '/evaluador',
    SOLICITANTE: '/solicitante',
  };

  return <Navigate to={redirectByRole[myRole]} replace />;
}
