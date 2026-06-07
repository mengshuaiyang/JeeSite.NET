import { post, get } from './request'
import type { PageRequest, PageResult, GenTableDto, DbTableInfo, GenPreviewItem } from '@/types/api'

export const codegenApi = {
  list: (data: PageRequest) => post<PageResult<GenTableDto>>('/codegen/table/list', data),
  get: (tableName: string) => get<GenTableDto>('/codegen/table/get', { tableName }),
  save: (data: any) => post('/codegen/table/save', data),
  delete: (tableName: string) => post('/codegen/table/delete', { tableName }),
  dbTables: () => get<DbTableInfo[]>('/codegen/table/db/tables'),
  dbColumns: (tableName: string) => get<any[]>('/codegen/table/db/columns', { tableName }),
  importTables: (tableNames: string[]) => post('/codegen/table/import', { tableNames }),
  preview: (tableName: string) => get<GenPreviewItem[]>('/codegen/table/preview', { tableName })
}
