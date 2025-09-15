// src/components/Nav.tsx
import { Link, useNavigate } from "react-router-dom";
import { useAuthStore } from "@/store/auth";
import { normalizeRole } from "@/utils/roles";

export default function Nav() {
  const nav = useNavigate();
  const user = useAuthStore((s) => s.user);
  const logout = useAuthStore((s) => s.logout);
  const role = normalizeRole(user?.role);

  return (
    <div className="w-full bg-gray-900 text-white">
      <div className="max-w-6xl mx-auto px-4 py-3 flex items-center gap-4">
        <Link to="/" className="font-semibold">Sistema</Link>

        <div className="flex-1" />

        {/* Menú por rol */}
        {role === "ADMIN" && (
          <nav className="flex gap-3">
            <Link to="/admin/users" className="hover:underline">Usuarios</Link>
          </nav>
        )}

        {role === "ANALISTA" && (
          <nav className="flex gap-3">
            <Link to="/analista" className="hover:underline">Mis muestras</Link>
          </nav>
        )}

        {role === "EVALUADOR" && (
          <nav className="flex gap-3">
            <Link to="/evaluador" className="hover:underline">Bandeja</Link>
          </nav>
        )}

        {role === "SOLICITANTE" && (
          <nav className="flex gap-3">
            <Link to="/solicitante" className="hover:underline">Mis muestras</Link>
          </nav>
        )}

        {/* Usuario / Logout */}
        <div className="border-l pl-4 flex items-center gap-3">
          <span className="text-sm opacity-80">{user?.email} · {role}</span>
          <button
            className="bg-white/10 hover:bg-white/20 rounded px-3 py-1 text-sm"
            onClick={async () => { await logout(); nav("/login"); }}
          >
            Cerrar sesión
          </button>
        </div>
      </div>
    </div>
  );
}
