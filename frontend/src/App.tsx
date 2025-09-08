import { Routes, Route, Navigate, Link } from 'react-router-dom'
import LoginPage from './routes/auth/login'
import Dashboard from './routes/dashboard'
import MuestrasList from './routes/muestras'
import NuevaMuestra from './routes/muestras-nueva'
import MuestraDetalle from './routes/muestras-detalle'
import AdminAsignaciones from './routes/admin-asignaciones'
import AnalistaMisMuestras from './routes/analista-mis-muestras'
import RegistrarPrueba from './routes/analista-registrar-prueba'
import EvaluadorBandeja from './routes/evaluador-bandeja'
import EvaluadorRevisar from './routes/evaluador-revisar'
import SolicitanteMisMuestras from './routes/solicitante-mis-muestras'
import SolicitanteCertificados from './routes/solicitante-certificados'
import SolicitanteNotificaciones from './routes/solicitante-notificaciones'
import { useAuthStore, type Role } from './store/auth'
import Protected from './components/Protected'

export default function App() {
  const isAuth = useAuthStore(s => !!s.token)
  return (
    <div className='min-h-screen'>
      <TopNav />
      <div className='mx-auto max-w-7xl px-4 py-6'>
        <Routes>
          <Route path='/login' element={isAuth ? <Navigate to='/' /> : <LoginPage />} />
          <Route path='/' element={<Protected><Dashboard /></Protected>} />

          {/* Comunes */}
          <Route path='/muestras' element={<Protected roles={rolesAll()}><MuestrasList /></Protected>} />
          <Route path='/muestras/nueva' element={<Protected roles={['SOLICITANTE','ADMIN']}><NuevaMuestra /></Protected>} />
          <Route path='/muestras/:id' element={<Protected roles={rolesAll()}><MuestraDetalle /></Protected>} />

          {/* Solicitante */}
          <Route path='/solicitante/mis-muestras' element={<Protected roles={['SOLICITANTE','ADMIN']}><SolicitanteMisMuestras /></Protected>} />
          <Route path='/solicitante/certificados' element={<Protected roles={['SOLICITANTE','ADMIN']}><SolicitanteCertificados /></Protected>} />
          <Route path='/solicitante/notificaciones' element={<Protected roles={['SOLICITANTE','ADMIN']}><SolicitanteNotificaciones /></Protected>} />

          {/* Analista */}
          <Route path='/analista/mis-muestras' element={<Protected roles={['ANALISTA','ADMIN']}><AnalistaMisMuestras /></Protected>} />
          <Route path='/analista/muestras/:id/prueba' element={<Protected roles={['ANALISTA','ADMIN']}><RegistrarPrueba /></Protected>} />

          {/* Evaluador */}
          <Route path='/evaluador/bandeja' element={<Protected roles={['EVALUADOR','ADMIN']}><EvaluadorBandeja /></Protected>} />
          <Route path='/evaluador/muestras/:id/revisar' element={<Protected roles={['EVALUADOR','ADMIN']}><EvaluadorRevisar /></Protected>} />

          {/* Admin */}
          <Route path='/admin/asignaciones' element={<Protected roles={['ADMIN']}><AdminAsignaciones /></Protected>} />

          <Route path='*' element={<Navigate to={isAuth ? '/' : '/login'} />} />
        </Routes>
      </div>
    </div>
  )
}

function rolesAll(): Role[] { return ['SOLICITANTE','ANALISTA','EVALUADOR','ADMIN'] }

function TopNav() {
  const { token, role, logout } = useAuthStore()
  return (
    <header className='border-b bg-white'>
      <div className='mx-auto flex max-w-7xl items-center justify-between px-4 py-3'>
        <Link to='/' className='font-semibold text-slate-800'>Sistema de Trazabilidad</Link>
        <nav className='flex items-center gap-4 text-sm'>
          {token && (
            <>
              {role === 'SOLICITANTE' && (<>
                <NavLink to='/solicitante/mis-muestras' label='Mis muestras' />
                <NavLink to='/solicitante/certificados' label='Certificados' />
                <NavLink to='/solicitante/notificaciones' label='Notificaciones' />
              </>)}
              {role === 'ANALISTA' && (<>
                <NavLink to='/analista/mis-muestras' label='Mis muestras' />
              </>)}
              {role === 'EVALUADOR' && (<>
                <NavLink to='/evaluador/bandeja' label='Bandeja' />
              </>)}
              {role === 'ADMIN' && (<>
                <NavLink to='/admin/asignaciones' label='Asignaciones' />
                <NavLink to='/muestras' label='Muestras' />
              </>)}
              <span className='rounded bg-slate-100 px-2 py-1'>{role}</span>
              <button onClick={logout} className='rounded bg-slate-900 px-3 py-1 text-white'>Salir</button>
            </>
          )}
          {!token && <NavLink to='/login' label='Entrar' />}
        </nav>
      </div>
    </header>
  )
}

function NavLink({ to, label }: { to: string, label: string }) {
  return <Link className='text-slate-700 hover:text-slate-900' to={to}>{label}</Link>
}
