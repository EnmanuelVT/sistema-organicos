import { useParams, Link } from "react-router-dom";
import { useQuery } from "@tanstack/react-query";
import { getSampleById } from "@/api/samples";
import { getTestsBySample } from "@/api/tests";
import { getResultsBySample } from "@/api/results";
import { ArrowLeft, TestTube, FlaskConical, BarChart3, Calendar, MapPin, Settings, FileText } from "lucide-react";
import { Users } from "lucide-react";
import { getStateDisplayName, getStateColor } from "@/utils/roles";
import { useAuthStore } from "@/store/auth";
import { hasRole } from "@/utils/roles";
import LoadingSpinner from "@/components/LoadingSpinner";
import ErrorMessage from "@/components/ErrorMessage";
import ChangeStatusModal from "@/components/ChangeStatusModal";
import DocumentsModal from "@/components/DocumentsModal";
import AssignAnalystModal from "@/components/AssignAnalystModal";
import { useState } from "react";

export default function SampleDetailPage() {
  const { id } = useParams<{ id: string }>();
  const { user } = useAuthStore();
  const isSolicitante = user?.role === "SOLICITANTE";
  const [showStatusModal, setShowStatusModal] = useState(false);
  const [showDocumentsModal, setShowDocumentsModal] = useState(false);
  const [showAssignModal, setShowAssignModal] = useState(false);

  const { data: sample, isLoading: loadingSample, error: sampleError } = useQuery({
    queryKey: ["sample", id],
    queryFn: () => getSampleById(id!),
    enabled: !!id,
  });

  const { data: tests, isLoading: loadingTests } = useQuery({
    queryKey: ["tests", id],
    queryFn: () => getTestsBySample(id!),
    enabled: !!id && !isSolicitante,
  });

  const { data: results, isLoading: loadingResults } = useQuery({
    queryKey: ["results", id],
    queryFn: () => getResultsBySample(id!),
    enabled: !!id && !isSolicitante,
  });

  if (loadingSample) {
    return (
      <div className="flex items-center justify-center h-64">
        <LoadingSpinner size="lg" />
      </div>
    );
  }

  if (sampleError || !sample) {
    return (
      <ErrorMessage 
        message="Error al cargar la muestra o muestra no encontrada" 
      />
    );
  }

  return (
    <div className="space-y-6">
      <div className="flex items-center space-x-4">
        <Link to="/samples" className="p-2 hover:bg-gray-100 rounded-lg transition-colors">
          <ArrowLeft className="h-5 w-5 text-gray-600" />
        </Link>
        <div>
          <h1 className="text-2xl font-bold text-gray-900">
            Muestra {sample.mstCodigo}
          </h1>
          <p className="text-gray-600 mt-1">Detalles y seguimiento de la muestra</p>
        </div>
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
        {/* Sample Information */}
        <div className="lg:col-span-2 space-y-6">
          <div className="card">
            <div className="flex items-center space-x-3 mb-6">
              <div className="p-2 bg-primary-100 rounded-lg">
                <TestTube className="h-6 w-6 text-primary-600" />
              </div>
              <div>
                <h2 className="text-lg font-semibold text-gray-900">Información General</h2>
                <p className="text-sm text-gray-600">Datos básicos de la muestra</p>
              </div>
            </div>

            <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">
                  Código
                </label>
                <p className="text-sm text-gray-900 font-mono bg-gray-50 px-3 py-2 rounded-lg">
                  {sample.mstCodigo}
                </p>
              </div>

              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">
                  Estado
                </label>
                <span className={`badge ${getStateColor(sample.estadoActual)}`}>
                  {getStateDisplayName(sample.estadoActual)}
                </span>
              </div>

              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">
                  Nombre
                </label>
                <p className="text-sm text-gray-900">
                  {sample.nombre || "Sin nombre especificado"}
                </p>
              </div>

              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">
                  Tipo
                </label>
                <p className="text-sm text-gray-900">
                  {getSampleTypeName(sample.tpmstId)}
                </p>
              </div>

              <div className="md:col-span-2">
                <label className="block text-sm font-medium text-gray-700 mb-1">
                  Origen
                </label>
                <p className="text-sm text-gray-900 flex items-center">
                  <MapPin className="h-4 w-4 text-gray-400 mr-1" />
                  {sample.origen}
                </p>
              </div>

              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">
                  Fecha de Recepción
                </label>
                <p className="text-sm text-gray-900 flex items-center">
                  <Calendar className="h-4 w-4 text-gray-400 mr-1" />
                  {new Date(sample.fechaRecepcion).toLocaleDateString()}
                </p>
              </div>

              {sample.fechaSalidaEstimada && (
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1">
                    Fecha de Salida Estimada
                  </label>
                  <p className="text-sm text-gray-900 flex items-center">
                    <Calendar className="h-4 w-4 text-gray-400 mr-1" />
                    {new Date(sample.fechaSalidaEstimada).toLocaleDateString()}
                  </p>
                </div>
              )}
            </div>

            {(sample.condicionesAlmacenamiento || sample.condicionesTransporte) && (
              <div className="mt-6 pt-6 border-t border-gray-200">
                {sample.condicionesAlmacenamiento && (
                  <div className="mb-4">
                    <label className="block text-sm font-medium text-gray-700 mb-1">
                      Condiciones de Almacenamiento
                    </label>
                    <p className="text-sm text-gray-900 bg-gray-50 px-3 py-2 rounded-lg">
                      {sample.condicionesAlmacenamiento}
                    </p>
                  </div>
                )}

                {sample.condicionesTransporte && (
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-1">
                      Condiciones de Transporte
                    </label>
                    <p className="text-sm text-gray-900 bg-gray-50 px-3 py-2 rounded-lg">
                      {sample.condicionesTransporte}
                    </p>
                  </div>
                )}
              </div>
            )}
          </div>

          {/* Tests */}
          {!isSolicitante && (
          <div className="card">
            <div className="flex items-center justify-between mb-6">
              <div className="flex items-center space-x-3">
                <div className="p-2 bg-success-100 rounded-lg">
                  <FlaskConical className="h-6 w-6 text-success-600" />
                </div>
                <div>
                  <h2 className="text-lg font-semibold text-gray-900">Pruebas</h2>
                  <p className="text-sm text-gray-600">Pruebas realizadas a esta muestra</p>
                </div>
              </div>
            </div>

            {loadingTests ? (
              <div className="flex justify-center py-8">
                <LoadingSpinner />
              </div>
            ) : tests && tests.length > 0 ? (
              <div className="space-y-3">
                {tests.map((test) => (
                  <div key={test.idPrueba} className="p-4 bg-gray-50 rounded-lg">
                    <div className="flex items-center justify-between">
                      <div>
                        <h3 className="font-medium text-gray-900">{test.nombrePrueba}</h3>
                        <p className="text-sm text-gray-500">ID: {test.idPrueba}</p>
                      </div>
                    </div>
                  </div>
                ))}
              </div>
            ) : (
              <div className="text-center py-8">
                <FlaskConical className="h-12 w-12 text-gray-400 mx-auto mb-4" />
                <p className="text-gray-500">No hay pruebas registradas para esta muestra</p>
              </div>
            )}
          </div>
          )}

          {/* Results */}
          {!isSolicitante && (
          <div className="card">
            <div className="flex items-center space-x-3 mb-6">
              <div className="p-2 bg-warning-100 rounded-lg">
                <BarChart3 className="h-6 w-6 text-warning-600" />
              </div>
              <div>
                <h2 className="text-lg font-semibold text-gray-900">Resultados</h2>
                <p className="text-sm text-gray-600">Resultados de análisis obtenidos</p>
              </div>
            </div>

            {loadingResults ? (
              <div className="flex justify-center py-8">
                <LoadingSpinner />
              </div>
            ) : results && results.length > 0 ? (
              <div className="space-y-3">
                {results.map((result) => (
                  <div key={result.idResultado} className="p-4 bg-gray-50 rounded-lg">
                    <div className="flex items-center justify-between">
                      <div>
                        <h3 className="font-medium text-gray-900">
                          Parámetro {result.nombreParametro || result.idParametro}
                        </h3>
                        <p className="text-sm text-gray-500">
                          Valor: {result.valorObtenido} {result.unidad}
                        </p>
                      </div>
                      {result.cumpleNorma !== null && (
                        <span className={`badge ${result.cumpleNorma ? 'badge-success' : 'badge-error'}`}>
                          {result.cumpleNorma ? 'Cumple norma' : 'No cumple norma'}
                        </span>
                      )}
                    </div>
                  </div>
                ))}
              </div>
            ) : (
              <div className="text-center py-8">
                <BarChart3 className="h-12 w-12 text-gray-400 mx-auto mb-4" />
                <p className="text-gray-500">No hay resultados registrados para esta muestra</p>
              </div>
            )}
          </div>
          )}
        </div>

        {/* Sidebar */}
        <div className="space-y-6">
          <div className="card">
            <h3 className="font-semibold text-gray-900 mb-4">Acciones</h3>
            <div className="space-y-2">
              {hasRole(user?.role, ["ANALISTA", "ADMIN"]) && (
                <>
                  <Link
                    to={`/tests/create?sample=${sample.mstCodigo}`}
                    className="w-full btn-primary text-center"
                  >
                    <FlaskConical className="h-4 w-4 mr-2" />
                    Nueva Prueba
                  </Link>
                  <Link
                    to={`/results/create?sample=${sample.mstCodigo}`}
                    className="w-full btn-secondary text-center"
                  >
                    <BarChart3 className="h-4 w-4 mr-2" />
                    Nuevo Resultado
                  </Link>
                </>
              )}
              
              {hasRole(user?.role, ["ANALISTA", "EVALUADOR", "ADMIN"]) && (
                <button
                  onClick={() => setShowStatusModal(true)}
                  className="w-full btn-secondary text-center"
                >
                  <Settings className="h-4 w-4 mr-2" />
                  Cambiar Estado
                </button>
              )}
              
              {hasRole(user?.role, ["ADMIN"]) && (
                <button
                  onClick={() => setShowAssignModal(true)}
                  className="w-full btn-secondary text-center"
                >
                  <Users className="h-4 w-4 mr-2" />
                  Asignar Analista
                </button>
              )}
              
              <button
                onClick={() => setShowDocumentsModal(true)}
                className="w-full btn-secondary text-center"
              >
                <FileText className="h-4 w-4 mr-2" />
                Ver Documentos
              </button>
            </div>
          </div>

          <div className="card">
            <h3 className="font-semibold text-gray-900 mb-4">Información Técnica</h3>
            <div className="space-y-3 text-sm">
              <div>
                <span className="font-medium text-gray-700">ID Tipo:</span>
                <span className="ml-2 text-gray-900">{sample.tpmstId}</span>
              </div>
              <div>
                <span className="font-medium text-gray-700">Estado Actual:</span>
                <span className="ml-2 text-gray-900">{sample.estadoActual}</span>
              </div>
              <div>
                <span className="font-medium text-gray-700">Fecha Registro:</span>
                <span className="ml-2 text-gray-900">
                  {new Date(sample.fechaRecepcion).toLocaleString()}
                </span>
              </div>
            </div>
          </div>
        </div>
      </div>

      {/* Modals */}
      <ChangeStatusModal
        isOpen={showStatusModal}
        onClose={() => setShowStatusModal(false)}
        sampleCode={sample.mstCodigo!}
        currentStatus={sample.estadoActual}
      />

      <AssignAnalystModal
        isOpen={showAssignModal}
        onClose={() => setShowAssignModal(false)}
        sampleCode={sample.mstCodigo!}
      />

      <DocumentsModal
        isOpen={showDocumentsModal}
        onClose={() => setShowDocumentsModal(false)}
        sampleCode={sample.mstCodigo!}
        tests={tests}
      />
    </div>
  );
}

function getSampleTypeName(typeId: number): string {
  const types: Record<number, string> = {
    1: "Agua",
    2: "Alimento",
    3: "Bebida Alcohólica",
  };
  return types[typeId] || `Tipo ${typeId}`;
}
