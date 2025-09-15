// src/routes/admin-asignaciones.tsx
import { useEffect, useState } from "react";
import { getAllMuestras, assignAnalystToMuestra } from "@/api/muestras";
import * as usersApi from "@/api/users";
import { normalizeRole } from "@/utils/roles";
import type { MuestraDto, UserDto } from "@/types/api";

export default function AdminAsignaciones() {
  const [muestras, setMuestras] = useState<MuestraDto[]>([]);
  const [analistas, setAnalistas] = useState<UserDto[]>([]);
  const [selMuestra, setSelMuestra] = useState<string>("");
  const [selAnalista, setSelAnalista] = useState<string>("");
  const [obs, setObs] = useState("");

  useEffect(() => {
    (async () => {
      const ms = await getAllMuestras();
      setMuestras(ms);
      const users = await usersApi.getAllUsers();
      setAnalistas(users.filter(u => normalizeRole(u.role) === "ANALISTA"));
    })();
  }, []);

  async function asignar() {
    if (!selMuestra || !selAnalista) {
      alert("Seleccione muestra y analista");
      return;
    }
    await assignAnalystToMuestra({ mstCodigo: selMuestra, idAnalista: selAnalista, observaciones: obs || "" });
    alert("Asignado correctamente");
  }

  const estadoTxt = (n?: number) => (typeof n === "number" ? String(n) : "-");

  return (
    <div className="max-w-5xl mx-auto p-6 space-y-4">
      <h1 className="text-xl font-semibold">Asignaciones (ADMIN)</h1>

      <div className="grid md:grid-cols-3 gap-3">
        <select className="border rounded px-3 py-2" value={selMuestra} onChange={e => setSelMuestra(e.target.value)}>
          <option value="">-- Seleccione muestra --</option>
          {muestras.map(m => (
            <option key={m.mstCodigo} value={m.mstCodigo ?? ""}>
              {m.mstCodigo} · {m.nombre}
            </option>
          ))}
        </select>

        <select className="border rounded px-3 py-2" value={selAnalista} onChange={e => setSelAnalista(e.target.value)}>
          <option value="">-- Seleccione analista --</option>
          {analistas.map(a => (
            <option key={a.id ?? a.email} value={a.id ?? ""}>
              {a.userName} · {a.email}
            </option>
          ))}
        </select>

        <input
          className="border rounded px-3 py-2"
          placeholder="Observaciones (opcional)"
          value={obs}
          onChange={(e) => setObs(e.target.value)}
        />
      </div>

      <button className="bg-gray-900 text-white rounded px-4 py-2" onClick={asignar}>
        Asignar
      </button>

      <div className="border rounded mt-6">
        <table className="w-full text-sm">
          <thead className="bg-gray-50">
            <tr>
              <th className="text-left p-2">Código</th>
              <th className="text-left p-2">Nombre</th>
              <th className="text-left p-2">Tipo</th>
              <th className="text-left p-2">Estado</th>
            </tr>
          </thead>
          <tbody>
            {muestras.map((m) => (
              <tr key={m.mstCodigo} className="border-t">
                <td className="p-2">{m.mstCodigo}</td>
                <td className="p-2">{m.nombre}</td>
                <td className="p-2">{m.tpmstId}</td>
                <td className="p-2">{estadoTxt(m.estadoActual)}</td>
              </tr>
            ))}
            {muestras.length === 0 && (
              <tr><td className="p-3" colSpan={4}>Sin muestras.</td></tr>
            )}
          </tbody>
        </table>
      </div>
    </div>
  );
}
