import { useState } from "react";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { useForm } from "react-hook-form";
import { getAllUsers, createUser, updateUser, deleteUser } from "@/api/users";
import { Users, Plus, Edit, Trash2, Search } from "lucide-react";
import LoadingSpinner from "@/components/LoadingSpinner";
import ErrorMessage from "@/components/ErrorMessage";
import EmptyState from "@/components/EmptyState";
import { getRoleDisplayName } from "@/utils/roles";
import type { CreateUserDto, UserDto } from "@/types/api";

interface UserForm {
  userName: string;
  email: string;
  role: string;
  nombre: string;
  apellido: string;
  usCedula: string;
  password: string;
}

export default function UsersPage() {
  const queryClient = useQueryClient();
  const [searchTerm, setSearchTerm] = useState("");
  const [showCreateForm, setShowCreateForm] = useState(false);
  const [editingUser, setEditingUser] = useState<UserDto | null>(null);
  const [error, setError] = useState<string | null>(null);

  const { register, handleSubmit, reset, formState: { errors } } = useForm<UserForm>();

  const { data: users, isLoading, error: usersError } = useQuery({
    queryKey: ["users"],
    queryFn: getAllUsers,
  });

  const createMutation = useMutation({
    mutationFn: createUser,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["users"] });
      setShowCreateForm(false);
      reset();
      setError(null);
    },
    onError: (error: any) => {
      setError(error?.response?.data?.message || "Error al crear el usuario");
    },
  });

  const updateMutation = useMutation({
    mutationFn: ({ id, user }: { id: string; user: UserDto }) => updateUser(id, user),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["users"] });
      setEditingUser(null);
      reset();
      setError(null);
    },
    onError: (error: any) => {
      setError(error?.response?.data?.message || "Error al actualizar el usuario");
    },
  });

  const deleteMutation = useMutation({
    mutationFn: deleteUser,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["users"] });
    },
    onError: (error: any) => {
      setError(error?.response?.data?.message || "Error al eliminar el usuario");
    },
  });

  const filteredUsers = users?.filter((user) => {
    return !searchTerm || 
      user.userName?.toLowerCase().includes(searchTerm.toLowerCase()) ||
      user.email?.toLowerCase().includes(searchTerm.toLowerCase()) ||
      user.nombre?.toLowerCase().includes(searchTerm.toLowerCase()) ||
      user.apellido?.toLowerCase().includes(searchTerm.toLowerCase());
  }) || [];

  const onSubmit = (values: UserForm) => {
    setError(null);
    
    if (editingUser) {
      const updatedUser: UserDto = {
        ...editingUser,
        userName: values.userName,
        email: values.email,
        role: values.role,
        nombre: values.nombre || null,
        apellido: values.apellido || null,
        usCedula: values.usCedula || null,
      };
      updateMutation.mutate({ id: editingUser.id!, user: updatedUser });
    } else {
      const newUser: CreateUserDto = {
        userName: values.userName,
        email: values.email,
        role: values.role,
        nombre: values.nombre || null,
        apellido: values.apellido || null,
        usCedula: values.usCedula || null,
        password: values.password,
      };
      createMutation.mutate(newUser);
    }
  };

  const handleEdit = (user: UserDto) => {
    setEditingUser(user);
    setShowCreateForm(true);
    reset({
      userName: user.userName,
      email: user.email,
      role: user.role,
      nombre: user.nombre || "",
      apellido: user.apellido || "",
      usCedula: user.usCedula || "",
      password: "",
    });
  };

  const handleDelete = (userId: string) => {
    if (window.confirm("¿Está seguro de que desea eliminar este usuario?")) {
      deleteMutation.mutate(userId);
    }
  };

  const handleCancel = () => {
    setShowCreateForm(false);
    setEditingUser(null);
    reset();
    setError(null);
  };

  if (isLoading) {
    return (
      <div className="flex items-center justify-center h-64">
        <LoadingSpinner size="lg" />
      </div>
    );
  }

  if (usersError) {
    return (
      <ErrorMessage 
        message={(usersError as any)?.message || "Error al cargar los usuarios"} 
      />
    );
  }

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold text-gray-900">Gestión de Usuarios</h1>
          <p className="text-gray-600 mt-1">
            Administrar usuarios del sistema
          </p>
        </div>
        
        <button
          onClick={() => setShowCreateForm(true)}
          className="btn-primary"
        >
          <Plus className="h-4 w-4 mr-2" />
          Nuevo Usuario
        </button>
      </div>

      {error && <ErrorMessage message={error} />}

      {/* Create/Edit Form */}
      {showCreateForm && (
        <div className="card">
          <h2 className="text-lg font-semibold text-gray-900 mb-4">
            {editingUser ? "Editar Usuario" : "Crear Nuevo Usuario"}
          </h2>

          <form onSubmit={handleSubmit(onSubmit)} className="space-y-6">
            <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-2">
                  Nombre de Usuario *
                </label>
                <input
                  {...register("userName", { required: "El nombre de usuario es requerido" })}
                  type="text"
                  className="input"
                  placeholder="usuario123"
                />
                {errors.userName && (
                  <p className="mt-1 text-sm text-error-600">{errors.userName.message}</p>
                )}
              </div>

              <div>
                <label className="block text-sm font-medium text-gray-700 mb-2">
                  Correo Electrónico *
                </label>
                <input
                  {...register("email", { 
                    required: "El correo es requerido",
                    pattern: {
                      value: /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$/i,
                      message: "Correo electrónico inválido"
                    }
                  })}
                  type="email"
                  className="input"
                  placeholder="usuario@ejemplo.com"
                />
                {errors.email && (
                  <p className="mt-1 text-sm text-error-600">{errors.email.message}</p>
                )}
              </div>

              <div>
                <label className="block text-sm font-medium text-gray-700 mb-2">
                  Rol *
                </label>
                <select
                  {...register("role", { required: "El rol es requerido" })}
                  className="input"
                >
                  <option value="">Seleccionar rol</option>
                  <option value="Solicitante">Solicitante</option>
                  <option value="Analista">Analista</option>
                  <option value="Evaluador">Evaluador</option>
                  <option value="Admin">Administrador</option>
                </select>
                {errors.role && (
                  <p className="mt-1 text-sm text-error-600">{errors.role.message}</p>
                )}
              </div>

              <div>
                <label className="block text-sm font-medium text-gray-700 mb-2">
                  Cédula
                </label>
                <input
                  {...register("usCedula")}
                  type="text"
                  className="input"
                  placeholder="000-0000000-0"
                />
              </div>

              <div>
                <label className="block text-sm font-medium text-gray-700 mb-2">
                  Nombre
                </label>
                <input
                  {...register("nombre")}
                  type="text"
                  className="input"
                  placeholder="Juan"
                />
              </div>

              <div>
                <label className="block text-sm font-medium text-gray-700 mb-2">
                  Apellido
                </label>
                <input
                  {...register("apellido")}
                  type="text"
                  className="input"
                  placeholder="Pérez"
                />
              </div>
            </div>

            {!editingUser && (
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-2">
                  Contraseña *
                </label>
                <input
                  {...register("password", { 
                    required: !editingUser ? "La contraseña es requerida" : false,
                    minLength: {
                      value: 6,
                      message: "La contraseña debe tener al menos 6 caracteres"
                    }
                  })}
                  type="password"
                  className="input"
                  placeholder="••••••••"
                />
                {errors.password && (
                  <p className="mt-1 text-sm text-error-600">{errors.password.message}</p>
                )}
              </div>
            )}

            <div className="flex justify-end space-x-4 pt-6 border-t border-gray-200">
              <button type="button" onClick={handleCancel} className="btn-secondary">
                Cancelar
              </button>
              <button
                type="submit"
                disabled={createMutation.isPending || updateMutation.isPending}
                className="btn-primary"
              >
                {(createMutation.isPending || updateMutation.isPending) ? (
                  <div className="flex items-center space-x-2">
                    <LoadingSpinner size="sm" />
                    <span>{editingUser ? "Actualizando..." : "Creando..."}</span>
                  </div>
                ) : (
                  editingUser ? "Actualizar Usuario" : "Crear Usuario"
                )}
              </button>
            </div>
          </form>
        </div>
      )}

      {/* Users Table */}
      <div className="card">
        <div className="flex items-center justify-between mb-6">
          <h2 className="text-lg font-semibold text-gray-900">Lista de Usuarios</h2>
          
          <div className="w-64">
            <div className="relative">
              <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 h-4 w-4 text-gray-400" />
              <input
                type="text"
                placeholder="Buscar usuarios..."
                value={searchTerm}
                onChange={(e) => setSearchTerm(e.target.value)}
                className="input pl-10"
              />
            </div>
          </div>
        </div>

        {filteredUsers.length === 0 ? (
          <EmptyState
            icon={<Users className="h-12 w-12" />}
            title="No hay usuarios"
            description="No se encontraron usuarios que coincidan con la búsqueda."
            action={
              <button onClick={() => setShowCreateForm(true)} className="btn-primary">
                <Plus className="h-4 w-4 mr-2" />
                Crear Primer Usuario
              </button>
            }
          />
        ) : (
          <div className="overflow-x-auto">
            <table className="w-full">
              <thead>
                <tr className="border-b border-gray-200">
                  <th className="text-left py-3 px-4 font-medium text-gray-900">Usuario</th>
                  <th className="text-left py-3 px-4 font-medium text-gray-900">Correo</th>
                  <th className="text-left py-3 px-4 font-medium text-gray-900">Rol</th>
                  <th className="text-left py-3 px-4 font-medium text-gray-900">Nombre</th>
                  <th className="text-left py-3 px-4 font-medium text-gray-900">Estado</th>
                  <th className="text-left py-3 px-4 font-medium text-gray-900">Acciones</th>
                </tr>
              </thead>
              <tbody>
                {filteredUsers.map((user) => (
                  <tr key={user.id} className="border-b border-gray-100 hover:bg-gray-50">
                    <td className="py-3 px-4 font-medium text-gray-900">
                      {user.userName}
                    </td>
                    <td className="py-3 px-4 text-gray-700">
                      {user.email}
                    </td>
                    <td className="py-3 px-4">
                      <span className="badge badge-info">
                        {getRoleDisplayName(user.role as any)}
                      </span>
                    </td>
                    <td className="py-3 px-4 text-gray-700">
                      {user.nombre && user.apellido 
                        ? `${user.nombre} ${user.apellido}`
                        : "Sin especificar"
                      }
                    </td>
                    <td className="py-3 px-4">
                      <span className={`badge ${user.estado ? 'badge-success' : 'badge-error'}`}>
                        {user.estado ? 'Activo' : 'Inactivo'}
                      </span>
                    </td>
                    <td className="py-3 px-4">
                      <div className="flex items-center space-x-2">
                        <button
                          onClick={() => handleEdit(user)}
                          className="p-1 text-gray-400 hover:text-primary-600 transition-colors"
                        >
                          <Edit className="h-4 w-4" />
                        </button>
                        <button
                          onClick={() => handleDelete(user.id!)}
                          disabled={deleteMutation.isPending}
                          className="p-1 text-gray-400 hover:text-error-600 transition-colors"
                        >
                          <Trash2 className="h-4 w-4" />
                        </button>
                      </div>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
      </div>
    </div>
  );
}
