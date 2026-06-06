import { post, get } from './request'
import type { PageRequest, PageResult, DictTypeDto, DictDataDto, DictTypeSaveDto, DictDataSaveDto } from '@/types/api'

export const dictApi = {
  // DictType
  typeList: (data: PageRequest) => post<PageResult<DictTypeDto>>('/sys/dict-type/list', data),
  typeGet: (dictTypeCode: string) => get<DictTypeDto>('/sys/dict-type/get', { dictTypeCode }),
  typeSave: (data: DictTypeSaveDto) => post<DictTypeDto>('/sys/dict-type/save', data),
  typeDelete: (dictTypeCode: string) => post('/sys/dict-type/delete', { dictTypeCode }),
  // DictData
  dataList: (data: PageRequest) => post<PageResult<DictDataDto>>('/sys/dict-data/list', data),
  dataByType: (dictType: string) => get<DictDataDto[]>('/sys/dict-data/by-type', { dictType }),
  dataGet: (dictCode: string) => get<DictDataDto>('/sys/dict-data/get', { dictCode }),
  dataSave: (data: DictDataSaveDto) => post<DictDataDto>('/sys/dict-data/save', data),
  dataDelete: (dictCode: string) => post('/sys/dict-data/delete', { dictCode })
}
