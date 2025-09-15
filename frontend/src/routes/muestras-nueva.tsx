// src/routes/muestras-nueva.tsx
import { useState } from "react";
import { createMuestra } from "@/api/muestras"; // alias agregado; también podrías usar: import { create } ...

export default function MuestrasNueva() {
  const [form, setForm] = useState({ tpmstId: 0, nombre: "", origen: "" });

  async function guardar() {
    if (!form.tpmstId) return alert("tpmstId requerido");
    await createMuestra({
      tpmstId: Number(form.tpmstId),
      nombre: form.nombre || null,
      origen: form.origen || null,
      condicionesAlmacenamiento: null,
      condicionesTransporte: null,
    } as any);
    setForm({ tpmstId: 0, nombre: "", origen: "" });
    alert("Muestra creada");
  }

  return (
    <div className="max-w-md mx-auto p-6 space-y-3">
      <h1 className="text-xl font-semibold">Nueva muestra</h1>
      <input className="border rounded px-3 py-2 w-full" placeholder="tpmstId"
        value={form.tpmstId || ""} onChange={(e) => setForm({ ...form, tpmstId: Number(e.target.value) })} />
      <input className="border rounded px-3 py-2 w-full" placeholder="Nombre"
        value={form.nombre} onChange={(e) => setForm({ ...form, nombre: e.target.value })} />
      <input className="border rounded px-3 py-2 w-full" placeholder="Origen"
        value={form.origen} onChange={(e) => setForm({ ...form, origen: e.target.value })} />
      <button className="bg-gray-900 text-white rounded px-4 py-2" onClick={guardar}>Guardar</button>
    </div>
  );
}
