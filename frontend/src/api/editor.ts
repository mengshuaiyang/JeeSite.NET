import request from './request'

export const editorApi = {
  upload: async (file: File, editor = 'vditor') => {
    const formData = new FormData()
    formData.append('file', file)
    const res = await request.post(`/api/v1/sys/editor/upload?editor=${editor}`, formData, {
      headers: { 'Content-Type': 'multipart/form-data' },
    })
    return res.data
  },
  uploadImage: async (file: File) => {
    const formData = new FormData()
    formData.append('file', file)
    const res = await request.post('/api/v1/sys/editor/upload/image', formData, {
      headers: { 'Content-Type': 'multipart/form-data' },
    })
    return res.data
  },
}
