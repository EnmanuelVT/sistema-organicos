import { useState } from "react";
import { useQuery } from "@tanstack/react-query";
import { getAllAuditorias, getAuditoriaByUserId } from "@/api/audit";
import { getAllUsers } from "@/api/users";
import { Shield, Search, Calendar, User, Activity } from "lucide-react";
import LoadingSpinner from "@/components/LoadingSpinner";
import ErrorMessage from "@/components/ErrorMessage";
import EmptyState from "@/components/EmptyState";

export default function AuditPage() {
  const [searchTerm, setSearchTerm] = useState("");
  const [selectedUserId, setSelectedUserId] = useState<string>("");

  const { data: allAuditorias, isLoading: loadingAll, error: errorAll } = useQuery({
    queryKey: ["auditoria", "all"],
    queryFn: getAllAuditorias,
    enabled: !selectedUserId,
  });

  const { data: userAuditorias, isLoading: loadingUser, error: errorUser } = useQuery({
    queryKey: ["auditoria", "user", selectedUserId],
    queryFn: () => getAuditoriaByUserId(selectedUserId),
    enabled: !!selectedUserId,
  });

  const { data: users } = useQuery({
    queryKey: ["users"],
    queryFn: getAllUsers,
  });

  const auditorias = selectedUserId ? userAuditorias : allAuditorias;
  const isLoading = selectedUserId ? loadingUser : loadingAll;
  const error = selectedUserId ? errorUser : errorAll;

  const filteredAuditorias = auditorias?.filter((auditoria) => {
    return !searchTerm || 
      auditoria.accion?.toLowerCase().includes(searchTerm.toLowerCase()) ||
      auditoria.descripcion?.toLowerCase().includes(searchTerm.toLowerCase()) ||
      auditoria.idUsuario?.toLowerCase().includes(searchTerm.toLowerCase());
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
        message={(error as any)?.message || "Error al cargar la auditoría"} 
      />
    );
  }

  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-2xl font-bold text-gray-900">Auditoría del Sistema</h1>
        <p className="text-gray-600 mt-1">
          Registro completo de todas las acciones realizadas en el sistema
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
                placeholder="Buscar por acción, descripción o usuario..."
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

      {/* Audit Records */}
      <div className="card">
        {filteredAuditorias.length === 0 ? (
          <EmptyState
            icon={<Shield className="h-12 w-12" />}
            title="No hay registros de auditoría"
            description="No se encontraron registros que coincidan con los filtros aplicados."
          />
        ) : (
          <div className="overflow-x-auto">
            <table className="w-full">
              <thead>
                <tr className="border-b border-gray-200">
                  <th className="text-left py-3 px-4 font-medium text-gray-900">Fecha</th>
                  <th className="text-left py-3 px-4 font-medium text-gray-900">Usuario</th>
                  <th className="text-left py-3 px-4 font-medium text-gray-900">Acción</th>
                  <th className="text-left py-3 px-4 font-medium text-gray-900">Descripción</th>
                </tr>
              </thead>
              <tbody>
                {filteredAuditorias.map((auditoria) => (
                  <tr key={auditoria.idAuditoria} className="border-b border-gray-100 hover:bg-gray-50">
                    <td className="py-3 px-4 text-gray-700">
                      <div className="flex items-center space-x-2">
                        <Calendar className="h-4 w-4 text-gray-400" />
                        <span>{new Date(auditoria.fechaAcción).toLocaleString()}</span>
                      </div>
                    </td>
                    <td className="py-3 px-4 text-gray-700">
                      <div className="flex items-center space-x-2">
                        <User className="h-4 w-4 text-gray-400" />
                        <span>{getUserDisplayName(auditoria.idUsuario)}</span>
                      </div>
                    </td>
                    <td className="py-3 px-4">
                      <div className="flex items-center space-x-2">
                        <Activity className="h-4 w-4 text-primary-600" />
                        <span className="font-medium text-gray-900">{auditoria.accion}</span>
                      </div>
                    </td>
                    <td className="py-3 px-4 text-gray-600">
                      {auditoria.descripcion || "Sin descripción"}
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