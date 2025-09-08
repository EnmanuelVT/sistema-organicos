import React, { createContext, useContext, useState } from 'react'
import dayjs from 'dayjs'

const AppContext = createContext(null)

const defaultSolicitudes = [
  { id: 1, nombre: 'Muestra A', tipo: 'Líquida', estado: 'Enviada', campo4: 'Opción 1' },
  { id: 2, nombre: 'Muestra B', tipo: 'Sólida', estado: 'En análisis', campo3: true },
  { id: 3, nombre: 'Muestra C', tipo: 'Gas',    estado: 'Completada' }
]

const defaultInformes = [
  { id: 1, solicitudId: 3, jobTitle: 'INFORME RESULTADO', formato: 'PDF', version: '1.0', fecha: dayjs().format('YYYY-MM-DD'), resultados: 'Valores dentro del rango.', evaluador: null, validado: null }
]

export function AppProvider({ children }) {
  const [rol, setRol] = useState('Solicitante')
  const [solicitudes, setSolicitudes] = useState(defaultSolicitudes)
  const [informes, setInformes] = useState(defaultInformes)
  const roles = ['Solicitante', 'Analista', 'Administrador']

  return (
    <AppContext.Provider value={{ rol, setRol, roles, solicitudes, setSolicitudes, informes, setInformes }}>
      {children}
    </AppContext.Provider>
  )
}

export const useApp = () => useContext(AppContext)
