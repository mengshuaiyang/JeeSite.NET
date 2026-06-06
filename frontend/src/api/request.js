import axios from 'axios';
const request = axios.create({ baseURL: '/api/v1' });
request.interceptors.request.use(config => {
    const token = localStorage.getItem('token');
    if (token)
        config.headers.Authorization = `Bearer ${token}`;
    return config;
});
request.interceptors.response.use(res => res, error => {
    if (error.response?.status === 401) {
        localStorage.removeItem('token');
        window.location.href = '/login';
    }
    return Promise.reject(error);
});
export async function post(url, data) {
    const res = await request.post(url, data);
    return res.data;
}
export async function get(url, params) {
    const res = await request.get(url, { params });
    return res.data;
}
export default request;
