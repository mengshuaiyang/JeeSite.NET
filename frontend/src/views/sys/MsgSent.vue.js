import { ref, reactive, onMounted } from 'vue';
import { msgApi } from '@/api';
import { message } from 'ant-design-vue';
const loading = ref(false);
const saving = ref(false);
const modalOpen = ref(false);
const data = reactive({ list: [], total: 0, pageNo: 1, pageSize: 20 });
const form = reactive({ msgTitle: '', msgContent: '', receiveType: 'all', receiveCodes: '' });
const columns = [
    { title: '标题', dataIndex: 'msgTitle' }, { title: '接收类型', key: 'receiveType' },
    { title: '时间', dataIndex: 'createDate' }, { title: '操作', key: 'action' }
];
async function loadData() {
    loading.value = true;
    const res = await msgApi.sent({ pageNo: data.pageNo, pageSize: data.pageSize, entity: {} });
    if (res.data) {
        data.list = res.data.list;
        data.total = res.data.total;
    }
    loading.value = false;
}
function onPageChange(page, size) { data.pageNo = page; data.pageSize = size; loadData(); }
function showSend() { form.msgTitle = ''; form.msgContent = ''; form.receiveType = 'all'; form.receiveCodes = ''; modalOpen.value = true; }
async function handleSend() { saving.value = true; await msgApi.send(form); message.success('发送成功'); modalOpen.value = false; loadData(); saving.value = false; }
async function handleDelete(msgId) { await msgApi.delete(msgId); message.success('删除成功'); loadData(); }
onMounted(loadData);
debugger; /* PartiallyEnd: #3632/scriptSetup.vue */
const __VLS_ctx = {};
let __VLS_components;
let __VLS_directives;
const __VLS_0 = {}.ACard;
/** @type {[typeof __VLS_components.ACard, typeof __VLS_components.aCard, typeof __VLS_components.ACard, typeof __VLS_components.aCard, ]} */ ;
// @ts-ignore
const __VLS_1 = __VLS_asFunctionalComponent(__VLS_0, new __VLS_0({
    title: "已发送",
}));
const __VLS_2 = __VLS_1({
    title: "已发送",
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
    onClick: (__VLS_ctx.showSend)
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
    rowKey: "msgId",
    pagination: ({ current: __VLS_ctx.data.pageNo, pageSize: __VLS_ctx.data.pageSize, total: __VLS_ctx.data.total, onChange: __VLS_ctx.onPageChange }),
}));
const __VLS_15 = __VLS_14({
    dataSource: (__VLS_ctx.data.list),
    columns: (__VLS_ctx.columns),
    loading: (__VLS_ctx.loading),
    rowKey: "msgId",
    pagination: ({ current: __VLS_ctx.data.pageNo, pageSize: __VLS_ctx.data.pageSize, total: __VLS_ctx.data.total, onChange: __VLS_ctx.onPageChange }),
}, ...__VLS_functionalComponentArgsRest(__VLS_14));
__VLS_16.slots.default;
{
    const { bodyCell: __VLS_thisSlot } = __VLS_16.slots;
    const [{ record, column }] = __VLS_getSlotParams(__VLS_thisSlot);
    if (column.key === 'receiveType') {
        const __VLS_17 = {}.ATag;
        /** @type {[typeof __VLS_components.ATag, typeof __VLS_components.aTag, typeof __VLS_components.ATag, typeof __VLS_components.aTag, ]} */ ;
        // @ts-ignore
        const __VLS_18 = __VLS_asFunctionalComponent(__VLS_17, new __VLS_17({}));
        const __VLS_19 = __VLS_18({}, ...__VLS_functionalComponentArgsRest(__VLS_18));
        __VLS_20.slots.default;
        (record.receiveType);
        var __VLS_20;
    }
    if (column.key === 'action') {
        const __VLS_21 = {}.ASpace;
        /** @type {[typeof __VLS_components.ASpace, typeof __VLS_components.aSpace, typeof __VLS_components.ASpace, typeof __VLS_components.aSpace, ]} */ ;
        // @ts-ignore
        const __VLS_22 = __VLS_asFunctionalComponent(__VLS_21, new __VLS_21({}));
        const __VLS_23 = __VLS_22({}, ...__VLS_functionalComponentArgsRest(__VLS_22));
        __VLS_24.slots.default;
        __VLS_asFunctionalElement(__VLS_intrinsicElements.a, __VLS_intrinsicElements.a)({
            ...{ onClick: (...[$event]) => {
                    if (!(column.key === 'action'))
                        return;
                    __VLS_ctx.handleDelete(record.msgId);
                } },
        });
        var __VLS_24;
    }
}
var __VLS_16;
const __VLS_25 = {}.AModal;
/** @type {[typeof __VLS_components.AModal, typeof __VLS_components.aModal, typeof __VLS_components.AModal, typeof __VLS_components.aModal, ]} */ ;
// @ts-ignore
const __VLS_26 = __VLS_asFunctionalComponent(__VLS_25, new __VLS_25({
    ...{ 'onOk': {} },
    open: (__VLS_ctx.modalOpen),
    title: "发送消息",
    confirmLoading: (__VLS_ctx.saving),
}));
const __VLS_27 = __VLS_26({
    ...{ 'onOk': {} },
    open: (__VLS_ctx.modalOpen),
    title: "发送消息",
    confirmLoading: (__VLS_ctx.saving),
}, ...__VLS_functionalComponentArgsRest(__VLS_26));
let __VLS_29;
let __VLS_30;
let __VLS_31;
const __VLS_32 = {
    onOk: (__VLS_ctx.handleSend)
};
__VLS_28.slots.default;
const __VLS_33 = {}.AForm;
/** @type {[typeof __VLS_components.AForm, typeof __VLS_components.aForm, typeof __VLS_components.AForm, typeof __VLS_components.aForm, ]} */ ;
// @ts-ignore
const __VLS_34 = __VLS_asFunctionalComponent(__VLS_33, new __VLS_33({
    model: (__VLS_ctx.form),
    layout: "vertical",
}));
const __VLS_35 = __VLS_34({
    model: (__VLS_ctx.form),
    layout: "vertical",
}, ...__VLS_functionalComponentArgsRest(__VLS_34));
__VLS_36.slots.default;
const __VLS_37 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_38 = __VLS_asFunctionalComponent(__VLS_37, new __VLS_37({
    label: "标题",
}));
const __VLS_39 = __VLS_38({
    label: "标题",
}, ...__VLS_functionalComponentArgsRest(__VLS_38));
__VLS_40.slots.default;
const __VLS_41 = {}.AInput;
/** @type {[typeof __VLS_components.AInput, typeof __VLS_components.aInput, ]} */ ;
// @ts-ignore
const __VLS_42 = __VLS_asFunctionalComponent(__VLS_41, new __VLS_41({
    value: (__VLS_ctx.form.msgTitle),
}));
const __VLS_43 = __VLS_42({
    value: (__VLS_ctx.form.msgTitle),
}, ...__VLS_functionalComponentArgsRest(__VLS_42));
var __VLS_40;
const __VLS_45 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_46 = __VLS_asFunctionalComponent(__VLS_45, new __VLS_45({
    label: "内容",
}));
const __VLS_47 = __VLS_46({
    label: "内容",
}, ...__VLS_functionalComponentArgsRest(__VLS_46));
__VLS_48.slots.default;
const __VLS_49 = {}.ATextarea;
/** @type {[typeof __VLS_components.ATextarea, typeof __VLS_components.aTextarea, ]} */ ;
// @ts-ignore
const __VLS_50 = __VLS_asFunctionalComponent(__VLS_49, new __VLS_49({
    value: (__VLS_ctx.form.msgContent),
}));
const __VLS_51 = __VLS_50({
    value: (__VLS_ctx.form.msgContent),
}, ...__VLS_functionalComponentArgsRest(__VLS_50));
var __VLS_48;
const __VLS_53 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_54 = __VLS_asFunctionalComponent(__VLS_53, new __VLS_53({
    label: "接收类型",
}));
const __VLS_55 = __VLS_54({
    label: "接收类型",
}, ...__VLS_functionalComponentArgsRest(__VLS_54));
__VLS_56.slots.default;
const __VLS_57 = {}.ASelect;
/** @type {[typeof __VLS_components.ASelect, typeof __VLS_components.aSelect, typeof __VLS_components.ASelect, typeof __VLS_components.aSelect, ]} */ ;
// @ts-ignore
const __VLS_58 = __VLS_asFunctionalComponent(__VLS_57, new __VLS_57({
    value: (__VLS_ctx.form.receiveType),
}));
const __VLS_59 = __VLS_58({
    value: (__VLS_ctx.form.receiveType),
}, ...__VLS_functionalComponentArgsRest(__VLS_58));
__VLS_60.slots.default;
const __VLS_61 = {}.ASelectOption;
/** @type {[typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, ]} */ ;
// @ts-ignore
const __VLS_62 = __VLS_asFunctionalComponent(__VLS_61, new __VLS_61({
    value: "all",
}));
const __VLS_63 = __VLS_62({
    value: "all",
}, ...__VLS_functionalComponentArgsRest(__VLS_62));
__VLS_64.slots.default;
var __VLS_64;
const __VLS_65 = {}.ASelectOption;
/** @type {[typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, ]} */ ;
// @ts-ignore
const __VLS_66 = __VLS_asFunctionalComponent(__VLS_65, new __VLS_65({
    value: "user",
}));
const __VLS_67 = __VLS_66({
    value: "user",
}, ...__VLS_functionalComponentArgsRest(__VLS_66));
__VLS_68.slots.default;
var __VLS_68;
const __VLS_69 = {}.ASelectOption;
/** @type {[typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, ]} */ ;
// @ts-ignore
const __VLS_70 = __VLS_asFunctionalComponent(__VLS_69, new __VLS_69({
    value: "role",
}));
const __VLS_71 = __VLS_70({
    value: "role",
}, ...__VLS_functionalComponentArgsRest(__VLS_70));
__VLS_72.slots.default;
var __VLS_72;
const __VLS_73 = {}.ASelectOption;
/** @type {[typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, ]} */ ;
// @ts-ignore
const __VLS_74 = __VLS_asFunctionalComponent(__VLS_73, new __VLS_73({
    value: "office",
}));
const __VLS_75 = __VLS_74({
    value: "office",
}, ...__VLS_functionalComponentArgsRest(__VLS_74));
__VLS_76.slots.default;
var __VLS_76;
var __VLS_60;
var __VLS_56;
const __VLS_77 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_78 = __VLS_asFunctionalComponent(__VLS_77, new __VLS_77({
    label: "接收编码",
}));
const __VLS_79 = __VLS_78({
    label: "接收编码",
}, ...__VLS_functionalComponentArgsRest(__VLS_78));
__VLS_80.slots.default;
const __VLS_81 = {}.AInput;
/** @type {[typeof __VLS_components.AInput, typeof __VLS_components.aInput, ]} */ ;
// @ts-ignore
const __VLS_82 = __VLS_asFunctionalComponent(__VLS_81, new __VLS_81({
    value: (__VLS_ctx.form.receiveCodes),
    placeholder: "多个用逗号分隔",
}));
const __VLS_83 = __VLS_82({
    value: (__VLS_ctx.form.receiveCodes),
    placeholder: "多个用逗号分隔",
}, ...__VLS_functionalComponentArgsRest(__VLS_82));
var __VLS_80;
var __VLS_36;
var __VLS_28;
var __VLS_3;
var __VLS_dollars;
const __VLS_self = (await import('vue')).defineComponent({
    setup() {
        return {
            loading: loading,
            saving: saving,
            modalOpen: modalOpen,
            data: data,
            form: form,
            columns: columns,
            onPageChange: onPageChange,
            showSend: showSend,
            handleSend: handleSend,
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
