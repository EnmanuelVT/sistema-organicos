// src/routes/analista-registrar-prueba.tsx
import { useState } from "react";
import { registrarPrueba } from "@/api/ensayos";                    // alias -> create
import { getParametrosByTipoMuestra } from "@/api/ensayos";         // re-export desde parametros
import type { ParametroDto } from "@/types/api";

export default function AnalistaRegistrarPrueba() {
  const [mstCodigo, setMstCodigo] = useState("");
  const [nombrePrueba, setNombrePrueba] = useState("");
  const [tipoId, setTipoId] = useState<number>(0);
  const [params, setParams] = useState<ParametroDto[]>([]);

  async function cargarParametros() {
    if (!tipoId) return alert("Ingresa tipo de muestra");
    const p = await getParametrosByTipoMuestra(Number(tipoId));
    setParams(p);
  }

  async function guardar() {
    if (!mstCodigo || !nombrePrueba) return alert("Completa los campos");
    await registrarPrueba({ idMuestra: mstCodigo, nombrePrueba });
    setNombrePrueba("");
    alert("Prueba creada");
  }

  return (
    <div className="max-w-3xl mx-auto p-6 space-y-3">
      <h1 className="text-xl font-semibold">Registrar Prueba (Analista)</h1>

      <input className="border rounded px-3 py-2 w-full" placeholder="Código de muestra"
        value={mstCodigo} onChange={(e) => setMstCodigo(e.target.value)} />
      <input className="border rounded px-3 py-2 w-full" placeholder="Nombre de la prueba"
        value={nombrePrueba} onChange={(e) => setNombrePrueba(e.target.value)} />

      <div className="flex gap-2">
        <input className="border rounded px-3 py-2" placeholder="Tipo de muestra (tpmstId)"
          value={tipoId || ""} onChange={(e) => setTipoId(Number(e.target.value))} />
        <button className="bg-gray-900 text-white rounded px-4 py-2" onClick={cargarParametros}>Cargar parámetros</button>
      </div>

      {params.length > 0 && (
        <div className="border rounded p-3">
          <div className="font-medium mb-2">Parámetros del tipo</div>
          <ul className="list-disc pl-5 text-sm">
            {params.map((p) => (
              <li key={p.idParametro}>{p.nombreParametro} · {p.unidad}</li>
            ))}
          </ul>
        </div>
      )}

      <button className="bg-green-600 text-white rounded px-4 py-2" onClick={guardar}>
        Guardar prueba
      </button>
    </div>
  );
}
