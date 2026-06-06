import { ref, reactive } from 'vue';
import { useRouter } from 'vue-router';
import { useUserStore } from '@/stores/user';
import { message } from 'ant-design-vue';
import { UserOutlined, LockOutlined } from '@ant-design/icons-vue';
const router = useRouter();
const userStore = useUserStore();
const loading = ref(false);
const form = reactive({ loginCode: 'admin', password: 'admin' });
async function handleLogin() {
    loading.value = true;
    try {
        const res = await userStore.login(form.loginCode, form.password);
        if (res.code === 200)
            router.push('/');
        else
            message.error(res.message || '登录失败');
    }
    catch {
        message.error('登录失败');
    }
    finally {
        loading.value = false;
    }
}
debugger; /* PartiallyEnd: #3632/scriptSetup.vue */
const __VLS_ctx = {};
let __VLS_components;
let __VLS_directives;
// CSS variable injection 
// CSS variable injection end 
__VLS_asFunctionalElement(__VLS_intrinsicElements.div, __VLS_intrinsicElements.div)({
    ...{ class: "login-container" },
});
const __VLS_0 = {}.ACard;
/** @type {[typeof __VLS_components.ACard, typeof __VLS_components.aCard, typeof __VLS_components.ACard, typeof __VLS_components.aCard, ]} */ ;
// @ts-ignore
const __VLS_1 = __VLS_asFunctionalComponent(__VLS_0, new __VLS_0({
    title: "JeeSite.NET 管理平台",
    ...{ style: {} },
}));
const __VLS_2 = __VLS_1({
    title: "JeeSite.NET 管理平台",
    ...{ style: {} },
}, ...__VLS_functionalComponentArgsRest(__VLS_1));
__VLS_3.slots.default;
const __VLS_4 = {}.AForm;
/** @type {[typeof __VLS_components.AForm, typeof __VLS_components.aForm, typeof __VLS_components.AForm, typeof __VLS_components.aForm, ]} */ ;
// @ts-ignore
const __VLS_5 = __VLS_asFunctionalComponent(__VLS_4, new __VLS_4({
    ...{ 'onFinish': {} },
    model: (__VLS_ctx.form),
}));
const __VLS_6 = __VLS_5({
    ...{ 'onFinish': {} },
    model: (__VLS_ctx.form),
}, ...__VLS_functionalComponentArgsRest(__VLS_5));
let __VLS_8;
let __VLS_9;
let __VLS_10;
const __VLS_11 = {
    onFinish: (__VLS_ctx.handleLogin)
};
__VLS_7.slots.default;
const __VLS_12 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_13 = __VLS_asFunctionalComponent(__VLS_12, new __VLS_12({
    name: "loginCode",
    rules: ([{ required: true, message: '请输入登录名' }]),
}));
const __VLS_14 = __VLS_13({
    name: "loginCode",
    rules: ([{ required: true, message: '请输入登录名' }]),
}, ...__VLS_functionalComponentArgsRest(__VLS_13));
__VLS_15.slots.default;
const __VLS_16 = {}.AInput;
/** @type {[typeof __VLS_components.AInput, typeof __VLS_components.aInput, typeof __VLS_components.AInput, typeof __VLS_components.aInput, ]} */ ;
// @ts-ignore
const __VLS_17 = __VLS_asFunctionalComponent(__VLS_16, new __VLS_16({
    value: (__VLS_ctx.form.loginCode),
    placeholder: "登录名",
    size: "large",
}));
const __VLS_18 = __VLS_17({
    value: (__VLS_ctx.form.loginCode),
    placeholder: "登录名",
    size: "large",
}, ...__VLS_functionalComponentArgsRest(__VLS_17));
__VLS_19.slots.default;
{
    const { prefix: __VLS_thisSlot } = __VLS_19.slots;
    const __VLS_20 = {}.UserOutlined;
    /** @type {[typeof __VLS_components.UserOutlined, typeof __VLS_components.userOutlined, ]} */ ;
    // @ts-ignore
    const __VLS_21 = __VLS_asFunctionalComponent(__VLS_20, new __VLS_20({}));
    const __VLS_22 = __VLS_21({}, ...__VLS_functionalComponentArgsRest(__VLS_21));
}
var __VLS_19;
var __VLS_15;
const __VLS_24 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_25 = __VLS_asFunctionalComponent(__VLS_24, new __VLS_24({
    name: "password",
    rules: ([{ required: true, message: '请输入密码' }]),
}));
const __VLS_26 = __VLS_25({
    name: "password",
    rules: ([{ required: true, message: '请输入密码' }]),
}, ...__VLS_functionalComponentArgsRest(__VLS_25));
__VLS_27.slots.default;
const __VLS_28 = {}.AInputPassword;
/** @type {[typeof __VLS_components.AInputPassword, typeof __VLS_components.aInputPassword, typeof __VLS_components.AInputPassword, typeof __VLS_components.aInputPassword, ]} */ ;
// @ts-ignore
const __VLS_29 = __VLS_asFunctionalComponent(__VLS_28, new __VLS_28({
    value: (__VLS_ctx.form.password),
    placeholder: "密码",
    size: "large",
}));
const __VLS_30 = __VLS_29({
    value: (__VLS_ctx.form.password),
    placeholder: "密码",
    size: "large",
}, ...__VLS_functionalComponentArgsRest(__VLS_29));
__VLS_31.slots.default;
{
    const { prefix: __VLS_thisSlot } = __VLS_31.slots;
    const __VLS_32 = {}.LockOutlined;
    /** @type {[typeof __VLS_components.LockOutlined, typeof __VLS_components.lockOutlined, ]} */ ;
    // @ts-ignore
    const __VLS_33 = __VLS_asFunctionalComponent(__VLS_32, new __VLS_32({}));
    const __VLS_34 = __VLS_33({}, ...__VLS_functionalComponentArgsRest(__VLS_33));
}
var __VLS_31;
var __VLS_27;
const __VLS_36 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_37 = __VLS_asFunctionalComponent(__VLS_36, new __VLS_36({}));
const __VLS_38 = __VLS_37({}, ...__VLS_functionalComponentArgsRest(__VLS_37));
__VLS_39.slots.default;
const __VLS_40 = {}.AButton;
/** @type {[typeof __VLS_components.AButton, typeof __VLS_components.aButton, typeof __VLS_components.AButton, typeof __VLS_components.aButton, ]} */ ;
// @ts-ignore
const __VLS_41 = __VLS_asFunctionalComponent(__VLS_40, new __VLS_40({
    type: "primary",
    htmlType: "submit",
    block: true,
    size: "large",
    loading: (__VLS_ctx.loading),
}));
const __VLS_42 = __VLS_41({
    type: "primary",
    htmlType: "submit",
    block: true,
    size: "large",
    loading: (__VLS_ctx.loading),
}, ...__VLS_functionalComponentArgsRest(__VLS_41));
__VLS_43.slots.default;
var __VLS_43;
var __VLS_39;
var __VLS_7;
var __VLS_3;
/** @type {__VLS_StyleScopedClasses['login-container']} */ ;
var __VLS_dollars;
const __VLS_self = (await import('vue')).defineComponent({
    setup() {
        return {
            UserOutlined: UserOutlined,
            LockOutlined: LockOutlined,
            loading: loading,
            form: form,
            handleLogin: handleLogin,
        };
    },
});
export default (await import('vue')).defineComponent({
    setup() {
        return {};
    },
});
; /* PartiallyEnd: #4569/main.vue */
