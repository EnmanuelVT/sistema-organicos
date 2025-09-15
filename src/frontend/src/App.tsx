import { Routes, Route, Navigate } from "react-router-dom";
import { useAuthStore } from "@/store/auth";
import Layout from "@/components/Layout";
import LoginPage from "@/pages/auth/LoginPage";
import DashboardPage from "@/pages/DashboardPage";
import SamplesPage from "@/pages/samples/SamplesPage";
import CreateSamplePage from "@/pages/samples/CreateSamplePage";
import SampleDetailPage from "@/pages/samples/SampleDetailPage";
import TestsPage from "@/pages/tests/TestsPage";
import CreateTestPage from "@/pages/tests/CreateTestPage";
import ResultsPage from "@/pages/results/ResultsPage";
import CreateResultPage from "@/pages/results/CreateResultPage";
import EvaluationPage from "@/pages/evaluation/EvaluationPage";
import UsersPage from "@/pages/admin/UsersPage";
import ParametersPage from "@/pages/admin/ParametersPage";
import AuditPage from "@/pages/admin/AuditPage";
import TraceabilityPage from "@/pages/admin/TraceabilityPage";
import ProtectedRoute from "@/components/ProtectedRoute";
import RoleRoute from "@/components/RoleRoute";

export default function App() {
  const { user } = useAuthStore();

  if (!user) {
    return (
      <Routes>
        <Route path="/login" element={<LoginPage />} />
        <Route path="*" element={<Navigate to="/login" replace />} />
      </Routes>
    );
  }

  return (
    <Layout>
      <Routes>
        <Route path="/login" element={<Navigate to="/dashboard" replace />} />
        
        <Route
          path="/dashboard"
          element={
            <ProtectedRoute>
              <DashboardPage />
            </ProtectedRoute>
          }
        />

        {/* Samples Routes */}
        <Route
          path="/samples"
          element={
            <ProtectedRoute>
              <SamplesPage />
            </ProtectedRoute>
          }
        />
        <Route
          path="/samples/create"
          element={
            <ProtectedRoute>
              <RoleRoute allowedRoles={["SOLICITANTE", "ADMIN"]}>
                <CreateSamplePage />
              </RoleRoute>
            </ProtectedRoute>
          }
        />
        <Route
          path="/samples/:id"
          element={
            <ProtectedRoute>
              <SampleDetailPage />
            </ProtectedRoute>
          }
        />

        {/* Tests Routes */}
        <Route
          path="/tests"
          element={
            <ProtectedRoute>
              <RoleRoute allowedRoles={["ANALISTA", "ADMIN"]}>
                <TestsPage />
              </RoleRoute>
            </ProtectedRoute>
          }
        />
        <Route
          path="/tests/create"
          element={
            <ProtectedRoute>
              <RoleRoute allowedRoles={["ANALISTA", "ADMIN"]}>
                <CreateTestPage />
              </RoleRoute>
            </ProtectedRoute>
          }
        />

        {/* Results Routes */}
        <Route
          path="/results"
          element={
            <ProtectedRoute>
              <RoleRoute allowedRoles={["ANALISTA", "ADMIN"]}>
                <ResultsPage />
              </RoleRoute>
            </ProtectedRoute>
          }
        />
        <Route
          path="/results/create"
          element={
            <ProtectedRoute>
              <RoleRoute allowedRoles={["ANALISTA", "ADMIN"]}>
                <CreateResultPage />
              </RoleRoute>
            </ProtectedRoute>
          }
        />

        {/* Evaluation Routes */}
        <Route
          path="/evaluation"
          element={
            <ProtectedRoute>
              <RoleRoute allowedRoles={["EVALUADOR", "ADMIN"]}>
                <EvaluationPage />
              </RoleRoute>
            </ProtectedRoute>
          }
        />

        {/* Admin Routes */}
        <Route
          path="/admin/users"
          element={
            <ProtectedRoute>
              <RoleRoute allowedRoles={["ADMIN"]}>
                <UsersPage />
              </RoleRoute>
            </ProtectedRoute>
          }
        />
        <Route
          path="/admin/parameters"
          element={
            <ProtectedRoute>
              <RoleRoute allowedRoles={["ADMIN"]}>
                <ParametersPage />
              </RoleRoute>
            </ProtectedRoute>
          }
        />

        <Route
          path="/admin/audit"
          element={
            <ProtectedRoute>
              <RoleRoute allowedRoles={["ADMIN"]}>
                <AuditPage />
              </RoleRoute>
            </ProtectedRoute>
          }
        />

        <Route
          path="/admin/traceability"
          element={
            <ProtectedRoute>
              <RoleRoute allowedRoles={["ADMIN"]}>
                <TraceabilityPage />
              </RoleRoute>
            </ProtectedRoute>
          }
        />

        <Route path="/" element={<Navigate to="/dashboard" replace />} />
        <Route path="*" element={<Navigate to="/dashboard" replace />} />
      </Routes>
    </Layout>
  );
}
