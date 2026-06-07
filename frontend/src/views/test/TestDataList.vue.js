import { ref, reactive, onMounted } from 'vue';
import { testDataApi } from '@/api';
import { message } from 'ant-design-vue';
const loading = ref(false);
const saving = ref(false);
const modalOpen = ref(false);
const editItem = ref(null);
const list = ref([]);
const form = reactive({ testInput: '', testTextarea: '', testSelect: '', testRadio: '', testDate: '' });
const columns = [
    { title: '文本框', dataIndex: 'testInput' }, { title: '下拉框', dataIndex: 'testSelect' },
    { title: '单选框', dataIndex: 'testRadio' }, { title: '日期', dataIndex: 'testDate' },
    { title: '状态', dataIndex: 'status' }, { title: '操作', key: 'action' }
];
async function loadData() {
    loading.value = true;
    const res = await testDataApi.list();
    if (res.data)
        list.value = res.data;
    loading.value = false;
}
function showAdd() { editItem.value = null; form.testInput = ''; form.testTextarea = ''; form.testSelect = ''; form.testRadio = ''; form.testDate = ''; modalOpen.value = true; }
function showEdit(row) { editItem.value = row; Object.assign(form, row); modalOpen.value = true; }
async function handleSave() { saving.value = true; await testDataApi.save(form); message.success('保存成功'); modalOpen.value = false; loadData(); saving.value = false; }
async function handleDelete(id) { await testDataApi.delete(id); message.success('删除成功'); loadData(); }
onMounted(loadData);
debugger; /* PartiallyEnd: #3632/scriptSetup.vue */
const __VLS_ctx = {};
let __VLS_components;
let __VLS_directives;
const __VLS_0 = {}.ACard;
/** @type {[typeof __VLS_components.ACard, typeof __VLS_components.aCard, typeof __VLS_components.ACard, typeof __VLS_components.aCard, ]} */ ;
// @ts-ignore
const __VLS_1 = __VLS_asFunctionalComponent(__VLS_0, new __VLS_0({
    title: "测试数据管理",
}));
const __VLS_2 = __VLS_1({
    title: "测试数据管理",
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
    dataSource: (__VLS_ctx.list),
    columns: (__VLS_ctx.columns),
    loading: (__VLS_ctx.loading),
    rowKey: "id",
}));
const __VLS_15 = __VLS_14({
    dataSource: (__VLS_ctx.list),
    columns: (__VLS_ctx.columns),
    loading: (__VLS_ctx.loading),
    rowKey: "id",
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
                    __VLS_ctx.handleDelete(record.id);
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
    title: (__VLS_ctx.editItem ? '编辑测试数据' : '新增测试数据'),
    confirmLoading: (__VLS_ctx.saving),
    width: "600",
}));
const __VLS_23 = __VLS_22({
    ...{ 'onOk': {} },
    open: (__VLS_ctx.modalOpen),
    title: (__VLS_ctx.editItem ? '编辑测试数据' : '新增测试数据'),
    confirmLoading: (__VLS_ctx.saving),
    width: "600",
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
    label: "文本框",
}));
const __VLS_35 = __VLS_34({
    label: "文本框",
}, ...__VLS_functionalComponentArgsRest(__VLS_34));
__VLS_36.slots.default;
const __VLS_37 = {}.AInput;
/** @type {[typeof __VLS_components.AInput, typeof __VLS_components.aInput, ]} */ ;
// @ts-ignore
const __VLS_38 = __VLS_asFunctionalComponent(__VLS_37, new __VLS_37({
    value: (__VLS_ctx.form.testInput),
}));
const __VLS_39 = __VLS_38({
    value: (__VLS_ctx.form.testInput),
}, ...__VLS_functionalComponentArgsRest(__VLS_38));
var __VLS_36;
const __VLS_41 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_42 = __VLS_asFunctionalComponent(__VLS_41, new __VLS_41({
    label: "文本域",
}));
const __VLS_43 = __VLS_42({
    label: "文本域",
}, ...__VLS_functionalComponentArgsRest(__VLS_42));
__VLS_44.slots.default;
const __VLS_45 = {}.ATextarea;
/** @type {[typeof __VLS_components.ATextarea, typeof __VLS_components.aTextarea, ]} */ ;
// @ts-ignore
const __VLS_46 = __VLS_asFunctionalComponent(__VLS_45, new __VLS_45({
    value: (__VLS_ctx.form.testTextarea),
    rows: "4",
}));
const __VLS_47 = __VLS_46({
    value: (__VLS_ctx.form.testTextarea),
    rows: "4",
}, ...__VLS_functionalComponentArgsRest(__VLS_46));
var __VLS_44;
const __VLS_49 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_50 = __VLS_asFunctionalComponent(__VLS_49, new __VLS_49({
    label: "下拉框",
}));
const __VLS_51 = __VLS_50({
    label: "下拉框",
}, ...__VLS_functionalComponentArgsRest(__VLS_50));
__VLS_52.slots.default;
const __VLS_53 = {}.ASelect;
/** @type {[typeof __VLS_components.ASelect, typeof __VLS_components.aSelect, typeof __VLS_components.ASelect, typeof __VLS_components.aSelect, ]} */ ;
// @ts-ignore
const __VLS_54 = __VLS_asFunctionalComponent(__VLS_53, new __VLS_53({
    value: (__VLS_ctx.form.testSelect),
    ...{ style: {} },
}));
const __VLS_55 = __VLS_54({
    value: (__VLS_ctx.form.testSelect),
    ...{ style: {} },
}, ...__VLS_functionalComponentArgsRest(__VLS_54));
__VLS_56.slots.default;
const __VLS_57 = {}.ASelectOption;
/** @type {[typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, ]} */ ;
// @ts-ignore
const __VLS_58 = __VLS_asFunctionalComponent(__VLS_57, new __VLS_57({
    value: "option1",
}));
const __VLS_59 = __VLS_58({
    value: "option1",
}, ...__VLS_functionalComponentArgsRest(__VLS_58));
__VLS_60.slots.default;
var __VLS_60;
const __VLS_61 = {}.ASelectOption;
/** @type {[typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, ]} */ ;
// @ts-ignore
const __VLS_62 = __VLS_asFunctionalComponent(__VLS_61, new __VLS_61({
    value: "option2",
}));
const __VLS_63 = __VLS_62({
    value: "option2",
}, ...__VLS_functionalComponentArgsRest(__VLS_62));
__VLS_64.slots.default;
var __VLS_64;
const __VLS_65 = {}.ASelectOption;
/** @type {[typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, ]} */ ;
// @ts-ignore
const __VLS_66 = __VLS_asFunctionalComponent(__VLS_65, new __VLS_65({
    value: "option3",
}));
const __VLS_67 = __VLS_66({
    value: "option3",
}, ...__VLS_functionalComponentArgsRest(__VLS_66));
__VLS_68.slots.default;
var __VLS_68;
var __VLS_56;
var __VLS_52;
const __VLS_69 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_70 = __VLS_asFunctionalComponent(__VLS_69, new __VLS_69({
    label: "单选框",
}));
const __VLS_71 = __VLS_70({
    label: "单选框",
}, ...__VLS_functionalComponentArgsRest(__VLS_70));
__VLS_72.slots.default;
const __VLS_73 = {}.ARadioGroup;
/** @type {[typeof __VLS_components.ARadioGroup, typeof __VLS_components.aRadioGroup, typeof __VLS_components.ARadioGroup, typeof __VLS_components.aRadioGroup, ]} */ ;
// @ts-ignore
const __VLS_74 = __VLS_asFunctionalComponent(__VLS_73, new __VLS_73({
    value: (__VLS_ctx.form.testRadio),
}));
const __VLS_75 = __VLS_74({
    value: (__VLS_ctx.form.testRadio),
}, ...__VLS_functionalComponentArgsRest(__VLS_74));
__VLS_76.slots.default;
const __VLS_77 = {}.ARadio;
/** @type {[typeof __VLS_components.ARadio, typeof __VLS_components.aRadio, typeof __VLS_components.ARadio, typeof __VLS_components.aRadio, ]} */ ;
// @ts-ignore
const __VLS_78 = __VLS_asFunctionalComponent(__VLS_77, new __VLS_77({
    value: "radio1",
}));
const __VLS_79 = __VLS_78({
    value: "radio1",
}, ...__VLS_functionalComponentArgsRest(__VLS_78));
__VLS_80.slots.default;
var __VLS_80;
const __VLS_81 = {}.ARadio;
/** @type {[typeof __VLS_components.ARadio, typeof __VLS_components.aRadio, typeof __VLS_components.ARadio, typeof __VLS_components.aRadio, ]} */ ;
// @ts-ignore
const __VLS_82 = __VLS_asFunctionalComponent(__VLS_81, new __VLS_81({
    value: "radio2",
}));
const __VLS_83 = __VLS_82({
    value: "radio2",
}, ...__VLS_functionalComponentArgsRest(__VLS_82));
__VLS_84.slots.default;
var __VLS_84;
const __VLS_85 = {}.ARadio;
/** @type {[typeof __VLS_components.ARadio, typeof __VLS_components.aRadio, typeof __VLS_components.ARadio, typeof __VLS_components.aRadio, ]} */ ;
// @ts-ignore
const __VLS_86 = __VLS_asFunctionalComponent(__VLS_85, new __VLS_85({
    value: "radio3",
}));
const __VLS_87 = __VLS_86({
    value: "radio3",
}, ...__VLS_functionalComponentArgsRest(__VLS_86));
__VLS_88.slots.default;
var __VLS_88;
var __VLS_76;
var __VLS_72;
const __VLS_89 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_90 = __VLS_asFunctionalComponent(__VLS_89, new __VLS_89({
    label: "日期",
}));
const __VLS_91 = __VLS_90({
    label: "日期",
}, ...__VLS_functionalComponentArgsRest(__VLS_90));
__VLS_92.slots.default;
const __VLS_93 = {}.ADatePicker;
/** @type {[typeof __VLS_components.ADatePicker, typeof __VLS_components.aDatePicker, ]} */ ;
// @ts-ignore
const __VLS_94 = __VLS_asFunctionalComponent(__VLS_93, new __VLS_93({
    value: (__VLS_ctx.form.testDate),
    ...{ style: {} },
}));
const __VLS_95 = __VLS_94({
    value: (__VLS_ctx.form.testDate),
    ...{ style: {} },
}, ...__VLS_functionalComponentArgsRest(__VLS_94));
var __VLS_92;
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
            list: list,
            form: form,
            columns: columns,
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
