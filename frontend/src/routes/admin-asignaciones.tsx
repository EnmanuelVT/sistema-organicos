import { useEffect, useState } from "react";
import { listarMuestras } from "@/api/muestras";
import type { Muestra } from "@/types/domain";
import api from "@/api/apiClient";

export default function AdminAsignaciones() {
  const [items, setItems] = useState<Muestra[]>([]);
  const [loading, setLoading] = useState(true);
  const [analista, setAnalista] = useState("");
  const [evaluador, setEvaluador] = useState("");

  const load = async () => {
    setLoading(true);
    const res = await listarMuestras({ estado: 1 }); // por ejemplo, pendientes
    setItems(res.items ?? []);
    setLoading(false);
  };

  useEffect(() => { load(); }, []);

  const asignar = async (mstCodigo: string) => {
    await api.put('/admin/asignaciones', { mstCodigo, idAnalista: analista || null, idEvaluador: evaluador || null });
    await load();
  };

  return (
    <div className="p-6 space-y-4">
      <h1 className="text-xl font-semibold">Asignaciones</h1>

      <div className="flex gap-2">
        <input className="border p-2" placeholder="Id analista" value={analista} onChange={e=>setAnalista(e.target.value)} />
        <input className="border p-2" placeholder="Id evaluador" value={evaluador} onChange={e=>setEvaluador(e.target.value)} />
      </div>

      {loading ? <div>Cargando…</div> : (
        <table className="text-sm min-w-full">
          <thead><tr><th className="text-left">Código</th><th>Origen</th><th>Estado</th><th></th></tr></thead>
          <tbody>
            {items.map(m => (
              <tr key={m.mstCodigo} className="border-t [&>td]:py-2">
                <td>{m.mstCodigo}</td>
                <td>{m.origen}</td>
                <td>{m.estadoActual}</td>
                <td><button className="border px-3 py-1" onClick={()=>asignar(m.mstCodigo)}>Asignar</button></td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
}
