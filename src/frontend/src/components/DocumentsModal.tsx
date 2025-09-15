import { useState } from "react";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { getDocumentsBySample } from "@/api/samples";
import { generatePreliminaryDocument } from "@/api/tests";
import { X, FileText, Download, Plus, Calendar } from "lucide-react";
import LoadingSpinner from "@/components/LoadingSpinner";
import ErrorMessage from "@/components/ErrorMessage";
import EmptyState from "@/components/EmptyState";

interface DocumentsModalProps {
  isOpen: boolean;
  onClose: () => void;
  sampleCode: string;
  tests?: Array<{ idPrueba: number; nombrePrueba: string }>;
}

interface GeneratePreliminaryForm {
  testId: number;
  observaciones: string;
}

export default function DocumentsModal({ 
  isOpen, 
  onClose, 
  sampleCode,
  tests = []
}: DocumentsModalProps) {
  const queryClient = useQueryClient();
  const [showGenerateForm, setShowGenerateForm] = useState(false);
  const [selectedTestId, setSelectedTestId] = useState<number | null>(null);
  const [observaciones, setObservaciones] = useState("");
  const [error, setError] = useState<string | null>(null);

  const { data: documents, isLoading: loadingDocuments } = useQuery({
    queryKey: ["documents", sampleCode],
    queryFn: () => getDocumentsBySample(sampleCode),
    enabled: isOpen && !!sampleCode,
  });

  const generateMutation = useMutation({
    mutationFn: ({ testId, observaciones }: { testId: number; observaciones?: string }) =>
      generatePreliminaryDocument(testId, observaciones),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["documents", sampleCode] });
      setShowGenerateForm(false);
      setSelectedTestId(null);
      setObservaciones("");
      setError(null);
    },
    onError: (error: any) => {
      setError(error?.response?.data?.message || "Error al generar el documento");
    },
  });

  const handleGenerate = () => {
    if (!selectedTestId) {
      setError("Debe seleccionar una prueba");
      return;
    }
    
    setError(null);
    generateMutation.mutate({
      testId: selectedTestId,
      observaciones: observaciones || undefined,
    });
  };

  const handleClose = () => {
    onClose();
    setShowGenerateForm(false);
    setSelectedTestId(null);
    setObservaciones("");
    setError(null);
  };

  const downloadDocument = (document: any) => {
    if (document.docPdf) {
      const blob = new Blob([new Uint8Array(document.docPdf)], { type: 'application/pdf' });
      const url = URL.createObjectURL(blob);
      const a = document.createElement('a');
      a.href = url;
      a.download = `${sampleCode}_v${document.version}.pdf`;
      document.body.appendChild(a);
      a.click();
      document.body.removeChild(a);
      URL.revokeObjectURL(url);
    }
  };

  const getDocumentTypeName = (typeId: number) => {
    const types: Record<number, string> = {
      1: "Certificado",
      2: "Informe",
    };
    return types[typeId] || `Tipo ${typeId}`;
  };

  const getDocumentStatusName = (statusId: number) => {
    const statuses: Record<number, string> = {
      1: "Rechazado",
      2: "Aprobado", 
      3: "Preliminar",
    };
    return statuses[statusId] || `Estado ${statusId}`;
  };

  const getDocumentStatusColor = (statusId: number) => {
    const colors: Record<number, string> = {
      1: "badge-error",
      2: "badge-success",
      3: "badge-warning",
    };
    return colors[statusId] || "badge-gray";
  };

  if (!isOpen) return null;

  return (
    <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center p-4 z-50">
      <div className="bg-white rounded-xl shadow-xl max-w-4xl w-full max-h-[90vh] overflow-hidden">
        <div className="flex items-center justify-between p-6 border-b border-gray-200">
          <div className="flex items-center space-x-3">
            <div className="p-2 bg-primary-100 rounded-lg">
              <FileText className="h-5 w-5 text-primary-600" />
            </div>
            <div>
              <h2 className="text-lg font-semibold text-gray-900">Documentos</h2>
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

        <div className="p-6 overflow-y-auto max-h-[calc(90vh-120px)]">
          {error && <ErrorMessage message={error} className="mb-4" />}

          {/* Generate Preliminary Document Form */}
          {showGenerateForm && (
            <div className="mb-6 p-4 bg-gray-50 rounded-lg">
              <h3 className="font-medium text-gray-900 mb-4">Generar Documento Preliminar</h3>
              
              <div className="space-y-4">
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-2">
                    Seleccionar Prueba *
                  </label>
                  <select
                    value={selectedTestId || ""}
                    onChange={(e) => setSelectedTestId(Number(e.target.value) || null)}
                    className="input"
                  >
                    <option value="">Seleccionar prueba</option>
                    {tests.map((test) => (
                      <option key={test.idPrueba} value={test.idPrueba}>
                        {test.nombrePrueba} (ID: {test.idPrueba})
                      </option>
                    ))}
                  </select>
                </div>

                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-2">
                    Observaciones
                  </label>
                  <textarea
                    value={observaciones}
                    onChange={(e) => setObservaciones(e.target.value)}
                    rows={3}
                    className="input"
                    placeholder="Observaciones adicionales (opcional)"
                  />
                </div>

                <div className="flex justify-end space-x-3">
                  <button
                    type="button"
                    onClick={() => setShowGenerateForm(false)}
                    className="btn-secondary"
                  >
                    Cancelar
                  </button>
                  <button
                    type="button"
                    onClick={handleGenerate}
                    disabled={generateMutation.isPending || !selectedTestId}
                    className="btn-primary"
                  >
                    {generateMutation.isPending ? (
                      <div className="flex items-center space-x-2">
                        <LoadingSpinner size="sm" />
                        <span>Generando...</span>
                      </div>
                    ) : (
                      "Generar Documento"
                    )}
                  </button>
                </div>
              </div>
            </div>
          )}

          {/* Action Buttons */}
          <div className="flex justify-between items-center mb-6">
            <h3 className="text-lg font-semibold text-gray-900">Lista de Documentos</h3>
            
            {tests.length > 0 && (
              <button
                onClick={() => setShowGenerateForm(true)}
                className="btn-primary"
              >
                <Plus className="h-4 w-4 mr-2" />
                Generar Preliminar
              </button>
            )}
          </div>

          {/* Documents List */}
          {loadingDocuments ? (
            <div className="flex justify-center py-8">
              <LoadingSpinner />
            </div>
          ) : documents && documents.length > 0 ? (
            <div className="space-y-4">
              {documents.map((doc) => (
                <div key={doc.idDocumento} className="p-4 border border-gray-200 rounded-lg">
                  <div className="flex items-center justify-between">
                    <div className="flex-1">
                      <div className="flex items-center space-x-3 mb-2">
                        <FileText className="h-5 w-5 text-gray-400" />
                        <h4 className="font-medium text-gray-900">
                          {getDocumentTypeName(doc.idTipoDoc)} v{doc.version}
                        </h4>
                        <span className={`badge ${getDocumentStatusColor(doc.idEstadoDocumento)}`}>
                          {getDocumentStatusName(doc.idEstadoDocumento)}
                        </span>
                      </div>
                      
                      <div className="flex items-center space-x-4 text-sm text-gray-500">
                        <div className="flex items-center space-x-1">
                          <Calendar className="h-4 w-4" />
                          <span>{new Date(doc.fechaCreacion).toLocaleDateString()}</span>
                        </div>
                        {doc.rutaArchivo && (
                          <span className="font-mono text-xs bg-gray-100 px-2 py-1 rounded">
                            {doc.rutaArchivo.split('/').pop()}
                          </span>
                        )}
                      </div>
                    </div>

                    <div className="flex items-center space-x-2">
                      {doc.docPdf && (
                        <button
                          onClick={() => downloadDocument(doc)}
                          className="p-2 text-gray-400 hover:text-primary-600 transition-colors"
                          title="Descargar PDF"
                        >
                          <Download className="h-4 w-4" />
                        </button>
                      )}
                    </div>
                  </div>
                </div>
              ))}
            </div>
          ) : (
            <EmptyState
              icon={<FileText className="h-12 w-12" />}
              title="No hay documentos"
              description="Esta muestra no tiene documentos generados."
              action={
                tests.length > 0 ? (
                  <button onClick={() => setShowGenerateForm(true)} className="btn-primary">
                    <Plus className="h-4 w-4 mr-2" />
                    Generar Primer Documento
                  </button>
                ) : undefined
              }
            />
          )}
        </div>
      </div>
    </div>
  );
}