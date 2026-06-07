import request, { post, get } from './request';
export const fileApi = {
    upload: async (formData) => { const res = await request.post('/api/v1/sys/file/upload', formData, { headers: { 'Content-Type': 'multipart/form-data' } }); return res.data; },
    bizList: (bizType, bizKey) => get('/sys/file/biz-list', { bizType, bizKey }),
    delete: (id) => post('/sys/file/delete', { id })
};
