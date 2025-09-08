import { useParams, useNavigate } from "react-router-dom";
import { useState } from "react";
import { registrarEnsayo } from "@/api/ensayos";

export default function RegistrarPrueba() {
  const { mstCodigo = "" } = useParams();
  const nav = useNavigate();
  const [nombrePrueba, setNombrePrueba] = useState("Análisis X");
  const [tipoMuestraAsociada, setTipo] = useState<number>(1);
  const [parametros, setParametros] = useState<Array<{idParametro:number; valorObtenido:number}>>([]);
  const [valor, setValor] = useState<number>(0);
  const [idParam, setIdParam] = useState<number>(1);
  const [saving, setSaving] = useState(false);

  const add = () => {
    if (!idParam) return;
    setParametros([...parametros, { idParametro: idParam, valorObtenido: Number(valor) }]);
    setIdParam(1); setValor(0);
  };

  const submit = async (e: React.FormEvent) => {
    e.preventDefault();
    setSaving(true);
    await registrarEnsayo(mstCodigo, { nombrePrueba, tipoMuestraAsociada, parametros });
    setSaving(false);
    nav(`/muestras/${mstCodigo}`);
  };

  return (
    <form onSubmit={submit} className="p-6 space-y-3 max-w-xl">
      <h1 className="text-xl font-semibold">Registrar ensayo para {mstCodigo}</h1>
      <input className="border p-2 w-full" value={nombrePrueba} onChange={e=>setNombrePrueba(e.target.value)} />
      <label className="block">
        <span className="text-sm">Tipo de muestra asociada</span>
        <select className="border p-2 w-full" value={tipoMuestraAsociada} onChange={e=>setTipo(+e.target.value)}>
          <option value={1}>AGUA</option><option value={2}>ALIMENTO</option><option value={3}>BEBIDA</option>
        </select>
      </label>

      <div className="border p-3 rounded space-y-2">
        <div className="flex gap-2">
          <input className="border p-2 w-24" type="number" placeholder="Id parámetro" value={idParam} onChange={e=>setIdParam(+e.target.value)} />
          <input className="border p-2 w-32" type="number" placeholder="Valor" value={valor} onChange={e=>setValor(+e.target.value)} />
          <button type="button" onClick={add} className="border px-3 py-2">Agregar</button>
        </div>
        <ul className="text-sm">
          {parametros.map((p,i)=><li key={i}>Param {p.idParametro}: {p.valorObtenido}</li>)}
        </ul>
      </div>

      <button disabled={saving} className="border px-4 py-2">{saving ? "Guardando…" : "Guardar"}</button>
    </form>
  );
}
