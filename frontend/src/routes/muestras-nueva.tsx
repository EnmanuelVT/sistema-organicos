import { useForm } from 'react-hook-form'
import { z } from 'zod'
import { zodResolver } from '@hookform/resolvers/zod'
import { useMutation, useQueryClient } from '@tanstack/react-query'
import { createMuestra } from '../api/muestras'
import { useNavigate } from 'react-router-dom'
import { useAuthStore } from '../store/auth'

const schema = z.object({
  mstCodigo: z.string().min(2, 'Código requerido'),
  tpmstId: z.number().min(1, 'Tipo de muestra requerido'),
  nombre: z.string().min(1, 'Nombre requerido'),
  origen: z.string().min(1, 'Origen requerido'),
  condicionesAlmacenamiento: z.string().optional(),
  condicionesTransporte: z.string().optional(),
  fechaRecepcion: z.string().min(1),
  estadoActual: z.number().default(1), // Default to "RECIBIDA" state
})

type FormData = z.infer<typeof schema>

export default function NuevaMuestra() {
  const { user } = useAuthStore()
  const qc = useQueryClient()
  const navigate = useNavigate()
  const { register, handleSubmit, formState: { errors, isSubmitting } } = useForm<FormData>({
    resolver: zodResolver(schema),
    defaultValues: {
      fechaRecepcion: new Date().toISOString().slice(0,16),
      tpmstId: 1, // Default to first sample type
      estadoActual: 1, // RECIBIDA state
    }
  })

  const { mutateAsync } = useMutation({
    mutationFn: async (data: FormData) => {
      return createMuestra({
        mstCodigo: data.mstCodigo,
        tpmstId: data.tpmstId,
        nombre: data.nombre,
        origen: data.origen,
        condicionesAlmacenamiento: data.condicionesAlmacenamiento,
        condicionesTransporte: data.condicionesTransporte,
        estadoActual: data.estadoActual,
      })
    },
    onSuccess: () => { 
      qc.invalidateQueries({ queryKey: ['muestras'] })
      navigate('/solicitante/mis-muestras') 
    }
  })

  return (
    <div className='mx-auto max-w-2xl rounded-xl border bg-white p-6'>
      <h1 className='mb-4 text-xl font-semibold'>Registrar nueva muestra</h1>
      <form onSubmit={handleSubmit((d)=>mutateAsync(d))} className='grid grid-cols-1 gap-4 sm:grid-cols-2'>
        <Text label='Código' error={errors.mstCodigo?.message}>
          <input className='input' {...register('mstCodigo')} />
        </Text>
        <Text label='Tipo de muestra' error={errors.tpmstId?.message}>
          <select className='input' {...register('tpmstId', { valueAsNumber: true })}>
            <option value={1}>Agua</option>
            <option value={2}>Alimento</option>
            <option value={3}>Bebida alcohólica</option>
          </select>
        </Text>
        <Text label='Nombre' error={errors.nombre?.message}>
          <input className='input' {...register('nombre')} />
        </Text>
        <Text label='Origen' error={errors.origen?.message}>
          <input className='input' {...register('origen')} />
        </Text>
        <Text label='Fecha recepción' error={errors.fechaRecepcion?.message}>
          <input type='datetime-local' className='input' {...register('fechaRecepcion')} />
        </Text>
        <Text label='Condiciones de almacenamiento' error={errors.condicionesAlmacenamiento?.message}>
          <input className='input' {...register('condicionesAlmacenamiento')} />
        </Text>
        <Text label='Condiciones de transporte' error={errors.condicionesTransporte?.message}>
          <input className='input' {...register('condicionesTransporte')} />
        </Text>
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
