import { useQuery } from '@tanstack/react-query'
import { listMuestras } from '../libs/fakeApi'
import { Link } from 'react-router-dom'
import { useAuthStore } from '../store/auth'

export default function MuestrasList() {
  const { data, isLoading } = useQuery({ queryKey: ['muestras'], queryFn: listMuestras })
  const { role } = useAuthStore()
  return (
    <div className='space-y-4'>
      <div className='flex items-center justify-between'>
        <h1 className='text-xl font-semibold'>Muestras (todas)</h1>
        {(role==='SOLICITANTE' || role==='ADMIN') && (
          <Link to='/muestras/nueva' className='rounded bg-slate-900 px-4 py-2 text-white'>Registrar muestra</Link>
        )}
      </div>
      <div className='overflow-hidden rounded-xl border bg-white'>
        <table className='w-full text-left text-sm'>
          <thead className='bg-slate-50 text-slate-600'>
            <tr>
              <th className='px-4 py-2'>Código</th>
              <th className='px-4 py-2'>Tipo</th>
              <th className='px-4 py-2'>Estado</th>
            </tr>
          </thead>
          <tbody>
            {isLoading && <tr><td className='px-4 py-6' colSpan={3}>Cargando...</td></tr>}
            {data?.map(m => (
              <tr key={m.id} className='border-t'>
                <td className='px-4 py-2 font-medium'>{m.codigo}</td>
                <td className='px-4 py-2'>{m.tipo}</td>
                <td className='px-4 py-2'><span className='rounded bg-slate-100 px-2 py-1'>{m.estado}</span></td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  )
}
