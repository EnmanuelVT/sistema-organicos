// src/utils/coerce.ts
export const toBool = (v: unknown) =>
  typeof v === 'boolean' ? v : v === 1 || v === '1' || v === 'true';

export const toNum = (v: unknown) =>
  v === '' || v == null ? undefined : Number(v);

export const toIso = (d: string | Date | undefined | null) =>
  d ? new Date(d).toISOString() : undefined;
