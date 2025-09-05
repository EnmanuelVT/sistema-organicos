import { useParams } from 'react-router-dom'
import { useQuery } from '@tanstack/react-query'
import { getMuestra, listDocumentosByMuestra } from '../libs/fakeApi'

export default function MuestraDetalle() {
  const { id } = useParams()
  const { data: muestra } = useQuery({ queryKey: ['muestra', id], queryFn: ()=> getMuestra(id!) })
  const { data: docs } = useQuery({ queryKey: ['docs', id], queryFn: ()=> listDocumentosByMuestra(id!), enabled: !!id })

  if (!muestra) return <div>No encontrada.</div>

  return (
    <div className='space-y-6'>
      <header className='flex flex-col justify-between gap-3 sm:flex-row sm:items-center'>
        <h1 className='text-xl font-semibold'>Muestra {muestra.codigo}</h1>
      </header>
      <section className='grid gap-4 sm:grid-cols-3'>
        <div className='rounded border bg-white p-4 sm:col-span-2'>
          <h2 className='mb-2 font-medium'>Resumen</h2>
          <ul className='text-sm text-slate-700'>
            <li><b>Tipo:</b> {muestra.tipo}</li>
            <li><b>Origen:</b> {muestra.origen}</li>
            <li><b>Estado:</b> {muestra.estado}</li>
            <li><b>Fecha recepción:</b> {new Date(muestra.fechaRecepcion).toLocaleString()}</li>
            <li><b>Responsable:</b> {muestra.responsableTecnicoId}</li>
            <li><b>Solicitante:</b> {muestra.solicitante.nombre} ({muestra.solicitante.contacto})</li>
            <li><b>Analista:</b> {muestra.analistaId || '-'}</li>
            <li><b>Evaluador:</b> {muestra.evaluadorId || '-'}</li>
            {muestra.observaciones && <li><b>Obs. evaluador:</b> {muestra.observaciones}</li>}
          </ul>
          <div className='mt-4'>
            <h3 className='font-medium'>Tests</h3>
            {!muestra.tests.length ? <p className='text-sm text-slate-600'>Sin tests.</p> :
              <ul className='text-sm'>{muestra.tests.map((t,i)=>(<li key={i}>• {t.nombre}: {t.valor} {t.unidad||''}</li>))}</ul>}
          </div>
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
