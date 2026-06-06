import { defineStore } from 'pinia';
import { ref } from 'vue';
import { authApi } from '@/api/auth';
import { useAppStore } from './app';
export const useUserStore = defineStore('user', () => {
    const token = ref(localStorage.getItem('token') || '');
    const user = ref(null);
    async function login(loginCode, password) {
        const res = await authApi.login({ loginCode, password });
        if (res.code === 200 && res.data) {
            token.value = res.data.token;
            user.value = res.data.user;
            localStorage.setItem('token', res.data.token);
            const app = useAppStore();
            if (res.data.user?.permissions)
                app.setPermissions(res.data.user.permissions);
            await app.loadMenus();
        }
        return res;
    }
    function logout() {
        token.value = '';
        user.value = null;
        localStorage.removeItem('token');
    }
    return { token, user, login, logout };
});
