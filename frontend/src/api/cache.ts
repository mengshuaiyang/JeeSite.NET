import { get, post } from './request'

export const cacheApi = {
  keys: (prefix?: string) => get<string[]>('/sys/cache/keys', { prefix }),
  clear: (key: string) => post('/sys/cache/clear', null, { params: { key } }),
  clearAll: () => post('/sys/cache/clearAll')
}
