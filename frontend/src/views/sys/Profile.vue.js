import { ref, reactive, onMounted } from 'vue';
import { profileApi, fileApi } from '@/api';
import { message } from 'ant-design-vue';
import { UserOutlined, UploadOutlined } from '@ant-design/icons-vue';
const saving = ref(false);
const pwdSaving = ref(false);
const profile = reactive({ loginCode: '', userName: '', email: '', phone: '', orgName: '', userType: '', status: '', loginDate: '', createDate: '' });
const avatarUrl = ref('');
const editForm = reactive({ userName: '', email: '', phone: '' });
const pwdForm = reactive({ oldPassword: '', newPassword: '', confirmPassword: '' });
async function loadProfile() {
    const res = await profileApi.get();
    if (res.data) {
        Object.assign(profile, res.data);
        editForm.userName = res.data.userName || '';
        editForm.email = res.data.email || '';
        editForm.phone = res.data.phone || '';
        avatarUrl.value = res.data.avatar || '';
    }
}
async function saveProfile() {
    saving.value = true;
    await profileApi.update(editForm);
    message.success('保存成功');
    saving.value = false;
    await loadProfile();
}
async function changePwd() {
    if (!pwdForm.oldPassword || !pwdForm.newPassword) {
        message.warning('请填写完整');
        return;
    }
    if (pwdForm.newPassword !== pwdForm.confirmPassword) {
        message.warning('两次密码不一致');
        return;
    }
    pwdSaving.value = true;
    await profileApi.changePassword({ oldPassword: pwdForm.oldPassword, newPassword: pwdForm.newPassword });
    message.success('密码修改成功,请重新登录');
    pwdSaving.value = false;
    pwdForm.oldPassword = '';
    pwdForm.newPassword = '';
    pwdForm.confirmPassword = '';
}
async function handleAvatarUpload(file) {
    const res = await fileApi.upload(file, 'user_avatar', profile.userCode);
    if (res.data) {
        const url = `/api/v1/sys/file/download/${res.data.uploadId}`;
        await profileApi.updateAvatar(url);
        avatarUrl.value = url;
        message.success('头像已更新');
    }
    return false;
}
onMounted(loadProfile);
debugger; /* PartiallyEnd: #3632/scriptSetup.vue */
const __VLS_ctx = {};
let __VLS_components;
let __VLS_directives;
const __VLS_0 = {}.ACard;
/** @type {[typeof __VLS_components.ACard, typeof __VLS_components.aCard, typeof __VLS_components.ACard, typeof __VLS_components.aCard, ]} */ ;
// @ts-ignore
const __VLS_1 = __VLS_asFunctionalComponent(__VLS_0, new __VLS_0({
    title: "个人中心",
}));
const __VLS_2 = __VLS_1({
    title: "个人中心",
}, ...__VLS_functionalComponentArgsRest(__VLS_1));
var __VLS_4 = {};
__VLS_3.slots.default;
const __VLS_5 = {}.ARow;
/** @type {[typeof __VLS_components.ARow, typeof __VLS_components.aRow, typeof __VLS_components.ARow, typeof __VLS_components.aRow, ]} */ ;
// @ts-ignore
const __VLS_6 = __VLS_asFunctionalComponent(__VLS_5, new __VLS_5({
    gutter: (24),
}));
const __VLS_7 = __VLS_6({
    gutter: (24),
}, ...__VLS_functionalComponentArgsRest(__VLS_6));
__VLS_8.slots.default;
const __VLS_9 = {}.ACol;
/** @type {[typeof __VLS_components.ACol, typeof __VLS_components.aCol, typeof __VLS_components.ACol, typeof __VLS_components.aCol, ]} */ ;
// @ts-ignore
const __VLS_10 = __VLS_asFunctionalComponent(__VLS_9, new __VLS_9({
    span: (6),
}));
const __VLS_11 = __VLS_10({
    span: (6),
}, ...__VLS_functionalComponentArgsRest(__VLS_10));
__VLS_12.slots.default;
const __VLS_13 = {}.ACard;
/** @type {[typeof __VLS_components.ACard, typeof __VLS_components.aCard, typeof __VLS_components.ACard, typeof __VLS_components.aCard, ]} */ ;
// @ts-ignore
const __VLS_14 = __VLS_asFunctionalComponent(__VLS_13, new __VLS_13({
    title: "头像",
}));
const __VLS_15 = __VLS_14({
    title: "头像",
}, ...__VLS_functionalComponentArgsRest(__VLS_14));
__VLS_16.slots.default;
const __VLS_17 = {}.AAvatar;
/** @type {[typeof __VLS_components.AAvatar, typeof __VLS_components.aAvatar, typeof __VLS_components.AAvatar, typeof __VLS_components.aAvatar, ]} */ ;
// @ts-ignore
const __VLS_18 = __VLS_asFunctionalComponent(__VLS_17, new __VLS_17({
    size: (120),
    src: (__VLS_ctx.avatarUrl),
    ...{ style: {} },
}));
const __VLS_19 = __VLS_18({
    size: (120),
    src: (__VLS_ctx.avatarUrl),
    ...{ style: {} },
}, ...__VLS_functionalComponentArgsRest(__VLS_18));
__VLS_20.slots.default;
{
    const { icon: __VLS_thisSlot } = __VLS_20.slots;
    const __VLS_21 = {}.UserOutlined;
    /** @type {[typeof __VLS_components.UserOutlined, typeof __VLS_components.userOutlined, ]} */ ;
    // @ts-ignore
    const __VLS_22 = __VLS_asFunctionalComponent(__VLS_21, new __VLS_21({}));
    const __VLS_23 = __VLS_22({}, ...__VLS_functionalComponentArgsRest(__VLS_22));
}
var __VLS_20;
const __VLS_25 = {}.AUpload;
/** @type {[typeof __VLS_components.AUpload, typeof __VLS_components.aUpload, typeof __VLS_components.AUpload, typeof __VLS_components.aUpload, ]} */ ;
// @ts-ignore
const __VLS_26 = __VLS_asFunctionalComponent(__VLS_25, new __VLS_25({
    beforeUpload: (__VLS_ctx.handleAvatarUpload),
    accept: "image/*",
    showUploadList: (false),
}));
const __VLS_27 = __VLS_26({
    beforeUpload: (__VLS_ctx.handleAvatarUpload),
    accept: "image/*",
    showUploadList: (false),
}, ...__VLS_functionalComponentArgsRest(__VLS_26));
__VLS_28.slots.default;
const __VLS_29 = {}.AButton;
/** @type {[typeof __VLS_components.AButton, typeof __VLS_components.aButton, typeof __VLS_components.AButton, typeof __VLS_components.aButton, ]} */ ;
// @ts-ignore
const __VLS_30 = __VLS_asFunctionalComponent(__VLS_29, new __VLS_29({
    block: true,
}));
const __VLS_31 = __VLS_30({
    block: true,
}, ...__VLS_functionalComponentArgsRest(__VLS_30));
__VLS_32.slots.default;
const __VLS_33 = {}.UploadOutlined;
/** @type {[typeof __VLS_components.UploadOutlined, typeof __VLS_components.uploadOutlined, ]} */ ;
// @ts-ignore
const __VLS_34 = __VLS_asFunctionalComponent(__VLS_33, new __VLS_33({}));
const __VLS_35 = __VLS_34({}, ...__VLS_functionalComponentArgsRest(__VLS_34));
var __VLS_32;
var __VLS_28;
var __VLS_16;
var __VLS_12;
const __VLS_37 = {}.ACol;
/** @type {[typeof __VLS_components.ACol, typeof __VLS_components.aCol, typeof __VLS_components.ACol, typeof __VLS_components.aCol, ]} */ ;
// @ts-ignore
const __VLS_38 = __VLS_asFunctionalComponent(__VLS_37, new __VLS_37({
    span: (9),
}));
const __VLS_39 = __VLS_38({
    span: (9),
}, ...__VLS_functionalComponentArgsRest(__VLS_38));
__VLS_40.slots.default;
const __VLS_41 = {}.ACard;
/** @type {[typeof __VLS_components.ACard, typeof __VLS_components.aCard, typeof __VLS_components.ACard, typeof __VLS_components.aCard, ]} */ ;
// @ts-ignore
const __VLS_42 = __VLS_asFunctionalComponent(__VLS_41, new __VLS_41({
    title: "基本信息",
}));
const __VLS_43 = __VLS_42({
    title: "基本信息",
}, ...__VLS_functionalComponentArgsRest(__VLS_42));
__VLS_44.slots.default;
const __VLS_45 = {}.AForm;
/** @type {[typeof __VLS_components.AForm, typeof __VLS_components.aForm, typeof __VLS_components.AForm, typeof __VLS_components.aForm, ]} */ ;
// @ts-ignore
const __VLS_46 = __VLS_asFunctionalComponent(__VLS_45, new __VLS_45({
    layout: "vertical",
}));
const __VLS_47 = __VLS_46({
    layout: "vertical",
}, ...__VLS_functionalComponentArgsRest(__VLS_46));
__VLS_48.slots.default;
const __VLS_49 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_50 = __VLS_asFunctionalComponent(__VLS_49, new __VLS_49({
    label: "登录名",
}));
const __VLS_51 = __VLS_50({
    label: "登录名",
}, ...__VLS_functionalComponentArgsRest(__VLS_50));
__VLS_52.slots.default;
const __VLS_53 = {}.AInput;
/** @type {[typeof __VLS_components.AInput, typeof __VLS_components.aInput, ]} */ ;
// @ts-ignore
const __VLS_54 = __VLS_asFunctionalComponent(__VLS_53, new __VLS_53({
    value: (__VLS_ctx.profile.loginCode),
    disabled: true,
}));
const __VLS_55 = __VLS_54({
    value: (__VLS_ctx.profile.loginCode),
    disabled: true,
}, ...__VLS_functionalComponentArgsRest(__VLS_54));
var __VLS_52;
const __VLS_57 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_58 = __VLS_asFunctionalComponent(__VLS_57, new __VLS_57({
    label: "姓名",
}));
const __VLS_59 = __VLS_58({
    label: "姓名",
}, ...__VLS_functionalComponentArgsRest(__VLS_58));
__VLS_60.slots.default;
const __VLS_61 = {}.AInput;
/** @type {[typeof __VLS_components.AInput, typeof __VLS_components.aInput, ]} */ ;
// @ts-ignore
const __VLS_62 = __VLS_asFunctionalComponent(__VLS_61, new __VLS_61({
    value: (__VLS_ctx.editForm.userName),
}));
const __VLS_63 = __VLS_62({
    value: (__VLS_ctx.editForm.userName),
}, ...__VLS_functionalComponentArgsRest(__VLS_62));
var __VLS_60;
const __VLS_65 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_66 = __VLS_asFunctionalComponent(__VLS_65, new __VLS_65({
    label: "邮箱",
}));
const __VLS_67 = __VLS_66({
    label: "邮箱",
}, ...__VLS_functionalComponentArgsRest(__VLS_66));
__VLS_68.slots.default;
const __VLS_69 = {}.AInput;
/** @type {[typeof __VLS_components.AInput, typeof __VLS_components.aInput, ]} */ ;
// @ts-ignore
const __VLS_70 = __VLS_asFunctionalComponent(__VLS_69, new __VLS_69({
    value: (__VLS_ctx.editForm.email),
}));
const __VLS_71 = __VLS_70({
    value: (__VLS_ctx.editForm.email),
}, ...__VLS_functionalComponentArgsRest(__VLS_70));
var __VLS_68;
const __VLS_73 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_74 = __VLS_asFunctionalComponent(__VLS_73, new __VLS_73({
    label: "手机",
}));
const __VLS_75 = __VLS_74({
    label: "手机",
}, ...__VLS_functionalComponentArgsRest(__VLS_74));
__VLS_76.slots.default;
const __VLS_77 = {}.AInput;
/** @type {[typeof __VLS_components.AInput, typeof __VLS_components.aInput, ]} */ ;
// @ts-ignore
const __VLS_78 = __VLS_asFunctionalComponent(__VLS_77, new __VLS_77({
    value: (__VLS_ctx.editForm.phone),
}));
const __VLS_79 = __VLS_78({
    value: (__VLS_ctx.editForm.phone),
}, ...__VLS_functionalComponentArgsRest(__VLS_78));
var __VLS_76;
const __VLS_81 = {}.AButton;
/** @type {[typeof __VLS_components.AButton, typeof __VLS_components.aButton, typeof __VLS_components.AButton, typeof __VLS_components.aButton, ]} */ ;
// @ts-ignore
const __VLS_82 = __VLS_asFunctionalComponent(__VLS_81, new __VLS_81({
    ...{ 'onClick': {} },
    type: "primary",
    loading: (__VLS_ctx.saving),
}));
const __VLS_83 = __VLS_82({
    ...{ 'onClick': {} },
    type: "primary",
    loading: (__VLS_ctx.saving),
}, ...__VLS_functionalComponentArgsRest(__VLS_82));
let __VLS_85;
let __VLS_86;
let __VLS_87;
const __VLS_88 = {
    onClick: (__VLS_ctx.saveProfile)
};
__VLS_84.slots.default;
var __VLS_84;
var __VLS_48;
var __VLS_44;
var __VLS_40;
const __VLS_89 = {}.ACol;
/** @type {[typeof __VLS_components.ACol, typeof __VLS_components.aCol, typeof __VLS_components.ACol, typeof __VLS_components.aCol, ]} */ ;
// @ts-ignore
const __VLS_90 = __VLS_asFunctionalComponent(__VLS_89, new __VLS_89({
    span: (9),
}));
const __VLS_91 = __VLS_90({
    span: (9),
}, ...__VLS_functionalComponentArgsRest(__VLS_90));
__VLS_92.slots.default;
const __VLS_93 = {}.ACard;
/** @type {[typeof __VLS_components.ACard, typeof __VLS_components.aCard, typeof __VLS_components.ACard, typeof __VLS_components.aCard, ]} */ ;
// @ts-ignore
const __VLS_94 = __VLS_asFunctionalComponent(__VLS_93, new __VLS_93({
    title: "修改密码",
}));
const __VLS_95 = __VLS_94({
    title: "修改密码",
}, ...__VLS_functionalComponentArgsRest(__VLS_94));
__VLS_96.slots.default;
const __VLS_97 = {}.AForm;
/** @type {[typeof __VLS_components.AForm, typeof __VLS_components.aForm, typeof __VLS_components.AForm, typeof __VLS_components.aForm, ]} */ ;
// @ts-ignore
const __VLS_98 = __VLS_asFunctionalComponent(__VLS_97, new __VLS_97({
    layout: "vertical",
}));
const __VLS_99 = __VLS_98({
    layout: "vertical",
}, ...__VLS_functionalComponentArgsRest(__VLS_98));
__VLS_100.slots.default;
const __VLS_101 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_102 = __VLS_asFunctionalComponent(__VLS_101, new __VLS_101({
    label: "旧密码",
}));
const __VLS_103 = __VLS_102({
    label: "旧密码",
}, ...__VLS_functionalComponentArgsRest(__VLS_102));
__VLS_104.slots.default;
const __VLS_105 = {}.AInputPassword;
/** @type {[typeof __VLS_components.AInputPassword, typeof __VLS_components.aInputPassword, ]} */ ;
// @ts-ignore
const __VLS_106 = __VLS_asFunctionalComponent(__VLS_105, new __VLS_105({
    value: (__VLS_ctx.pwdForm.oldPassword),
}));
const __VLS_107 = __VLS_106({
    value: (__VLS_ctx.pwdForm.oldPassword),
}, ...__VLS_functionalComponentArgsRest(__VLS_106));
var __VLS_104;
const __VLS_109 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_110 = __VLS_asFunctionalComponent(__VLS_109, new __VLS_109({
    label: "新密码",
}));
const __VLS_111 = __VLS_110({
    label: "新密码",
}, ...__VLS_functionalComponentArgsRest(__VLS_110));
__VLS_112.slots.default;
const __VLS_113 = {}.AInputPassword;
/** @type {[typeof __VLS_components.AInputPassword, typeof __VLS_components.aInputPassword, ]} */ ;
// @ts-ignore
const __VLS_114 = __VLS_asFunctionalComponent(__VLS_113, new __VLS_113({
    value: (__VLS_ctx.pwdForm.newPassword),
}));
const __VLS_115 = __VLS_114({
    value: (__VLS_ctx.pwdForm.newPassword),
}, ...__VLS_functionalComponentArgsRest(__VLS_114));
var __VLS_112;
const __VLS_117 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_118 = __VLS_asFunctionalComponent(__VLS_117, new __VLS_117({
    label: "确认密码",
}));
const __VLS_119 = __VLS_118({
    label: "确认密码",
}, ...__VLS_functionalComponentArgsRest(__VLS_118));
__VLS_120.slots.default;
const __VLS_121 = {}.AInputPassword;
/** @type {[typeof __VLS_components.AInputPassword, typeof __VLS_components.aInputPassword, ]} */ ;
// @ts-ignore
const __VLS_122 = __VLS_asFunctionalComponent(__VLS_121, new __VLS_121({
    value: (__VLS_ctx.pwdForm.confirmPassword),
}));
const __VLS_123 = __VLS_122({
    value: (__VLS_ctx.pwdForm.confirmPassword),
}, ...__VLS_functionalComponentArgsRest(__VLS_122));
var __VLS_120;
const __VLS_125 = {}.AButton;
/** @type {[typeof __VLS_components.AButton, typeof __VLS_components.aButton, typeof __VLS_components.AButton, typeof __VLS_components.aButton, ]} */ ;
// @ts-ignore
const __VLS_126 = __VLS_asFunctionalComponent(__VLS_125, new __VLS_125({
    ...{ 'onClick': {} },
    type: "primary",
    loading: (__VLS_ctx.pwdSaving),
}));
const __VLS_127 = __VLS_126({
    ...{ 'onClick': {} },
    type: "primary",
    loading: (__VLS_ctx.pwdSaving),
}, ...__VLS_functionalComponentArgsRest(__VLS_126));
let __VLS_129;
let __VLS_130;
let __VLS_131;
const __VLS_132 = {
    onClick: (__VLS_ctx.changePwd)
};
__VLS_128.slots.default;
var __VLS_128;
var __VLS_100;
var __VLS_96;
var __VLS_92;
var __VLS_8;
const __VLS_133 = {}.ACard;
/** @type {[typeof __VLS_components.ACard, typeof __VLS_components.aCard, typeof __VLS_components.ACard, typeof __VLS_components.aCard, ]} */ ;
// @ts-ignore
const __VLS_134 = __VLS_asFunctionalComponent(__VLS_133, new __VLS_133({
    title: "登录信息",
    ...{ style: {} },
}));
const __VLS_135 = __VLS_134({
    title: "登录信息",
    ...{ style: {} },
}, ...__VLS_functionalComponentArgsRest(__VLS_134));
__VLS_136.slots.default;
const __VLS_137 = {}.ADescriptions;
/** @type {[typeof __VLS_components.ADescriptions, typeof __VLS_components.aDescriptions, typeof __VLS_components.ADescriptions, typeof __VLS_components.aDescriptions, ]} */ ;
// @ts-ignore
const __VLS_138 = __VLS_asFunctionalComponent(__VLS_137, new __VLS_137({
    column: (3),
}));
const __VLS_139 = __VLS_138({
    column: (3),
}, ...__VLS_functionalComponentArgsRest(__VLS_138));
__VLS_140.slots.default;
const __VLS_141 = {}.ADescriptionsItem;
/** @type {[typeof __VLS_components.ADescriptionsItem, typeof __VLS_components.aDescriptionsItem, typeof __VLS_components.ADescriptionsItem, typeof __VLS_components.aDescriptionsItem, ]} */ ;
// @ts-ignore
const __VLS_142 = __VLS_asFunctionalComponent(__VLS_141, new __VLS_141({
    label: "机构",
}));
const __VLS_143 = __VLS_142({
    label: "机构",
}, ...__VLS_functionalComponentArgsRest(__VLS_142));
__VLS_144.slots.default;
(__VLS_ctx.profile.orgName);
var __VLS_144;
const __VLS_145 = {}.ADescriptionsItem;
/** @type {[typeof __VLS_components.ADescriptionsItem, typeof __VLS_components.aDescriptionsItem, typeof __VLS_components.ADescriptionsItem, typeof __VLS_components.aDescriptionsItem, ]} */ ;
// @ts-ignore
const __VLS_146 = __VLS_asFunctionalComponent(__VLS_145, new __VLS_145({
    label: "用户类型",
}));
const __VLS_147 = __VLS_146({
    label: "用户类型",
}, ...__VLS_functionalComponentArgsRest(__VLS_146));
__VLS_148.slots.default;
(__VLS_ctx.profile.userType);
var __VLS_148;
const __VLS_149 = {}.ADescriptionsItem;
/** @type {[typeof __VLS_components.ADescriptionsItem, typeof __VLS_components.aDescriptionsItem, typeof __VLS_components.ADescriptionsItem, typeof __VLS_components.aDescriptionsItem, ]} */ ;
// @ts-ignore
const __VLS_150 = __VLS_asFunctionalComponent(__VLS_149, new __VLS_149({
    label: "状态",
}));
const __VLS_151 = __VLS_150({
    label: "状态",
}, ...__VLS_functionalComponentArgsRest(__VLS_150));
__VLS_152.slots.default;
const __VLS_153 = {}.ATag;
/** @type {[typeof __VLS_components.ATag, typeof __VLS_components.aTag, typeof __VLS_components.ATag, typeof __VLS_components.aTag, ]} */ ;
// @ts-ignore
const __VLS_154 = __VLS_asFunctionalComponent(__VLS_153, new __VLS_153({
    color: (__VLS_ctx.profile.status === '0' ? 'green' : 'red'),
}));
const __VLS_155 = __VLS_154({
    color: (__VLS_ctx.profile.status === '0' ? 'green' : 'red'),
}, ...__VLS_functionalComponentArgsRest(__VLS_154));
__VLS_156.slots.default;
(__VLS_ctx.profile.status === '0' ? '正常' : '停用');
var __VLS_156;
var __VLS_152;
const __VLS_157 = {}.ADescriptionsItem;
/** @type {[typeof __VLS_components.ADescriptionsItem, typeof __VLS_components.aDescriptionsItem, typeof __VLS_components.ADescriptionsItem, typeof __VLS_components.aDescriptionsItem, ]} */ ;
// @ts-ignore
const __VLS_158 = __VLS_asFunctionalComponent(__VLS_157, new __VLS_157({
    label: "最后登录",
}));
const __VLS_159 = __VLS_158({
    label: "最后登录",
}, ...__VLS_functionalComponentArgsRest(__VLS_158));
__VLS_160.slots.default;
(__VLS_ctx.profile.loginDate);
var __VLS_160;
const __VLS_161 = {}.ADescriptionsItem;
/** @type {[typeof __VLS_components.ADescriptionsItem, typeof __VLS_components.aDescriptionsItem, typeof __VLS_components.ADescriptionsItem, typeof __VLS_components.aDescriptionsItem, ]} */ ;
// @ts-ignore
const __VLS_162 = __VLS_asFunctionalComponent(__VLS_161, new __VLS_161({
    label: "创建时间",
}));
const __VLS_163 = __VLS_162({
    label: "创建时间",
}, ...__VLS_functionalComponentArgsRest(__VLS_162));
__VLS_164.slots.default;
(__VLS_ctx.profile.createDate);
var __VLS_164;
var __VLS_140;
var __VLS_136;
var __VLS_3;
var __VLS_dollars;
const __VLS_self = (await import('vue')).defineComponent({
    setup() {
        return {
            UserOutlined: UserOutlined,
            UploadOutlined: UploadOutlined,
            saving: saving,
            pwdSaving: pwdSaving,
            profile: profile,
            avatarUrl: avatarUrl,
            editForm: editForm,
            pwdForm: pwdForm,
            saveProfile: saveProfile,
            changePwd: changePwd,
            handleAvatarUpload: handleAvatarUpload,
        };
    },
});
export default (await import('vue')).defineComponent({
    setup() {
        return {};
    },
});
; /* PartiallyEnd: #4569/main.vue */
