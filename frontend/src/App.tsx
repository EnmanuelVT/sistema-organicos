import { Routes, Route, Navigate } from "react-router-dom";
import Protected from "@/components/Protected";
import RoleRoute from "@/routes/RoleRoute";
import Login from "@/routes/auth/login";
import Nav from "@/components/Nav";

import AdminAsignaciones from "@/routes/admin-asignaciones";
import AnalistaMisMuestras from "@/routes/analista-mis-muestras";
import EvaluadorBandeja from "@/routes/evaluador-bandeja";
import SolicitanteMisMuestras from "@/routes/solicitante-mis-muestras";

export default function App() {
  return (
    <>
      <Nav />
      <Routes>
        <Route path="/login" element={<Login />} />

        <Route
          path="/admin/*"
          element={
            <Protected>
              <RoleRoute role="ADMIN">
                <AdminAsignaciones />
              </RoleRoute>
            </Protected>
          }
        />

        <Route
          path="/analista/*"
          element={
            <Protected>
              <RoleRoute role="ANALISTA">
                <AnalistaMisMuestras />
              </RoleRoute>
            </Protected>
          }
        />

        <Route
          path="/evaluador/*"
          element={
            <Protected>
              <RoleRoute role="EVALUADOR">
                <EvaluadorBandeja />
              </RoleRoute>
            </Protected>
          }
        />

        <Route
          path="/solicitante/*"
          element={
            <Protected>
              <RoleRoute role="SOLICITANTE">
                <SolicitanteMisMuestras />
              </RoleRoute>
            </Protected>
          }
        />

        <Route path="/" element={<Navigate to="/login" replace />} />
        <Route path="*" element={<Navigate to="/login" replace />} />
      </Routes>
    </>
  );
}
