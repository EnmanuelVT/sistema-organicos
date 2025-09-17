import { useQuery } from "@tanstack/react-query";
import { useAuthStore } from "@/store/auth";
import { hasRole, getStateDisplayName, getStateColor } from "@/utils/roles";
import { getAllSamples, getMySamples, getAssignedSamples } from "@/api/samples";
import { TestTube, Plus, Search, Filter, Settings, FileText } from "lucide-react";
import { UserCheck } from "lucide-react";
import { Link } from "react-router-dom";
import { useState } from "react";
import LoadingSpinner from "@/components/LoadingSpinner";
import ErrorMessage from "@/components/ErrorMessage";
import EmptyState from "@/components/EmptyState";
import ChangeStatusModal from "@/components/ChangeStatusModal";
import DocumentsModal from "@/components/DocumentsModal";
import AssignAnalystModal from "@/components/AssignAnalystModal";

export default function SamplesPage() {
  const { user } = useAuthStore();
  const [searchTerm, setSearchTerm] = useState("");
  const [statusFilter, setStatusFilter] = useState<number | "">("");
  const [selectedSampleForStatus, setSelectedSampleForStatus] = useState<{code: string, status: number} | null>(null);
  const [selectedSampleForDocs, setSelectedSampleForDocs] = useState<string | null>(null);
  const [selectedSampleForAssign, setSelectedSampleForAssign] = useState<string | null>(null);

  const { data: samples, isLoading, error } = useQuery({
    queryKey: ["samples"],
    queryFn: () => {
      if (hasRole(user?.role, ["ADMIN", "EVALUADOR"])) {
        return getAllSamples();
      } else if (hasRole(user?.role, ["ANALISTA"])) {
        return getAssignedSamples();
      } else {
        return getMySamples();
      }
    },
  });

  const filteredSamples = samples?.filter((sample) => {
    const matchesSearch = !searchTerm || 
      sample.mstCodigo?.toLowerCase().includes(searchTerm.toLowerCase()) ||
      sample.nombre?.toLowerCase().includes(searchTerm.toLowerCase()) ||
      sample.origen?.toLowerCase().includes(searchTerm.toLowerCase());
    
    const matchesStatus = statusFilter === "" || sample.estadoActual === statusFilter;
    
    return matchesSearch && matchesStatus;
  }) || [];

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
        message={(error as any)?.message || "Error al cargar las muestras"} 
      />
    );
  }

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold text-gray-900">Muestras</h1>
          <p className="text-gray-600 mt-1">
            {hasRole(user?.role, ["ADMIN", "EVALUADOR"]) 
              ? "Gestión de todas las muestras del sistema"
              : hasRole(user?.role, ["ANALISTA"])
              ? "Muestras asignadas para análisis"
              : "Mis muestras registradas"
            }
          </p>
        </div>
        
        {hasRole(user?.role, ["SOLICITANTE", "ADMIN"]) && (
          <Link to="/samples/create" className="btn-primary">
            <Plus className="h-4 w-4 mr-2" />
            Nueva Muestra
          </Link>
        )}
      </div>

      {/* Filters */}
      <div className="card">
        <div className="flex flex-col sm:flex-row gap-4">
          <div className="flex-1">
            <div className="relative">
              <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 h-4 w-4 text-gray-400" />
              <input
                type="text"
                placeholder="Buscar por código, nombre o origen..."
                value={searchTerm}
                onChange={(e) => setSearchTerm(e.target.value)}
                className="input pl-10"
              />
            </div>
          </div>
          
          <div className="sm:w-48">
            <select
              value={statusFilter}
              onChange={(e) => setStatusFilter(e.target.value === "" ? "" : Number(e.target.value))}
              className="input"
            >
              <option value="">Todos los estados</option>
              <option value={1}>Recibida</option>
              <option value={2}>En análisis</option>
              <option value={3}>En espera</option>
              <option value={4}>Evaluada</option>
              <option value={5}>Certificada</option>
            </select>
          </div>
        </div>
      </div>

      {/* Modals */}
      {selectedSampleForStatus && (
        <ChangeStatusModal
          isOpen={true}
          onClose={() => setSelectedSampleForStatus(null)}
          sampleCode={selectedSampleForStatus.code}
          currentStatus={selectedSampleForStatus.status}
        />
      )}

      {selectedSampleForDocs && (
        <DocumentsModal
          isOpen={true}
          onClose={() => setSelectedSampleForDocs(null)}
          sampleCode={selectedSampleForDocs}
        />
      )}

      {selectedSampleForAssign && (
        <AssignAnalystModal
          isOpen={true}
          onClose={() => setSelectedSampleForAssign(null)}
          sampleCode={selectedSampleForAssign}
        />
      )}

      {/* Samples Table */}
      <div className="card">
        {filteredSamples.length === 0 ? (
          <EmptyState
            icon={<TestTube className="h-12 w-12" />}
            title="No hay muestras"
            description="No se encontraron muestras que coincidan con los filtros aplicados."
            action={
              hasRole(user?.role, ["SOLICITANTE", "ADMIN"]) ? (
                <Link to="/samples/create" className="btn-primary">
                  <Plus className="h-4 w-4 mr-2" />
                  Crear Primera Muestra
                </Link>
              ) : undefined
            }
          />
        ) : (
          <div className="overflow-x-auto">
            <table className="w-full">
              <thead>
                <tr className="border-b border-gray-200">
                  <th className="text-left py-3 px-4 font-medium text-gray-900">Código</th>
                  <th className="text-left py-3 px-4 font-medium text-gray-900">Nombre</th>
                  <th className="text-left py-3 px-4 font-medium text-gray-900">Tipo</th>
                  <th className="text-left py-3 px-4 font-medium text-gray-900">Origen</th>
                  <th className="text-left py-3 px-4 font-medium text-gray-900">Estado</th>
                  <th className="text-left py-3 px-4 font-medium text-gray-900">Fecha Recepción</th>
                  <th className="text-left py-3 px-4 font-medium text-gray-900">Acciones</th>
                </tr>
              </thead>
              <tbody>
                {filteredSamples.map((sample) => (
                  <tr key={sample.mstCodigo} className="border-b border-gray-100 hover:bg-gray-50">
                    <td className="py-3 px-4 font-medium text-gray-900">
                      {sample.mstCodigo}
                    </td>
                    <td className="py-3 px-4 text-gray-700">
                      {sample.nombre || "Sin nombre"}
                    </td>
                    <td className="py-3 px-4 text-gray-700">
                      Tipo {sample.tpmstId}
                    </td>
                    <td className="py-3 px-4 text-gray-700">
                      {sample.origen}
                    </td>
                    <td className="py-3 px-4">
                      <span className={`badge ${getStateColor(sample.estadoActual)}`}>
                        {getStateDisplayName(sample.estadoActual)}
                      </span>
                    </td>
                    <td className="py-3 px-4 text-gray-500">
                      {new Date(sample.fechaRecepcion).toLocaleDateString()}
                    </td>
                    <td className="py-3 px-4">
                      <div className="flex items-center space-x-2">
                        <Link
                          to={`/samples/${sample.mstCodigo}`}
                          className="text-primary-600 hover:text-primary-700 text-sm font-medium"
                        >
                          Ver detalles
                        </Link>
                        
                        {hasRole(user?.role, ["ANALISTA", "EVALUADOR", "ADMIN"]) && (
                          <button
                            onClick={() => setSelectedSampleForStatus({
                              code: sample.mstCodigo!,
                              status: sample.estadoActual
                            })}
                            className="p-1 text-gray-400 hover:text-warning-600 transition-colors"
                            title="Cambiar estado"
                          >
                            <Settings className="h-4 w-4" />
                          </button>
                        )}
                        
                        {hasRole(user?.role, ["ADMIN"]) && (
                          <button
                            onClick={() => setSelectedSampleForAssign(sample.mstCodigo!)}
                            className="p-1 text-gray-400 hover:text-primary-600 transition-colors"
                            title="Asignar analista"
                          >
                            <UserCheck className="h-4 w-4" />
                          </button>
                        )}
                        
                        <button
                          onClick={() => setSelectedSampleForDocs(sample.mstCodigo!)}
                          className="p-1 text-gray-400 hover:text-primary-600 transition-colors"
                          title="Ver documentos"
                        >
                          <FileText className="h-4 w-4" />
                        </button>
                      </div>
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
