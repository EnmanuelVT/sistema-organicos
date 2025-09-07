import type { Muestra, Documento, TestRegistro } from '../types/domain'

let muestras: Muestra[] = [
  {
    id: 'MX-001',
    codigo: 'MX-001',
    tipo: 'AGUA',
    fechaRecepcion: new Date().toISOString(),
    origen: 'Planta A',
    condiciones: '4°C, contenedor estéril',
    solicitante: { nombre: 'Laboratorios Caribe', direccion: 'Santo Domingo', contacto: '809-000-0000' },
    solicitanteId: 'sol-1',
    responsableTecnicoId: 'user-ana',
    estado: 'RECIBIDA',
    analistaId: undefined,
    evaluadorId: undefined,
    observaciones: undefined,
    tests: [],
  },
  {
    id: 'MX-002',
    codigo: 'MX-002',
    tipo: 'BEBIDA_ALCOHOLICA',
    fechaRecepcion: new Date().toISOString(),
    origen: 'Bodega B',
    condiciones: 'Ambiental',
    solicitante: { nombre: 'Distribuidora Quisqueya', direccion: 'Santiago', contacto: '809-111-1111' },
    solicitanteId: 'sol-2',
    responsableTecnicoId: 'user-ana',
    estado: 'RECIBIDA',
    analistaId: 'analista-1',
    evaluadorId: 'eval-1',
    observaciones: undefined,
    tests: [{ nombre: 'Graduación alcohólica', unidad: '%', valor: 12.5 }],
  },
]

let documentos: Documento[] = []

function delay(ms: number) { return new Promise(res => setTimeout(res, ms)) }

export async function listMuestras(): Promise<Muestra[]> { await delay(150); return muestras }
export async function getMuestra(id: string) { await delay(100); return muestras.find(m=>m.id===id) }

export async function listMuestrasByAnalista(analistaId: string) {
  await delay(120); return muestras.filter(m=>m.analistaId===analistaId)
}
export async function listMuestrasByEvaluador(evaluadorId: string) {
  await delay(120); return muestras.filter(m=>m.evaluadorId===evaluadorId && m.tests.length>0)
}
export async function listMuestrasBySolicitante(solicitanteId: string) {
  await delay(120); return muestras.filter(m=>m.solicitanteId===solicitanteId)
}

export async function createMuestra(payload: Omit<Muestra,'id'|'estado'|'tests'|'analistaId'|'evaluadorId'|'observaciones'>) {
  await delay(200)
  const nueva: Muestra = { ...payload, id: payload.codigo, estado:'RECIBIDA', tests:[], analistaId: undefined, evaluadorId: undefined, observaciones: undefined }
  muestras = [nueva, ...muestras]
  return nueva
}

export async function assignAnalista(muestraId: string, analistaId: string) {
  await delay(100)
  const m = muestras.find(x=>x.id===muestraId); if(!m) return undefined
  m.analistaId = analistaId
  if (m.estado==='RECIBIDA') m.estado='EN_ANALISIS'
  return m
}

export async function assignEvaluador(muestraId: string, evaluadorId: string) {
  await delay(100)
  const m = muestras.find(x=>x.id===muestraId); if(!m) return undefined
  m.evaluadorId = evaluadorId
  return m
}

export async function registrarTest(muestraId: string, test: TestRegistro) {
  await delay(150)
  const m = muestras.find(x=>x.id===muestraId); if(!m) return undefined
  m.tests.push({ nombre: test.nombre, unidad: test.unidad, valor: test.valor })
  m.estado = 'EVALUADA' // queda pendiente de decisión del evaluador
  return m
}

export async function evaluarMuestra(muestraId: string, aprobado: boolean, observaciones?: string) {
  await delay(200)
  const m = muestras.find(x=>x.id===muestraId); if(!m) return { muestra: undefined, documento: undefined }
  if (aprobado) {
    m.estado = 'CERTIFICADA'
    const version = (documentos.filter(d=>d.muestraId===muestraId).length)+1
    const doc: Documento = {
      id: `DOC-${muestraId}-${version}`,
      muestraId,
      tipo: 'CERTIFICADO',
      version,
      url: '#', // aquí luego se enlaza al PDF real
      creadoPor: 'evaluador',
      creadoEn: new Date().toISOString(),
    }
    documentos = [doc, ...documentos]
    return { muestra: m, documento: doc }
  } else {
    m.estado = 'EN_ANALISIS'
    m.observaciones = observaciones || 'Observaciones no especificadas'
    return { muestra: m, documento: undefined }
  }
}

export async function listDocumentosByMuestra(muestraId: string) {
  await delay(100); return documentos.filter(d=>d.muestraId===muestraId)
}
