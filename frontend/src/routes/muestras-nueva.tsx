import { useState } from "react";
import { crearMuestra } from "@/api/muestras";
import { useAuth } from "@/store/auth";
import { useNavigate } from "react-router-dom";

export default function MuestraNueva() {
  const user = useAuth(s => s.user)!;
  const nav = useNavigate();

  const [mstCodigo, setCodigo] = useState("");
  const [tpmstId, setTipo] = useState<number>(1);
  const [origen, setOrigen] = useState("");
  const [nombre, setNombre] = useState("");
  const [condAlm, setCondAlm] = useState("");
  const [condTrans, setCondTrans] = useState("");
  const [saving, setSaving] = useState(false);

  const submit = async (e: React.FormEvent) => {
    e.preventDefault();
    setSaving(true);
    await crearMuestra({
      mstCodigo,
      tpmstId,
      nombre,
      origen,
      fechaRecepcion: new Date().toISOString(),
      idUsuarioSolicitante: user.id,
      estadoActual: 1,
      condicionesAlmacenamiento: condAlm || undefined,
      condicionesTransporte: condTrans || undefined,
    } as any);
    setSaving(false);
    nav(`/muestras/${mstCodigo}`);
  };

  return (
    <form onSubmit={submit} className="p-6 space-y-3 max-w-xl">
      <h1 className="text-xl font-semibold">Registrar nueva muestra</h1>
      <input className="border p-2 w-full" placeholder="Código (único)" value={mstCodigo} onChange={e=>setCodigo(e.target.value)} required />
      <input className="border p-2 w-full" placeholder="Nombre" value={nombre} onChange={e=>setNombre(e.target.value)} required />
      <input className="border p-2 w-full" placeholder="Origen" value={origen} onChange={e=>setOrigen(e.target.value)} required />
      <label className="block">
        <span className="text-sm">Tipo de muestra</span>
        <select className="border p-2 w-full" value={tpmstId} onChange={e=>setTipo(+e.target.value)}>
          <option value={1}>AGUA</option>
          <option value={2}>ALIMENTO</option>
          <option value={3}>BEBIDA</option>
        </select>
      </label>
      <input className="border p-2 w-full" placeholder="Condiciones de almacenamiento (opcional)" value={condAlm} onChange={e=>setCondAlm(e.target.value)} />
      <input className="border p-2 w-full" placeholder="Condiciones de transporte (opcional)" value={condTrans} onChange={e=>setCondTrans(e.target.value)} />
      <button disabled={saving} className="border px-4 py-2">{saving ? "Guardando…" : "Guardar"}</button>
    </form>
  );
}
