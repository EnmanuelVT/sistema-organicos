import { useState } from "react";
import { useForm } from "react-hook-form";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { changeStatus } from "@/api/samples";
import { X, AlertTriangle } from "lucide-react";
import LoadingSpinner from "@/components/LoadingSpinner";
import ErrorMessage from "@/components/ErrorMessage";
import type { AsignarEstadoMuestraDto } from "@/types/api";

interface ChangeStatusModalProps {
  isOpen: boolean;
  onClose: () => void;
  sampleCode: string;
  currentStatus: number;
}

interface FormValues {
  estadoMuestra: number;
  observaciones: string;
}

const statusOptions = [
  { value: 1, label: "Recibida" },
  { value: 2, label: "En análisis" },
  { value: 3, label: "En espera" },
  { value: 4, label: "Evaluada" },
  { value: 5, label: "Certificada" },
];

export default function ChangeStatusModal({ 
  isOpen, 
  onClose, 
  sampleCode, 
  currentStatus 
}: ChangeStatusModalProps) {
  const queryClient = useQueryClient();
  const [error, setError] = useState<string | null>(null);

  const { register, handleSubmit, reset, formState: { errors } } = useForm<FormValues>({
    defaultValues: {
      estadoMuestra: currentStatus,
      observaciones: "",
    },
  });

  const changeStatusMutation = useMutation({
    mutationFn: changeStatus,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["samples"] });
      queryClient.invalidateQueries({ queryKey: ["sample", sampleCode] });
      onClose();
      reset();
      setError(null);
    },
    onError: (error: any) => {
      setError(error?.response?.data?.message || "Error al cambiar el estado");
    },
  });

  const onSubmit = (values: FormValues) => {
    setError(null);
    const payload: AsignarEstadoMuestraDto = {
      mstCodigo: sampleCode,
      estadoMuestra: Number(values.estadoMuestra),
      observaciones: values.observaciones,
    };
    changeStatusMutation.mutate(payload);
  };

  const handleClose = () => {
    onClose();
    reset();
    setError(null);
  };

  if (!isOpen) return null;

  return (
    <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center p-4 z-50">
      <div className="bg-white rounded-xl shadow-xl max-w-md w-full">
        <div className="flex items-center justify-between p-6 border-b border-gray-200">
          <div className="flex items-center space-x-3">
            <div className="p-2 bg-warning-100 rounded-lg">
              <AlertTriangle className="h-5 w-5 text-warning-600" />
            </div>
            <div>
              <h2 className="text-lg font-semibold text-gray-900">Cambiar Estado</h2>
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
                Nuevo Estado *
              </label>
              <select
                {...register("estadoMuestra", { required: "Debe seleccionar un estado" })}
                className="input"
              >
                {statusOptions.map((option) => (
                  <option key={option.value} value={option.value}>
                    {option.label}
                  </option>
                ))}
              </select>
              {errors.estadoMuestra && (
                <p className="mt-1 text-sm text-error-600">{errors.estadoMuestra.message}</p>
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
                placeholder="Describa el motivo del cambio de estado..."
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
                disabled={changeStatusMutation.isPending}
                className="btn-primary"
              >
                {changeStatusMutation.isPending ? (
                  <div className="flex items-center space-x-2">
                    <LoadingSpinner size="sm" />
                    <span>Cambiando...</span>
                  </div>
                ) : (
                  "Cambiar Estado"
                )}
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
}