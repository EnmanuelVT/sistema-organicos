import { useState } from "react";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { useForm } from "react-hook-form";
import { getParametersBySampleType, addParameterToSampleType } from "@/api/parameters";
import { getTestTypes } from "@/api/tests";
import { Settings, Plus } from "lucide-react";
import LoadingSpinner from "@/components/LoadingSpinner";
import ErrorMessage from "@/components/ErrorMessage";
import EmptyState from "@/components/EmptyState";
import type { CreateParametroDto } from "@/types/api";

interface ParameterForm {
  tpmstId: number;
  tipoPruebaId: number;
  nombreParametro: string;
  valorMin: number;
  valorMax: number;
  unidad: string;
}

export default function ParametersPage() {
  const queryClient = useQueryClient();
  const [selectedType, setSelectedType] = useState<number>(1);
  const [showCreateForm, setShowCreateForm] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const { register, handleSubmit, reset, formState: { errors } } = useForm<ParameterForm>();

  const { data: testTypes, isLoading: loadingTestTypes } = useQuery({
    queryKey: ["testTypes"],
    queryFn: () => getTestTypes(),
  });

  const { data: parameters, isLoading, error: parametersError } = useQuery({
    queryKey: ["parameters", selectedType],
    queryFn: () => getParametersBySampleType(selectedType),
    enabled: !!selectedType,
  });

  const createMutation = useMutation({
    mutationFn: addParameterToSampleType,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["parameters", selectedType] });
      setShowCreateForm(false);
      reset();
      setError(null);
    },
    onError: (error: any) => {
      setError(error?.response?.data?.message || "Error al crear el parámetro");
    },
  });

  const onSubmit = (values: ParameterForm) => {
    setError(null);
    const payload: CreateParametroDto = {
      tpmstId: selectedType,
      tipoPruebaId: values.tipoPruebaId,
      nombreParametro: values.nombreParametro,
      valorMin: values.valorMin || null,
      valorMax: values.valorMax || null,
      unidad: values.unidad || null,
    };
    createMutation.mutate(payload);
  };

  const sampleTypes = [
    { id: 1, name: "Agua" },
    { id: 2, name: "Alimento" },
    { id: 3, name: "Bebida Alcohólica" },
  ];

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold text-gray-900">Gestión de Parámetros</h1>
          <p className="text-gray-600 mt-1">
            Configurar parámetros de análisis por tipo de muestra
          </p>
        </div>
        
        <button
          onClick={() => setShowCreateForm(true)}
          className="btn-primary"
        >
          <Plus className="h-4 w-4 mr-2" />
          Nuevo Parámetro
        </button>
      </div>

      {error && <ErrorMessage message={error} />}

      {/* Create Form */}
      {showCreateForm && (
        <div className="card">
          <h2 className="text-lg font-semibold text-gray-900 mb-4">
            Crear Nuevo Parámetro
          </h2>

          <form onSubmit={handleSubmit(onSubmit)} className="space-y-6">
            <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-2">
                  Tipo de Muestra *
                </label>
                <select
                  value={selectedType}
                  onChange={(e) => setSelectedType(Number(e.target.value))}
                  className="input"
                >
                  {sampleTypes.map((type) => (
                    <option key={type.id} value={type.id}>
                      {type.name}
                    </option>
                  ))}
                </select>
              </div>

              <div>
                <label className="block text-sm font-medium text-gray-700 mb-2">
                  Nombre del Parámetro *
                </label>
                <input
                  {...register("nombreParametro", { required: "El nombre es requerido" })}
                  type="text"
                  className="input"
                  placeholder="Ej: pH, Coliformes totales"
                />
                {errors.nombreParametro && (
                  <p className="mt-1 text-sm text-error-600">{errors.nombreParametro.message}</p>
                )}
              </div>

              <div>
                <label className="block text-sm font-medium text-gray-700 mb-2">
                  Tipo de Prueba *
                </label>
                <select
                  {...register("tipoPruebaId", {
                    valueAsNumber: true,
                    validate: (v) => (Number.isFinite(v) && v > 0) || "Debe seleccionar un tipo de prueba",
                  })}
                  className="input"
                  defaultValue=""
                >
                  <option value="">Seleccionar tipo</option>
                  {testTypes?.map((t) => (
                    <option key={t.idTipoPrueba} value={t.idTipoPrueba}>
                      {t.nombre} ({t.codigo})
                    </option>
                  ))}
                </select>
                {errors.tipoPruebaId && (
                  <p className="mt-1 text-sm text-error-600">{errors.tipoPruebaId.message}</p>
                )}
                {loadingTestTypes && (
                  <p className="mt-1 text-sm text-gray-500">Cargando tipos de prueba...</p>
                )}
              </div>

              <div>
                <label className="block text-sm font-medium text-gray-700 mb-2">
                  Valor Mínimo
                </label>
                <input
                  {...register("valorMin", { valueAsNumber: true })}
                  type="number"
                  step="any"
                  className="input"
                  placeholder="0"
                />
              </div>

              <div>
                <label className="block text-sm font-medium text-gray-700 mb-2">
                  Valor Máximo
                </label>
                <input
                  {...register("valorMax", { valueAsNumber: true })}
                  type="number"
                  step="any"
                  className="input"
                  placeholder="100"
                />
              </div>

              <div className="md:col-span-2">
                <label className="block text-sm font-medium text-gray-700 mb-2">
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
              <button
                type="button"
                onClick={() => {
                  setShowCreateForm(false);
                  reset();
                  setError(null);
                }}
                className="btn-secondary"
              >
                Cancelar
              </button>
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
                  "Crear Parámetro"
                )}
              </button>
            </div>
          </form>
        </div>
      )}

      {/* Sample Type Selector */}
      <div className="card">
        <h2 className="text-lg font-semibold text-gray-900 mb-4">Tipo de Muestra</h2>
        <div className="flex space-x-2">
          {sampleTypes.map((type) => (
            <button
              key={type.id}
              onClick={() => setSelectedType(type.id)}
              className={`px-4 py-2 rounded-lg text-sm font-medium transition-colors ${
                selectedType === type.id
                  ? "bg-primary-600 text-white"
                  : "bg-gray-100 text-gray-700 hover:bg-gray-200"
              }`}
            >
              {type.name}
            </button>
          ))}
        </div>
      </div>

      {/* Parameters Table */}
      <div className="card">
        <h2 className="text-lg font-semibold text-gray-900 mb-4">
          Parámetros para {sampleTypes.find(t => t.id === selectedType)?.name}
        </h2>

        {isLoading ? (
          <div className="flex justify-center py-8">
            <LoadingSpinner />
          </div>
        ) : parametersError ? (
          <ErrorMessage message="Error al cargar los parámetros" />
        ) : parameters && parameters.length > 0 ? (
          <div className="overflow-x-auto">
            <table className="w-full">
              <thead>
                <tr className="border-b border-gray-200">
                  <th className="text-left py-3 px-4 font-medium text-gray-900">ID</th>
                  <th className="text-left py-3 px-4 font-medium text-gray-900">Nombre</th>
                  <th className="text-left py-3 px-4 font-medium text-gray-900">Tipo Prueba</th>
                  <th className="text-left py-3 px-4 font-medium text-gray-900">Rango</th>
                  <th className="text-left py-3 px-4 font-medium text-gray-900">Unidad</th>
                </tr>
              </thead>
              <tbody>
                {parameters.map((param) => (
                  <tr key={param.idParametro} className="border-b border-gray-100 hover:bg-gray-50">
                    <td className="py-3 px-4 font-medium text-gray-900">
                      {param.idParametro}
                    </td>
                    <td className="py-3 px-4 text-gray-700">
                      {param.nombreParametro}
                    </td>
                    <td className="py-3 px-4 text-gray-700">
                      {param.tipoPruebaId
                        ? (testTypes?.find(t => t.idTipoPrueba === param.tipoPruebaId)?.nombre ?? param.tipoPruebaId)
                        : "(sin tipo)"}
                    </td>
                    <td className="py-3 px-4 text-gray-700">
                      {param.valorMin !== null && param.valorMax !== null
                        ? `${param.valorMin} - ${param.valorMax}`
                        : param.valorMin !== null
                        ? `≥ ${param.valorMin}`
                        : param.valorMax !== null
                        ? `≤ ${param.valorMax}`
                        : "Sin rango"
                      }
                    </td>
                    <td className="py-3 px-4 text-gray-700">
                      {param.unidad || "Sin unidad"}
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        ) : (
          <EmptyState
            icon={<Settings className="h-12 w-12" />}
            title="No hay parámetros"
            description={`No hay parámetros configurados para ${sampleTypes.find(t => t.id === selectedType)?.name}.`}
            action={
              <button onClick={() => setShowCreateForm(true)} className="btn-primary">
                <Plus className="h-4 w-4 mr-2" />
                Crear Primer Parámetro
              </button>
            }
          />
        )}
      </div>
    </div>
  );
}
