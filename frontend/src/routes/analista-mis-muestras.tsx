// src/routes/analista-mis-muestras.tsx
import { useQuery } from "@tanstack/react-query";
import { useAuthStore } from "@/store/auth";
import { normalizeRole } from "@/utils/roles";
import { getAssignedToMe } from "@/api/muestras";
import type { MuestraDto } from "@/types/api";

export default function AnalistaMisMuestras() {
  const user = useAuthStore((s) => s.user);

  // Normalizamos SIEMPRE el rol que venga del backend/estado
  const role = normalizeRole(user?.role);
  const isAnalista = role === "ANALISTA";

  const { data, isLoading, error } = useQuery<MuestraDto[]>({
    queryKey: ["my-assigned-muestras"],
    queryFn: getAssignedToMe,
    enabled: !!user && isAnalista, // <-- había un !user y 'Analista' (mal)
  });

  if (!user) return <div className="p-6">Debes iniciar sesión.</div>;

  return (
    <div className="max-w-5xl mx-auto p-6 space-y-4">
      <h1 className="text-xl font-semibold">Mis muestras (Analista)</h1>

      {isLoading && <div>Cargando...</div>}
      {error && (
        <div className="p-3 rounded bg-red-100 text-red-700">
          {(error as any)?.message ?? "Error al cargar"}
        </div>
      )}

      {!isLoading && !error && (
        <div className="grid md:grid-cols-2 gap-3">
          {(data ?? []).map((m) => (
            <div key={m.mstCodigo || Math.random()} className="border rounded p-3">
              <div className="font-medium">{m.nombre || m.mstCodigo}</div>
              <div className="text-xs opacity-70">
                Código: {m.mstCodigo} · Tipo: {m.tpmstId} · Estado: {m.estadoActual}
              </div>
            </div>
          ))}
          {(data ?? []).length === 0 && (
            <div className="text-sm opacity-70">No tienes muestras asignadas.</div>
          )}
        </div>
      )}
    </div>
  );
}
