import { post, get } from './request'
import type { AppCommentDto, AppUpgradeDto, AppUpgradeSaveDto } from '@/types/api'

export const appFeedbackApi = {
  commentList: () => get<AppCommentDto[]>('/app/comment/list'),
  commentReply: (id: string, replyContent: string) => post('/app/comment/reply', { id, replyContent }),
  commentDelete: (id: string) => post('/app/comment/delete', { id }),
  upgradeList: () => get<AppUpgradeDto[]>('/app/upgrade/list'),
  upgradeSave: (data: AppUpgradeSaveDto) => post<AppUpgradeDto>('/app/upgrade/save', data),
  upgradeDelete: (id: string) => post('/app/upgrade/delete', { id })
}
