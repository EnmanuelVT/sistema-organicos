import { useParams } from 'react-router-dom'
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query'
import { getMuestraById, evaluateMuestra } from '../api/muestras'
import { getResultadosByMuestra } from '../api/ensayos'
import { useForm } from 'react-hook-form'

export default function EvaluadorRevisar() {
  const { id } = useParams()
  const qc = useQueryClient()
  const { data: muestra } = useQuery({ 
    queryKey: ['muestra', id], 
    queryFn: ()=> getMuestraById(id!),
    enabled: !!id
  })
  const { data: resultados } = useQuery({ 
    queryKey: ['resultados', id], 
    queryFn: ()=> getResultadosByMuestra(id!), 
    enabled: !!id 
  })

  const mut = useMutation({
    mutationFn: ({ aprobado, obs }: { aprobado: boolean, obs?: string }) => 
      evaluateMuestra(id!, { muestraId: id, aprobado, observaciones: obs }),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: ['muestra', id] })
      qc.invalidateQueries({ queryKey: ['resultados', id] })
      alert('Decisión registrada.')
    }
  })

  const { register, handleSubmit } = useForm<{ obs: string }>()

  if (!muestra) return <div>No encontrada.</div>

  return (
    <div className='space-y-6'>
      <header className='flex flex-col justify-between gap-3 sm:flex-row sm:items-center'>
        <h1 className='text-xl font-semibold'>Revisión de muestra {muestra.mstCodigo}</h1>
      </header>
      <section className='grid gap-4 sm:grid-cols-3'>
        <div className='rounded border bg-white p-4 sm:col-span-2'>
          <h2 className='mb-2 font-medium'>Resultados registrados</h2>
          {resultados?.length ? (
            <ul className='text-sm'>
              {resultados.map((r)=>(
                <li key={r.idResultado} className='border-t py-2 first:border-t-0'>
                  <b>Parámetro {r.idParametro}</b>: {r.valorObtenido} {r.unidad || ''}
                  {r.cumpleNorma !== null && (
                    <span className={`ml-2 px-2 py-1 rounded text-xs ${r.cumpleNorma ? 'bg-green-100 text-green-800' : 'bg-red-100 text-red-800'}`}>
                      {r.cumpleNorma ? 'Cumple' : 'No cumple'}
                    </span>
                  )}
                </li>
              ))}
            </ul>
          ) : <p className='text-sm text-slate-600'>No hay resultados registrados.</p>}

          <form onSubmit={handleSubmit((d)=>mut.mutate({ aprobado: false, obs: d.obs }))} className='mt-4 space-y-2'>
            <label className='text-sm'>
              <span className='mb-1 block font-medium'>Observaciones (si rechaza)</span>
              <textarea className='w-full rounded border px-3 py-2' rows={3} {...register('obs')} />
            </label>
            <div className='flex gap-2'>
              <button type='button' onClick={()=>mut.mutate({ aprobado: true })} className='rounded bg-green-600 px-4 py-2 text-white'>Aprobar</button>
              <button type='submit' className='rounded bg-red-600 px-4 py-2 text-white'>Rechazar</button>
            </div>
          </form>
        </div>
        <div className='rounded border bg-white p-4'>
          <h3 className='mb-2 font-medium'>Información de la muestra</h3>
          <dl className='text-sm space-y-1'>
            <div><dt className='font-medium'>Nombre:</dt><dd>{muestra.nombre}</dd></div>
            <div><dt className='font-medium'>Origen:</dt><dd>{muestra.origen}</dd></div>
            <div><dt className='font-medium'>Estado:</dt><dd>{muestra.estadoActual}</dd></div>
            <div><dt className='font-medium'>Fecha recepción:</dt><dd>{new Date(muestra.fechaRecepcion).toLocaleDateString()}</dd></div>
          </dl>
        </div>
      </section>
    </div>
  )
}
