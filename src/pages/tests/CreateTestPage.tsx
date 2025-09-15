import { useState, useEffect } from "react";
import { useForm } from "react-hook-form";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { useNavigate, useSearchParams, Link } from "react-router-dom";
import { createTest } from "@/api/tests";
import { getAllSamples } from "@/api/samples";
import { ArrowLeft, FlaskConical } from "lucide-react";
import LoadingSpinner from "@/components/LoadingSpinner";
import ErrorMessage from "@/components/ErrorMessage";
import type { CreatePruebaDto } from "@/types/api";

interface FormValues {
  idMuestra: string;
  nombrePrueba: string;
}

export default function CreateTestPage() {
  const navigate = useNavigate();
  const queryClient = useQueryClient();
  const [searchParams] = useSearchParams();
  const [error, setError] = useState<string | null>(null);

  const { register, handleSubmit, setValue, formState: { errors } } = useForm<FormValues>();

  const { data: samples, isLoading: loadingSamples } = useQuery({
    queryKey: ["samples"],
    queryFn: getAllSamples,
  });

  const createMutation = useMutation({
    mutationFn: createTest,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["tests"] });
      navigate("/tests");
    },
    onError: (error: any) => {
      setError(error?.response?.data?.message || "Error al crear la prueba");
    },
  });

  // Set default sample from URL params
  useEffect(() => {
    const sampleId = searchParams.get("sample");
    if (sampleId) {
      setValue("idMuestra", sampleId);
    }
  }, [searchParams, setValue]);

  const onSubmit = (values: FormValues) => {
    setError(null);
    const payload: CreatePruebaDto = {
      idMuestra: values.idMuestra,
      nombrePrueba: values.nombrePrueba,
    };
    createMutation.mutate(payload);
  };

  return (
    <div className="max-w-2xl mx-auto space-y-6">
      <div className="flex items-center space-x-4">
        <Link to="/tests" className="p-2 hover:bg-gray-100 rounded-lg transition-colors">
          <ArrowLeft className="h-5 w-5 text-gray-600" />
        </Link>
        <div>
          <h1 className="text-2xl font-bold text-gray-900">Nueva Prueba</h1>
          <p className="text-gray-600 mt-1">Crear una nueva prueba de laboratorio</p>
        </div>
      </div>

      {error && <ErrorMessage message={error} />}

      <div className="card">
        <div className="flex items-center space-x-3 mb-6">
          <div className="p-2 bg-success-100 rounded-lg">
            <FlaskConical className="h-6 w-6 text-success-600" />
          </div>
          <div>
            <h2 className="text-lg font-semibold text-gray-900">Información de la Prueba</h2>
            <p className="text-sm text-gray-600">Complete los datos de la nueva prueba</p>
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

          <div>
            <label htmlFor="nombrePrueba" className="block text-sm font-medium text-gray-700 mb-2">
              Nombre de la Prueba *
            </label>
            <input
              {...register("nombrePrueba", { required: "El nombre de la prueba es requerido" })}
              type="text"
              className="input"
              placeholder="Ej: Análisis microbiológico completo"
            />
            {errors.nombrePrueba && (
              <p className="mt-1 text-sm text-error-600">{errors.nombrePrueba.message}</p>
            )}
          </div>

          <div className="flex justify-end space-x-4 pt-6 border-t border-gray-200">
            <Link to="/tests" className="btn-secondary">
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
                  <span>Creando...</span>
                </div>
              ) : (
                "Crear Prueba"
              )}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}