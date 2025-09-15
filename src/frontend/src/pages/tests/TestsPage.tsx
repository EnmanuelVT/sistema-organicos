import { useQuery } from "@tanstack/react-query";
import { getAllSamples } from "@/api/samples";
import { getTestsBySample } from "@/api/tests";
import { FlaskConical, Plus, Search, FileText } from "lucide-react";
import { Link } from "react-router-dom";
import { useState } from "react";
import LoadingSpinner from "@/components/LoadingSpinner";
import ErrorMessage from "@/components/ErrorMessage";
import EmptyState from "@/components/EmptyState";
import DocumentsModal from "@/components/DocumentsModal";

export default function TestsPage() {
  const [searchTerm, setSearchTerm] = useState("");
  const [selectedSample, setSelectedSample] = useState<string>("");
  const [showDocumentsModal, setShowDocumentsModal] = useState(false);

  const { data: samples, isLoading: loadingSamples, error: samplesError } = useQuery({
    queryKey: ["samples"],
    queryFn: getAllSamples,
  });

  const { data: tests, isLoading: loadingTests, error: testsError } = useQuery({
    queryKey: ["tests", selectedSample],
    queryFn: () => getTestsBySample(selectedSample),
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
          <h1 className="text-2xl font-bold text-gray-900">Pruebas</h1>
          <p className="text-gray-600 mt-1">
            Gestión de pruebas de laboratorio
          </p>
        </div>
        
        <Link to="/tests/create" className="btn-primary">
          <Plus className="h-4 w-4 mr-2" />
          Nueva Prueba
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

        {/* Tests for Selected Sample */}
        <div className="card">
          <h2 className="text-lg font-semibold text-gray-900 mb-4">
            Pruebas {selectedSample && `- ${selectedSample}`}
          </h2>

          {!selectedSample ? (
            <div className="text-center py-8">
              <FlaskConical className="h-12 w-12 text-gray-400 mx-auto mb-4" />
              <p className="text-gray-500">Selecciona una muestra para ver sus pruebas</p>
            </div>
          ) : loadingTests ? (
            <div className="flex justify-center py-8">
              <LoadingSpinner />
            </div>
          ) : testsError ? (
            <ErrorMessage message="Error al cargar las pruebas" />
          ) : tests && tests.length > 0 ? (
            <div className="space-y-3">
              {tests.map((test) => (
                <div key={test.idPrueba} className="p-4 bg-gray-50 rounded-lg">
                  <div className="flex items-center justify-between">
                    <div>
                      <h3 className="font-medium text-gray-900">{test.nombrePrueba}</h3>
                      <p className="text-sm text-gray-500">ID: {test.idPrueba}</p>
                    </div>
                    <div className="flex items-center space-x-2">
                      <Link
                        to={`/results/create?test=${test.idPrueba}&sample=${selectedSample}`}
                        className="text-primary-600 hover:text-primary-700 text-sm font-medium"
                      >
                        Agregar Resultado
                      </Link>
                      <button
                        onClick={() => setShowDocumentsModal(true)}
                        className="p-1 text-gray-400 hover:text-primary-600 transition-colors"
                        title="Ver documentos"
                      >
                        <FileText className="h-4 w-4" />
                      </button>
                    </div>
                  </div>
                </div>
              ))}
            </div>
          ) : (
            <EmptyState
              icon={<FlaskConical className="h-12 w-12" />}
              title="No hay pruebas"
              description="Esta muestra no tiene pruebas registradas."
              action={
                <Link
                  to={`/tests/create?sample=${selectedSample}`}
                  className="btn-primary"
                >
                  <Plus className="h-4 w-4 mr-2" />
                  Crear Primera Prueba
                </Link>
              }
            />
          )}
        </div>
      </div>

      {/* Documents Modal */}
      {selectedSample && (
        <DocumentsModal
          isOpen={showDocumentsModal}
          onClose={() => setShowDocumentsModal(false)}
          sampleCode={selectedSample}
          tests={tests}
        />
      )}
    </div>
  );
}
