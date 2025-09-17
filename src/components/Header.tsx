import { useAuthStore } from "@/store/auth";
import { useNavigate } from "react-router-dom";
import { LogOut, User } from "lucide-react";
import { getRoleDisplayName } from "@/utils/roles";

export default function Header() {
  const { user, logout } = useAuthStore();
  const navigate = useNavigate();

  const handleLogout = async () => {
    await logout();
    navigate("/login");
  };

  const displayName = user?.nombre && user?.apellido 
    ? `${user.nombre} ${user.apellido}`
    : user?.userName || user?.email;

  return (
    <header className="bg-white border-b border-gray-200 px-6 py-4">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-xl font-semibold text-gray-900">
            Sistema de Trazabilidad
          </h1>
          <p className="text-sm text-gray-500">
            Gestión de muestras y análisis de laboratorio
          </p>
        </div>
        
        <div className="flex items-center space-x-4">
          <div className="flex items-center space-x-3">
            <div className="flex items-center space-x-2">
              <User className="h-5 w-5 text-gray-400" />
              <div className="text-right">
                <p className="text-sm font-medium text-gray-900">{displayName}</p>
                <p className="text-xs text-gray-500">
                  {user ? getRoleDisplayName(user.role) : ""}
                </p>
              </div>
            </div>
            
            <button
              onClick={handleLogout}
              className="flex items-center space-x-2 px-3 py-2 text-sm text-gray-700 hover:bg-gray-100 rounded-lg transition-colors"
            >
              <LogOut className="h-4 w-4" />
              <span>Cerrar sesión</span>
            </button>
          </div>
        </div>
      </div>
    </header>
  );
}