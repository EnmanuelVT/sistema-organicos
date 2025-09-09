// src/pages/analista-registrar-prueba.tsx
import { useParams, useNavigate } from "react-router-dom";
import { useState } from "react";
import { registrarEnsayo } from "@/api/ensayos";

type Parametro = { idParametro: number; valorObtenido: number };

export default function AnalistaRegistrarPrueba() {
  const { mstCodigo } = useParams<{ mstCodigo: string }>();
  const nav = useNavigate();

  const codigo = (mstCodigo ?? "").trim();
  const [nombrePrueba, setNombrePrueba] = useState("Análisis X");
  const [tipoMuestraAsociada, setTipo] = useState<number>(1);
  const [parametros, setParametros] = useState<Parametro[]>([]);
  const [valor, setValor] = useState<number>(0);
  const [idParam, setIdParam] = useState<number>(1);
  const [saving, setSaving] = useState(false);

  const paramMissing = !codigo;

  const add = () => {
    if (!idParam && idParam !== 0) return;
    setParametros((prev) => [
      ...prev,
      { idParametro: Number(idParam), valorObtenido: Number(valor) },
    ]);
    setIdParam(1);
    setValor(0);
  };

  const submit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (paramMissing) return;

    try {
      setSaving(true);
      await registrarEnsayo(codigo, {
        nombrePrueba,
        tipoMuestraAsociada,
        parametros,
      });
      nav(`/muestras/${encodeURIComponent(codigo)}`);
    } finally {
      setSaving(false);
    }
  };

  if (paramMissing) {
    return (
      <div className="p-6 text-red-600">
        Falta el parámetro <code>mstCodigo</code> en la URL. Debes navegar a{" "}
        <code>/analista/muestras/:mstCodigo/prueba</code>.
      </div>
    );
  }

  return (
    <form onSubmit={submit} className="p-6 space-y-3 max-w-xl">
      <h1 className="text-xl font-semibold">Registrar ensayo para {codigo}</h1>

      <label className="block">
        <span className="sr-only">Nombre de la prueba</span>
        <input
          className="border p-2 w-full"
          value={nombrePrueba}
          onChange={(e) => setNombrePrueba(e.target.value)}
          placeholder="Nombre de la prueba"
        />
      </label>

      <label className="block">
        <span className="text-sm">Tipo de muestra asociada</span>
        <select
          className="border p-2 w-full"
          value={tipoMuestraAsociada}
          onChange={(e) => setTipo(Number(e.target.value))}
        >
          <option value={1}>AGUA</option>
          <option value={2}>ALIMENTO</option>
          <option value={3}>BEBIDA</option>
        </select>
      </label>

      <div className="border p-3 rounded space-y-2">
        <div className="flex gap-2">
          <input
            className="border p-2 w-24"
            type="number"
            min={1}
            placeholder="Id parámetro"
            value={idParam}
            onChange={(e) => setIdParam(Number(e.target.value))}
          />
          <input
            className="border p-2 w-32"
            type="number"
            placeholder="Valor"
            value={valor}
            onChange={(e) => setValor(Number(e.target.value))}
          />
          <button type="button" onClick={add} className="border px-3 py-2">
            Agregar
          </button>
        </div>

        <ul className="text-sm">
          {parametros.map((p, i) => (
            <li key={`${p.idParametro}-${i}`}>
              Param {p.idParametro}: {p.valorObtenido}
            </li>
          ))}
        </ul>
      </div>

      <button disabled={saving} className="border px-4 py-2">
        {saving ? "Guardando…" : "Guardar"}
      </button>
    </form>
  );
}
