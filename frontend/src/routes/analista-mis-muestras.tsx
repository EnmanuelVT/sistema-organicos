import { useQuery } from '@tanstack/react-query'
import { getMyAssignedMuestras } from '../api/muestras'
import { useAuthStore } from '../store/auth'
import { Link } from 'react-router-dom'

export default function AnalistaMisMuestras() {
  const { user } = useAuthStore()
  const { data, isLoading } = useQuery({ 
    queryKey: ['my-assigned-muestras'], 
    queryFn: getMyAssignedMuestras, 
    enabled: !!user && user.role === 'Analista'
  })

  return (
    <div className='space-y-4'>
      <h1 className='text-xl font-semibold'>Mis muestras (Analista)</h1>
      {isLoading && <div>Cargando...</div>}
      <div className='overflow-hidden rounded-xl border bg-white'>
        <table className='w-full text-left text-sm'>
          <thead className='bg-slate-50 text-slate-600'>
            <tr>
              <th className='px-4 py-2'>Código</th>
              <th className='px-4 py-2'>Nombre</th>
              <th className='px-4 py-2'>Estado</th>
              <th className='px-4 py-2'>Acciones</th>
            </tr>
          </thead>
          <tbody>
            {data?.map(m => (
              <tr key={m.mstCodigo} className='border-t'>
                <td className='px-4 py-2 font-medium'>{m.mstCodigo}</td>
                <td className='px-4 py-2'>{m.nombre}</td>
                <td className='px-4 py-2'><span className='rounded bg-slate-100 px-2 py-1'>{m.estadoActual}</span></td>
                <td className='px-4 py-2'>
                  <Link to={`/analista/muestras/${m.mstCodigo}/prueba`} className='text-slate-700 underline'>Registrar prueba</Link>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  )
}
