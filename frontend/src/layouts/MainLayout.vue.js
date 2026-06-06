import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { useAppStore } from '@/stores/app';
import { useUserStore } from '@/stores/user';
import { PieChartOutlined, SettingOutlined, MenuUnfoldOutlined, MenuFoldOutlined, UserOutlined, TeamOutlined, AppstoreOutlined, FileTextOutlined, SafetyOutlined, ToolOutlined, FolderOutlined, ProfileOutlined } from '@ant-design/icons-vue';
const iconMap = {
    'pie-chart-outlined': PieChartOutlined,
    'setting-outlined': SettingOutlined,
    'user-outlined': UserOutlined,
    'team-outlined': TeamOutlined,
    'appstore-outlined': AppstoreOutlined,
    'file-text-outlined': FileTextOutlined,
    'safety-outlined': SafetyOutlined,
    'tool-outlined': ToolOutlined,
    'folder-outlined': FolderOutlined,
    'profile-outlined': ProfileOutlined
};
const router = useRouter();
const app = useAppStore();
const userStore = useUserStore();
const selectedKeys = ref([]);
function getIcon(name) { return name && iconMap[name] ? iconMap[name] : AppstoreOutlined; }
function onMenuClick({ key }) { router.push(key); }
function handleLogout() { userStore.logout(); router.push('/login'); }
debugger; /* PartiallyEnd: #3632/scriptSetup.vue */
const __VLS_ctx = {};
let __VLS_components;
let __VLS_directives;
// CSS variable injection 
// CSS variable injection end 
const __VLS_0 = {}.ALayout;
/** @type {[typeof __VLS_components.ALayout, typeof __VLS_components.aLayout, typeof __VLS_components.ALayout, typeof __VLS_components.aLayout, ]} */ ;
// @ts-ignore
const __VLS_1 = __VLS_asFunctionalComponent(__VLS_0, new __VLS_0({
    ...{ style: {} },
}));
const __VLS_2 = __VLS_1({
    ...{ style: {} },
}, ...__VLS_functionalComponentArgsRest(__VLS_1));
var __VLS_4 = {};
__VLS_3.slots.default;
const __VLS_5 = {}.ALayoutSider;
/** @type {[typeof __VLS_components.ALayoutSider, typeof __VLS_components.aLayoutSider, typeof __VLS_components.ALayoutSider, typeof __VLS_components.aLayoutSider, ]} */ ;
// @ts-ignore
const __VLS_6 = __VLS_asFunctionalComponent(__VLS_5, new __VLS_5({
    collapsed: (__VLS_ctx.app.collapsed),
    theme: "dark",
    collapsible: true,
}));
const __VLS_7 = __VLS_6({
    collapsed: (__VLS_ctx.app.collapsed),
    theme: "dark",
    collapsible: true,
}, ...__VLS_functionalComponentArgsRest(__VLS_6));
__VLS_8.slots.default;
__VLS_asFunctionalElement(__VLS_intrinsicElements.div, __VLS_intrinsicElements.div)({
    ...{ class: "logo" },
});
(__VLS_ctx.app.collapsed ? 'JS' : 'JeeSite.NET');
const __VLS_9 = {}.AMenu;
/** @type {[typeof __VLS_components.AMenu, typeof __VLS_components.aMenu, typeof __VLS_components.AMenu, typeof __VLS_components.aMenu, ]} */ ;
// @ts-ignore
const __VLS_10 = __VLS_asFunctionalComponent(__VLS_9, new __VLS_9({
    ...{ 'onClick': {} },
    theme: "dark",
    mode: "inline",
    selectedKeys: (__VLS_ctx.selectedKeys),
}));
const __VLS_11 = __VLS_10({
    ...{ 'onClick': {} },
    theme: "dark",
    mode: "inline",
    selectedKeys: (__VLS_ctx.selectedKeys),
}, ...__VLS_functionalComponentArgsRest(__VLS_10));
let __VLS_13;
let __VLS_14;
let __VLS_15;
const __VLS_16 = {
    onClick: (__VLS_ctx.onMenuClick)
};
__VLS_12.slots.default;
for (const [item] of __VLS_getVForSourceType((__VLS_ctx.app.menus))) {
    (item.key);
    if (!item.children?.length) {
        const __VLS_17 = {}.AMenuItem;
        /** @type {[typeof __VLS_components.AMenuItem, typeof __VLS_components.aMenuItem, typeof __VLS_components.AMenuItem, typeof __VLS_components.aMenuItem, ]} */ ;
        // @ts-ignore
        const __VLS_18 = __VLS_asFunctionalComponent(__VLS_17, new __VLS_17({
            key: (item.key),
        }));
        const __VLS_19 = __VLS_18({
            key: (item.key),
        }, ...__VLS_functionalComponentArgsRest(__VLS_18));
        __VLS_20.slots.default;
        if (item.icon) {
            const __VLS_21 = ((__VLS_ctx.getIcon(item.icon)));
            // @ts-ignore
            const __VLS_22 = __VLS_asFunctionalComponent(__VLS_21, new __VLS_21({}));
            const __VLS_23 = __VLS_22({}, ...__VLS_functionalComponentArgsRest(__VLS_22));
        }
        (item.title);
        var __VLS_20;
    }
    else {
        const __VLS_25 = {}.ASubMenu;
        /** @type {[typeof __VLS_components.ASubMenu, typeof __VLS_components.aSubMenu, typeof __VLS_components.ASubMenu, typeof __VLS_components.aSubMenu, ]} */ ;
        // @ts-ignore
        const __VLS_26 = __VLS_asFunctionalComponent(__VLS_25, new __VLS_25({
            key: (item.key),
        }));
        const __VLS_27 = __VLS_26({
            key: (item.key),
        }, ...__VLS_functionalComponentArgsRest(__VLS_26));
        __VLS_28.slots.default;
        {
            const { title: __VLS_thisSlot } = __VLS_28.slots;
            if (item.icon) {
                const __VLS_29 = ((__VLS_ctx.getIcon(item.icon)));
                // @ts-ignore
                const __VLS_30 = __VLS_asFunctionalComponent(__VLS_29, new __VLS_29({}));
                const __VLS_31 = __VLS_30({}, ...__VLS_functionalComponentArgsRest(__VLS_30));
            }
            __VLS_asFunctionalElement(__VLS_intrinsicElements.span, __VLS_intrinsicElements.span)({});
            (item.title);
        }
        for (const [child] of __VLS_getVForSourceType((item.children))) {
            const __VLS_33 = {}.AMenuItem;
            /** @type {[typeof __VLS_components.AMenuItem, typeof __VLS_components.aMenuItem, typeof __VLS_components.AMenuItem, typeof __VLS_components.aMenuItem, ]} */ ;
            // @ts-ignore
            const __VLS_34 = __VLS_asFunctionalComponent(__VLS_33, new __VLS_33({
                key: (child.key),
            }));
            const __VLS_35 = __VLS_34({
                key: (child.key),
            }, ...__VLS_functionalComponentArgsRest(__VLS_34));
            __VLS_36.slots.default;
            (child.title);
            var __VLS_36;
        }
        var __VLS_28;
    }
}
var __VLS_12;
var __VLS_8;
const __VLS_37 = {}.ALayout;
/** @type {[typeof __VLS_components.ALayout, typeof __VLS_components.aLayout, typeof __VLS_components.ALayout, typeof __VLS_components.aLayout, ]} */ ;
// @ts-ignore
const __VLS_38 = __VLS_asFunctionalComponent(__VLS_37, new __VLS_37({}));
const __VLS_39 = __VLS_38({}, ...__VLS_functionalComponentArgsRest(__VLS_38));
__VLS_40.slots.default;
const __VLS_41 = {}.ALayoutHeader;
/** @type {[typeof __VLS_components.ALayoutHeader, typeof __VLS_components.aLayoutHeader, typeof __VLS_components.ALayoutHeader, typeof __VLS_components.aLayoutHeader, ]} */ ;
// @ts-ignore
const __VLS_42 = __VLS_asFunctionalComponent(__VLS_41, new __VLS_41({
    ...{ class: "header" },
}));
const __VLS_43 = __VLS_42({
    ...{ class: "header" },
}, ...__VLS_functionalComponentArgsRest(__VLS_42));
__VLS_44.slots.default;
if (__VLS_ctx.app.collapsed) {
    const __VLS_45 = {}.MenuUnfoldOutlined;
    /** @type {[typeof __VLS_components.MenuUnfoldOutlined, typeof __VLS_components.menuUnfoldOutlined, ]} */ ;
    // @ts-ignore
    const __VLS_46 = __VLS_asFunctionalComponent(__VLS_45, new __VLS_45({
        ...{ 'onClick': {} },
    }));
    const __VLS_47 = __VLS_46({
        ...{ 'onClick': {} },
    }, ...__VLS_functionalComponentArgsRest(__VLS_46));
    let __VLS_49;
    let __VLS_50;
    let __VLS_51;
    const __VLS_52 = {
        onClick: (__VLS_ctx.app.toggleCollapsed)
    };
    var __VLS_48;
}
else {
    const __VLS_53 = {}.MenuFoldOutlined;
    /** @type {[typeof __VLS_components.MenuFoldOutlined, typeof __VLS_components.menuFoldOutlined, ]} */ ;
    // @ts-ignore
    const __VLS_54 = __VLS_asFunctionalComponent(__VLS_53, new __VLS_53({
        ...{ 'onClick': {} },
    }));
    const __VLS_55 = __VLS_54({
        ...{ 'onClick': {} },
    }, ...__VLS_functionalComponentArgsRest(__VLS_54));
    let __VLS_57;
    let __VLS_58;
    let __VLS_59;
    const __VLS_60 = {
        onClick: (__VLS_ctx.app.toggleCollapsed)
    };
    var __VLS_56;
}
__VLS_asFunctionalElement(__VLS_intrinsicElements.span, __VLS_intrinsicElements.span)({
    ...{ style: {} },
});
(__VLS_ctx.userStore.user?.userName || '用户');
const __VLS_61 = {}.AButton;
/** @type {[typeof __VLS_components.AButton, typeof __VLS_components.aButton, typeof __VLS_components.AButton, typeof __VLS_components.aButton, ]} */ ;
// @ts-ignore
const __VLS_62 = __VLS_asFunctionalComponent(__VLS_61, new __VLS_61({
    ...{ 'onClick': {} },
    type: "link",
    ...{ style: {} },
}));
const __VLS_63 = __VLS_62({
    ...{ 'onClick': {} },
    type: "link",
    ...{ style: {} },
}, ...__VLS_functionalComponentArgsRest(__VLS_62));
let __VLS_65;
let __VLS_66;
let __VLS_67;
const __VLS_68 = {
    onClick: (__VLS_ctx.handleLogout)
};
__VLS_64.slots.default;
var __VLS_64;
var __VLS_44;
const __VLS_69 = {}.ALayoutContent;
/** @type {[typeof __VLS_components.ALayoutContent, typeof __VLS_components.aLayoutContent, typeof __VLS_components.ALayoutContent, typeof __VLS_components.aLayoutContent, ]} */ ;
// @ts-ignore
const __VLS_70 = __VLS_asFunctionalComponent(__VLS_69, new __VLS_69({
    ...{ class: "content" },
}));
const __VLS_71 = __VLS_70({
    ...{ class: "content" },
}, ...__VLS_functionalComponentArgsRest(__VLS_70));
__VLS_72.slots.default;
const __VLS_73 = {}.RouterView;
/** @type {[typeof __VLS_components.RouterView, typeof __VLS_components.routerView, ]} */ ;
// @ts-ignore
const __VLS_74 = __VLS_asFunctionalComponent(__VLS_73, new __VLS_73({}));
const __VLS_75 = __VLS_74({}, ...__VLS_functionalComponentArgsRest(__VLS_74));
var __VLS_72;
var __VLS_40;
var __VLS_3;
/** @type {__VLS_StyleScopedClasses['logo']} */ ;
/** @type {__VLS_StyleScopedClasses['header']} */ ;
/** @type {__VLS_StyleScopedClasses['content']} */ ;
var __VLS_dollars;
const __VLS_self = (await import('vue')).defineComponent({
    setup() {
        return {
            MenuUnfoldOutlined: MenuUnfoldOutlined,
            MenuFoldOutlined: MenuFoldOutlined,
            app: app,
            userStore: userStore,
            selectedKeys: selectedKeys,
            getIcon: getIcon,
            onMenuClick: onMenuClick,
            handleLogout: handleLogout,
        };
    },
});
export default (await import('vue')).defineComponent({
    setup() {
        return {};
    },
});
; /* PartiallyEnd: #4569/main.vue */
