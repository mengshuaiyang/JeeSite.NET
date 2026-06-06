import { defineStore } from 'pinia';
import { ref } from 'vue';
import { menuApi } from '@/api/menu';
function menuToTree(dto) {
    const node = { key: dto.menuHref || dto.menuCode, title: dto.menuName, icon: dto.menuIcon };
    if (dto.children?.length)
        node.children = dto.children.filter(c => c.isShow !== '0').map(menuToTree);
    return node;
}
export const useAppStore = defineStore('app', () => {
    const collapsed = ref(false);
    const menus = ref([]);
    const permissions = ref([]);
    function toggleCollapsed() { collapsed.value = !collapsed.value; }
    async function loadMenus() {
        const res = await menuApi.getUserMenus();
        if (res.data) {
            menus.value = res.data.filter(m => m.isShow !== '0').map(menuToTree);
        }
    }
    function setPermissions(list) { permissions.value = list; }
    function hasPermission(p) { return permissions.value.length === 0 || permissions.value.includes(p); }
    return { collapsed, menus, permissions, toggleCollapsed, loadMenus, setPermissions, hasPermission };
});
