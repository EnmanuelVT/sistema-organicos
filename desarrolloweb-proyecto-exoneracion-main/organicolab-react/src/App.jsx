import React from 'react'
import { Routes, Route, Navigate } from 'react-router-dom'
import Sidebar from './components/Sidebar.jsx'
import SolicitudesPage from './pages/SolicitudesPage.jsx'
import AnalisisPage from './pages/AnalisisPage.jsx'
import ValidacionPage from './pages/ValidacionPage.jsx'
import { useApp } from './store/useAppStore.jsx'

export default function App() {
  const { rol } = useApp()
  return (
    <div className="app-shell">
      <Sidebar />
      <div className="content">
        <Routes>
          <Route path="/" element={<Navigate to="/solicitudes" replace />} />
          <Route path="/solicitudes" element={<SolicitudesPage />} />
          {(rol === 'Analista' || rol === 'Administrador') && (
            <Route path="/analisis" element={<AnalisisPage />} />
          )}
          {rol === 'Administrador' && (
            <Route path="/validacion" element={<ValidacionPage />} />
          )}
          <Route path="*" element={<h3>404 - No encontrado</h3>} />
        </Routes>
      </div>
    </div>
  )
}
