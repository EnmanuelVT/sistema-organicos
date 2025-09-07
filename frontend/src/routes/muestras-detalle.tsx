import { useEffect, useState } from "react";
import { useParams, Link } from "react-router-dom";
import { detalleMuestra } from "@/api/muestras";
import type { MuestraDetalle } from "@/types/domain";
import { useAuth } from "@/store/auth";

export default function MuestrasDetalle() {
  const { mstCodigo = "" } = useParams();
  const [data, setData] = useState<MuestraDetalle | null>(null);
  const [loading, setLoading] = useState(true);
  const user = useAuth(s => s.user);

  useEffect(() => {
    (async () => {
      setLoading(true);
      const d = await detalleMuestra(mstCodigo);
      setData(d);
      setLoading(false);
    })();
  }, [mstCodigo]);

  if (loading) return <div className="p-6">Cargando…</div>;
  if (!data) return <div className="p-6">No encontrado.</div>;

  return (
    <div className="p-6 space-y-6">
      <header>
        <h1 className="text-xl font-semibold">Muestra {data.mstCodigo}</h1>
        <p className="text-sm text-slate-600">Origen: {data.origen} | Tipo: {data.tpmstId} | Estado: {data.estadoActual}</p>
      </header>

<section>
  <h2 className="font-medium mb-2">Bitácora</h2>
  {!data.bitacora?.length ? (
    <div className="text-sm text-slate-600">—</div>
  ) : (
    <ul className="text-sm list-disc pl-5 space-y-1">
      {data.bitacora.map(b => (
        <li key={b.idBitacora}>
          {new Date(b.fechaAsignacion).toLocaleString()} — Analista: {b.idAnalista}
          {b.observaciones ? ` (${b.observaciones})` : ''}
        </li>
      ))}
    </ul>
  )}
</section>


      <section>
        <h2 className="font-medium mb-2">Pruebas / Resultados</h2>
        {!data.resultados?.length ? <div className="text-sm text-slate-600">—</div> : (
          <table className="text-sm min-w-full">
            <thead><tr><th className="text-left">Parámetro</th><th className="text-left">Valor</th><th>Validación</th></tr></thead>
            <tbody>
              {data.resultados!.map(r => (
                <tr key={r.idResultado} className="border-t [&>td]:py-2">
                  <td>{r.idParametro}</td>
                  <td>{r.valorObtenido ?? '—'}</td>
                  <td>{r.estadoValidacion ?? 'PENDIENTE'}</td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </section>

      <section className="flex gap-2">
        {(user?.role === "ANALISTA" || user?.role === "ADMINISTRADOR") && (
          <Link to={`/ensayos/registrar/${data.mstCodigo}`} className="border px-3 py-2 rounded">Registrar ensayo</Link>
        )}
        {(user?.role === "EVALUADOR" || user?.role === "ADMINISTRADOR") && (
          <Link to={`/evaluador/revisar/${data.mstCodigo}`} className="border px-3 py-2 rounded">Revisar / validar</Link>
        )}
      </section>
    </div>
  );
}
