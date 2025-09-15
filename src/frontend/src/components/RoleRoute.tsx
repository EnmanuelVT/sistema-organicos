import { ReactNode } from "react";
import { Navigate } from "react-router-dom";
import { useAuthStore } from "@/store/auth";
import { hasRole, type AppRole } from "@/utils/roles";

interface RoleRouteProps {
  children: ReactNode;
  allowedRoles: AppRole[];
}

export default function RoleRoute({ children, allowedRoles }: RoleRouteProps) {
  const { user } = useAuthStore();

  if (!user) {
    return <Navigate to="/login" replace />;
  }

  if (!hasRole(user.role, allowedRoles)) {
    return <Navigate to="/dashboard" replace />;
  }

  return <>{children}</>;
}