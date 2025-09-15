import { useQuery } from "@tanstack/react-query";
import { useAuthStore } from "@/store/auth";
import { hasRole } from "@/utils/roles";
import { getAllSamples, getMySamples, getAssignedSamples } from "@/api/samples";
import { TestTube, FlaskConical, BarChart3, CheckCircle, TrendingUp, Clock } from "lucide-react";
import LoadingSpinner from "@/components/LoadingSpinner";
import ErrorMessage from "@/components/ErrorMessage";
import { Link } from "react-router-dom";

export default function DashboardPage() {
  const { user } = useAuthStore();

  const { data: allSamples, isLoading: loadingAll } = useQuery({
    queryKey: ["samples", "all"],
    queryFn: getAllSamples,
    enabled: hasRole(user?.role, ["ADMIN", "ANALISTA", "EVALUADOR"]),
  });

  const { data: mySamples, isLoading: loadingMy } = useQuery({
    queryKey: ["samples", "my"],
    queryFn: getMySamples,
    enabled: hasRole(user?.role, ["SOLICITANTE"]),
  });

  const { data: assignedSamples, isLoading: loadingAssigned } = useQuery({
    queryKey: ["samples", "assigned"],
    queryFn: getAssignedSamples,
    enabled: hasRole(user?.role, ["ANALISTA"]),
  });

  const samples = allSamples || mySamples || assignedSamples || [];
  const isLoading = loadingAll || loadingMy || loadingAssigned;

  const stats = [
    {
      name: "Total de Muestras",
      value: samples.length,
      icon: TestTube,
      color: "text-primary-600",
      bgColor: "bg-primary-100",
    },
    {
      name: "En Análisis",
      value: samples.filter(s => s.estadoActual === 2).length,
      icon: FlaskConical,
      color: "text-warning-600",
      bgColor: "bg-warning-100",
    },
    {
      name: "En Espera",
      value: samples.filter(s => s.estadoActual === 3).length,
      icon: Clock,
      color: "text-warning-600",
      bgColor: "bg-warning-100",
    },
    {
      name: "Certificadas",
      value: samples.filter(s => s.estadoActual === 5).length,
      icon: CheckCircle,
      color: "text-success-600",
      bgColor: "bg-success-100",
    },
  ];

  const quickActions = [
    {
      name: "Nueva Muestra",
      description: "Registrar una nueva muestra para análisis",
      href: "/samples/create",
      icon: TestTube,
      color: "bg-primary-600 hover:bg-primary-700",
      roles: ["SOLICITANTE", "ADMIN"],
    },
    {
      name: "Nueva Prueba",
      description: "Crear una nueva prueba para una muestra",
      href: "/tests/create",
      icon: FlaskConical,
      color: "bg-success-600 hover:bg-success-700",
      roles: ["ANALISTA", "ADMIN"],
    },
    {
      name: "Registrar Resultado",
      description: "Ingresar resultados de análisis",
      href: "/results/create",
      icon: BarChart3,
      color: "bg-warning-600 hover:bg-warning-700",
      roles: ["ANALISTA", "ADMIN"],
    },
    {
      name: "Evaluar Muestras",
      description: "Revisar y aprobar muestras analizadas",
      href: "/evaluation",
      icon: CheckCircle,
      color: "bg-purple-600 hover:bg-purple-700",
      roles: ["EVALUADOR", "ADMIN"],
    },
  ];

  if (isLoading) {
    return (
      <div className="flex items-center justify-center h-64">
        <LoadingSpinner size="lg" />
      </div>
    );
  }

  return (
    <div className="space-y-8">
      <div>
        <h1 className="text-3xl font-bold text-gray-900">Dashboard</h1>
        <p className="text-gray-600 mt-2">
          Resumen general del sistema de trazabilidad
        </p>
      </div>

      {/* Stats */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
        {stats.map((stat) => (
          <div key={stat.name} className="card">
            <div className="flex items-center">
              <div className={`p-3 rounded-lg ${stat.bgColor}`}>
                <stat.icon className={`h-6 w-6 ${stat.color}`} />
              </div>
              <div className="ml-4">
                <p className="text-sm font-medium text-gray-600">{stat.name}</p>
                <p className="text-2xl font-bold text-gray-900">{stat.value}</p>
              </div>
            </div>
          </div>
        ))}
      </div>

      {/* Quick Actions */}
      <div>
        <h2 className="text-xl font-semibold text-gray-900 mb-4">Acciones Rápidas</h2>
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
          {quickActions.map((action) => {
            if (!hasRole(user?.role, action.roles as any)) return null;
            
            return (
              <Link
                key={action.name}
                to={action.href}
                className={`${action.color} text-white p-6 rounded-xl transition-colors group`}
              >
                <action.icon className="h-8 w-8 mb-3 group-hover:scale-110 transition-transform" />
                <h3 className="font-semibold mb-1">{action.name}</h3>
                <p className="text-sm opacity-90">{action.description}</p>
              </Link>
            );
          })}
        </div>
      </div>

      {/* Recent Samples */}
      <div>
        <div className="flex items-center justify-between mb-4">
          <h2 className="text-xl font-semibold text-gray-900">Muestras Recientes</h2>
          <Link to="/samples" className="text-primary-600 hover:text-primary-700 text-sm font-medium">
            Ver todas →
          </Link>
        </div>
        
        <div className="card">
          {samples.length === 0 ? (
            <div className="text-center py-8">
              <TestTube className="h-12 w-12 text-gray-400 mx-auto mb-4" />
              <p className="text-gray-500">No hay muestras disponibles</p>
            </div>
          ) : (
            <div className="overflow-x-auto">
              <table className="w-full">
                <thead>
                  <tr className="border-b border-gray-200">
                    <th className="text-left py-3 px-4 font-medium text-gray-900">Código</th>
                    <th className="text-left py-3 px-4 font-medium text-gray-900">Nombre</th>
                    <th className="text-left py-3 px-4 font-medium text-gray-900">Estado</th>
                    <th className="text-left py-3 px-4 font-medium text-gray-900">Fecha</th>
                  </tr>
                </thead>
                <tbody>
                  {samples.slice(0, 5).map((sample) => (
                    <tr key={sample.mstCodigo} className="border-b border-gray-100 hover:bg-gray-50">
                      <td className="py-3 px-4 font-medium text-gray-900">
                        {sample.mstCodigo}
                      </td>
                      <td className="py-3 px-4 text-gray-700">
                        {sample.nombre || "Sin nombre"}
                      </td>
                      <td className="py-3 px-4">
                        <span className={`badge ${getStateColor(sample.estadoActual)}`}>
                          {getStateDisplayName(sample.estadoActual)}
                        </span>
                      </td>
                      <td className="py-3 px-4 text-gray-500">
                        {new Date(sample.fechaRecepcion).toLocaleDateString()}
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          )}
        </div>
      </div>
    </div>
  );
}

function getStateDisplayName(state: number): string {
  const states: Record<number, string> = {
    1: 'Recibida',
    2: 'En análisis',
    3: 'En espera',
    4: 'Evaluada',
    5: 'Certificada',
  };
  return states[state] || `Estado ${state}`;
}

function getStateColor(state: number): string {
  const colors: Record<number, string> = {
    1: 'badge-gray',
    2: 'badge-info',
    3: 'badge-warning',
    4: 'badge-info',
    5: 'badge-success',
  };
  return colors[state] || 'badge-gray';
}
