import { useState } from "react";
import { useForm } from "react-hook-form";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { assignAnalyst } from "@/api/samples";
import { getAllUsers } from "@/api/users";
import { X, UserCheck } from "lucide-react";
import LoadingSpinner from "@/components/LoadingSpinner";
import ErrorMessage from "@/components/ErrorMessage";
import type { AsignarAnalistaEnMuestraDto } from "@/types/api";

interface AssignAnalystModalProps {
  isOpen: boolean;
  onClose: () => void;
  sampleCode: string;
}

interface FormValues {
  idAnalista: string;
  observaciones: string;
}

export default function AssignAnalystModal({ 
  isOpen, 
  onClose, 
  sampleCode 
}: AssignAnalystModalProps) {
  const queryClient = useQueryClient();
  const [error, setError] = useState<string | null>(null);

  const { register, handleSubmit, reset, formState: { errors } } = useForm<FormValues>();

  const { data: users, isLoading: loadingUsers } = useQuery({
    queryKey: ["users"],
    queryFn: getAllUsers,
    enabled: isOpen,
  });

  const assignMutation = useMutation({
    mutationFn: assignAnalyst,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["samples"] });
      queryClient.invalidateQueries({ queryKey: ["sample", sampleCode] });
      onClose();
      reset();
      setError(null);
    },
    onError: (error: any) => {
      setError(error?.response?.data?.message || "Error al asignar el analista");
    },
  });

  const onSubmit = (values: FormValues) => {
    setError(null);
    const payload: AsignarAnalistaEnMuestraDto = {
      mstCodigo: sampleCode,
      idAnalista: values.idAnalista,
      observaciones: values.observaciones,
    };
    assignMutation.mutate(payload);
  };

  const handleClose = () => {
    onClose();
    reset();
    setError(null);
  };

  // Filter users to show only analysts
  const analysts = users?.filter(user => 
    user.role === "Analista" || user.role === "Admin"
  ) || [];

  if (!isOpen) return null;

  return (
    <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center p-4 z-50">
      <div className="bg-white rounded-xl shadow-xl max-w-md w-full">
        <div className="flex items-center justify-between p-6 border-b border-gray-200">
          <div className="flex items-center space-x-3">
            <div className="p-2 bg-primary-100 rounded-lg">
              <UserCheck className="h-5 w-5 text-primary-600" />
            </div>
            <div>
              <h2 className="text-lg font-semibold text-gray-900">Asignar Analista</h2>
              <p className="text-sm text-gray-600">Muestra: {sampleCode}</p>
            </div>
          </div>
          <button
            onClick={handleClose}
            className="p-2 hover:bg-gray-100 rounded-lg transition-colors"
          >
            <X className="h-5 w-5 text-gray-400" />
          </button>
        </div>

        <div className="p-6">
          {error && <ErrorMessage message={error} className="mb-4" />}

          <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-2">
                Analista *
              </label>
              <select
                {...register("idAnalista", { required: "Debe seleccionar un analista" })}
                className="input"
                disabled={loadingUsers}
              >
                <option value="">Seleccionar analista</option>
                {analysts.map((analyst) => (
                  <option key={analyst.id} value={analyst.id!}>
                    {analyst.nombre && analyst.apellido 
                      ? `${analyst.nombre} ${analyst.apellido} (${analyst.email})`
                      : `${analyst.userName} (${analyst.email})`
                    }
                  </option>
                ))}
              </select>
              {errors.idAnalista && (
                <p className="mt-1 text-sm text-error-600">{errors.idAnalista.message}</p>
              )}
              {loadingUsers && (
                <p className="mt-1 text-sm text-gray-500">Cargando analistas...</p>
              )}
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 mb-2">
                Observaciones *
              </label>
              <textarea
                {...register("observaciones", { required: "Las observaciones son requeridas" })}
                rows={4}
                className="input"
                placeholder="Describa el motivo de la asignación..."
              />
              {errors.observaciones && (
                <p className="mt-1 text-sm text-error-600">{errors.observaciones.message}</p>
              )}
            </div>

            <div className="flex justify-end space-x-3 pt-4">
              <button
                type="button"
                onClick={handleClose}
                className="btn-secondary"
              >
                Cancelar
              </button>
              <button
                type="submit"
                disabled={assignMutation.isPending || loadingUsers}
                className="btn-primary"
              >
                {assignMutation.isPending ? (
                  <div className="flex items-center space-x-2">
                    <LoadingSpinner size="sm" />
                    <span>Asignando...</span>
                  </div>
                ) : (
                  "Asignar Analista"
                )}
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
}