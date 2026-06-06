import { ref, reactive, onMounted } from 'vue';
import { moduleApi } from '@/api';
import { message } from 'ant-design-vue';
const loading = ref(false);
const data = reactive({ list: [], total: 0, pageNo: 1, pageSize: 20 });
const columns = [{ title: '编码', dataIndex: 'moduleCode' }, { title: '名称', dataIndex: 'moduleName' }, { title: '版本', dataIndex: 'moduleVersion' }, { title: '启用', dataIndex: 'isEnabled' }, { title: '操作', key: 'action' }];
const modalVisible = ref(false);
const modalTitle = ref('新增模块');
const isEdit = ref(false);
const form = reactive({ moduleCode: '', moduleName: '', moduleVersion: '', isEnabledBool: '1' });
async function loadData() { loading.value = true; const r = await moduleApi.list({ pageNo: data.pageNo, pageSize: data.pageSize, entity: {} }); if (r.data) {
    data.list = r.data.list;
    data.total = r.data.total;
} loading.value = false; }
function onPageChange(p, s) { data.pageNo = p; data.pageSize = s; loadData(); }
function showAdd() { isEdit.value = false; modalTitle.value = '新增模块'; form.moduleCode = ''; form.moduleName = ''; form.moduleVersion = ''; form.isEnabledBool = '1'; modalVisible.value = true; }
function showEdit(r) { isEdit.value = true; modalTitle.value = '编辑模块'; form.moduleCode = r.moduleCode; form.moduleName = r.moduleName; form.moduleVersion = r.moduleVersion || ''; form.isEnabledBool = r.isEnabled || '1'; modalVisible.value = true; }
async function save() { await moduleApi.save({ moduleCode: isEdit.value ? form.moduleCode : form.moduleCode || undefined, moduleName: form.moduleName, moduleVersion: form.moduleVersion, isEnabled: form.isEnabledBool }); message.success('保存成功'); modalVisible.value = false; loadData(); }
async function handleDelete(k) { await moduleApi.delete(k); message.success('删除成功'); loadData(); }
onMounted(loadData);
debugger; /* PartiallyEnd: #3632/scriptSetup.vue */
const __VLS_ctx = {};
let __VLS_components;
let __VLS_directives;
const __VLS_0 = {}.ACard;
/** @type {[typeof __VLS_components.ACard, typeof __VLS_components.aCard, typeof __VLS_components.ACard, typeof __VLS_components.aCard, ]} */ ;
// @ts-ignore
const __VLS_1 = __VLS_asFunctionalComponent(__VLS_0, new __VLS_0({
    title: "模块管理",
}));
const __VLS_2 = __VLS_1({
    title: "模块管理",
}, ...__VLS_functionalComponentArgsRest(__VLS_1));
var __VLS_4 = {};
__VLS_3.slots.default;
const __VLS_5 = {}.AButton;
/** @type {[typeof __VLS_components.AButton, typeof __VLS_components.aButton, typeof __VLS_components.AButton, typeof __VLS_components.aButton, ]} */ ;
// @ts-ignore
const __VLS_6 = __VLS_asFunctionalComponent(__VLS_5, new __VLS_5({
    ...{ 'onClick': {} },
    type: "primary",
    ...{ style: {} },
}));
const __VLS_7 = __VLS_6({
    ...{ 'onClick': {} },
    type: "primary",
    ...{ style: {} },
}, ...__VLS_functionalComponentArgsRest(__VLS_6));
let __VLS_9;
let __VLS_10;
let __VLS_11;
const __VLS_12 = {
    onClick: (__VLS_ctx.showAdd)
};
__VLS_8.slots.default;
var __VLS_8;
const __VLS_13 = {}.ATable;
/** @type {[typeof __VLS_components.ATable, typeof __VLS_components.aTable, typeof __VLS_components.ATable, typeof __VLS_components.aTable, ]} */ ;
// @ts-ignore
const __VLS_14 = __VLS_asFunctionalComponent(__VLS_13, new __VLS_13({
    dataSource: (__VLS_ctx.data.list),
    columns: (__VLS_ctx.columns),
    loading: (__VLS_ctx.loading),
    rowKey: "moduleCode",
    pagination: ({ current: __VLS_ctx.data.pageNo, pageSize: __VLS_ctx.data.pageSize, total: __VLS_ctx.data.total, onChange: __VLS_ctx.onPageChange }),
}));
const __VLS_15 = __VLS_14({
    dataSource: (__VLS_ctx.data.list),
    columns: (__VLS_ctx.columns),
    loading: (__VLS_ctx.loading),
    rowKey: "moduleCode",
    pagination: ({ current: __VLS_ctx.data.pageNo, pageSize: __VLS_ctx.data.pageSize, total: __VLS_ctx.data.total, onChange: __VLS_ctx.onPageChange }),
}, ...__VLS_functionalComponentArgsRest(__VLS_14));
__VLS_16.slots.default;
{
    const { bodyCell: __VLS_thisSlot } = __VLS_16.slots;
    const [{ record, column }] = __VLS_getSlotParams(__VLS_thisSlot);
    if (column.key === 'action') {
        const __VLS_17 = {}.ASpace;
        /** @type {[typeof __VLS_components.ASpace, typeof __VLS_components.aSpace, typeof __VLS_components.ASpace, typeof __VLS_components.aSpace, ]} */ ;
        // @ts-ignore
        const __VLS_18 = __VLS_asFunctionalComponent(__VLS_17, new __VLS_17({}));
        const __VLS_19 = __VLS_18({}, ...__VLS_functionalComponentArgsRest(__VLS_18));
        __VLS_20.slots.default;
        __VLS_asFunctionalElement(__VLS_intrinsicElements.a, __VLS_intrinsicElements.a)({
            ...{ onClick: (...[$event]) => {
                    if (!(column.key === 'action'))
                        return;
                    __VLS_ctx.showEdit(record);
                } },
        });
        __VLS_asFunctionalElement(__VLS_intrinsicElements.a, __VLS_intrinsicElements.a)({
            ...{ onClick: (...[$event]) => {
                    if (!(column.key === 'action'))
                        return;
                    __VLS_ctx.handleDelete(record.moduleCode);
                } },
        });
        var __VLS_20;
    }
}
var __VLS_16;
const __VLS_21 = {}.AModal;
/** @type {[typeof __VLS_components.AModal, typeof __VLS_components.aModal, typeof __VLS_components.AModal, typeof __VLS_components.aModal, ]} */ ;
// @ts-ignore
const __VLS_22 = __VLS_asFunctionalComponent(__VLS_21, new __VLS_21({
    ...{ 'onOk': {} },
    open: (__VLS_ctx.modalVisible),
    title: (__VLS_ctx.modalTitle),
}));
const __VLS_23 = __VLS_22({
    ...{ 'onOk': {} },
    open: (__VLS_ctx.modalVisible),
    title: (__VLS_ctx.modalTitle),
}, ...__VLS_functionalComponentArgsRest(__VLS_22));
let __VLS_25;
let __VLS_26;
let __VLS_27;
const __VLS_28 = {
    onOk: (__VLS_ctx.save)
};
__VLS_24.slots.default;
const __VLS_29 = {}.AForm;
/** @type {[typeof __VLS_components.AForm, typeof __VLS_components.aForm, typeof __VLS_components.AForm, typeof __VLS_components.aForm, ]} */ ;
// @ts-ignore
const __VLS_30 = __VLS_asFunctionalComponent(__VLS_29, new __VLS_29({
    layout: "vertical",
}));
const __VLS_31 = __VLS_30({
    layout: "vertical",
}, ...__VLS_functionalComponentArgsRest(__VLS_30));
__VLS_32.slots.default;
const __VLS_33 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_34 = __VLS_asFunctionalComponent(__VLS_33, new __VLS_33({
    label: "编码",
}));
const __VLS_35 = __VLS_34({
    label: "编码",
}, ...__VLS_functionalComponentArgsRest(__VLS_34));
__VLS_36.slots.default;
const __VLS_37 = {}.AInput;
/** @type {[typeof __VLS_components.AInput, typeof __VLS_components.aInput, ]} */ ;
// @ts-ignore
const __VLS_38 = __VLS_asFunctionalComponent(__VLS_37, new __VLS_37({
    value: (__VLS_ctx.form.moduleCode),
    disabled: (__VLS_ctx.isEdit),
}));
const __VLS_39 = __VLS_38({
    value: (__VLS_ctx.form.moduleCode),
    disabled: (__VLS_ctx.isEdit),
}, ...__VLS_functionalComponentArgsRest(__VLS_38));
var __VLS_36;
const __VLS_41 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_42 = __VLS_asFunctionalComponent(__VLS_41, new __VLS_41({
    label: "名称",
}));
const __VLS_43 = __VLS_42({
    label: "名称",
}, ...__VLS_functionalComponentArgsRest(__VLS_42));
__VLS_44.slots.default;
const __VLS_45 = {}.AInput;
/** @type {[typeof __VLS_components.AInput, typeof __VLS_components.aInput, ]} */ ;
// @ts-ignore
const __VLS_46 = __VLS_asFunctionalComponent(__VLS_45, new __VLS_45({
    value: (__VLS_ctx.form.moduleName),
}));
const __VLS_47 = __VLS_46({
    value: (__VLS_ctx.form.moduleName),
}, ...__VLS_functionalComponentArgsRest(__VLS_46));
var __VLS_44;
const __VLS_49 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_50 = __VLS_asFunctionalComponent(__VLS_49, new __VLS_49({
    label: "版本",
}));
const __VLS_51 = __VLS_50({
    label: "版本",
}, ...__VLS_functionalComponentArgsRest(__VLS_50));
__VLS_52.slots.default;
const __VLS_53 = {}.AInput;
/** @type {[typeof __VLS_components.AInput, typeof __VLS_components.aInput, ]} */ ;
// @ts-ignore
const __VLS_54 = __VLS_asFunctionalComponent(__VLS_53, new __VLS_53({
    value: (__VLS_ctx.form.moduleVersion),
}));
const __VLS_55 = __VLS_54({
    value: (__VLS_ctx.form.moduleVersion),
}, ...__VLS_functionalComponentArgsRest(__VLS_54));
var __VLS_52;
const __VLS_57 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_58 = __VLS_asFunctionalComponent(__VLS_57, new __VLS_57({
    label: "启用",
}));
const __VLS_59 = __VLS_58({
    label: "启用",
}, ...__VLS_functionalComponentArgsRest(__VLS_58));
__VLS_60.slots.default;
const __VLS_61 = {}.ASwitch;
/** @type {[typeof __VLS_components.ASwitch, typeof __VLS_components.aSwitch, ]} */ ;
// @ts-ignore
const __VLS_62 = __VLS_asFunctionalComponent(__VLS_61, new __VLS_61({
    checked: (__VLS_ctx.form.isEnabledBool),
    checkedValue: "1",
    unCheckedValue: "0",
}));
const __VLS_63 = __VLS_62({
    checked: (__VLS_ctx.form.isEnabledBool),
    checkedValue: "1",
    unCheckedValue: "0",
}, ...__VLS_functionalComponentArgsRest(__VLS_62));
var __VLS_60;
var __VLS_32;
var __VLS_24;
var __VLS_3;
var __VLS_dollars;
const __VLS_self = (await import('vue')).defineComponent({
    setup() {
        return {
            loading: loading,
            data: data,
            columns: columns,
            modalVisible: modalVisible,
            modalTitle: modalTitle,
            isEdit: isEdit,
            form: form,
            onPageChange: onPageChange,
            showAdd: showAdd,
            showEdit: showEdit,
            save: save,
            handleDelete: handleDelete,
        };
    },
});
export default (await import('vue')).defineComponent({
    setup() {
        return {};
    },
});
; /* PartiallyEnd: #4569/main.vue */
