import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query'
import { listMuestras, assignAnalista, assignEvaluador } from '../libs/fakeApi'
import { useState } from 'react'

export default function AdminAsignaciones() {
  const { data } = useQuery({ queryKey: ['muestras-all'], queryFn: listMuestras })
  const qc = useQueryClient()
  const [analista, setAnalista] = useState('analista-1')
  const [evaluador, setEvaluador] = useState('eval-1')

  const mutAnalista = useMutation({
    mutationFn: ({ id, analistaId }: { id: string, analistaId: string }) => assignAnalista(id, analistaId),
    onSuccess: () => qc.invalidateQueries({ queryKey: ['muestras-all'] })
  })
  const mutEvaluador = useMutation({
    mutationFn: ({ id, evaluadorId }: { id: string, evaluadorId: string }) => assignEvaluador(id, evaluadorId),
    onSuccess: () => qc.invalidateQueries({ queryKey: ['muestras-all'] })
  })

  return (
    <div className='space-y-4'>
      <h1 className='text-xl font-semibold'>Asignaciones</h1>
      <div className='overflow-hidden rounded-xl border bg-white'>
        <table className='w-full text-left text-sm'>
          <thead className='bg-slate-50 text-slate-600'>
            <tr>
              <th className='px-4 py-2'>Código</th>
              <th className='px-4 py-2'>Estado</th>
              <th className='px-4 py-2'>Analista</th>
              <th className='px-4 py-2'>Evaluador</th>
              <th className='px-4 py-2'>Acciones</th>
            </tr>
          </thead>
          <tbody>
            {data?.map(m => (
              <tr key={m.id} className='border-t'>
                <td className='px-4 py-2 font-medium'>{m.codigo}</td>
                <td className='px-4 py-2'><span className='rounded bg-slate-100 px-2 py-1'>{m.estado}</span></td>
                <td className='px-4 py-2'>{m.analistaId || '-'}</td>
                <td className='px-4 py-2'>{m.evaluadorId || '-'}</td>
                <td className='px-4 py-2'>
                  <div className='flex flex-wrap gap-2'>
                    <select className='rounded border px-2 py-1' value={analista} onChange={e=>setAnalista(e.target.value)}>
                      <option value='analista-1'>analista-1</option>
                      <option value='analista-2'>analista-2</option>
                    </select>
                    <button className='rounded bg-slate-900 px-3 py-1 text-white' onClick={()=>mutAnalista.mutate({ id: m.id, analistaId: analista })}>Asignar analista</button>
                    <select className='rounded border px-2 py-1' value={evaluador} onChange={e=>setEvaluador(e.target.value)}>
                      <option value='eval-1'>eval-1</option>
                      <option value='eval-2'>eval-2</option>
                    </select>
                    <button className='rounded bg-slate-900 px-3 py-1 text-white' onClick={()=>mutEvaluador.mutate({ id: m.id, evaluadorId: evaluador })}>Asignar evaluador</button>
                  </div>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  )
}
