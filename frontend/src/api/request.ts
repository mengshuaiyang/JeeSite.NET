import axios from 'axios'
import type { ApiResult } from '@/types/api'

const request = axios.create({ baseURL: '/api/v1' })

request.interceptors.request.use(config => {
  const token = localStorage.getItem('token')
  if (token) config.headers.Authorization = `Bearer ${token}`
  return config
})

request.interceptors.response.use(
  res => res,
  error => {
    if (error.response?.status === 401) {
      localStorage.removeItem('token')
      window.location.href = '/login'
    }
    return Promise.reject(error)
  }
)

export async function post<T>(url: string, data?: any): Promise<ApiResult<T>> {
  const res = await request.post<ApiResult<T>>(url, data)
  return res.data
}

export async function get<T>(url: string, params?: any): Promise<ApiResult<T>> {
  const res = await request.get<ApiResult<T>>(url, { params })
  return res.data
}

export default request
