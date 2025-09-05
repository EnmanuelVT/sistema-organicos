import { useQuery } from '@tanstack/react-query'
import { listMuestrasBySolicitante, listDocumentosByMuestra } from '../libs/fakeApi'
import { useAuthStore } from '../store/auth'
import { useEffect, useState } from 'react'

export default function SolicitanteCertificados() {
  const { userId } = useAuthStore()
  const { data: muestras } = useQuery({ queryKey: ['muestras-solic', userId], queryFn: ()=> listMuestrasBySolicitante(userId!), enabled: !!userId })
  const [docs, setDocs] = useState<{ muestraId: string, items: any[] }[]>([])

  useEffect(()=>{
    async function load() {
      if (!muestras) return
      const all: { muestraId: string, items: any[] }[] = []
      for (const m of muestras) {
        const d = await listDocumentosByMuestra(m.id)
        if (d.length) all.push({ muestraId: m.id, items: d })
      }
      setDocs(all)
    }
    load()
  }, [muestras])

  return (
    <div className='space-y-4'>
      <h1 className='text-xl font-semibold'>Certificados recibidos</h1>
      {!docs.length ? <p className='text-sm text-slate-600'>Aún no tienes certificados.</p> :
      docs.map(group => (
        <div key={group.muestraId} className='rounded border bg-white p-4'>
          <h2 className='font-medium'>Muestra {group.muestraId}</h2>
          <ul className='text-sm'>
            {group.items.map(d => (
              <li key={d.id} className='flex items-center justify-between border-t py-2 first:border-t-0'>
                <span>#{d.version} - {d.tipo}</span>
                <a className='text-slate-700 underline' href={d.url} target='_blank'>Descargar</a>
              </li>
            ))}
          </ul>
        </div>
      ))}
    </div>
  )
}
