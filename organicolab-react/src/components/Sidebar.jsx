import React from 'react'
import { Link, useLocation } from 'react-router-dom'
import { Select, Divider } from 'antd'
import { useApp } from '../store/useAppStore.jsx'

export default function Sidebar() {
  const { rol, setRol, roles } = useApp()
  const loc = useLocation()
  const linkStyle = (path) => ({
    fontWeight: loc.pathname === path ? 'bold' : 'normal'
  })

  return (
    <aside className="sidebar">
      <h2>OrganicoLab</h2>
      <Divider style={{ borderColor: 'rgba(255,255,255,.25)' }} />
      <nav>
        <Link to="/solicitudes" style={linkStyle('/solicitudes')}>Solicitudes</Link><br/>
        {(rol === 'Analista' || rol === 'Administrador') && (
          <><Link to="/analisis" style={linkStyle('/analisis')}>Análisis</Link><br/></>
        )}
        {rol === 'Administrador' && (
          <Link to="/validacion" style={linkStyle('/validacion')}>Validación</Link>
        )}
      </nav>

      <div style={{ marginTop: 20 }}>
        <div style={{ fontSize: 12, opacity: .75 }}>Sesión (Rol)</div>
        <Select value={rol} style={{ width: '100%', marginTop: 4 }}
                onChange={setRol}
                options={roles.map(r => ({ label: r, value: r }))} />
      </div>

      <div className="footer">
        React + Ant Design · Demo
      </div>
    </aside>
  )
}
