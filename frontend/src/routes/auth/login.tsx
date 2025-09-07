import { useForm } from 'react-hook-form';
import { z } from 'zod';
import { zodResolver } from '@hookform/resolvers/zod';
import { useNavigate, useLocation } from 'react-router-dom';
import { useAuth } from '@/store/auth';

// Ajusta si tu backend usa username en lugar de email
const schema = z.object({
  email: z.string().min(3, 'Correo requerido'),
  password: z.string().min(3, 'Contraseña requerida'),
});

type FormData = z.infer<typeof schema>;

export default function LoginPage() {
  const navigate = useNavigate();
  const location = useLocation() as any;
  const from = location.state?.from?.pathname || '/';

  // hooks del store real
  const login = useAuth((s) => s.login);
  const loading = useAuth((s) => s.loading);
  const error = useAuth((s) => s.error);

  const { register, handleSubmit, formState: { isSubmitting, errors } } =
    useForm<FormData>({
      resolver: zodResolver(schema),
      defaultValues: { email: '', password: '' },
    });

  const onSubmit = async (data: FormData) => {
    await login(data.email, data.password);
    if (!useAuth.getState().error) {
      navigate(from, { replace: true });
    }
  };

  return (
    <div className="mx-auto max-w-md rounded-xl border bg-white p-6 shadow-sm">
      <h1 className="mb-4 text-xl font-semibold">Entrar</h1>
      <p className="mb-4 text-sm text-slate-600">
        Ingresa tus credenciales (ADMINISTRADOR, ANALISTA, EVALUADOR o SOLICITANTE según tu usuario).
      </p>

      <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
        <label className="block text-sm">
          <span className="mb-1 block font-medium">Correo</span>
          <input
            className="w-full rounded border px-3 py-2"
            placeholder="admin@tuapp.com"
            {...register('email')}
          />
          {errors.email && (
            <span className="text-xs text-red-600">{errors.email.message}</span>
          )}
        </label>

        <label className="block text-sm">
          <span className="mb-1 block font-medium">Contraseña</span>
          <input
            type="password"
            className="w-full rounded border px-3 py-2"
            placeholder="••••••••"
            {...register('password')}
          />
          {errors.password && (
            <span className="text-xs text-red-600">{errors.password.message}</span>
          )}
        </label>

        {error && <div className="text-sm text-red-600">{error}</div>}

        <button
          disabled={isSubmitting || loading}
          className="w-full rounded bg-slate-900 px-4 py-2 text-white disabled:opacity-60"
        >
          {isSubmitting || loading ? 'Entrando…' : 'Entrar'}
        </button>
      </form>
    </div>
  );
}
