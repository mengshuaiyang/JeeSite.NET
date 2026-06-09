import { get, post } from './request'
import type { PageRequest, PageResult, ArticleDto, CategoryDto, TagDto } from '@/types/api'

export interface SiteDto {
  siteCode: string
  siteName: string
  domain?: string
  logo?: string
  keywords?: string
  description?: string
  copyright?: string
  status?: string
}

export const cmsFrontApi = {
  getSites: () => get<SiteDto[]>('/cms/front/site'),
  getCategories: (siteCode: string) => get<CategoryDto[]>(`/cms/front/category/list/${siteCode}`),
  articleList: (data: PageRequest) => post<PageResult<ArticleDto>>('/cms/front/article/list', data),
  articleGet: (articleCode: string) => get<ArticleDto>(`/cms/front/article/get/${articleCode}`),
  articleSearch: (data: PageRequest) => post<PageResult<ArticleDto>>('/cms/front/article/search', data),
  tagCloud: () => get<TagDto[]>('/cms/front/tag/cloud')
}
