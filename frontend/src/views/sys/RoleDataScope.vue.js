import { ref, onMounted } from 'vue';
import { roleApi, menuApi, roleDataScopeApi } from '@/api';
import { message } from 'ant-design-vue';
const loading = ref(false);
const saving = ref(false);
const selectedRole = ref('');
const roles = ref([]);
const menuTree = ref([]);
const columns = [
    { title: '菜单名称', dataIndex: 'menuName' },
    { title: '规则名称', key: 'ruleName' },
    { title: '数据范围', key: 'ruleType' },
    { title: '规则配置', key: 'ruleConfig' }
];
async function loadRoles() {
    const res = await roleApi.list({ pageNo: 1, pageSize: 999 });
    if (res.data)
        roles.value = res.data.list;
}
async function loadMenus() {
    const res = await menuApi.tree();
    if (res.data) {
        menuTree.value = flattenTree(res.data);
    }
}
function flattenTree(items, parent) {
    const result = [];
    for (const item of items) {
        const row = { ...item, ruleType: 'all', ruleConfig: '', ruleName: '' };
        if (parent)
            row._parent = parent;
        result.push(row);
        if (item.children)
            result.push(...flattenTree(item.children, row));
    }
    return result;
}
async function loadScopes() {
    if (!selectedRole.value)
        return;
    loading.value = true;
    const res = await roleDataScopeApi.getByRole(selectedRole.value);
    if (res.data) {
        const map = new Map(res.data.map((s) => [s.menuCode, s]));
        for (const row of menuTree.value) {
            const scope = map.get(row.menuCode);
            if (scope) {
                row.ruleType = scope.ruleType || 'all';
                row.ruleConfig = scope.ruleConfig || '';
                row.ruleName = scope.ruleName || '';
            }
            else {
                row.ruleType = 'all';
                row.ruleConfig = '';
                row.ruleName = '';
            }
        }
    }
    loading.value = false;
}
function onRoleChange() { loadScopes(); }
function onRuleTypeChange(record, val) { record.ruleType = val; }
async function saveAll() {
    if (!selectedRole.value) {
        message.warning('请先选择角色');
        return;
    }
    saving.value = true;
    for (const row of menuTree.value) {
        await roleDataScopeApi.save({ roleCode: selectedRole.value, menuCode: row.menuCode, ruleName: row.ruleName, ruleType: row.ruleType, ruleConfig: row.ruleConfig });
    }
    message.success('保存成功');
    saving.value = false;
}
onMounted(async () => { await loadRoles(); await loadMenus(); });
debugger; /* PartiallyEnd: #3632/scriptSetup.vue */
const __VLS_ctx = {};
let __VLS_components;
let __VLS_directives;
const __VLS_0 = {}.ACard;
/** @type {[typeof __VLS_components.ACard, typeof __VLS_components.aCard, typeof __VLS_components.ACard, typeof __VLS_components.aCard, ]} */ ;
// @ts-ignore
const __VLS_1 = __VLS_asFunctionalComponent(__VLS_0, new __VLS_0({
    title: "角色数据权限",
}));
const __VLS_2 = __VLS_1({
    title: "角色数据权限",
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
    placeholder: "请选择角色",
}));
const __VLS_15 = __VLS_14({
    ...{ 'onChange': {} },
    value: (__VLS_ctx.selectedRole),
    ...{ style: {} },
    placeholder: "请选择角色",
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
const __VLS_26 = __VLS_asFunctionalComponent(__VLS_25, new __VLS_25({}));
const __VLS_27 = __VLS_26({}, ...__VLS_functionalComponentArgsRest(__VLS_26));
__VLS_28.slots.default;
const __VLS_29 = {}.AButton;
/** @type {[typeof __VLS_components.AButton, typeof __VLS_components.aButton, typeof __VLS_components.AButton, typeof __VLS_components.aButton, ]} */ ;
// @ts-ignore
const __VLS_30 = __VLS_asFunctionalComponent(__VLS_29, new __VLS_29({
    ...{ 'onClick': {} },
    type: "primary",
    loading: (__VLS_ctx.saving),
}));
const __VLS_31 = __VLS_30({
    ...{ 'onClick': {} },
    type: "primary",
    loading: (__VLS_ctx.saving),
}, ...__VLS_functionalComponentArgsRest(__VLS_30));
let __VLS_33;
let __VLS_34;
let __VLS_35;
const __VLS_36 = {
    onClick: (__VLS_ctx.saveAll)
};
__VLS_32.slots.default;
var __VLS_32;
var __VLS_28;
var __VLS_8;
const __VLS_37 = {}.ATable;
/** @type {[typeof __VLS_components.ATable, typeof __VLS_components.aTable, typeof __VLS_components.ATable, typeof __VLS_components.aTable, ]} */ ;
// @ts-ignore
const __VLS_38 = __VLS_asFunctionalComponent(__VLS_37, new __VLS_37({
    dataSource: (__VLS_ctx.menuTree),
    columns: (__VLS_ctx.columns),
    rowKey: "menuCode",
    loading: (__VLS_ctx.loading),
    pagination: (false),
    defaultExpandAllRows: true,
}));
const __VLS_39 = __VLS_38({
    dataSource: (__VLS_ctx.menuTree),
    columns: (__VLS_ctx.columns),
    rowKey: "menuCode",
    loading: (__VLS_ctx.loading),
    pagination: (false),
    defaultExpandAllRows: true,
}, ...__VLS_functionalComponentArgsRest(__VLS_38));
__VLS_40.slots.default;
{
    const { bodyCell: __VLS_thisSlot } = __VLS_40.slots;
    const [{ record, column }] = __VLS_getSlotParams(__VLS_thisSlot);
    if (column.key === 'ruleType') {
        const __VLS_41 = {}.ASelect;
        /** @type {[typeof __VLS_components.ASelect, typeof __VLS_components.aSelect, typeof __VLS_components.ASelect, typeof __VLS_components.aSelect, ]} */ ;
        // @ts-ignore
        const __VLS_42 = __VLS_asFunctionalComponent(__VLS_41, new __VLS_41({
            ...{ 'onChange': {} },
            value: (record.ruleType),
            ...{ style: {} },
        }));
        const __VLS_43 = __VLS_42({
            ...{ 'onChange': {} },
            value: (record.ruleType),
            ...{ style: {} },
        }, ...__VLS_functionalComponentArgsRest(__VLS_42));
        let __VLS_45;
        let __VLS_46;
        let __VLS_47;
        const __VLS_48 = {
            onChange: ((val) => __VLS_ctx.onRuleTypeChange(record, val))
        };
        __VLS_44.slots.default;
        const __VLS_49 = {}.ASelectOption;
        /** @type {[typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, ]} */ ;
        // @ts-ignore
        const __VLS_50 = __VLS_asFunctionalComponent(__VLS_49, new __VLS_49({
            value: "all",
        }));
        const __VLS_51 = __VLS_50({
            value: "all",
        }, ...__VLS_functionalComponentArgsRest(__VLS_50));
        __VLS_52.slots.default;
        var __VLS_52;
        const __VLS_53 = {}.ASelectOption;
        /** @type {[typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, ]} */ ;
        // @ts-ignore
        const __VLS_54 = __VLS_asFunctionalComponent(__VLS_53, new __VLS_53({
            value: "custom",
        }));
        const __VLS_55 = __VLS_54({
            value: "custom",
        }, ...__VLS_functionalComponentArgsRest(__VLS_54));
        __VLS_56.slots.default;
        var __VLS_56;
        const __VLS_57 = {}.ASelectOption;
        /** @type {[typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, ]} */ ;
        // @ts-ignore
        const __VLS_58 = __VLS_asFunctionalComponent(__VLS_57, new __VLS_57({
            value: "org",
        }));
        const __VLS_59 = __VLS_58({
            value: "org",
        }, ...__VLS_functionalComponentArgsRest(__VLS_58));
        __VLS_60.slots.default;
        var __VLS_60;
        const __VLS_61 = {}.ASelectOption;
        /** @type {[typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, ]} */ ;
        // @ts-ignore
        const __VLS_62 = __VLS_asFunctionalComponent(__VLS_61, new __VLS_61({
            value: "org_child",
        }));
        const __VLS_63 = __VLS_62({
            value: "org_child",
        }, ...__VLS_functionalComponentArgsRest(__VLS_62));
        __VLS_64.slots.default;
        var __VLS_64;
        const __VLS_65 = {}.ASelectOption;
        /** @type {[typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, ]} */ ;
        // @ts-ignore
        const __VLS_66 = __VLS_asFunctionalComponent(__VLS_65, new __VLS_65({
            value: "self",
        }));
        const __VLS_67 = __VLS_66({
            value: "self",
        }, ...__VLS_functionalComponentArgsRest(__VLS_66));
        __VLS_68.slots.default;
        var __VLS_68;
        var __VLS_44;
    }
    if (column.key === 'ruleConfig') {
        const __VLS_69 = {}.AInput;
        /** @type {[typeof __VLS_components.AInput, typeof __VLS_components.aInput, ]} */ ;
        // @ts-ignore
        const __VLS_70 = __VLS_asFunctionalComponent(__VLS_69, new __VLS_69({
            value: (record.ruleConfig),
            placeholder: "自定义数据范围",
            disabled: (record.ruleType !== 'custom'),
            ...{ style: {} },
        }));
        const __VLS_71 = __VLS_70({
            value: (record.ruleConfig),
            placeholder: "自定义数据范围",
            disabled: (record.ruleType !== 'custom'),
            ...{ style: {} },
        }, ...__VLS_functionalComponentArgsRest(__VLS_70));
    }
    if (column.key === 'ruleName') {
        const __VLS_73 = {}.AInput;
        /** @type {[typeof __VLS_components.AInput, typeof __VLS_components.aInput, ]} */ ;
        // @ts-ignore
        const __VLS_74 = __VLS_asFunctionalComponent(__VLS_73, new __VLS_73({
            value: (record.ruleName),
            placeholder: "规则名称",
            ...{ style: {} },
        }));
        const __VLS_75 = __VLS_74({
            value: (record.ruleName),
            placeholder: "规则名称",
            ...{ style: {} },
        }, ...__VLS_functionalComponentArgsRest(__VLS_74));
    }
}
var __VLS_40;
var __VLS_3;
var __VLS_dollars;
const __VLS_self = (await import('vue')).defineComponent({
    setup() {
        return {
            loading: loading,
            saving: saving,
            selectedRole: selectedRole,
            roles: roles,
            menuTree: menuTree,
            columns: columns,
            onRoleChange: onRoleChange,
            onRuleTypeChange: onRuleTypeChange,
            saveAll: saveAll,
        };
    },
});
export default (await import('vue')).defineComponent({
    setup() {
        return {};
    },
});
; /* PartiallyEnd: #4569/main.vue */
