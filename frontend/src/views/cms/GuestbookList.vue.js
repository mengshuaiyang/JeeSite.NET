import { ref, reactive, onMounted } from 'vue';
import { guestbookApi } from '@/api';
import { message } from 'ant-design-vue';
const loading = ref(false);
const saving = ref(false);
const modalOpen = ref(false);
const data = reactive({ list: [], total: 0, pageNo: 1, pageSize: 20 });
const query = reactive({ name: '' });
const replyForm = reactive({ gbCode: '', reContent: '' });
const columns = [
    { title: '类型', dataIndex: 'gbType' }, { title: '内容', dataIndex: 'content' },
    { title: '姓名', dataIndex: 'name' }, { title: '邮箱', dataIndex: 'email' },
    { title: '状态', dataIndex: 'status' }, { title: '时间', dataIndex: 'createDate' },
    { title: '操作', key: 'action' }
];
async function loadData() {
    loading.value = true;
    const res = await guestbookApi.list({ pageNo: data.pageNo, pageSize: data.pageSize, entity: { name: query.name } });
    if (res.data) {
        data.list = res.data.list;
        data.total = res.data.total;
    }
    loading.value = false;
}
function onPageChange(page, size) { data.pageNo = page; data.pageSize = size; loadData(); }
function openReply(row) { replyForm.gbCode = row.gbCode; replyForm.reContent = row.reContent || ''; modalOpen.value = true; }
async function handleReply() { saving.value = true; await guestbookApi.reply(replyForm.gbCode, replyForm.reContent); message.success('回复成功'); modalOpen.value = false; loadData(); saving.value = false; }
async function handleDelete(code) { await guestbookApi.delete(code); message.success('删除成功'); loadData(); }
onMounted(loadData);
debugger; /* PartiallyEnd: #3632/scriptSetup.vue */
const __VLS_ctx = {};
let __VLS_components;
let __VLS_directives;
const __VLS_0 = {}.ACard;
/** @type {[typeof __VLS_components.ACard, typeof __VLS_components.aCard, typeof __VLS_components.ACard, typeof __VLS_components.aCard, ]} */ ;
// @ts-ignore
const __VLS_1 = __VLS_asFunctionalComponent(__VLS_0, new __VLS_0({
    title: "留言管理",
}));
const __VLS_2 = __VLS_1({
    title: "留言管理",
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
    label: "姓名",
}));
const __VLS_11 = __VLS_10({
    label: "姓名",
}, ...__VLS_functionalComponentArgsRest(__VLS_10));
__VLS_12.slots.default;
const __VLS_13 = {}.AInput;
/** @type {[typeof __VLS_components.AInput, typeof __VLS_components.aInput, ]} */ ;
// @ts-ignore
const __VLS_14 = __VLS_asFunctionalComponent(__VLS_13, new __VLS_13({
    value: (__VLS_ctx.query.name),
    placeholder: "搜索",
}));
const __VLS_15 = __VLS_14({
    value: (__VLS_ctx.query.name),
    placeholder: "搜索",
}, ...__VLS_functionalComponentArgsRest(__VLS_14));
var __VLS_12;
const __VLS_17 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_18 = __VLS_asFunctionalComponent(__VLS_17, new __VLS_17({}));
const __VLS_19 = __VLS_18({}, ...__VLS_functionalComponentArgsRest(__VLS_18));
__VLS_20.slots.default;
const __VLS_21 = {}.AButton;
/** @type {[typeof __VLS_components.AButton, typeof __VLS_components.aButton, typeof __VLS_components.AButton, typeof __VLS_components.aButton, ]} */ ;
// @ts-ignore
const __VLS_22 = __VLS_asFunctionalComponent(__VLS_21, new __VLS_21({
    ...{ 'onClick': {} },
    type: "primary",
}));
const __VLS_23 = __VLS_22({
    ...{ 'onClick': {} },
    type: "primary",
}, ...__VLS_functionalComponentArgsRest(__VLS_22));
let __VLS_25;
let __VLS_26;
let __VLS_27;
const __VLS_28 = {
    onClick: (__VLS_ctx.loadData)
};
__VLS_24.slots.default;
var __VLS_24;
var __VLS_20;
var __VLS_8;
const __VLS_29 = {}.ATable;
/** @type {[typeof __VLS_components.ATable, typeof __VLS_components.aTable, typeof __VLS_components.ATable, typeof __VLS_components.aTable, ]} */ ;
// @ts-ignore
const __VLS_30 = __VLS_asFunctionalComponent(__VLS_29, new __VLS_29({
    dataSource: (__VLS_ctx.data.list),
    columns: (__VLS_ctx.columns),
    loading: (__VLS_ctx.loading),
    rowKey: "gbCode",
    pagination: ({ current: __VLS_ctx.data.pageNo, pageSize: __VLS_ctx.data.pageSize, total: __VLS_ctx.data.total, onChange: __VLS_ctx.onPageChange }),
}));
const __VLS_31 = __VLS_30({
    dataSource: (__VLS_ctx.data.list),
    columns: (__VLS_ctx.columns),
    loading: (__VLS_ctx.loading),
    rowKey: "gbCode",
    pagination: ({ current: __VLS_ctx.data.pageNo, pageSize: __VLS_ctx.data.pageSize, total: __VLS_ctx.data.total, onChange: __VLS_ctx.onPageChange }),
}, ...__VLS_functionalComponentArgsRest(__VLS_30));
__VLS_32.slots.default;
{
    const { bodyCell: __VLS_thisSlot } = __VLS_32.slots;
    const [{ record, column }] = __VLS_getSlotParams(__VLS_thisSlot);
    if (column.key === 'action') {
        const __VLS_33 = {}.ASpace;
        /** @type {[typeof __VLS_components.ASpace, typeof __VLS_components.aSpace, typeof __VLS_components.ASpace, typeof __VLS_components.aSpace, ]} */ ;
        // @ts-ignore
        const __VLS_34 = __VLS_asFunctionalComponent(__VLS_33, new __VLS_33({}));
        const __VLS_35 = __VLS_34({}, ...__VLS_functionalComponentArgsRest(__VLS_34));
        __VLS_36.slots.default;
        __VLS_asFunctionalElement(__VLS_intrinsicElements.a, __VLS_intrinsicElements.a)({
            ...{ onClick: (...[$event]) => {
                    if (!(column.key === 'action'))
                        return;
                    __VLS_ctx.openReply(record);
                } },
        });
        __VLS_asFunctionalElement(__VLS_intrinsicElements.a, __VLS_intrinsicElements.a)({
            ...{ onClick: (...[$event]) => {
                    if (!(column.key === 'action'))
                        return;
                    __VLS_ctx.handleDelete(record.gbCode);
                } },
        });
        var __VLS_36;
    }
}
var __VLS_32;
const __VLS_37 = {}.AModal;
/** @type {[typeof __VLS_components.AModal, typeof __VLS_components.aModal, typeof __VLS_components.AModal, typeof __VLS_components.aModal, ]} */ ;
// @ts-ignore
const __VLS_38 = __VLS_asFunctionalComponent(__VLS_37, new __VLS_37({
    ...{ 'onOk': {} },
    open: (__VLS_ctx.modalOpen),
    title: "回复留言",
    confirmLoading: (__VLS_ctx.saving),
}));
const __VLS_39 = __VLS_38({
    ...{ 'onOk': {} },
    open: (__VLS_ctx.modalOpen),
    title: "回复留言",
    confirmLoading: (__VLS_ctx.saving),
}, ...__VLS_functionalComponentArgsRest(__VLS_38));
let __VLS_41;
let __VLS_42;
let __VLS_43;
const __VLS_44 = {
    onOk: (__VLS_ctx.handleReply)
};
__VLS_40.slots.default;
const __VLS_45 = {}.AForm;
/** @type {[typeof __VLS_components.AForm, typeof __VLS_components.aForm, typeof __VLS_components.AForm, typeof __VLS_components.aForm, ]} */ ;
// @ts-ignore
const __VLS_46 = __VLS_asFunctionalComponent(__VLS_45, new __VLS_45({
    model: (__VLS_ctx.replyForm),
    layout: "vertical",
}));
const __VLS_47 = __VLS_46({
    model: (__VLS_ctx.replyForm),
    layout: "vertical",
}, ...__VLS_functionalComponentArgsRest(__VLS_46));
__VLS_48.slots.default;
const __VLS_49 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_50 = __VLS_asFunctionalComponent(__VLS_49, new __VLS_49({
    label: "回复内容",
}));
const __VLS_51 = __VLS_50({
    label: "回复内容",
}, ...__VLS_functionalComponentArgsRest(__VLS_50));
__VLS_52.slots.default;
const __VLS_53 = {}.ATextarea;
/** @type {[typeof __VLS_components.ATextarea, typeof __VLS_components.aTextarea, ]} */ ;
// @ts-ignore
const __VLS_54 = __VLS_asFunctionalComponent(__VLS_53, new __VLS_53({
    value: (__VLS_ctx.replyForm.reContent),
    rows: "4",
}));
const __VLS_55 = __VLS_54({
    value: (__VLS_ctx.replyForm.reContent),
    rows: "4",
}, ...__VLS_functionalComponentArgsRest(__VLS_54));
var __VLS_52;
var __VLS_48;
var __VLS_40;
var __VLS_3;
var __VLS_dollars;
const __VLS_self = (await import('vue')).defineComponent({
    setup() {
        return {
            loading: loading,
            saving: saving,
            modalOpen: modalOpen,
            data: data,
            query: query,
            replyForm: replyForm,
            columns: columns,
            loadData: loadData,
            onPageChange: onPageChange,
            openReply: openReply,
            handleReply: handleReply,
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
