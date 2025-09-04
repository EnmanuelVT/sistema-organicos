import React, { useMemo, useState } from 'react'
import { Button, Table, Modal, Form, Input, Select, DatePicker, InputNumber, Checkbox, Space, message } from 'antd'
import dayjs from 'dayjs'
import { useApp } from '../store/useAppStore.jsx'

export default function SolicitudesPage() {
  const { solicitudes, setSolicitudes } = useApp()
  const [open, setOpen] = useState(false)
  const [form] = Form.useForm()

  const columns = useMemo(() => ([
    { title: 'Nombre muestra', dataIndex: 'nombre' },
    { title: 'Tipo', dataIndex: 'tipo' },
    { title: 'Estado', dataIndex: 'estado' }
  ]), [])

  const onOk = async () => {
    try {
      const values = await form.validateFields()
      const nueva = {
        id: solicitudes.length ? Math.max(...solicitudes.map(s => s.id)) + 1 : 1,
        nombre: values.nombre,
        tipo: values.tipo,
        estado: 'Enviada',
        campo1: values.campo1,
        campo2: values.campo2,
        campo3: values.campo3,
        campo4: values.campo4,
        campo5: values.campo5,
        phMaximo: values.phMaximo,
        fechaCaducidadMaxima: values.fechaCaducidadMaxima?.format('YYYY-MM-DD')
      }
      setSolicitudes([...solicitudes, nueva])
      setOpen(false); form.resetFields()
      message.success('Solicitud enviada')
    } catch {}
  }

  return (
    <div>
      <div className="table-actions">
        <Button type="primary" onClick={() => setOpen(true)}>Enviar solicitud</Button>
      </div>

      <Table columns={columns} dataSource={solicitudes} rowKey="id" />

      <Modal title="Enviar solicitud" open={open} onOk={onOk} onCancel={() => setOpen(false)} okText="Enviar">
        <Form form={form} layout="vertical">
          <Form.Item label="Nombre muestra" name="nombre" rules={[{ required: true }]}>
            <Input />
          </Form.Item>
          <Form.Item label="Tipo muestra" name="tipo" rules={[{ required: true }]}>
            <Select options={[
              { value: 'Líquida', label: 'Líquida' },
              { value: 'Sólida', label: 'Sólida' },
              { value: 'Gas', label: 'Gas' }
            ]} />
          </Form.Item>
          <Form.Item label="Campo 1" name="campo1"><Input /></Form.Item>
          <Form.Item label="Campo 2" name="campo2"><Input /></Form.Item>
          <Form.Item name="campo3" valuePropName="checked">
            <Checkbox>Campo 3 (Sí/No/Tal vez)</Checkbox>
          </Form.Item>
          <Form.Item label="Campo 4" name="campo4">
            <Select options={[{value:'Opción 1'},{value:'Opción 2'},{value:'Opción 3'}]} />
          </Form.Item>
          <Form.Item label="Campo 5" name="campo5"><Input /></Form.Item>

          <Form.Item label="pH Máximo" name="phMaximo">
            <InputNumber style={{ width: '100%' }} step={0.1} />
          </Form.Item>
          <Form.Item label="Fecha caducidad máxima" name="fechaCaducidadMaxima">
            <DatePicker style={{ width: '100%' }} />
          </Form.Item>
        </Form>
      </Modal>
    </div>
  )
}
