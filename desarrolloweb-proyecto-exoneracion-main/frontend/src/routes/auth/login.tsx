import { useForm } from 'react-hook-form'
import { z } from 'zod'
import { zodResolver } from '@hookform/resolvers/zod'
import { useAuthStore } from '../../store/auth'
import { useNavigate, useLocation } from 'react-router-dom'
import { useState } from 'react'

const schema = z.object({
  email: z.string().email('Email válido requerido'),
  password: z.string().min(1, 'Contraseña requerida'),
})

type FormData = z.infer<typeof schema>

export default function LoginPage() {
  const navigate = useNavigate()
  const location = useLocation() as any
  const from = location.state?.from?.pathname || '/'
  const login = useAuthStore(s => s.login)
  const [error, setError] = useState<string | null>(null)
  
  const { register, handleSubmit, formState: { isSubmitting, errors } } = useForm<FormData>({
    resolver: zodResolver(schema),
    defaultValues: { email: '', password: '' }
  })

  const onSubmit = async (data: FormData) => {
    try {
      setError(null)
      await login(data.email, data.password)
      navigate(from, { replace: true })
    } catch (err) {
      setError('Error de autenticación. Verifica tus credenciales.')
    }
  }

  return (
    <div className='mx-auto max-w-md rounded-xl border bg-white p-6 shadow-sm'>
      <h1 className='mb-4 text-xl font-semibold'>Iniciar Sesión</h1>
      <p className='mb-4 text-sm text-slate-600'>Ingresa tu email y contraseña para acceder al sistema.</p>
      
      {error && (
        <div className='mb-4 rounded border border-red-200 bg-red-50 p-3 text-sm text-red-700'>
          {error}
        </div>
      )}
      
      <form onSubmit={handleSubmit(onSubmit)} className='space-y-4'>
        <label className='block text-sm'>
          <span className='mb-1 block font-medium'>Email</span>
          <input 
            type="email"
            className='w-full rounded border px-3 py-2' 
            placeholder='usuario@ejemplo.com' 
            {...register('email')} 
          />
          {errors.email && <span className='text-xs text-red-600'>{errors.email.message}</span>}
        </label>
        
        <label className='block text-sm'>
          <span className='mb-1 block font-medium'>Contraseña</span>
          <input 
            type="password"
            className='w-full rounded border px-3 py-2' 
            placeholder='Contraseña' 
            {...register('password')} 
          />
          {errors.password && <span className='text-xs text-red-600'>{errors.password.message}</span>}
        </label>
        
        <button disabled={isSubmitting} className='w-full rounded bg-slate-900 px-4 py-2 text-white disabled:opacity-50'>
          {isSubmitting ? 'Iniciando sesión...' : 'Iniciar Sesión'}
        </button>
      </form>
    </div>
  )
}
