import request, { get, del } from './request';
const base = '/sys/file';
export const fileApi = {
    upload: async (file, bizType, bizKey) => {
        const formData = new FormData();
        formData.append('file', file);
        if (bizType)
            formData.append('bizType', bizType);
        if (bizKey)
            formData.append('bizKey', bizKey);
        const res = await request.post(`/api/v1${base}/upload${bizType || bizKey ? `?bizType=${bizType || ''}&bizKey=${bizKey || ''}` : ''}`, formData, { headers: { 'Content-Type': 'multipart/form-data' } });
        return res.data;
    },
    list: (bizType, bizKey) => get(`${base}/biz`, { bizType: bizType || '', bizKey: bizKey || '' }),
    downloadUrl: (uploadId) => `/api/v1${base}/download/${uploadId}`,
    delete: (uploadId) => del(`${base}/${uploadId}`)
};
