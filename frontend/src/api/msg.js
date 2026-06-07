import { post } from './request';
export const msgApi = {
    inbox: (data) => post('/sys/msg/inbox', data),
    sent: (data) => post('/sys/msg/sent', data),
    send: (data) => post('/sys/msg/send', data),
    markRead: (msgId) => post('/sys/msg/mark-read', { msgId }),
    delete: (msgId) => post('/sys/msg/delete', { msgId }),
    templateList: (data) => post('/sys/msg/template-list', data),
    templateSave: (data) => post('/sys/msg/template-save', data),
    templateDelete: (id) => post('/sys/msg/template-delete', { templateId: id })
};
