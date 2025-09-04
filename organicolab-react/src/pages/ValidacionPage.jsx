import React, { useMemo, useState } from 'react'
import { Button, Table, Modal, Form, Input, Space, message } from 'antd'
import { useApp } from '../store/useAppStore.jsx'

export default function ValidacionPage() {
  const { informes, setInformes } = useApp()
  const [selected, setSelected] = useState(null)
  const [open, setOpen] = useState(false)
  const [form] = Form.useForm()

  const columns = useMemo(() => ([
    { title: 'Solicitud', dataIndex: 'solicitudId' },
    { title: 'Título', dataIndex: 'jobTitle' },
    { title: 'Fecha', dataIndex: 'fecha' },
    { title: 'Formato', dataIndex: 'formato' },
    { title: 'Versión', dataIndex: 'version' },
    { title: 'Validado', dataIndex: 'validado', render: (v) => v === null ? '—' : v ? 'Sí' : 'No' }
  ]), [])

  const validate = (flag) => {
    const { evaluador, observaciones } = form.getFieldsValue()
    setInformes(informes.map(i => i.id === selected.id ? { ...i, validado: flag, evaluador } : i))
    setOpen(false); form.resetFields()
    message.success(flag ? 'Informe validado' : 'Informe denegado')
  }

  return (
    <div>
      <div className="table-actions">
        <Button type="primary" disabled={!selected} onClick={() => setOpen(true)}>Validar informes</Button>
      </div>

      <Table columns={columns}
             dataSource={informes}
             rowKey="id"
             onRow={(record) => ({ onClick: () => setSelected(record) })}
             rowClassName={(record) => record.id === selected?.id ? 'ant-table-row-selected' : ''}
      />

      <Modal title="Validar informe" open={open} onCancel={() => setOpen(false)} footer={null}>
        <Form form={form} layout="vertical" initialValues={{ evaluador: 'Administrador' }}>
          <Form.Item label="Evaluador" name="evaluador"><Input /></Form.Item>
          <Form.Item label="Observaciones" name="observaciones"><Input.TextArea rows={6}/></Form.Item>
          <Space style={{ display: 'flex', justifyContent: 'flex-end' }}>
            <Button onClick={() => setOpen(false)}>Cerrar</Button>
            <Button danger onClick={() => validate(false)}>Denegar</Button>
            <Button type="primary" onClick={() => validate(true)}>Validar</Button>
          </Space>
        </Form>
      </Modal>
    </div>
  )
}
