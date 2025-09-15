import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { getAllSamples } from "@/api/samples";
import { getResultsBySample } from "@/api/results";
import { evaluateTest } from "@/api/tests";
import { CheckCircle, XCircle, Search, AlertTriangle } from "lucide-react";
import { useState } from "react";
import { useForm } from "react-hook-form";
import LoadingSpinner from "@/components/LoadingSpinner";
import ErrorMessage from "@/components/ErrorMessage";
import EmptyState from "@/components/EmptyState";

interface EvaluationForm {
  observaciones: string;
}

export default function EvaluationPage() {
  const queryClient = useQueryClient();
  const [searchTerm, setSearchTerm] = useState("");
  const [selectedSample, setSelectedSample] = useState<string>("");
  const [error, setError] = useState<string | null>(null);

  const { register, handleSubmit, reset } = useForm<EvaluationForm>();

  const { data: samples, isLoading: loadingSamples } = useQuery({
    queryKey: ["samples"],
    queryFn: getAllSamples,
  });

  const { data: results, isLoading: loadingResults } = useQuery({
    queryKey: ["results", selectedSample],
    queryFn: () => getResultsBySample(selectedSample),
    enabled: !!selectedSample,
  });

  const evaluateMutation = useMutation({
    mutationFn: ({ testId, approved, observaciones }: { 
      testId: number; 
      approved: boolean; 
      observaciones?: string 
    }) => evaluateTest(testId, { 
      idPrueba: testId, 
      aprobado: approved, 
      observaciones 
    }),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["samples"] });
      queryClient.invalidateQueries({ queryKey: ["results"] });
      setSelectedSample("");
      reset();
      setError(null);
    },
    onError: (error: any) => {
      setError(error?.response?.data?.message || "Error al evaluar la muestra");
    },
  });

  // Filter samples that are ready for evaluation (state 3 = "En espera")
  const samplesForEvaluation = samples?.filter(s => s.estadoActual === 3) || [];
  
  const filteredSamples = samplesForEvaluation.filter((sample) => {
    return !searchTerm || 
      sample.mstCodigo?.toLowerCase().includes(searchTerm.toLowerCase()) ||
      sample.nombre?.toLowerCase().includes(searchTerm.toLowerCase());
  });

  const handleEvaluation = (approved: boolean) => {
    return handleSubmit((data) => {
      if (!selectedSample || !results?.length) return;
      
      // For simplicity, we'll evaluate the first test found in results
      const firstResult = results[0];
      if (firstResult) {
        evaluateMutation.mutate({
          testId: firstResult.idPrueba,
          approved,
          observaciones: data.observaciones || undefined,
        });
      }
    });
  };

  if (loadingSamples) {
    return (
      <div className="flex items-center justify-center h-64">
        <LoadingSpinner size="lg" />
      </div>
    );
  }

  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-2xl font-bold text-gray-900">Evaluación de Muestras</h1>
        <p className="text-gray-600 mt-1">
          Revisar y aprobar muestras que han completado el análisis
        </p>
      </div>

      {error && <ErrorMessage message={error} />}

      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        {/* Samples for Evaluation */}
        <div className="card">
          <h2 className="text-lg font-semibold text-gray-900 mb-4">
            Muestras Pendientes de Evaluación
          </h2>
          
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
                <div className="flex items-center justify-between">
                  <div>
                    <div className="font-medium text-gray-900">{sample.mstCodigo}</div>
                    <div className="text-sm text-gray-600">{sample.nombre || "Sin nombre"}</div>
                    <div className="text-xs text-gray-500 mt-1">
                      {new Date(sample.fechaRecepcion).toLocaleDateString()}
                    </div>
                  </div>
                  <AlertTriangle className="h-5 w-5 text-warning-500" />
                </div>
              </button>
            ))}
          </div>

          {filteredSamples.length === 0 && (
            <EmptyState
              icon={<CheckCircle className="h-12 w-12" />}
              title="No hay muestras pendientes"
              description="No hay muestras esperando evaluación en este momento."
            />
          )}
        </div>

        {/* Evaluation Panel */}
        <div className="card">
          <h2 className="text-lg font-semibold text-gray-900 mb-4">
            Evaluación {selectedSample && `- ${selectedSample}`}
          </h2>

          {!selectedSample ? (
            <div className="text-center py-8">
              <CheckCircle className="h-12 w-12 text-gray-400 mx-auto mb-4" />
              <p className="text-gray-500">Selecciona una muestra para evaluar</p>
            </div>
          ) : loadingResults ? (
            <div className="flex justify-center py-8">
              <LoadingSpinner />
            </div>
          ) : results && results.length > 0 ? (
            <div className="space-y-6">
              {/* Results Review */}
              <div>
                <h3 className="font-medium text-gray-900 mb-3">Resultados para Revisar</h3>
                <div className="space-y-3">
                  {results.map((result) => (
                    <div key={result.idResultado} className="p-4 bg-gray-50 rounded-lg">
                      <div className="flex items-center justify-between">
                        <div>
                          <h4 className="font-medium text-gray-900">
                            Parámetro {result.idParametro}
                          </h4>
                          <p className="text-sm text-gray-600">
                            Valor: {result.valorObtenido} {result.unidad}
                          </p>
                          <p className="text-xs text-gray-500">
                            Registrado: {new Date(result.fechaRegistro).toLocaleString()}
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
              </div>

              {/* Evaluation Form */}
              <form className="space-y-4">
                <div>
                  <label htmlFor="observaciones" className="block text-sm font-medium text-gray-700 mb-2">
                    Observaciones
                  </label>
                  <textarea
                    {...register("observaciones")}
                    rows={4}
                    className="input"
                    placeholder="Ingrese observaciones sobre la evaluación (requerido para rechazo)"
                  />
                </div>

                <div className="flex space-x-4">
                  <button
                    type="button"
                    onClick={handleEvaluation(true)}
                    disabled={evaluateMutation.isPending}
                    className="flex-1 btn-success"
                  >
                    {evaluateMutation.isPending ? (
                      <LoadingSpinner size="sm" />
                    ) : (
                      <>
                        <CheckCircle className="h-4 w-4 mr-2" />
                        Aprobar
                      </>
                    )}
                  </button>
                  
                  <button
                    type="button"
                    onClick={handleEvaluation(false)}
                    disabled={evaluateMutation.isPending}
                    className="flex-1 btn-error"
                  >
                    {evaluateMutation.isPending ? (
                      <LoadingSpinner size="sm" />
                    ) : (
                      <>
                        <XCircle className="h-4 w-4 mr-2" />
                        Rechazar
                      </>
                    )}
                  </button>
                </div>
              </form>
            </div>
          ) : (
            <div className="text-center py-8">
              <BarChart3 className="h-12 w-12 text-gray-400 mx-auto mb-4" />
              <p className="text-gray-500">Esta muestra no tiene resultados para evaluar</p>
            </div>
          )}
        </div>
      </div>
    </div>
  );
}