// src/utils/roles.ts
export type AppRole = 'ADMIN' | 'ANALISTA' | 'EVALUADOR' | 'SOLICITANTE';

const MAP_VARIANTS: Record<string, AppRole> = {
  'ADMIN': 'ADMIN',
  'ADMINISTRADOR': 'ADMIN',
  'ADMINISTRADORA': 'ADMIN',
  'ANALISTA': 'ANALISTA',
  'EVALUADOR': 'EVALUADOR',
  'EVALUADORA': 'EVALUADOR',
  'SOLICITANTE': 'SOLICITANTE',
};

export function normalizeRole(raw?: string | null): AppRole {
  if (!raw) return 'SOLICITANTE';                  // default cuando viene nulo
  const key = raw.toString().trim().toUpperCase();
  return MAP_VARIANTS[key] ?? 'SOLICITANTE';        // fallback seguro
}

export function hasAnyRole(userRole: string | undefined | null, roles: AppRole[]): boolean {
  const norm = normalizeRole(userRole);
  return roles.includes(norm);
}
