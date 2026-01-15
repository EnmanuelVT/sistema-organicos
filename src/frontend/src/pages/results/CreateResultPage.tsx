import { useState, useEffect } from "react";
import { useForm } from "react-hook-form";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { useNavigate, useSearchParams, Link } from "react-router-dom";
import { createResult } from "@/api/results";
import { getAllSamples, getAssignedSamples } from "@/api/samples";
import { getTestsBySample } from "@/api/tests";
import { getParametersByTestId } from "@/api/parameters";
import { ArrowLeft, BarChart3 } from "lucide-react";
import LoadingSpinner from "@/components/LoadingSpinner";
import ErrorMessage from "@/components/ErrorMessage";
import { useAuthStore } from "@/store/auth";
import { hasRole } from "@/utils/roles";
import type { CreateResultadoPruebaDto } from "@/types/api";

interface FormValues {
  idMuestra: string;
  idPrueba?: number;
  idParametro?: number;
  valorObtenido: number;
  unidad: string;
}

export default function CreateResultPage() {
  const { user } = useAuthStore();
  const navigate = useNavigate();
  const queryClient = useQueryClient();
  const [searchParams] = useSearchParams();
  const [error, setError] = useState<string | null>(null);
  const [selectedSample, setSelectedSample] = useState<string>("");

  const { register, handleSubmit, setValue, watch, formState: { errors } } = useForm<FormValues>();
  const watchedSample = watch("idMuestra");
  const watchedTestId = watch("idPrueba");
  const selectedTestId = watchedTestId ?? 0;

  const { data: samples, isLoading: loadingSamples, error: samplesError } = useQuery({
    queryKey: ["samples", hasRole(user?.role, ["ADMIN"]) ? "all" : "assigned"],
    queryFn: () =>
      hasRole(user?.role, ["ADMIN"])
        ? getAllSamples()
        : getAssignedSamples(),
  });


  const { data: tests } = useQuery({
    queryKey: ["tests", watchedSample],
    queryFn: () => getTestsBySample(watchedSample),
    enabled: !!watchedSample,
  });

  const { data: parameters } = useQuery({
    queryKey: ["parameters", selectedTestId],
    queryFn: () => getParametersByTestId(selectedTestId),
    enabled: selectedTestId > 0,
  });

  const createMutation = useMutation({
    mutationFn: createResult,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["results"] });
      navigate("/results");
    },
    onError: (error: any) => {
      setError(error?.response?.data?.message || "Error al crear el resultado");
    },
  });

  // When test changes, clear parameter selection to avoid stale invalid value
  useEffect(() => {
    setValue("idParametro", undefined);
  }, [watchedTestId, setValue]);

  // Set defaults from URL params
  useEffect(() => {
    const sampleId = searchParams.get("sample");
    const testId = searchParams.get("test");
    
    if (sampleId) {
      setValue("idMuestra", sampleId);
      setSelectedSample(sampleId);
    }
    if (testId) {
      setValue("idPrueba", Number(testId));
    }
  }, [searchParams, setValue]);

  const onSubmit = (values: FormValues) => {
    setError(null);
    if (!values.idPrueba || !values.idParametro) {
      setError("Debe seleccionar una prueba y un parámetro");
      return;
    }
    const payload: CreateResultadoPruebaDto = {
      idMuestra: values.idMuestra,
      idPrueba: Number(values.idPrueba),
      idParametro: Number(values.idParametro),
      valorObtenido: Number(values.valorObtenido),
      unidad: values.unidad || null,
    };
    createMutation.mutate(payload);
  };

  return (
    <div className="max-w-2xl mx-auto space-y-6">
      <div className="flex items-center space-x-4">
        <Link to="/results" className="p-2 hover:bg-gray-100 rounded-lg transition-colors">
          <ArrowLeft className="h-5 w-5 text-gray-600" />
        </Link>
        <div>
          <h1 className="text-2xl font-bold text-gray-900">Nuevo Resultado</h1>
          <p className="text-gray-600 mt-1">Registrar resultado de análisis</p>
        </div>
      </div>

      {error && <ErrorMessage message={error} />}

      <div className="card">
        <div className="flex items-center space-x-3 mb-6">
          <div className="p-2 bg-warning-100 rounded-lg">
            <BarChart3 className="h-6 w-6 text-warning-600" />
          </div>
          <div>
            <h2 className="text-lg font-semibold text-gray-900">Información del Resultado</h2>
            <p className="text-sm text-gray-600">Complete los datos del resultado de análisis</p>
          </div>
        </div>

        <form onSubmit={handleSubmit(onSubmit)} className="space-y-6">
          <div>
            <label htmlFor="idMuestra" className="block text-sm font-medium text-gray-700 mb-2">
              Muestra *
            </label>
            <select
              {...register("idMuestra", { required: "Debe seleccionar una muestra" })}
              className="input"
            >
              <option value="">Seleccionar muestra</option>
              {samples?.map((sample) => (
                <option key={sample.mstCodigo} value={sample.mstCodigo!}>
                  {sample.mstCodigo} - {sample.nombre || "Sin nombre"}
                </option>
              ))}
            </select>
            {errors.idMuestra && (
              <p className="mt-1 text-sm text-error-600">{errors.idMuestra.message}</p>
            )}
            {loadingSamples && (
              <p className="mt-1 text-sm text-gray-500">Cargando muestras...</p>
            )}
          </div>

          {watchedSample && (
            <div>
              <label htmlFor="idPrueba" className="block text-sm font-medium text-gray-700 mb-2">
                Prueba *
              </label>
              <select
                {...register("idPrueba", {
                  required: "Debe seleccionar una prueba",
                  valueAsNumber: true,
                })}
                className="input"
              >
                <option value="">Seleccionar prueba</option>
                {tests?.map((test) => (
                  <option key={test.idPrueba} value={test.idPrueba}>
                    {test.nombrePrueba} (ID: {test.idPrueba})
                  </option>
                ))}
              </select>
              {errors.idPrueba && (
                <p className="mt-1 text-sm text-error-600">{errors.idPrueba.message}</p>
              )}
            </div>
          )}

          {selectedTestId > 0 && (
            <div>
              <label htmlFor="idParametro" className="block text-sm font-medium text-gray-700 mb-2">
                Parámetro *
              </label>
              <select
                {...register("idParametro", {
                  required: "Debe seleccionar un parámetro",
                  valueAsNumber: true,
                })}
                className="input"
              >
                <option value="">Seleccionar parámetro</option>
                {parameters?.map((param) => (
                  <option key={param.idParametro} value={param.idParametro}>
                    {param.nombreParametro} {param.unidad && `(${param.unidad})`}
                  </option>
                ))}
              </select>
              {errors.idParametro && (
                <p className="mt-1 text-sm text-error-600">{errors.idParametro.message}</p>
              )}
            </div>
          )}

          <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
            <div>
              <label htmlFor="valorObtenido" className="block text-sm font-medium text-gray-700 mb-2">
                Valor Obtenido *
              </label>
              <input
                {...register("valorObtenido", { 
                  required: "El valor es requerido",
                  valueAsNumber: true,
                })}
                type="number"
                step="any"
                className="input"
                placeholder="0.00"
              />
              {errors.valorObtenido && (
                <p className="mt-1 text-sm text-error-600">{errors.valorObtenido.message}</p>
              )}
            </div>

            <div>
              <label htmlFor="unidad" className="block text-sm font-medium text-gray-700 mb-2">
                Unidad
              </label>
              <input
                {...register("unidad")}
                type="text"
                className="input"
                placeholder="Ej: mg/L, UFC/mL, %"
              />
            </div>
          </div>

          <div className="flex justify-end space-x-4 pt-6 border-t border-gray-200">
            <Link to="/results" className="btn-secondary">
              Cancelar
            </Link>
            <button
              type="submit"
              disabled={createMutation.isPending}
              className="btn-primary"
            >
              {createMutation.isPending ? (
                <div className="flex items-center space-x-2">
                  <LoadingSpinner size="sm" />
                  <span>Guardando...</span>
                </div>
              ) : (
                "Guardar Resultado"
              )}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}
