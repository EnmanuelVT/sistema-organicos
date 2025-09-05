import { useForm } from 'react-hook-form'
import { z } from 'zod'
import { zodResolver } from '@hookform/resolvers/zod'
import { useMutation, useQueryClient } from '@tanstack/react-query'
import { createMuestra } from '../libs/fakeApi'
import { useNavigate } from 'react-router-dom'
import { useAuthStore } from '../store/auth'

const schema = z.object({
  codigo: z.string().min(2, 'Código requerido'),
  tipo: z.enum(['ALIMENTO','AGUA','BEBIDA_ALCOHOLICA']),
  fechaRecepcion: z.string().min(1),
  origen: z.string().min(1),
  condiciones: z.string().min(1),
  solicitanteNombre: z.string().min(1),
  solicitanteDireccion: z.string().min(1),
  solicitanteContacto: z.string().min(1),
  responsableTecnicoId: z.string().min(1),
})

type FormData = z.infer<typeof schema>

export default function NuevaMuestra() {
  const { userId } = useAuthStore()
  const qc = useQueryClient()
  const navigate = useNavigate()
  const { register, handleSubmit, formState: { errors, isSubmitting } } = useForm<FormData>({
    resolver: zodResolver(schema),
    defaultValues: {
      fechaRecepcion: new Date().toISOString().slice(0,16),
      tipo: 'AGUA',
    }
  })

  const { mutateAsync } = useMutation({
    mutationFn: async (data: FormData) => {
      return createMuestra({
        codigo: data.codigo,
        tipo: data.tipo,
        fechaRecepcion: new Date(data.fechaRecepcion).toISOString(),
        origen: data.origen,
        condiciones: data.condiciones,
        solicitante: { nombre: data.solicitanteNombre, direccion: data.solicitanteDireccion, contacto: data.solicitanteContacto },
        solicitanteId: userId || 'sol-unknown',
        responsableTecnicoId: data.responsableTecnicoId,
      })
    },
    onSuccess: () => { qc.invalidateQueries({ queryKey: ['muestras'] }); navigate('/solicitante/mis-muestras') }
  })

  return (
    <div className='mx-auto max-w-2xl rounded-xl border bg-white p-6'>
      <h1 className='mb-4 text-xl font-semibold'>Registrar nueva muestra</h1>
      <form onSubmit={handleSubmit((d)=>mutateAsync(d))} className='grid grid-cols-1 gap-4 sm:grid-cols-2'>
        <Text label='Código' error={errors.codigo?.message}><input className='input' {...register('codigo')} /></Text>
        <Text label='Tipo' error={errors.tipo?.message}>
          <select className='input' {...register('tipo')}>
            <option value='ALIMENTO'>Alimento</option>
            <option value='AGUA'>Agua</option>
            <option value='BEBIDA_ALCOHOLICA'>Bebida alcohólica</option>
          </select>
        </Text>
        <Text label='Fecha recepción' error={errors.fechaRecepcion?.message}><input type='datetime-local' className='input' {...register('fechaRecepcion')} /></Text>
        <Text label='Origen' error={errors.origen?.message}><input className='input' {...register('origen')} /></Text>
        <Text label='Condiciones' error={errors.condiciones?.message}><input className='input' {...register('condiciones')} /></Text>
        <Text label='Solicitante (nombre)' error={errors.solicitanteNombre?.message}><input className='input' {...register('solicitanteNombre')} /></Text>
        <Text label='Solicitante (dirección)' error={errors.solicitanteDireccion?.message}><input className='input' {...register('solicitanteDireccion')} /></Text>
        <Text label='Solicitante (contacto)' error={errors.solicitanteContacto?.message}><input className='input' {...register('solicitanteContacto')} /></Text>
        <Text label='Responsable técnico (ID)' error={errors.responsableTecnicoId?.message}><input className='input' {...register('responsableTecnicoId')} /></Text>
        <div className='col-span-full flex justify-end gap-3 pt-2'>
          <button type='button' onClick={()=>history.back()} className='rounded border px-4 py-2'>Cancelar</button>
          <button disabled={isSubmitting} className='rounded bg-slate-900 px-4 py-2 text-white'>{isSubmitting ? 'Guardando...' : 'Guardar'}</button>
        </div>
      </form>
      <style>{`.input{width:100%; @apply rounded border px-3 py-2}`}</style>
    </div>
  )
}

function Text({ label, error, children }: { label: string, error?: string, children: React.ReactNode }) {
  return (
    <label className='text-sm'>
      <span className='mb-1 block font-medium'>{label}</span>
      {children}
      {error && <span className='mt-1 block text-xs text-red-600'>{error}</span>}
    </label>
  )
}
