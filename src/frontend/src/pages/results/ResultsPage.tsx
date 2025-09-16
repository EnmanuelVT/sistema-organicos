import { useQuery } from "@tanstack/react-query";
import { getAllSamples, getAssignedSamples } from "@/api/samples";
import { getResultsBySample } from "@/api/results";
import { BarChart3, Plus, Search } from "lucide-react";
import { Link } from "react-router-dom";
import { useState } from "react";
import LoadingSpinner from "@/components/LoadingSpinner";
import ErrorMessage from "@/components/ErrorMessage";
import EmptyState from "@/components/EmptyState";
import { useAuthStore } from "@/store/auth";
import { hasRole } from "@/utils/roles";

export default function ResultsPage() {
  const { user } = useAuthStore();
  const [searchTerm, setSearchTerm] = useState("");
  const [selectedSample, setSelectedSample] = useState<string>("");

  const { data: samples, isLoading: loadingSamples, error: samplesError } = useQuery({
    queryKey: ["samples", hasRole(user?.role, ["ADMIN"]) ? "all" : "assigned"],
    queryFn: () =>
      hasRole(user?.role, ["ADMIN"])
        ? getAllSamples()
        : getAssignedSamples(),
  });

  const { data: results, isLoading: loadingResults, error: resultsError } = useQuery({
    queryKey: ["results", selectedSample],
    queryFn: () => getResultsBySample(selectedSample),
    enabled: !!selectedSample,
  });

  const filteredSamples = samples?.filter((sample) => {
    return !searchTerm || 
      sample.mstCodigo?.toLowerCase().includes(searchTerm.toLowerCase()) ||
      sample.nombre?.toLowerCase().includes(searchTerm.toLowerCase());
  }) || [];

  if (loadingSamples) {
    return (
      <div className="flex items-center justify-center h-64">
        <LoadingSpinner size="lg" />
      </div>
    );
  }

  if (samplesError) {
    return (
      <ErrorMessage 
        message={(samplesError as any)?.message || "Error al cargar las muestras"} 
      />
    );
  }

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold text-gray-900">Resultados</h1>
          <p className="text-gray-600 mt-1">
            Gestión de resultados de análisis
          </p>
        </div>
        
        <Link to="/results/create" className="btn-primary">
          <Plus className="h-4 w-4 mr-2" />
          Nuevo Resultado
        </Link>
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        {/* Samples List */}
        <div className="card">
          <h2 className="text-lg font-semibold text-gray-900 mb-4">Seleccionar Muestra</h2>
          
          <div className="mb-4">
            <div className="relative">
              <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 h-4 w-4 text-gray-400" />
              <input
                type="text"
                placeholder="Buscar muestra..."
                value={searchTerm}
                onChange={(e) => setSearchTerm(e.target.value)}
                className="input pl-10"
              />
            </div>
          </div>

          <div className="space-y-2 max-h-96 overflow-y-auto">
            {filteredSamples.map((sample) => (
              <button
                key={sample.mstCodigo}
                onClick={() => setSelectedSample(sample.mstCodigo!)}
                className={`w-full text-left p-3 rounded-lg border transition-colors ${
                  selectedSample === sample.mstCodigo
                    ? "border-primary-300 bg-primary-50"
                    : "border-gray-200 hover:bg-gray-50"
                }`}
              >
                <div className="font-medium text-gray-900">{sample.mstCodigo}</div>
                <div className="text-sm text-gray-600">{sample.nombre || "Sin nombre"}</div>
                <div className="text-xs text-gray-500 mt-1">
                  {new Date(sample.fechaRecepcion).toLocaleDateString()}
                </div>
              </button>
            ))}
          </div>

          {filteredSamples.length === 0 && (
            <div className="text-center py-8">
              <p className="text-gray-500">No se encontraron muestras</p>
            </div>
          )}
        </div>

        {/* Results for Selected Sample */}
        <div className="card">
          <h2 className="text-lg font-semibold text-gray-900 mb-4">
            Resultados {selectedSample && `- ${selectedSample}`}
          </h2>

          {!selectedSample ? (
            <div className="text-center py-8">
              <BarChart3 className="h-12 w-12 text-gray-400 mx-auto mb-4" />
              <p className="text-gray-500">Selecciona una muestra para ver sus resultados</p>
            </div>
          ) : loadingResults ? (
            <div className="flex justify-center py-8">
              <LoadingSpinner />
            </div>
          ) : resultsError ? (
            <ErrorMessage message="Error al cargar los resultados" />
          ) : results && results.length > 0 ? (
            <div className="space-y-3">
              {results.map((result) => (
                <div key={result.idResultado} className="p-4 bg-gray-50 rounded-lg">
                  <div className="flex items-center justify-between">
                    <div>
                      <h3 className="font-medium text-gray-900">
                        Parámetro {result.idParametro}
                      </h3>
                      <p className="text-sm text-gray-500">
                        Valor: {result.valorObtenido} {result.unidad}
                      </p>
                      <p className="text-xs text-gray-400">
                        {new Date(result.fechaRegistro).toLocaleString()}
                      </p>
                    </div>
                    {result.cumpleNorma !== null && (
                      <span className={`badge ${result.cumpleNorma ? 'badge-success' : 'badge-error'}`}>
                        {result.cumpleNorma ? 'Cumple' : 'No cumple'}
                      </span>
                    )}
                  </div>
                </div>
              ))}
            </div>
          ) : (
            <EmptyState
              icon={<BarChart3 className="h-12 w-12" />}
              title="No hay resultados"
              description="Esta muestra no tiene resultados registrados."
              action={
                <Link
                  to={`/results/create?sample=${selectedSample}`}
                  className="btn-primary"
                >
                  <Plus className="h-4 w-4 mr-2" />
                  Crear Primer Resultado
                </Link>
              }
            />
          )}
        </div>
      </div>
    </div>
  );
}
