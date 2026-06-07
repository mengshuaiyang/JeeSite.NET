import { ref, reactive, onMounted } from 'vue';
import { msgApi } from '@/api';
import { message } from 'ant-design-vue';
const loading = ref(false);
const saving = ref(false);
const modalOpen = ref(false);
const editItem = ref(null);
const data = reactive({ list: [], total: 0, pageNo: 1, pageSize: 20 });
const form = reactive({ templateName: '', templateKey: '', templateContent: '', templateType: '' });
const columns = [
    { title: '模板名称', dataIndex: 'templateName' }, { title: '模板键', dataIndex: 'templateKey' },
    { title: '类型', dataIndex: 'templateType' }, { title: '状态', dataIndex: 'status' },
    { title: '操作', key: 'action' }
];
async function loadData() {
    loading.value = true;
    const res = await msgApi.templateList({ pageNo: data.pageNo, pageSize: data.pageSize, entity: {} });
    if (res.data) {
        data.list = res.data.list;
        data.total = res.data.total;
    }
    loading.value = false;
}
function onPageChange(page, size) { data.pageNo = page; data.pageSize = size; loadData(); }
function showAdd() { editItem.value = null; form.templateName = ''; form.templateKey = ''; form.templateContent = ''; form.templateType = ''; modalOpen.value = true; }
function showEdit(row) { editItem.value = row; Object.assign(form, row); modalOpen.value = true; }
async function handleSave() { saving.value = true; await msgApi.templateSave(form); message.success('保存成功'); modalOpen.value = false; loadData(); saving.value = false; }
async function handleDelete(id) { await msgApi.templateDelete(id); message.success('删除成功'); loadData(); }
onMounted(loadData);
debugger; /* PartiallyEnd: #3632/scriptSetup.vue */
const __VLS_ctx = {};
let __VLS_components;
let __VLS_directives;
const __VLS_0 = {}.ACard;
/** @type {[typeof __VLS_components.ACard, typeof __VLS_components.aCard, typeof __VLS_components.ACard, typeof __VLS_components.aCard, ]} */ ;
// @ts-ignore
const __VLS_1 = __VLS_asFunctionalComponent(__VLS_0, new __VLS_0({
    title: "消息模板",
}));
const __VLS_2 = __VLS_1({
    title: "消息模板",
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
    rowKey: "templateId",
    pagination: ({ current: __VLS_ctx.data.pageNo, pageSize: __VLS_ctx.data.pageSize, total: __VLS_ctx.data.total, onChange: __VLS_ctx.onPageChange }),
}));
const __VLS_15 = __VLS_14({
    dataSource: (__VLS_ctx.data.list),
    columns: (__VLS_ctx.columns),
    loading: (__VLS_ctx.loading),
    rowKey: "templateId",
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
                    __VLS_ctx.handleDelete(record.templateId);
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
    open: (__VLS_ctx.modalOpen),
    title: (__VLS_ctx.editItem ? '编辑模板' : '新增模板'),
    confirmLoading: (__VLS_ctx.saving),
}));
const __VLS_23 = __VLS_22({
    ...{ 'onOk': {} },
    open: (__VLS_ctx.modalOpen),
    title: (__VLS_ctx.editItem ? '编辑模板' : '新增模板'),
    confirmLoading: (__VLS_ctx.saving),
}, ...__VLS_functionalComponentArgsRest(__VLS_22));
let __VLS_25;
let __VLS_26;
let __VLS_27;
const __VLS_28 = {
    onOk: (__VLS_ctx.handleSave)
};
__VLS_24.slots.default;
const __VLS_29 = {}.AForm;
/** @type {[typeof __VLS_components.AForm, typeof __VLS_components.aForm, typeof __VLS_components.AForm, typeof __VLS_components.aForm, ]} */ ;
// @ts-ignore
const __VLS_30 = __VLS_asFunctionalComponent(__VLS_29, new __VLS_29({
    model: (__VLS_ctx.form),
    layout: "vertical",
}));
const __VLS_31 = __VLS_30({
    model: (__VLS_ctx.form),
    layout: "vertical",
}, ...__VLS_functionalComponentArgsRest(__VLS_30));
__VLS_32.slots.default;
const __VLS_33 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_34 = __VLS_asFunctionalComponent(__VLS_33, new __VLS_33({
    label: "模板名称",
}));
const __VLS_35 = __VLS_34({
    label: "模板名称",
}, ...__VLS_functionalComponentArgsRest(__VLS_34));
__VLS_36.slots.default;
const __VLS_37 = {}.AInput;
/** @type {[typeof __VLS_components.AInput, typeof __VLS_components.aInput, ]} */ ;
// @ts-ignore
const __VLS_38 = __VLS_asFunctionalComponent(__VLS_37, new __VLS_37({
    value: (__VLS_ctx.form.templateName),
}));
const __VLS_39 = __VLS_38({
    value: (__VLS_ctx.form.templateName),
}, ...__VLS_functionalComponentArgsRest(__VLS_38));
var __VLS_36;
const __VLS_41 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_42 = __VLS_asFunctionalComponent(__VLS_41, new __VLS_41({
    label: "模板键",
}));
const __VLS_43 = __VLS_42({
    label: "模板键",
}, ...__VLS_functionalComponentArgsRest(__VLS_42));
__VLS_44.slots.default;
const __VLS_45 = {}.AInput;
/** @type {[typeof __VLS_components.AInput, typeof __VLS_components.aInput, ]} */ ;
// @ts-ignore
const __VLS_46 = __VLS_asFunctionalComponent(__VLS_45, new __VLS_45({
    value: (__VLS_ctx.form.templateKey),
}));
const __VLS_47 = __VLS_46({
    value: (__VLS_ctx.form.templateKey),
}, ...__VLS_functionalComponentArgsRest(__VLS_46));
var __VLS_44;
const __VLS_49 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_50 = __VLS_asFunctionalComponent(__VLS_49, new __VLS_49({
    label: "模板内容",
}));
const __VLS_51 = __VLS_50({
    label: "模板内容",
}, ...__VLS_functionalComponentArgsRest(__VLS_50));
__VLS_52.slots.default;
const __VLS_53 = {}.ATextarea;
/** @type {[typeof __VLS_components.ATextarea, typeof __VLS_components.aTextarea, ]} */ ;
// @ts-ignore
const __VLS_54 = __VLS_asFunctionalComponent(__VLS_53, new __VLS_53({
    value: (__VLS_ctx.form.templateContent),
    rows: "4",
}));
const __VLS_55 = __VLS_54({
    value: (__VLS_ctx.form.templateContent),
    rows: "4",
}, ...__VLS_functionalComponentArgsRest(__VLS_54));
var __VLS_52;
const __VLS_57 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_58 = __VLS_asFunctionalComponent(__VLS_57, new __VLS_57({
    label: "模板类型",
}));
const __VLS_59 = __VLS_58({
    label: "模板类型",
}, ...__VLS_functionalComponentArgsRest(__VLS_58));
__VLS_60.slots.default;
const __VLS_61 = {}.AInput;
/** @type {[typeof __VLS_components.AInput, typeof __VLS_components.aInput, ]} */ ;
// @ts-ignore
const __VLS_62 = __VLS_asFunctionalComponent(__VLS_61, new __VLS_61({
    value: (__VLS_ctx.form.templateType),
}));
const __VLS_63 = __VLS_62({
    value: (__VLS_ctx.form.templateType),
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
            saving: saving,
            modalOpen: modalOpen,
            editItem: editItem,
            data: data,
            form: form,
            columns: columns,
            onPageChange: onPageChange,
            showAdd: showAdd,
            showEdit: showEdit,
            handleSave: handleSave,
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
