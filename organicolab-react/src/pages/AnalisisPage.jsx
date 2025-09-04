import React, { useMemo, useState } from 'react'
import { Button, Table, Modal, Form, Input, DatePicker, message, Space, InputNumber } from 'antd'
import dayjs from 'dayjs'
import { useApp } from '../store/useAppStore.jsx'

export default function AnalisisPage() {
  const { solicitudes, setSolicitudes, informes, setInformes } = useApp()
  const [selected, setSelected] = useState(null)
  const [open, setOpen] = useState(false)
  const [form] = Form.useForm()

  const columns = useMemo(() => ([
    { title: 'Nombre muestra', dataIndex: 'nombre' },
    { title: 'Tipo', dataIndex: 'tipo' },
    { title: 'Estado', dataIndex: 'estado' },
    { title: 'pH Máximo', dataIndex: 'phMaximo' }
  ]), [])

  const onGenerate = async () => {
    try {
      const v = await form.validateFields()
      const nuevo = {
        id: informes.length ? Math.max(...informes.map(i => i.id)) + 1 : 1,
        solicitudId: selected.id,
        jobTitle: v.jobTitle,
        formato: v.formato,
        version: v.version,
        fecha: v.fecha?.format('YYYY-MM-DD') || dayjs().format('YYYY-MM-DD'),
        resultados: v.resultados,
        evaluador: 'Usuario analista',
        validado: null
      }
      setInformes([...informes, nuevo])
      setSolicitudes(solicitudes.map(s => s.id === selected.id ? { ...s, estado: 'Completada' } : s))
      setOpen(false); form.resetFields()
      message.success('Informe generado')
    } catch {}
  }

  return (
    <div>
      <div className="table-actions">
        <Button type="primary" disabled={!selected} onClick={() => setOpen(true)}>Analizar y generar informe</Button>
      </div>

      <Table columns={columns}
             dataSource={solicitudes}
             rowKey="id"
             onRow={(record) => ({
               onClick: () => setSelected(record)
             })}
             rowClassName={(record) => record.id === selected?.id ? 'ant-table-row-selected' : ''}
      />

      <Modal title="INFORME RESULTADO" open={open} onOk={onGenerate} onCancel={() => setOpen(false)} okText="Generar informe">
        <Form form={form} layout="vertical" initialValues={{ jobTitle: 'INFORME RESULTADO', formato: 'PDF', version: '1.0', fecha: dayjs() }}>
          <Form.Item label="(job title)" name="jobTitle" rules={[{ required: true }]}><Input /></Form.Item>
          <Form.Item label="Formato" name="formato" rules={[{ required: true }]}><Input /></Form.Item>
          <Form.Item label="Versión" name="version" rules={[{ required: true }]}><Input /></Form.Item>
          <Form.Item label="Fecha" name="fecha"><DatePicker style={{ width: '100%' }}/></Form.Item>
          <Form.Item label="RESULTADOS ANÁLISIS" name="resultados" rules={[{ required: true }]}>
            <Input.TextArea rows={6} />
          </Form.Item>
        </Form>
      </Modal>
    </div>
  )
}
