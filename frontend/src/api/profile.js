import { post, get } from './request';
export const profileApi = {
    get: () => get('/sys/profile'),
    update: (data) => post('/sys/profile/update', data),
    changePassword: (data) => post('/sys/profile/password', data),
    updateAvatar: (avatarUrl) => post('/sys/profile/avatar', { avatarUrl })
};
