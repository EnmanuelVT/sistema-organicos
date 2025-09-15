// src/routes/auth/login.tsx
import { useEffect, useState } from "react";
import { useForm } from "react-hook-form";
import { useAuthStore } from "@/store/auth";
import { useNavigate } from "react-router-dom";
import { normalizeRole } from "@/utils/roles";

type FormValues = { email: string; password: string };

export default function Login() {
  const { register, handleSubmit } = useForm<FormValues>();
  const { login, user, loading } = useAuthStore();
  const [error, setError] = useState<string | null>(null);
  const nav = useNavigate();

  const goByRole = (raw?: string | null) => {
    const role = normalizeRole(raw);
    switch (role) {
      case "ADMIN":     nav("/admin", { replace: true }); break;
      case "ANALISTA":  nav("/analista", { replace: true }); break;
      case "EVALUADOR": nav("/evaluador", { replace: true }); break;
      default:          nav("/solicitante", { replace: true }); break; // default si viene nulo
    }
  };

  const onSubmit = async (values: FormValues) => {
    try {
      setError(null);
      await login(values.email, values.password);      // hace POST /login y luego GET /api/usuarios/me
      const role = useAuthStore.getState().user?.role; // lee el rol normalizado del store
      goByRole(role);
    } catch (e: any) {
      const msg =
        e?.response?.data?.message ||
        e?.response?.data?.title ||
        e?.message ||
        "Error al iniciar sesión";
      setError(msg);
    }
  };

  // Redirección segura si ya está autenticado (evita side-effects durante el render)
  useEffect(() => {
    if (user) goByRole(user.role);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [user]);

  return (
    <div className="max-w-lg mx-auto mt-16 p-8 rounded-2xl shadow bg-white">
      <h1 className="text-2xl font-semibold mb-2">Iniciar Sesión</h1>
      <p className="text-sm text-gray-500 mb-6">
        Ingresa tu email y contraseña para acceder al sistema.
      </p>

      {error && (
        <div className="mb-4 p-3 rounded bg-red-100 text-red-700">{error}</div>
      )}

      {/* ⚠️ Mantén este onSubmitCapture para bloquear la navegación GET a /login (front) */}
      <form
        noValidate
        onSubmitCapture={(e) => {
          e.preventDefault();
          void handleSubmit(onSubmit)(e as any);
        }}
      >
        <label className="block text-sm mb-1">Email</label>
        <input
          type="email"
          className="w-full border rounded px-3 py-2 mb-4"
          placeholder="usuario@ejemplo.com"
          {...register("email", { required: true })}
        />

        <label className="block text-sm mb-1">Contraseña</label>
        <input
          type="password"
          className="w-full border rounded px-3 py-2 mb-6"
          placeholder="••••••••"
          {...register("password", { required: true })}
        />

        <button
          type="submit"
          disabled={loading}
          className="w-full rounded px-4 py-2 font-medium bg-gray-900 text-white"
        >
          {loading ? "Ingresando..." : "Iniciar Sesión"}
        </button>
      </form>
    </div>
  );
}
