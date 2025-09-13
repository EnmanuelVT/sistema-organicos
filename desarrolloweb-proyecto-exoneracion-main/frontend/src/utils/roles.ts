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

export function normalizeRole(raw?: string | null): AppRole | undefined {
  if (!raw) return undefined;
  const key = raw.toString().trim().toUpperCase();
  return MAP_VARIANTS[key];
}

export function hasAnyRole(userRole: string | undefined | null, roles: AppRole[]): boolean {
  const norm = normalizeRole(userRole);
  return !!norm && roles.includes(norm);
}
