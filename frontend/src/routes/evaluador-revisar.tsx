import { useParams } from 'react-router-dom'
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query'
import { getMuestra, evaluarMuestra, listDocumentosByMuestra } from '../libs/fakeApi'
import { useForm } from 'react-hook-form'

export default function EvaluadorRevisar() {
  const { id } = useParams()
  const qc = useQueryClient()
  const { data: muestra } = useQuery({ queryKey: ['muestra', id], queryFn: ()=> getMuestra(id!) })
  const { data: docs } = useQuery({ queryKey: ['docs', id], queryFn: ()=> listDocumentosByMuestra(id!), enabled: !!id })

  const mut = useMutation({
    mutationFn: ({ aprobado, obs }: { aprobado: boolean, obs?: string }) => evaluarMuestra(id!, aprobado, obs),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: ['muestra', id] })
      qc.invalidateQueries({ queryKey: ['docs', id] })
      alert('Decisión registrada.')
    }
  })

  const { register, handleSubmit } = useForm<{ obs: string }>()

  if (!muestra) return <div>No encontrada.</div>

  return (
    <div className='space-y-6'>
      <header className='flex flex-col justify-between gap-3 sm:flex-row sm:items-center'>
        <h1 className='text-xl font-semibold'>Revisión de muestra {muestra.codigo}</h1>
      </header>
      <section className='grid gap-4 sm:grid-cols-3'>
        <div className='rounded border bg-white p-4 sm:col-span-2'>
          <h2 className='mb-2 font-medium'>Tests registrados</h2>
          {muestra.tests.length ? (
            <ul className='text-sm'>
              {muestra.tests.map((t, i)=>(
                <li key={i} className='border-t py-2 first:border-t-0'>
                  <b>{t.nombre}</b>: {t.valor} {t.unidad || ''}
                </li>
              ))}
            </ul>
          ) : <p className='text-sm text-slate-600'>No hay pruebas registradas.</p>}

          <form onSubmit={handleSubmit((d)=>mut.mutate({ aprobado: false, obs: d.obs }))} className='mt-4 space-y-2'>
            <label className='text-sm'>
              <span className='mb-1 block font-medium'>Observaciones (si rechaza)</span>
              <textarea className='w-full rounded border px-3 py-2' rows={3} {...register('obs')} />
            </label>
            <div className='flex gap-2'>
              <button type='button' onClick={()=>mut.mutate({ aprobado: true })} className='rounded bg-green-600 px-4 py-2 text-white'>Aprobar y generar PDF</button>
              <button type='submit' className='rounded bg-red-600 px-4 py-2 text-white'>Rechazar</button>
            </div>
          </form>
        </div>
        <div className='rounded border bg-white p-4'>
          <h2 className='mb-2 font-medium'>Documentos</h2>
          <ul className='text-sm'>
            {docs?.length ? docs.map(d => (
              <li key={d.id} className='flex items-center justify-between border-t py-2 first:border-t-0'>
                <span>#{d.version} - {d.tipo}</span>
                <a className='text-slate-700 underline' href={d.url} target='_blank'>Ver</a>
              </li>
            )) : <li className='text-slate-600'>Sin documentos</li>}
          </ul>
        </div>
      </section>
    </div>
  )
}
