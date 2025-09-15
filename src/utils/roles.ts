export type AppRole = 'ADMIN' | 'ANALISTA' | 'EVALUADOR' | 'SOLICITANTE';

const ROLE_VARIANTS: Record<string, AppRole> = {
  'ADMIN': 'ADMIN',
  'ADMINISTRADOR': 'ADMIN',
  'ADMINISTRADORA': 'ADMIN',
  'ANALISTA': 'ANALISTA',
  'EVALUADOR': 'EVALUADOR',
  'EVALUADORA': 'EVALUADOR',
  'SOLICITANTE': 'SOLICITANTE',
};

export function normalizeRole(raw?: string | null): AppRole {
  if (!raw) return 'SOLICITANTE';
  const key = raw.toString().trim().toUpperCase();
  return ROLE_VARIANTS[key] ?? 'SOLICITANTE';
}

export function hasRole(userRole: string | undefined | null, allowedRoles: AppRole[]): boolean {
  const normalized = normalizeRole(userRole);
  return allowedRoles.includes(normalized) || normalized === 'ADMIN';
}

export function getRoleDisplayName(role: AppRole): string {
  const names: Record<AppRole, string> = {
    ADMIN: 'Administrador',
    ANALISTA: 'Analista',
    EVALUADOR: 'Evaluador',
    SOLICITANTE: 'Solicitante',
  };
  return names[role];
}

export function getStateDisplayName(state: number): string {
  const states: Record<number, string> = {
    1: 'Recibida',
    2: 'En análisis',
    3: 'En espera',
    4: 'Evaluada',
    5: 'Certificada',
  };
  return states[state] || `Estado ${state}`;
}

export function getStateColor(state: number): string {
  const colors: Record<number, string> = {
    1: 'badge-gray',
    2: 'badge-info',
    3: 'badge-warning',
    4: 'badge-info',
    5: 'badge-success',
  };
  return colors[state] || 'badge-gray';
}