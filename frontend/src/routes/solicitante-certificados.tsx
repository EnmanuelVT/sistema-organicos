import { useQuery } from '@tanstack/react-query'
import { getMyMuestras } from '../api/muestras'
import { useAuthStore } from '../store/auth'

export default function SolicitanteCertificados() {
  const { user } = useAuthStore()
  const { data: muestras } = useQuery({ 
    queryKey: ['my-muestras-certificados'], 
    queryFn: getMyMuestras, 
    enabled: !!user 
  })

  // Filter only certified samples (assuming state 4 or similar represents certified)
  const muestrasCertificadas = muestras?.filter(m => m.estadoActual >= 4) || []

  return (
    <div className='space-y-4'>
      <h1 className='text-xl font-semibold'>Certificados y documentos</h1>
      {!muestrasCertificadas.length ? (
        <p className='text-sm text-slate-600'>Aún no tienes muestras certificadas.</p>
      ) : (
        muestrasCertificadas.map(muestra => (
          <div key={muestra.mstCodigo} className='rounded border bg-white p-4'>
            <h2 className='font-medium'>Muestra {muestra.mstCodigo}</h2>
            <div className='text-sm text-slate-600 mt-1'>
              <p><b>Nombre:</b> {muestra.nombre}</p>
              <p><b>Origen:</b> {muestra.origen}</p>
              <p><b>Estado:</b> {muestra.estadoActual}</p>
              <p><b>Fecha recepción:</b> {new Date(muestra.fechaRecepcion).toLocaleDateString()}</p>
            </div>
            <div className='mt-3'>
              <p className='text-sm text-slate-500'>
                Los documentos y certificados estarán disponibles cuando se implemente la funcionalidad de generación de documentos en el backend.
              </p>
            </div>
          </div>
        ))
      )}
    </div>
  )
}
