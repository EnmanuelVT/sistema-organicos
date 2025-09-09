import { Link } from 'react-router-dom'
import { useAuthStore } from '../store/auth'

export default function Dashboard() {
  const { user } = useAuthStore()
  const role = user?.role
  
  if (!user) {
    return (
      <div className='space-y-6'>
        <h1 className='text-2xl font-semibold'>Cargando...</h1>
      </div>
    )
  }
  
  return (
    <div className='space-y-6'>
      <h1 className='text-2xl font-semibold'>Panel ({role})</h1>
      <div className='grid gap-4 sm:grid-cols-2 lg:grid-cols-3'>
        {role==='SOLICITANTE' && (<>
          <Card title='Mis muestras' to='/solicitante/mis-muestras' desc='Ver estado y registrar nuevas.' />
          <Card title='Certificados' to='/solicitante/certificados' desc='Descarga de certificados.' />
          <Card title='Notificaciones' to='/solicitante/notificaciones' desc='Alertas y avisos.' />
        </>)}
        {role==='Analista' && (<>
          <Card title='Mis muestras' to='/analista/mis-muestras' desc='Listado asignado.' />
        </>)}
        {role==='EVALUADOR' && (<>
          <Card title='Bandeja' to='/evaluador/bandeja' desc='Muestras con tests para decisión.' />
        </>)}
        {role==='Admin' && (<>
          <Card title='Asignaciones' to='/admin/asignaciones' desc='Asignar analista/evaluador.' />
          <Card title='Muestras' to='/muestras' desc='Vista general.' />
        </>)}
      </div>
    </div>
  )
}

function Card({ title, desc, to }: { title: string, desc: string, to: string }) {
  return (
    <Link to={to} className='block rounded-2xl border bg-white p-5 shadow-sm transition hover:shadow-md'>
      <div className='mb-2 text-lg font-medium'>{title}</div>
      <div className='text-sm text-slate-600'>{desc}</div>
    </Link>
  )
}
