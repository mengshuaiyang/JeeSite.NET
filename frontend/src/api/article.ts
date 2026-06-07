import { post, get } from './request'
import type { PageRequest, PageResult, ArticleDto, ArticleSaveDto } from '@/types/api'

export const articleApi = {
  list: (data: PageRequest) => post<PageResult<ArticleDto>>('/cms/article/list', data),
  get: (articleCode: string) => get<ArticleDto>('/cms/article/get', { articleCode }),
  save: (data: ArticleSaveDto) => post<ArticleDto>('/cms/article/save', data),
  delete: (articleCode: string) => post('/cms/article/delete', { articleCode })
}
