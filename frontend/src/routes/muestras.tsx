import { useEffect, useState } from "react";
import { listarMuestras } from "@/api/muestras";
import type { Muestra } from "@/types/domain";
import { Link } from "react-router-dom";

export default function Muestras() {
  const [items, setItems] = useState<Muestra[]>([]);
  const [loading, setLoading] = useState(true);
  const [q, setQ] = useState("");
  const [estado, setEstado] = useState<number | "">("");

  const load = async () => {
    setLoading(true);
    const res = await listarMuestras({ q, estado });
    setItems(res.items ?? []);
    setLoading(false);
  };

  useEffect(() => { load(); /* eslint-disable-next-line */ }, []);

  return (
    <div className="p-6 space-y-4">
      <h1 className="text-xl font-semibold">Muestras</h1>

      <div className="flex gap-2">
        <input className="border px-3 py-2" placeholder="Buscar por código/origen"
               value={q} onChange={e=>setQ(e.target.value)} />
        <select className="border px-3 py-2" value={estado}
                onChange={e=>setEstado(e.target.value === "" ? "" : +e.target.value)}>
          <option value="">Estado (todos)</option>
          <option value={1}>Recibida</option>
          <option value={2}>En análisis</option>
          <option value={3}>Evaluada</option>
          <option value={4}>Certificada</option>
        </select>
        <button onClick={load} className="border px-3 py-2">Filtrar</button>
      </div>

      {loading ? <div>Cargando…</div> : items.length === 0 ? (
        <div className="text-sm text-slate-600">No hay muestras.</div>
      ) : (
        <div className="overflow-x-auto">
          <table className="min-w-full text-sm">
            <thead>
              <tr className="[&>th]:text-left [&>th]:py-2">
                <th>Código</th><th>Tipo</th><th>Origen</th><th>Estado</th><th>Recepción</th><th></th>
              </tr>
            </thead>
            <tbody>
              {items.map(m => (
                <tr key={m.mstCodigo} className="[&>td]:py-2 border-t">
                  <td>{m.mstCodigo}</td>
                  <td>{m.tpmstId}</td>
                  <td>{m.origen}</td>
                  <td>{m.estadoActual}</td>
                  <td>{new Date(m.fechaRecepcion).toLocaleString()}</td>
                  <td><Link className="underline" to={`/muestras/${m.mstCodigo}`}>ver</Link></td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </div>
  );
}
