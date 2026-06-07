import { post } from './request';
export const cmsCommentApi = {
    list: (data) => post('/cms/comment/list', data),
    audit: (commentCode, status, auditComment) => post('/cms/comment/audit', { commentCode, status, auditComment }),
    delete: (commentCode) => post('/cms/comment/delete', { commentCode })
};
