import { useState } from "react";
import { useQuery } from "@tanstack/react-query";
import { getAllTrazabilidad, getTrazabilidadByUserId } from "@/api/traceability";
import { getAllUsers } from "@/api/users";
import { getStateDisplayName, getStateColor } from "@/utils/roles";
import { GitBranch, Search, Calendar, User, FileText } from "lucide-react";
import LoadingSpinner from "@/components/LoadingSpinner";
import ErrorMessage from "@/components/ErrorMessage";
import EmptyState from "@/components/EmptyState";

export default function TraceabilityPage() {
  const [searchTerm, setSearchTerm] = useState("");
  const [selectedUserId, setSelectedUserId] = useState<string>("");

  const { data: allTrazabilidad, isLoading: loadingAll, error: errorAll } = useQuery({
    queryKey: ["trazabilidad", "all"],
    queryFn: getAllTrazabilidad,
    enabled: !selectedUserId,
  });

  const { data: userTrazabilidad, isLoading: loadingUser, error: errorUser } = useQuery({
    queryKey: ["trazabilidad", "user", selectedUserId],
    queryFn: () => getTrazabilidadByUserId(selectedUserId),
    enabled: !!selectedUserId,
  });

  const { data: users } = useQuery({
    queryKey: ["users"],
    queryFn: getAllUsers,
  });

  const trazabilidad = selectedUserId ? userTrazabilidad : allTrazabilidad;
  const isLoading = selectedUserId ? loadingUser : loadingAll;
  const error = selectedUserId ? errorUser : errorAll;

  const filteredTrazabilidad = trazabilidad?.filter((historial) => {
    return !searchTerm || 
      historial.idMuestra?.toLowerCase().includes(searchTerm.toLowerCase()) ||
      historial.observaciones?.toLowerCase().includes(searchTerm.toLowerCase()) ||
      historial.idAnalista?.toLowerCase().includes(searchTerm.toLowerCase());
  }) || [];

  const getUserDisplayName = (userId: string) => {
    const user = users?.find(u => u.id === userId);
    if (!user) return userId;
    return user.nombre && user.apellido 
      ? `${user.nombre} ${user.apellido}`
      : user.userName || user.email;
  };

  if (isLoading) {
    return (
      <div className="flex items-center justify-center h-64">
        <LoadingSpinner size="lg" />
      </div>
    );
  }

  if (error) {
    return (
      <ErrorMessage 
        message={(error as any)?.message || "Error al cargar la trazabilidad"} 
      />
    );
  }

  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-2xl font-bold text-gray-900">Trazabilidad del Sistema</h1>
        <p className="text-gray-600 mt-1">
          Historial completo de cambios de estado y asignaciones de muestras
        </p>
      </div>

      {/* Filters */}
      <div className="card">
        <div className="flex flex-col sm:flex-row gap-4">
          <div className="flex-1">
            <div className="relative">
              <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 h-4 w-4 text-gray-400" />
              <input
                type="text"
                placeholder="Buscar por muestra, observaciones o analista..."
                value={searchTerm}
                onChange={(e) => setSearchTerm(e.target.value)}
                className="input pl-10"
              />
            </div>
          </div>
          
          <div className="sm:w-64">
            <select
              value={selectedUserId}
              onChange={(e) => setSelectedUserId(e.target.value)}
              className="input"
            >
              <option value="">Todos los usuarios</option>
              {users?.map((user) => (
                <option key={user.id} value={user.id!}>
                  {getUserDisplayName(user.id!)}
                </option>
              ))}
            </select>
          </div>
        </div>
      </div>

      {/* Traceability Records */}
      <div className="card">
        {filteredTrazabilidad.length === 0 ? (
          <EmptyState
            icon={<GitBranch className="h-12 w-12" />}
            title="No hay registros de trazabilidad"
            description="No se encontraron registros que coincidan con los filtros aplicados."
          />
        ) : (
          <div className="overflow-x-auto">
            <table className="w-full">
              <thead>
                <tr className="border-b border-gray-200">
                  <th className="text-left py-3 px-4 font-medium text-gray-900">Fecha</th>
                  <th className="text-left py-3 px-4 font-medium text-gray-900">Muestra</th>
                  <th className="text-left py-3 px-4 font-medium text-gray-900">Usuario</th>
                  <th className="text-left py-3 px-4 font-medium text-gray-900">Estado</th>
                  <th className="text-left py-3 px-4 font-medium text-gray-900">Observaciones</th>
                </tr>
              </thead>
              <tbody>
                {filteredTrazabilidad.map((historial) => (
                  <tr key={historial.idBitacora} className="border-b border-gray-100 hover:bg-gray-50">
                    <td className="py-3 px-4 text-gray-700">
                      <div className="flex items-center space-x-2">
                        <Calendar className="h-4 w-4 text-gray-400" />
                        <span>{new Date(historial.fechaCambio).toLocaleString()}</span>
                      </div>
                    </td>
                    <td className="py-3 px-4">
                      <div className="flex items-center space-x-2">
                        <FileText className="h-4 w-4 text-gray-400" />
                        <span className="font-medium text-gray-900">{historial.idMuestra}</span>
                      </div>
                    </td>
                    <td className="py-3 px-4 text-gray-700">
                      <div className="flex items-center space-x-2">
                        <User className="h-4 w-4 text-gray-400" />
                        <span>{getUserDisplayName(historial.idAnalista)}</span>
                      </div>
                    </td>
                    <td className="py-3 px-4">
                      <span className={`badge ${getStateColor(historial.estado)}`}>
                        {getStateDisplayName(historial.estado)}
                      </span>
                    </td>
                    <td className="py-3 px-4 text-gray-600">
                      {historial.observaciones || "Sin observaciones"}
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
      </div>
    </div>
  );
}