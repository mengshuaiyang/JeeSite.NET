import { ref, reactive, onMounted } from 'vue';
import { logApi } from '@/api';
const loading = ref(false);
const data = reactive({ list: [], total: 0, pageNo: 1, pageSize: 20 });
const query = reactive({ logType: '' });
const columns = [{ title: '标题', dataIndex: 'logTitle' }, { title: '类型', dataIndex: 'logType' }, { title: '方法', dataIndex: 'requestMethod' }, { title: '耗时(ms)', dataIndex: 'executeTime' }, { title: '用户', dataIndex: 'userName' }, { title: 'IP', dataIndex: 'remoteIp' }, { title: '时间', dataIndex: 'createDate' }];
async function loadData() { loading.value = true; const r = await logApi.list({ pageNo: data.pageNo, pageSize: data.pageSize, entity: { logType: query.logType } }); if (r.data) {
    data.list = r.data.list;
    data.total = r.data.total;
} loading.value = false; }
function onPageChange(p, s) { data.pageNo = p; data.pageSize = s; loadData(); }
onMounted(loadData);
debugger; /* PartiallyEnd: #3632/scriptSetup.vue */
const __VLS_ctx = {};
let __VLS_components;
let __VLS_directives;
const __VLS_0 = {}.ACard;
/** @type {[typeof __VLS_components.ACard, typeof __VLS_components.aCard, typeof __VLS_components.ACard, typeof __VLS_components.aCard, ]} */ ;
// @ts-ignore
const __VLS_1 = __VLS_asFunctionalComponent(__VLS_0, new __VLS_0({
    title: "操作日志",
}));
const __VLS_2 = __VLS_1({
    title: "操作日志",
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
    label: "类型",
}));
const __VLS_11 = __VLS_10({
    label: "类型",
}, ...__VLS_functionalComponentArgsRest(__VLS_10));
__VLS_12.slots.default;
const __VLS_13 = {}.AInput;
/** @type {[typeof __VLS_components.AInput, typeof __VLS_components.aInput, ]} */ ;
// @ts-ignore
const __VLS_14 = __VLS_asFunctionalComponent(__VLS_13, new __VLS_13({
    value: (__VLS_ctx.query.logType),
    placeholder: "access/error",
}));
const __VLS_15 = __VLS_14({
    value: (__VLS_ctx.query.logType),
    placeholder: "access/error",
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
/** @type {[typeof __VLS_components.ATable, typeof __VLS_components.aTable, ]} */ ;
// @ts-ignore
const __VLS_30 = __VLS_asFunctionalComponent(__VLS_29, new __VLS_29({
    dataSource: (__VLS_ctx.data.list),
    columns: (__VLS_ctx.columns),
    loading: (__VLS_ctx.loading),
    rowKey: "logId",
    pagination: ({ current: __VLS_ctx.data.pageNo, pageSize: __VLS_ctx.data.pageSize, total: __VLS_ctx.data.total, onChange: __VLS_ctx.onPageChange }),
}));
const __VLS_31 = __VLS_30({
    dataSource: (__VLS_ctx.data.list),
    columns: (__VLS_ctx.columns),
    loading: (__VLS_ctx.loading),
    rowKey: "logId",
    pagination: ({ current: __VLS_ctx.data.pageNo, pageSize: __VLS_ctx.data.pageSize, total: __VLS_ctx.data.total, onChange: __VLS_ctx.onPageChange }),
}, ...__VLS_functionalComponentArgsRest(__VLS_30));
var __VLS_3;
var __VLS_dollars;
const __VLS_self = (await import('vue')).defineComponent({
    setup() {
        return {
            loading: loading,
            data: data,
            query: query,
            columns: columns,
            loadData: loadData,
            onPageChange: onPageChange,
        };
    },
});
export default (await import('vue')).defineComponent({
    setup() {
        return {};
    },
});
; /* PartiallyEnd: #4569/main.vue */
