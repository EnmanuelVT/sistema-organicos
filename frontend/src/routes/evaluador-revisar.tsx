import { useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import { detalleMuestra } from "@/api/muestras";
import { validarEnsayo } from "@/api/ensayos";
import type { MuestraDetalle, ResultadoPrueba } from "@/types/domain";

export default function EvaluadorRevisar() {
  const { mstCodigo = "" } = useParams();
  const [data, setData] = useState<MuestraDetalle | null>(null);
  const [loading, setLoading] = useState(true);

  const load = async () => {
    setLoading(true);
    setData(await detalleMuestra(mstCodigo));
    setLoading(false);
  };

  useEffect(() => { load(); /* eslint-disable-next-line */ }, [mstCodigo]);

  const act = async (r: ResultadoPrueba, estado: "APROBADO" | "RECHAZADO") => {
    await validarEnsayo(r.idResultado, estado);
    await load();
  };

  if (loading) return <div className="p-6">Cargando…</div>;
  if (!data) return <div className="p-6">No encontrado.</div>;

  return (
    <div className="p-6 space-y-4">
      <h1 className="text-xl font-semibold">Revisión de resultados — {data.mstCodigo}</h1>
      {!data.resultados?.length ? <div>No hay resultados.</div> : (
        <table className="text-sm min-w-full">
          <thead><tr><th className="text-left">Parámetro</th><th>Valor</th><th>Estado</th><th>Acciones</th></tr></thead>
          <tbody>
            {data.resultados!.map(r => (
              <tr key={r.idResultado} className="border-t [&>td]:py-2">
                <td>{r.idParametro}</td>
                <td>{r.valorObtenido ?? "—"}</td>
                <td>{r.estadoValidacion ?? "PENDIENTE"}</td>
                <td className="flex gap-2">
                  <button onClick={()=>act(r,"APROBADO")}  className="border px-2 py-1">Aprobar</button>
                  <button onClick={()=>act(r,"RECHAZADO")} className="border px-2 py-1">Rechazar</button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
}
