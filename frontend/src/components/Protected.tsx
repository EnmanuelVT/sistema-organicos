// src/components/Protected.tsx
import { Navigate } from "react-router-dom";
import { useAuthStore } from "@/store/auth";

export default function Protected({ children }: { children: JSX.Element }) {
  const user = useAuthStore((s) => s.user);
  if (!user) return <Navigate to="/login" replace />;
  return children;
}
