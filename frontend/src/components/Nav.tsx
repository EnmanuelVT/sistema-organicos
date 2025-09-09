import { Link, useNavigate } from "react-router-dom";
import { useAuthStore } from "../store/auth";

export default function Nav() {
  const { user, logout } = useAuthStore();
  const nav = useNavigate();

  if (!user) return null;

  return (
    <header className="border-b bg-white">
      <div className="mx-auto max-w-6xl px-4 py-3 flex items-center gap-4">
        <Link to="/" className="font-semibold">Sistema de Trazabilidad</Link>
        <nav className="flex gap-3 text-sm">
          <Link to="/muestras">Muestras</Link>
          {(user.role === "SOLICITANTE" || user.role === "ADMIN") && (
            <Link to="/muestras/nueva">Nueva muestra</Link>
          )}
          {user.role === "ADMIN" && (
            <Link to="/admin/asignaciones">Asignaciones</Link>
          )}
          {user.role === "ANALISTA" && (
            <Link to="/analista/mis-muestras">Mis muestras</Link>
          )}
          {user.role === "EVALUADOR" && (
            <Link to="/evaluador/bandeja">Bandeja evaluación</Link>
          )}
        </nav>
        <div className="ml-auto text-sm flex items-center gap-3">
          <span className="opacity-70">{user.nombre || user.userName} ({user.role})</span>
          <button
            onClick={() => { logout(); nav("/login"); }}
            className="border px-3 py-1 rounded"
          >
            Salir
          </button>
        </div>
      </div>
    </header>
  );
}
