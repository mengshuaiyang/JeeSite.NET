import { ref, reactive, onMounted } from 'vue';
import { appFeedbackApi } from '@/api';
import { message } from 'ant-design-vue';
const loading = ref(false);
const saving = ref(false);
const modalOpen = ref(false);
const list = ref([]);
const replyForm = reactive({ id: '', replyContent: '' });
const columns = [
    { title: '分类', dataIndex: 'category' }, { title: '内容', dataIndex: 'content' },
    { title: '联系方式', dataIndex: 'contact' }, { title: '回复', dataIndex: 'replyContent' },
    { title: '状态', dataIndex: 'status' }, { title: '操作', key: 'action' }
];
async function loadData() {
    loading.value = true;
    const res = await appFeedbackApi.commentList();
    if (res.data)
        list.value = res.data;
    loading.value = false;
}
function openReply(row) { replyForm.id = row.id; replyForm.replyContent = row.replyContent || ''; modalOpen.value = true; }
async function handleReply() { saving.value = true; await appFeedbackApi.commentReply(replyForm.id, replyForm.replyContent); message.success('回复成功'); modalOpen.value = false; loadData(); saving.value = false; }
async function handleDelete(id) { await appFeedbackApi.commentDelete(id); message.success('删除成功'); loadData(); }
onMounted(loadData);
debugger; /* PartiallyEnd: #3632/scriptSetup.vue */
const __VLS_ctx = {};
let __VLS_components;
let __VLS_directives;
const __VLS_0 = {}.ACard;
/** @type {[typeof __VLS_components.ACard, typeof __VLS_components.aCard, typeof __VLS_components.ACard, typeof __VLS_components.aCard, ]} */ ;
// @ts-ignore
const __VLS_1 = __VLS_asFunctionalComponent(__VLS_0, new __VLS_0({
    title: "APP评论管理",
}));
const __VLS_2 = __VLS_1({
    title: "APP评论管理",
}, ...__VLS_functionalComponentArgsRest(__VLS_1));
var __VLS_4 = {};
__VLS_3.slots.default;
const __VLS_5 = {}.ATable;
/** @type {[typeof __VLS_components.ATable, typeof __VLS_components.aTable, typeof __VLS_components.ATable, typeof __VLS_components.aTable, ]} */ ;
// @ts-ignore
const __VLS_6 = __VLS_asFunctionalComponent(__VLS_5, new __VLS_5({
    dataSource: (__VLS_ctx.list),
    columns: (__VLS_ctx.columns),
    loading: (__VLS_ctx.loading),
    rowKey: "id",
}));
const __VLS_7 = __VLS_6({
    dataSource: (__VLS_ctx.list),
    columns: (__VLS_ctx.columns),
    loading: (__VLS_ctx.loading),
    rowKey: "id",
}, ...__VLS_functionalComponentArgsRest(__VLS_6));
__VLS_8.slots.default;
{
    const { bodyCell: __VLS_thisSlot } = __VLS_8.slots;
    const [{ record, column }] = __VLS_getSlotParams(__VLS_thisSlot);
    if (column.key === 'action') {
        const __VLS_9 = {}.ASpace;
        /** @type {[typeof __VLS_components.ASpace, typeof __VLS_components.aSpace, typeof __VLS_components.ASpace, typeof __VLS_components.aSpace, ]} */ ;
        // @ts-ignore
        const __VLS_10 = __VLS_asFunctionalComponent(__VLS_9, new __VLS_9({}));
        const __VLS_11 = __VLS_10({}, ...__VLS_functionalComponentArgsRest(__VLS_10));
        __VLS_12.slots.default;
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
                    __VLS_ctx.handleDelete(record.id);
                } },
        });
        var __VLS_12;
    }
}
var __VLS_8;
const __VLS_13 = {}.AModal;
/** @type {[typeof __VLS_components.AModal, typeof __VLS_components.aModal, typeof __VLS_components.AModal, typeof __VLS_components.aModal, ]} */ ;
// @ts-ignore
const __VLS_14 = __VLS_asFunctionalComponent(__VLS_13, new __VLS_13({
    ...{ 'onOk': {} },
    open: (__VLS_ctx.modalOpen),
    title: "回复评论",
    confirmLoading: (__VLS_ctx.saving),
}));
const __VLS_15 = __VLS_14({
    ...{ 'onOk': {} },
    open: (__VLS_ctx.modalOpen),
    title: "回复评论",
    confirmLoading: (__VLS_ctx.saving),
}, ...__VLS_functionalComponentArgsRest(__VLS_14));
let __VLS_17;
let __VLS_18;
let __VLS_19;
const __VLS_20 = {
    onOk: (__VLS_ctx.handleReply)
};
__VLS_16.slots.default;
const __VLS_21 = {}.AForm;
/** @type {[typeof __VLS_components.AForm, typeof __VLS_components.aForm, typeof __VLS_components.AForm, typeof __VLS_components.aForm, ]} */ ;
// @ts-ignore
const __VLS_22 = __VLS_asFunctionalComponent(__VLS_21, new __VLS_21({
    model: (__VLS_ctx.replyForm),
    layout: "vertical",
}));
const __VLS_23 = __VLS_22({
    model: (__VLS_ctx.replyForm),
    layout: "vertical",
}, ...__VLS_functionalComponentArgsRest(__VLS_22));
__VLS_24.slots.default;
const __VLS_25 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_26 = __VLS_asFunctionalComponent(__VLS_25, new __VLS_25({
    label: "回复内容",
}));
const __VLS_27 = __VLS_26({
    label: "回复内容",
}, ...__VLS_functionalComponentArgsRest(__VLS_26));
__VLS_28.slots.default;
const __VLS_29 = {}.ATextarea;
/** @type {[typeof __VLS_components.ATextarea, typeof __VLS_components.aTextarea, ]} */ ;
// @ts-ignore
const __VLS_30 = __VLS_asFunctionalComponent(__VLS_29, new __VLS_29({
    value: (__VLS_ctx.replyForm.replyContent),
    rows: "4",
}));
const __VLS_31 = __VLS_30({
    value: (__VLS_ctx.replyForm.replyContent),
    rows: "4",
}, ...__VLS_functionalComponentArgsRest(__VLS_30));
var __VLS_28;
var __VLS_24;
var __VLS_16;
var __VLS_3;
var __VLS_dollars;
const __VLS_self = (await import('vue')).defineComponent({
    setup() {
        return {
            loading: loading,
            saving: saving,
            modalOpen: modalOpen,
            list: list,
            replyForm: replyForm,
            columns: columns,
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
