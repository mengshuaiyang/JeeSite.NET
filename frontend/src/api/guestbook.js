import { post } from './request';
export const guestbookApi = {
    list: (data) => post('/cms/guestbook/list', data),
    reply: (gbCode, reContent) => post('/cms/guestbook/reply', { gbCode, reContent }),
    delete: (gbCode) => post('/cms/guestbook/delete', { gbCode })
};
