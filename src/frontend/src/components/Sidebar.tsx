import { Link, useLocation } from "react-router-dom";
import { useAuthStore } from "@/store/auth";
import { hasRole } from "@/utils/roles";
import {
  LayoutDashboard,
  TestTube,
  FlaskConical,
  BarChart3,
  CheckCircle,
  Users,
  Settings,
  Beaker,
} from "lucide-react";
import clsx from "clsx";

const navigation = [
  {
    name: "Dashboard",
    href: "/dashboard",
    icon: LayoutDashboard,
    roles: ["SOLICITANTE", "ANALISTA", "EVALUADOR", "ADMIN"],
  },
  {
    name: "Muestras",
    href: "/samples",
    icon: TestTube,
    roles: ["SOLICITANTE", "ANALISTA", "EVALUADOR", "ADMIN"],
  },
  {
    name: "Pruebas",
    href: "/tests",
    icon: FlaskConical,
    roles: ["ANALISTA", "ADMIN"],
  },
  {
    name: "Resultados",
    href: "/results",
    icon: BarChart3,
    roles: ["ANALISTA", "ADMIN"],
  },
  {
    name: "Evaluación",
    href: "/evaluation",
    icon: CheckCircle,
    roles: ["EVALUADOR", "ADMIN"],
  },
];

const adminNavigation = [
  {
    name: "Usuarios",
    href: "/admin/users",
    icon: Users,
  },
  {
    name: "Parámetros",
    href: "/admin/parameters",
    icon: Settings,
  },
];

export default function Sidebar() {
  const location = useLocation();
  const { user } = useAuthStore();

  const isActive = (href: string) => {
    return location.pathname === href || location.pathname.startsWith(href + "/");
  };

  return (
    <div className="w-64 bg-white border-r border-gray-200 flex flex-col">
      <div className="p-6 border-b border-gray-200">
        <div className="flex items-center space-x-3">
          <div className="p-2 bg-primary-600 rounded-lg">
            <Beaker className="h-6 w-6 text-white" />
          </div>
          <div>
            <h2 className="text-lg font-semibold text-gray-900">LabTrace</h2>
            <p className="text-xs text-gray-500">Sistema de Trazabilidad</p>
          </div>
        </div>
      </div>

      <nav className="flex-1 p-4 space-y-2">
        {navigation.map((item) => {
          if (!hasRole(user?.role, item.roles as any)) return null;
          
          return (
            <Link
              key={item.name}
              to={item.href}
              className={clsx(
                "flex items-center space-x-3 px-3 py-2 rounded-lg text-sm font-medium transition-colors",
                isActive(item.href)
                  ? "bg-primary-50 text-primary-700 border-r-2 border-primary-600"
                  : "text-gray-700 hover:bg-gray-100"
              )}
            >
              <item.icon className="h-5 w-5" />
              <span>{item.name}</span>
            </Link>
          );
        })}

        {hasRole(user?.role, ["ADMIN"]) && (
          <>
            <div className="pt-4 mt-4 border-t border-gray-200">
              <p className="px-3 text-xs font-semibold text-gray-500 uppercase tracking-wider">
                Administración
              </p>
            </div>
            {adminNavigation.map((item) => (
              <Link
                key={item.name}
                to={item.href}
                className={clsx(
                  "flex items-center space-x-3 px-3 py-2 rounded-lg text-sm font-medium transition-colors",
                  isActive(item.href)
                    ? "bg-primary-50 text-primary-700 border-r-2 border-primary-600"
                    : "text-gray-700 hover:bg-gray-100"
                )}
              >
                <item.icon className="h-5 w-5" />
                <span>{item.name}</span>
              </Link>
            ))}
          </>
        )}
      </nav>
    </div>
  );
}