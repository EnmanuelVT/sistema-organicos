import { useEffect, useState } from "react";
import { getAllMuestras, assignAnalystToMuestra } from "../api/muestras";
import { getAllUsers } from "../api/users";
import type { MuestraDto, UserDto } from "../types/domain";

export default function AdminAsignaciones() {
  const [muestras, setMuestras] = useState<MuestraDto[]>([]);
  const [users, setUsers] = useState<UserDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [selectedAnalista, setSelectedAnalista] = useState("");
  const [selectedMuestra, setSelectedMuestra] = useState("");

  const loadData = async () => {
    setLoading(true);
    try {
      const [muestrasData, usersData] = await Promise.all([
        getAllMuestras(),
        getAllUsers()
      ]);
      setMuestras(muestrasData);
      setUsers(usersData);
    } catch (error) {
      console.error('Error loading data:', error);
    }
    setLoading(false);
  };

  useEffect(() => { loadData(); }, []);

  const asignarAnalista = async (mstCodigo: string) => {
    if (!selectedAnalista) {
      alert('Seleccione un analista');
      return;
    }
    
    try {
      await assignAnalystToMuestra({
        mstCodigo,
        idAnalista: selectedAnalista,
        observaciones: `Asignado por administrador`
      });
      await loadData(); // Reload data
      alert('Analista asignado exitosamente');
    } catch (error) {
      console.error('Error assigning analyst:', error);
      alert('Error al asignar analista');
    }
  };

  const analistas = users.filter(u => u.role === 'Analista');
  const evaluadores = users.filter(u => u.role === 'EVALUADOR');

  return (
    <div className="p-6 space-y-4">
      <h1 className="text-xl font-semibold">Asignaciones de Muestras</h1>

      <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
        <div className="space-y-2">
          <label className="block text-sm font-medium">Seleccionar Analista:</label>
          <select 
            className="border p-2 w-full" 
            value={selectedAnalista} 
            onChange={e => setSelectedAnalista(e.target.value)}
          >
            <option value="">-- Seleccionar Analista --</option>
            {analistas.map(a => (
              <option key={a.id} value={a.id}>
                {a.nombre || a.userName} ({a.email})
              </option>
            ))}
          </select>
        </div>
      </div>

      {loading ? <div>Cargando…</div> : (
        <div className="overflow-x-auto">
          <table className="text-sm min-w-full border border-gray-200">
            <thead className="bg-gray-50">
              <tr>
                <th className="text-left px-4 py-2 border-b">Código</th>
                <th className="text-left px-4 py-2 border-b">Nombre</th>
                <th className="text-left px-4 py-2 border-b">Origen</th>
                <th className="text-left px-4 py-2 border-b">Estado</th>
                <th className="text-left px-4 py-2 border-b">Fecha</th>
                <th className="text-left px-4 py-2 border-b">Acciones</th>
              </tr>
            </thead>
            <tbody>
              {muestras.map(m => (
                <tr key={m.mstCodigo} className="border-b hover:bg-gray-50">
                  <td className="px-4 py-2 font-medium">{m.mstCodigo}</td>
                  <td className="px-4 py-2">{m.nombre}</td>
                  <td className="px-4 py-2">{m.origen}</td>
                  <td className="px-4 py-2">
                    <span className="px-2 py-1 bg-gray-100 rounded text-xs">
                      {m.estadoActual}
                    </span>
                  </td>
                  <td className="px-4 py-2">{new Date(m.fechaRecepcion).toLocaleDateString()}</td>
                  <td className="px-4 py-2">
                    <button 
                      className="border border-blue-500 text-blue-500 px-3 py-1 rounded hover:bg-blue-50" 
                      onClick={() => asignarAnalista(m.mstCodigo)}
                      disabled={!selectedAnalista}
                    >
                      Asignar Analista
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </div>
  );
}
