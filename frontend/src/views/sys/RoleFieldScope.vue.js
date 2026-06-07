import { ref, onMounted } from 'vue';
import { roleApi, menuApi, roleFieldScopeApi } from '@/api';
import { message } from 'ant-design-vue';
const loading = ref(false);
const selectedRole = ref('');
const selectedMenu = ref('');
const roles = ref([]);
const allMenus = ref([]);
const scopes = ref([]);
const columns = [
    { title: '实体名', key: 'entityName' }, { title: '显示名', key: 'entityLabel' },
    { title: '字段配置(JSON)', key: 'fieldConfig' }, { title: '操作', key: 'action' }
];
async function loadRoles() {
    const res = await roleApi.list({ pageNo: 1, pageSize: 999 });
    if (res.data)
        roles.value = res.data.list;
}
async function loadFlatMenus() {
    const res = await menuApi.tree();
    if (res.data) {
        const flat = [];
        function walk(items) { for (const m of items) {
            flat.push(m);
            if (m.children)
                walk(m.children);
        } }
        walk(res.data);
        allMenus.value = flat;
    }
}
async function loadFieldScopes() {
    if (!selectedRole.value || !selectedMenu.value)
        return;
    loading.value = true;
    const res = await roleFieldScopeApi.getByRoleMenu(selectedRole.value, selectedMenu.value);
    if (res.data)
        scopes.value = res.data;
    loading.value = false;
}
function onRoleChange() { scopes.value = []; selectedMenu.value = ''; }
function addEntity() {
    scopes.value.push({ entityName: '', entityLabel: '', entityClass: '', fieldConfig: '{}', roleCode: selectedRole.value, menuCode: selectedMenu.value });
}
async function saveScope(record) {
    record.roleCode = selectedRole.value;
    record.menuCode = selectedMenu.value;
    await roleFieldScopeApi.save(record);
    message.success('保存成功');
}
async function deleteScope(record) {
    if (record.id)
        await roleFieldScopeApi.delete(record.id);
    scopes.value = scopes.value.filter((s) => s !== record);
    message.success('删除成功');
}
onMounted(async () => { await loadRoles(); await loadFlatMenus(); });
debugger; /* PartiallyEnd: #3632/scriptSetup.vue */
const __VLS_ctx = {};
let __VLS_components;
let __VLS_directives;
const __VLS_0 = {}.ACard;
/** @type {[typeof __VLS_components.ACard, typeof __VLS_components.aCard, typeof __VLS_components.ACard, typeof __VLS_components.aCard, ]} */ ;
// @ts-ignore
const __VLS_1 = __VLS_asFunctionalComponent(__VLS_0, new __VLS_0({
    title: "角色字段权限",
}));
const __VLS_2 = __VLS_1({
    title: "角色字段权限",
}, ...__VLS_functionalComponentArgsRest(__VLS_1));
var __VLS_4 = {};
__VLS_3.slots.default;
const __VLS_5 = {}.AForm;
/** @type {[typeof __VLS_components.AForm, typeof __VLS_components.aForm, typeof __VLS_components.AForm, typeof __VLS_components.aForm, ]} */ ;
// @ts-ignore
const __VLS_6 = __VLS_asFunctionalComponent(__VLS_5, new __VLS_5({
    layout: "inline",
    ...{ style: {} },
}));
const __VLS_7 = __VLS_6({
    layout: "inline",
    ...{ style: {} },
}, ...__VLS_functionalComponentArgsRest(__VLS_6));
__VLS_8.slots.default;
const __VLS_9 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_10 = __VLS_asFunctionalComponent(__VLS_9, new __VLS_9({
    label: "选择角色",
}));
const __VLS_11 = __VLS_10({
    label: "选择角色",
}, ...__VLS_functionalComponentArgsRest(__VLS_10));
__VLS_12.slots.default;
const __VLS_13 = {}.ASelect;
/** @type {[typeof __VLS_components.ASelect, typeof __VLS_components.aSelect, typeof __VLS_components.ASelect, typeof __VLS_components.aSelect, ]} */ ;
// @ts-ignore
const __VLS_14 = __VLS_asFunctionalComponent(__VLS_13, new __VLS_13({
    ...{ 'onChange': {} },
    value: (__VLS_ctx.selectedRole),
    ...{ style: {} },
    placeholder: "选择角色",
}));
const __VLS_15 = __VLS_14({
    ...{ 'onChange': {} },
    value: (__VLS_ctx.selectedRole),
    ...{ style: {} },
    placeholder: "选择角色",
}, ...__VLS_functionalComponentArgsRest(__VLS_14));
let __VLS_17;
let __VLS_18;
let __VLS_19;
const __VLS_20 = {
    onChange: (__VLS_ctx.onRoleChange)
};
__VLS_16.slots.default;
for (const [r] of __VLS_getVForSourceType((__VLS_ctx.roles))) {
    const __VLS_21 = {}.ASelectOption;
    /** @type {[typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, ]} */ ;
    // @ts-ignore
    const __VLS_22 = __VLS_asFunctionalComponent(__VLS_21, new __VLS_21({
        key: (r.roleCode),
        value: (r.roleCode),
    }));
    const __VLS_23 = __VLS_22({
        key: (r.roleCode),
        value: (r.roleCode),
    }, ...__VLS_functionalComponentArgsRest(__VLS_22));
    __VLS_24.slots.default;
    (r.roleName);
    var __VLS_24;
}
var __VLS_16;
var __VLS_12;
const __VLS_25 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_26 = __VLS_asFunctionalComponent(__VLS_25, new __VLS_25({
    label: "选择菜单",
}));
const __VLS_27 = __VLS_26({
    label: "选择菜单",
}, ...__VLS_functionalComponentArgsRest(__VLS_26));
__VLS_28.slots.default;
const __VLS_29 = {}.ASelect;
/** @type {[typeof __VLS_components.ASelect, typeof __VLS_components.aSelect, typeof __VLS_components.ASelect, typeof __VLS_components.aSelect, ]} */ ;
// @ts-ignore
const __VLS_30 = __VLS_asFunctionalComponent(__VLS_29, new __VLS_29({
    ...{ 'onChange': {} },
    value: (__VLS_ctx.selectedMenu),
    ...{ style: {} },
    placeholder: "选择菜单",
}));
const __VLS_31 = __VLS_30({
    ...{ 'onChange': {} },
    value: (__VLS_ctx.selectedMenu),
    ...{ style: {} },
    placeholder: "选择菜单",
}, ...__VLS_functionalComponentArgsRest(__VLS_30));
let __VLS_33;
let __VLS_34;
let __VLS_35;
const __VLS_36 = {
    onChange: (__VLS_ctx.loadFieldScopes)
};
__VLS_32.slots.default;
for (const [m] of __VLS_getVForSourceType((__VLS_ctx.allMenus))) {
    const __VLS_37 = {}.ASelectOption;
    /** @type {[typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, ]} */ ;
    // @ts-ignore
    const __VLS_38 = __VLS_asFunctionalComponent(__VLS_37, new __VLS_37({
        key: (m.menuCode),
        value: (m.menuCode),
    }));
    const __VLS_39 = __VLS_38({
        key: (m.menuCode),
        value: (m.menuCode),
    }, ...__VLS_functionalComponentArgsRest(__VLS_38));
    __VLS_40.slots.default;
    (m.menuName);
    var __VLS_40;
}
var __VLS_32;
var __VLS_28;
const __VLS_41 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_42 = __VLS_asFunctionalComponent(__VLS_41, new __VLS_41({}));
const __VLS_43 = __VLS_42({}, ...__VLS_functionalComponentArgsRest(__VLS_42));
__VLS_44.slots.default;
const __VLS_45 = {}.AButton;
/** @type {[typeof __VLS_components.AButton, typeof __VLS_components.aButton, typeof __VLS_components.AButton, typeof __VLS_components.aButton, ]} */ ;
// @ts-ignore
const __VLS_46 = __VLS_asFunctionalComponent(__VLS_45, new __VLS_45({
    ...{ 'onClick': {} },
    type: "primary",
}));
const __VLS_47 = __VLS_46({
    ...{ 'onClick': {} },
    type: "primary",
}, ...__VLS_functionalComponentArgsRest(__VLS_46));
let __VLS_49;
let __VLS_50;
let __VLS_51;
const __VLS_52 = {
    onClick: (__VLS_ctx.addEntity)
};
__VLS_48.slots.default;
var __VLS_48;
var __VLS_44;
var __VLS_8;
const __VLS_53 = {}.ATable;
/** @type {[typeof __VLS_components.ATable, typeof __VLS_components.aTable, typeof __VLS_components.ATable, typeof __VLS_components.aTable, ]} */ ;
// @ts-ignore
const __VLS_54 = __VLS_asFunctionalComponent(__VLS_53, new __VLS_53({
    dataSource: (__VLS_ctx.scopes),
    columns: (__VLS_ctx.columns),
    rowKey: "entityClass",
    loading: (__VLS_ctx.loading),
    pagination: (false),
}));
const __VLS_55 = __VLS_54({
    dataSource: (__VLS_ctx.scopes),
    columns: (__VLS_ctx.columns),
    rowKey: "entityClass",
    loading: (__VLS_ctx.loading),
    pagination: (false),
}, ...__VLS_functionalComponentArgsRest(__VLS_54));
__VLS_56.slots.default;
{
    const { bodyCell: __VLS_thisSlot } = __VLS_56.slots;
    const [{ record, column }] = __VLS_getSlotParams(__VLS_thisSlot);
    if (column.key === 'entityName') {
        const __VLS_57 = {}.AInput;
        /** @type {[typeof __VLS_components.AInput, typeof __VLS_components.aInput, ]} */ ;
        // @ts-ignore
        const __VLS_58 = __VLS_asFunctionalComponent(__VLS_57, new __VLS_57({
            value: (record.entityName),
            placeholder: "实体名",
        }));
        const __VLS_59 = __VLS_58({
            value: (record.entityName),
            placeholder: "实体名",
        }, ...__VLS_functionalComponentArgsRest(__VLS_58));
    }
    if (column.key === 'entityLabel') {
        const __VLS_61 = {}.AInput;
        /** @type {[typeof __VLS_components.AInput, typeof __VLS_components.aInput, ]} */ ;
        // @ts-ignore
        const __VLS_62 = __VLS_asFunctionalComponent(__VLS_61, new __VLS_61({
            value: (record.entityLabel),
            placeholder: "显示名",
        }));
        const __VLS_63 = __VLS_62({
            value: (record.entityLabel),
            placeholder: "显示名",
        }, ...__VLS_functionalComponentArgsRest(__VLS_62));
    }
    if (column.key === 'fieldConfig') {
        const __VLS_65 = {}.ATextarea;
        /** @type {[typeof __VLS_components.ATextarea, typeof __VLS_components.aTextarea, ]} */ ;
        // @ts-ignore
        const __VLS_66 = __VLS_asFunctionalComponent(__VLS_65, new __VLS_65({
            value: (record.fieldConfig),
            rows: (2),
            placeholder: '{"fieldName":"visible|editable"}',
            ...{ style: {} },
        }));
        const __VLS_67 = __VLS_66({
            value: (record.fieldConfig),
            rows: (2),
            placeholder: '{"fieldName":"visible|editable"}',
            ...{ style: {} },
        }, ...__VLS_functionalComponentArgsRest(__VLS_66));
    }
    if (column.key === 'action') {
        const __VLS_69 = {}.ASpace;
        /** @type {[typeof __VLS_components.ASpace, typeof __VLS_components.aSpace, typeof __VLS_components.ASpace, typeof __VLS_components.aSpace, ]} */ ;
        // @ts-ignore
        const __VLS_70 = __VLS_asFunctionalComponent(__VLS_69, new __VLS_69({}));
        const __VLS_71 = __VLS_70({}, ...__VLS_functionalComponentArgsRest(__VLS_70));
        __VLS_72.slots.default;
        __VLS_asFunctionalElement(__VLS_intrinsicElements.a, __VLS_intrinsicElements.a)({
            ...{ onClick: (...[$event]) => {
                    if (!(column.key === 'action'))
                        return;
                    __VLS_ctx.saveScope(record);
                } },
        });
        const __VLS_73 = {}.APopconfirm;
        /** @type {[typeof __VLS_components.APopconfirm, typeof __VLS_components.aPopconfirm, typeof __VLS_components.APopconfirm, typeof __VLS_components.aPopconfirm, ]} */ ;
        // @ts-ignore
        const __VLS_74 = __VLS_asFunctionalComponent(__VLS_73, new __VLS_73({
            ...{ 'onConfirm': {} },
            title: "确定删除?",
        }));
        const __VLS_75 = __VLS_74({
            ...{ 'onConfirm': {} },
            title: "确定删除?",
        }, ...__VLS_functionalComponentArgsRest(__VLS_74));
        let __VLS_77;
        let __VLS_78;
        let __VLS_79;
        const __VLS_80 = {
            onConfirm: (...[$event]) => {
                if (!(column.key === 'action'))
                    return;
                __VLS_ctx.deleteScope(record);
            }
        };
        __VLS_76.slots.default;
        __VLS_asFunctionalElement(__VLS_intrinsicElements.a, __VLS_intrinsicElements.a)({
            ...{ style: {} },
        });
        var __VLS_76;
        var __VLS_72;
    }
}
var __VLS_56;
var __VLS_3;
var __VLS_dollars;
const __VLS_self = (await import('vue')).defineComponent({
    setup() {
        return {
            loading: loading,
            selectedRole: selectedRole,
            selectedMenu: selectedMenu,
            roles: roles,
            allMenus: allMenus,
            scopes: scopes,
            columns: columns,
            loadFieldScopes: loadFieldScopes,
            onRoleChange: onRoleChange,
            addEntity: addEntity,
            saveScope: saveScope,
            deleteScope: deleteScope,
        };
    },
});
export default (await import('vue')).defineComponent({
    setup() {
        return {};
    },
});
; /* PartiallyEnd: #4569/main.vue */
