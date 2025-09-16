import { useState } from "react";
import { useForm } from "react-hook-form";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { useNavigate } from "react-router-dom";
import { createSample } from "@/api/samples";
import { ArrowLeft, TestTube } from "lucide-react";
import { Link } from "react-router-dom";
import LoadingSpinner from "@/components/LoadingSpinner";
import ErrorMessage from "@/components/ErrorMessage";
import { useAuthStore } from "@/store/auth"
import type { CreateMuestraDto } from "@/types/api";

interface FormValues {
  tpmstId: number;
  nombre: string;
  origen: string;
  condicionesAlmacenamiento: string;
  condicionesTransporte: string;
}

export default function CreateSamplePage() {
  const navigate = useNavigate();
  const queryClient = useQueryClient();
  const { user } = useAuthStore(); // <— leer rol de usuario
  const [error, setError] = useState<string | null>(null);

  const { register, handleSubmit, formState: { errors } } = useForm<FormValues>();


  const createMutation = useMutation({
    mutationFn: createSample,
    onSuccess: (data) => {
      // refrescar listado
      queryClient.invalidateQueries({ queryKey: ["samples"] });

      // REGLA:
      // - SOLICITANTE → vuelve a la lista de “Mis muestras”
      // - Otros roles → va al detalle recien creado
      if (user?.role === "SOLICITANTE") {
        navigate("/samples", { replace: true });
      } else {
        // data.mstCodigo es el ID (string)
        navigate(`/samples/${data.mstCodigo}`, { replace: true });
      }
    },
    onError: (error: any) => {
      setError(error?.response?.data?.message || "Error al crear la muestra");
    },
  });


  const onSubmit = (values: FormValues) => {
    setError(null);
    const payload: CreateMuestraDto = {
      tpmstId: Number(values.tpmstId),
      nombre: values.nombre || null,
      origen: values.origen || null,
      condicionesAlmacenamiento: values.condicionesAlmacenamiento || null,
      condicionesTransporte: values.condicionesTransporte || null,
    };
    createMutation.mutate(payload);
  };

  return (
    <div className="max-w-2xl mx-auto space-y-6">
      <div className="flex items-center space-x-4">
        <Link to="/samples" className="p-2 hover:bg-gray-100 rounded-lg transition-colors">
          <ArrowLeft className="h-5 w-5 text-gray-600" />
        </Link>
        <div>
          <h1 className="text-2xl font-bold text-gray-900">Nueva Muestra</h1>
          <p className="text-gray-600 mt-1">Registrar una nueva muestra para análisis</p>
        </div>
      </div>

      {error && <ErrorMessage message={error} />}

      <div className="card">
        <div className="flex items-center space-x-3 mb-6">
          <div className="p-2 bg-primary-100 rounded-lg">
            <TestTube className="h-6 w-6 text-primary-600" />
          </div>
          <div>
            <h2 className="text-lg font-semibold text-gray-900">Información de la Muestra</h2>
            <p className="text-sm text-gray-600">Complete los datos básicos de la muestra</p>
          </div>
        </div>

        <form onSubmit={handleSubmit(onSubmit)} className="space-y-6">
          <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
            <div>
              <label htmlFor="tpmstId" className="block text-sm font-medium text-gray-700 mb-2">
                Tipo de Muestra *
              </label>
              <select
                {...register("tpmstId", { required: "El tipo de muestra es requerido" })}
                className="input"
              >
                <option value="">Seleccionar tipo</option>
                <option value={1}>Agua</option>
                <option value={2}>Alimento</option>
                <option value={3}>Bebida Alcohólica</option>
              </select>
              {errors.tpmstId && (
                <p className="mt-1 text-sm text-error-600">{errors.tpmstId.message}</p>
              )}
            </div>

            <div>
              <label htmlFor="nombre" className="block text-sm font-medium text-gray-700 mb-2">
                Nombre de la Muestra
              </label>
              <input
                {...register("nombre")}
                type="text"
                className="input"
                placeholder="Ej: Muestra de agua potable"
              />
            </div>
          </div>

          <div>
            <label htmlFor="origen" className="block text-sm font-medium text-gray-700 mb-2">
              Origen *
            </label>
            <input
              {...register("origen", { required: "El origen es requerido" })}
              type="text"
              className="input"
              placeholder="Ej: Planta de tratamiento Norte"
            />
            {errors.origen && (
              <p className="mt-1 text-sm text-error-600">{errors.origen.message}</p>
            )}
          </div>

          <div>
            <label htmlFor="condicionesAlmacenamiento" className="block text-sm font-medium text-gray-700 mb-2">
              Condiciones de Almacenamiento
            </label>
            <textarea
              {...register("condicionesAlmacenamiento")}
              rows={3}
              className="input"
              placeholder="Ej: Refrigerado a 4°C, contenedor estéril"
            />
          </div>

          <div>
            <label htmlFor="condicionesTransporte" className="block text-sm font-medium text-gray-700 mb-2">
              Condiciones de Transporte
            </label>
            <textarea
              {...register("condicionesTransporte")}
              rows={3}
              className="input"
              placeholder="Ej: Cadena de frío mantenida, tiempo máximo 2 horas"
            />
          </div>

          <div className="flex justify-end space-x-4 pt-6 border-t border-gray-200">
            <Link to="/samples" className="btn-secondary">
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
                "Crear Muestra"
              )}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}
