// src/routes/solicitante-mis-muestras.tsx
import { useQuery } from "@tanstack/react-query";
import { getMyMuestras } from "@/api/muestras"; // alias -> getMine
import type { MuestraDto } from "@/types/api";

export default function SolicitanteMisMuestras() {
  const { data, isLoading, error } = useQuery<MuestraDto[]>({
    queryKey: ["my-muestras"],
    queryFn: getMyMuestras,
  });

  return (
    <div className="max-w-5xl mx-auto p-6">
      <h1 className="text-xl font-semibold mb-4">Mis muestras (Solicitante)</h1>
      {isLoading && <div>Cargando...</div>}
      {error && <div className="p-3 bg-red-100 text-red-700 rounded">Error: {(error as any)?.message ?? "Error"}</div>}
      {!isLoading && !error && (
        <div className="grid md:grid-cols-2 gap-3">
          {(data ?? []).map((m) => (
            <div key={m.mstCodigo || Math.random()} className="border rounded p-3">
              <div className="font-medium">{m.nombre || m.mstCodigo}</div>
              <div className="text-xs opacity-70">Código: {m.mstCodigo} · Tipo: {m.tpmstId} · Estado: {m.estadoActual}</div>
            </div>
          ))}
          {(data ?? []).length === 0 && <div>No tienes muestras.</div>}
        </div>
      )}
    </div>
  );
}
