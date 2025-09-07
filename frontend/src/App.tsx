// src/App.tsx
import { useEffect } from "react";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import { useAuth } from "./store/auth";
import Protected, { RequireNoAuth } from "./components/Protected";
import Nav from "./components/Nav"; // 🔹 nuevo

import Login from "./routes/auth/login";
import Dashboard from "./routes/dashboard";
import Muestras from "./routes/muestras";
import MuestraNueva from "./routes/muestras-nueva";
import MuestrasDetalle from "./routes/muestras-detalle";
import RegistrarPrueba from "./routes/analista-registrar-prueba";
import EvaluadorRevisar from "./routes/evaluador-revisar";
import AdminAsignaciones from "./routes/admin-asignaciones";

export default function App() {
  const hydrate = useAuth((s) => s.hydrate);
  useEffect(() => { hydrate(); }, [hydrate]);

  return (
    <BrowserRouter>
      {/* 🔹 aparece en todas las páginas autenticadas */}
      <Nav />

      <Routes>
        <Route
          path="/login"
          element={
            <RequireNoAuth>
              <Login />
            </RequireNoAuth>
          }
        />

        <Route path="/" element={<Protected><Dashboard/></Protected>} />
        <Route path="/muestras" element={<Protected><Muestras/></Protected>} />
        <Route path="/muestras/nueva" element={<Protected roles={['SOLICITANTE','ADMINISTRADOR']}><MuestraNueva/></Protected>} />
        <Route path="/muestras/:mstCodigo" element={<Protected><MuestrasDetalle/></Protected>} />
        <Route path="/ensayos/registrar/:mstCodigo" element={<Protected roles={['ANALISTA','ADMINISTRADOR']}><RegistrarPrueba/></Protected>} />
        <Route path="/evaluador/revisar/:mstCodigo" element={<Protected roles={['EVALUADOR','ADMINISTRADOR']}><EvaluadorRevisar/></Protected>} />
        <Route path="/admin/asignaciones" element={<Protected roles={['ADMINISTRADOR']}><AdminAsignaciones/></Protected>} />

        <Route path="*" element={<div className="p-6">404</div>} />
      </Routes>
    </BrowserRouter>
  );
}
