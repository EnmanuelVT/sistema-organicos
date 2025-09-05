import { useForm } from 'react-hook-form'
import { z } from 'zod'
import { zodResolver } from '@hookform/resolvers/zod'
import { useAuthStore } from '../../store/auth'
import { useNavigate, useLocation } from 'react-router-dom'

const schema = z.object({
  role: z.enum(['SOLICITANTE','ANALISTA','EVALUADOR','ADMIN']),
  userId: z.string().min(3, 'ID de usuario requerido'),
})

type FormData = z.infer<typeof schema>

export default function LoginPage() {
  const navigate = useNavigate()
  const location = useLocation() as any
  const from = location.state?.from?.pathname || '/'
  const login = useAuthStore(s => s.login)
  const { register, handleSubmit, formState: { isSubmitting, errors } } = useForm<FormData>({
    resolver: zodResolver(schema),
    defaultValues: { role: 'SOLICITANTE', userId: 'sol-1' }
  })

  const onSubmit = (data: FormData) => {
    login(data.role, data.userId)
    navigate(from, { replace: true })
  }

  return (
    <div className='mx-auto max-w-md rounded-xl border bg-white p-6 shadow-sm'>
      <h1 className='mb-4 text-xl font-semibold'>Entrar</h1>
      <p className='mb-4 text-sm text-slate-600'>Elige rol y un ID (ej: sol-1, analista-1, eval-1).</p>
      <form onSubmit={handleSubmit(onSubmit)} className='space-y-4'>
        <label className='block text-sm'>
          <span className='mb-1 block font-medium'>Rol</span>
          <select className='w-full rounded border px-3 py-2' {...register('role')}>
            <option value='SOLICITANTE'>Solicitante</option>
            <option value='ANALISTA'>Analista</option>
            <option value='EVALUADOR'>Evaluador</option>
            <option value='ADMIN'>Administrador</option>
          </select>
        </label>
        <label className='block text-sm'>
          <span className='mb-1 block font-medium'>ID de usuario</span>
          <input className='w-full rounded border px-3 py-2' placeholder='sol-1 / analista-1 / eval-1' {...register('userId')} />
          {errors.userId && <span className='text-xs text-red-600'>{errors.userId.message}</span>}
        </label>
        <button disabled={isSubmitting} className='w-full rounded bg-slate-900 px-4 py-2 text-white'>
          {isSubmitting ? 'Entrando...' : 'Entrar'}
        </button>
      </form>
    </div>
  )
}
