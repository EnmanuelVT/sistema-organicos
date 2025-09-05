import { useParams } from 'react-router-dom'
import { useForm } from 'react-hook-form'
import { z } from 'zod'
import { zodResolver } from '@hookform/resolvers/zod'
import { useMutation } from '@tanstack/react-query'
import { registrarTest } from '../libs/fakeApi'

const schema = z.object({
  nombre: z.string().min(1),
  unidad: z.string().optional(),
  valor: z.coerce.number(),
})

type FormData = z.infer<typeof schema>

export default function RegistrarPrueba() {
  const { id } = useParams()
  const { register, handleSubmit, formState: { errors, isSubmitting } } = useForm<FormData>({
    resolver: zodResolver(schema),
    defaultValues: { nombre: '', unidad: '', valor: 0 }
  })
  const { mutateAsync } = useMutation({
    mutationFn: (d: FormData) => registrarTest(id!, d),
    onSuccess: () => alert('Prueba registrada. La muestra queda pendiente de evaluación.')
  })

  return (
    <div className='mx-auto max-w-md rounded-xl border bg-white p-6'>
      <h1 className='mb-4 text-xl font-semibold'>Registrar prueba para {id}</h1>
      <form onSubmit={handleSubmit((d)=>mutateAsync(d))} className='space-y-3'>
        <Field label='Nombre del parámetro' error={errors.nombre?.message}>
          <input className='input' {...register('nombre')} />
        </Field>
        <Field label='Unidad'>
          <input className='input' {...register('unidad')} placeholder='% , mg/L , pH, etc.' />
        </Field>
        <Field label='Valor' error={errors.valor?.message}>
          <input className='input' type='number' step='any' {...register('valor', { valueAsNumber: true })} />
        </Field>
        <div className='flex justify-end gap-2'>
          <button type='button' onClick={()=>history.back()} className='rounded border px-4 py-2'>Cancelar</button>
          <button disabled={isSubmitting} className='rounded bg-slate-900 px-4 py-2 text-white'>{isSubmitting? 'Guardando...' : 'Guardar'}</button>
        </div>
      </form>
      <style>{`.input{width:100%; @apply rounded border px-3 py-2}`}</style>
    </div>
  )
}

function Field({ label, error, children }: { label: string, error?: string, children: React.ReactNode }) {
  return (
    <label className='text-sm'>
      <span className='mb-1 block font-medium'>{label}</span>
      {children}
      {error && <span className='mt-1 block text-xs text-red-600'>{error}</span>}
    </label>
  )
}
