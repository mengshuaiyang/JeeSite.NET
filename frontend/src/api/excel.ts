import { post, get } from './request'
import { message } from 'ant-design-vue'
import type { ApiResult } from '@/types/api'

export async function exportExcel(type: string, data: any[], filename?: string) {
  try {
    const res = await fetch(`/api/v1/sys/excel/export?type=${type}`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json', 'Authorization': `Bearer ${localStorage.getItem('token')}` },
      body: JSON.stringify(data)
    })
    if (!res.ok) throw new Error('导出失败')
    const blob = await res.blob()
    const url = URL.createObjectURL(blob)
    const a = document.createElement('a')
    a.href = url
    a.download = filename || `${type}.xlsx`
    a.click()
    URL.revokeObjectURL(url)
    message.success('导出成功')
  } catch (e: any) {
    message.error(e.message || '导出失败')
  }
}

export async function downloadTemplate(type: string) {
  try {
    const res = await fetch(`/api/v1/sys/excel/template?type=${type}`, {
      headers: { 'Authorization': `Bearer ${localStorage.getItem('token')}` }
    })
    if (!res.ok) throw new Error('下载模板失败')
    const blob = await res.blob()
    const url = URL.createObjectURL(blob)
    const a = document.createElement('a')
    a.href = url
    a.download = `${type}_template.xlsx`
    a.click()
    URL.revokeObjectURL(url)
  } catch (e: any) {
    message.error(e.message || '下载模板失败')
  }
}

export async function importExcel<T = any>(type: string, file: File): Promise<T[]> {
  const formData = new FormData()
  formData.append('file', file)
  const res = await fetch(`/api/v1/sys/excel/import?type=${type}`, {
    method: 'POST',
    headers: { 'Authorization': `Bearer ${localStorage.getItem('token')}` },
    body: formData
  })
  const json: ApiResult<T[]> = await res.json()
  if (json.code !== 200) throw new Error(json.message || '导入失败')
  return json.data || []
}
