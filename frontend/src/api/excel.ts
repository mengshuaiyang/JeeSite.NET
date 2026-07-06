import { post, get } from './request'
import request from './request'
import { message } from 'ant-design-vue'
import type { ApiResult } from '@/types/api'

export async function exportExcel(type: string, data: any[], filename?: string) {
  try {
    const res = await request.post(`/sys/excel/export?type=${type}`, data, { responseType: 'blob' })
    const blob = res.data as Blob
    const url = URL.createObjectURL(blob)
    const a = document.createElement('a')
    a.href = url
    a.download = filename || `${type}.xlsx`
    a.click()
    URL.revokeObjectURL(url)
    message.success('导出成功')
  } catch (e: any) {
    message.error(e?.message || '导出失败')
  }
}

export async function downloadTemplate(type: string) {
  try {
    const res = await request.get(`/sys/excel/template?type=${type}`, { responseType: 'blob' })
    const blob = res.data as Blob
    const url = URL.createObjectURL(blob)
    const a = document.createElement('a')
    a.href = url
    a.download = `${type}_template.xlsx`
    a.click()
    URL.revokeObjectURL(url)
  } catch (e: any) {
    message.error(e?.message || '下载模板失败')
  }
}

export async function importExcel<T = any>(type: string, file: File): Promise<T[]> {
  const formData = new FormData()
  formData.append('file', file)
  const res = await post<T[]>(`/sys/excel/import?type=${type}`, formData)
  if (res.code !== 200) throw new Error(res.message || '导入失败')
  return res.data || []
}
