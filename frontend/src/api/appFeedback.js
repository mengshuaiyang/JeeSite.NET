import { post, get } from './request';
export const appFeedbackApi = {
    commentList: () => get('/app/comment/list'),
    commentReply: (id, replyContent) => post('/app/comment/reply', { id, replyContent }),
    commentDelete: (id) => post('/app/comment/delete', { id }),
    upgradeList: () => get('/app/upgrade/list'),
    upgradeSave: (data) => post('/app/upgrade/save', data),
    upgradeDelete: (id) => post('/app/upgrade/delete', { id })
};
