import { ref, reactive, onMounted } from 'vue';
import { langApi } from '@/api';
import { message } from 'ant-design-vue';
const loading = ref(false);
const saving = ref(false);
const modalOpen = ref(false);
const editItem = ref(null);
const data = ref([]);
const query = reactive({ langType: undefined });
const form = reactive({ moduleCode: '', langCode: '', langText: '', langType: 'zh_CN' });
const columns = [
    { title: '模块', dataIndex: 'moduleCode' }, { title: '翻译编码', dataIndex: 'langCode' },
    { title: '翻译文本', dataIndex: 'langText' }, { title: '语言类型', dataIndex: 'langType' },
    { title: '操作', key: 'action' }
];
async function loadData() {
    loading.value = true;
    if (query.langType) {
        const res = await langApi.getByType(query.langType);
        if (res.data)
            data.value = res.data;
    }
    else {
        const res = await langApi.list();
        if (res.data)
            data.value = res.data;
    }
    loading.value = false;
}
function showAdd() { editItem.value = null; form.moduleCode = ''; form.langCode = ''; form.langText = ''; form.langType = 'zh_CN'; modalOpen.value = true; }
function showEdit(row) { editItem.value = row; Object.assign(form, row); modalOpen.value = true; }
async function handleSave() { saving.value = true; await langApi.save(form); message.success('保存成功'); modalOpen.value = false; loadData(); saving.value = false; }
async function handleDelete(id) { await langApi.delete(id); message.success('删除成功'); loadData(); }
onMounted(loadData);
debugger; /* PartiallyEnd: #3632/scriptSetup.vue */
const __VLS_ctx = {};
let __VLS_components;
let __VLS_directives;
const __VLS_0 = {}.ACard;
/** @type {[typeof __VLS_components.ACard, typeof __VLS_components.aCard, typeof __VLS_components.ACard, typeof __VLS_components.aCard, ]} */ ;
// @ts-ignore
const __VLS_1 = __VLS_asFunctionalComponent(__VLS_0, new __VLS_0({
    title: "国际化管理",
}));
const __VLS_2 = __VLS_1({
    title: "国际化管理",
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
    label: "语言类型",
}));
const __VLS_11 = __VLS_10({
    label: "语言类型",
}, ...__VLS_functionalComponentArgsRest(__VLS_10));
__VLS_12.slots.default;
const __VLS_13 = {}.ASelect;
/** @type {[typeof __VLS_components.ASelect, typeof __VLS_components.aSelect, typeof __VLS_components.ASelect, typeof __VLS_components.aSelect, ]} */ ;
// @ts-ignore
const __VLS_14 = __VLS_asFunctionalComponent(__VLS_13, new __VLS_13({
    ...{ 'onChange': {} },
    value: (__VLS_ctx.query.langType),
    ...{ style: {} },
    allowClear: true,
}));
const __VLS_15 = __VLS_14({
    ...{ 'onChange': {} },
    value: (__VLS_ctx.query.langType),
    ...{ style: {} },
    allowClear: true,
}, ...__VLS_functionalComponentArgsRest(__VLS_14));
let __VLS_17;
let __VLS_18;
let __VLS_19;
const __VLS_20 = {
    onChange: (__VLS_ctx.loadData)
};
__VLS_16.slots.default;
const __VLS_21 = {}.ASelectOption;
/** @type {[typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, ]} */ ;
// @ts-ignore
const __VLS_22 = __VLS_asFunctionalComponent(__VLS_21, new __VLS_21({
    value: "zh_CN",
}));
const __VLS_23 = __VLS_22({
    value: "zh_CN",
}, ...__VLS_functionalComponentArgsRest(__VLS_22));
__VLS_24.slots.default;
var __VLS_24;
const __VLS_25 = {}.ASelectOption;
/** @type {[typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, ]} */ ;
// @ts-ignore
const __VLS_26 = __VLS_asFunctionalComponent(__VLS_25, new __VLS_25({
    value: "en_US",
}));
const __VLS_27 = __VLS_26({
    value: "en_US",
}, ...__VLS_functionalComponentArgsRest(__VLS_26));
__VLS_28.slots.default;
var __VLS_28;
const __VLS_29 = {}.ASelectOption;
/** @type {[typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, ]} */ ;
// @ts-ignore
const __VLS_30 = __VLS_asFunctionalComponent(__VLS_29, new __VLS_29({
    value: "ja",
}));
const __VLS_31 = __VLS_30({
    value: "ja",
}, ...__VLS_functionalComponentArgsRest(__VLS_30));
__VLS_32.slots.default;
var __VLS_32;
var __VLS_16;
var __VLS_12;
const __VLS_33 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_34 = __VLS_asFunctionalComponent(__VLS_33, new __VLS_33({}));
const __VLS_35 = __VLS_34({}, ...__VLS_functionalComponentArgsRest(__VLS_34));
__VLS_36.slots.default;
const __VLS_37 = {}.AButton;
/** @type {[typeof __VLS_components.AButton, typeof __VLS_components.aButton, typeof __VLS_components.AButton, typeof __VLS_components.aButton, ]} */ ;
// @ts-ignore
const __VLS_38 = __VLS_asFunctionalComponent(__VLS_37, new __VLS_37({
    ...{ 'onClick': {} },
    type: "primary",
}));
const __VLS_39 = __VLS_38({
    ...{ 'onClick': {} },
    type: "primary",
}, ...__VLS_functionalComponentArgsRest(__VLS_38));
let __VLS_41;
let __VLS_42;
let __VLS_43;
const __VLS_44 = {
    onClick: (__VLS_ctx.loadData)
};
__VLS_40.slots.default;
var __VLS_40;
var __VLS_36;
const __VLS_45 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_46 = __VLS_asFunctionalComponent(__VLS_45, new __VLS_45({}));
const __VLS_47 = __VLS_46({}, ...__VLS_functionalComponentArgsRest(__VLS_46));
__VLS_48.slots.default;
const __VLS_49 = {}.AButton;
/** @type {[typeof __VLS_components.AButton, typeof __VLS_components.aButton, typeof __VLS_components.AButton, typeof __VLS_components.aButton, ]} */ ;
// @ts-ignore
const __VLS_50 = __VLS_asFunctionalComponent(__VLS_49, new __VLS_49({
    ...{ 'onClick': {} },
    type: "primary",
}));
const __VLS_51 = __VLS_50({
    ...{ 'onClick': {} },
    type: "primary",
}, ...__VLS_functionalComponentArgsRest(__VLS_50));
let __VLS_53;
let __VLS_54;
let __VLS_55;
const __VLS_56 = {
    onClick: (__VLS_ctx.showAdd)
};
__VLS_52.slots.default;
var __VLS_52;
var __VLS_48;
var __VLS_8;
const __VLS_57 = {}.ATable;
/** @type {[typeof __VLS_components.ATable, typeof __VLS_components.aTable, typeof __VLS_components.ATable, typeof __VLS_components.aTable, ]} */ ;
// @ts-ignore
const __VLS_58 = __VLS_asFunctionalComponent(__VLS_57, new __VLS_57({
    dataSource: (__VLS_ctx.data),
    columns: (__VLS_ctx.columns),
    loading: (__VLS_ctx.loading),
    rowKey: "id",
}));
const __VLS_59 = __VLS_58({
    dataSource: (__VLS_ctx.data),
    columns: (__VLS_ctx.columns),
    loading: (__VLS_ctx.loading),
    rowKey: "id",
}, ...__VLS_functionalComponentArgsRest(__VLS_58));
__VLS_60.slots.default;
{
    const { bodyCell: __VLS_thisSlot } = __VLS_60.slots;
    const [{ record, column }] = __VLS_getSlotParams(__VLS_thisSlot);
    if (column.key === 'action') {
        const __VLS_61 = {}.ASpace;
        /** @type {[typeof __VLS_components.ASpace, typeof __VLS_components.aSpace, typeof __VLS_components.ASpace, typeof __VLS_components.aSpace, ]} */ ;
        // @ts-ignore
        const __VLS_62 = __VLS_asFunctionalComponent(__VLS_61, new __VLS_61({}));
        const __VLS_63 = __VLS_62({}, ...__VLS_functionalComponentArgsRest(__VLS_62));
        __VLS_64.slots.default;
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
        var __VLS_64;
    }
}
var __VLS_60;
const __VLS_65 = {}.AModal;
/** @type {[typeof __VLS_components.AModal, typeof __VLS_components.aModal, typeof __VLS_components.AModal, typeof __VLS_components.aModal, ]} */ ;
// @ts-ignore
const __VLS_66 = __VLS_asFunctionalComponent(__VLS_65, new __VLS_65({
    ...{ 'onOk': {} },
    open: (__VLS_ctx.modalOpen),
    title: (__VLS_ctx.editItem ? '编辑翻译' : '新增翻译'),
    confirmLoading: (__VLS_ctx.saving),
}));
const __VLS_67 = __VLS_66({
    ...{ 'onOk': {} },
    open: (__VLS_ctx.modalOpen),
    title: (__VLS_ctx.editItem ? '编辑翻译' : '新增翻译'),
    confirmLoading: (__VLS_ctx.saving),
}, ...__VLS_functionalComponentArgsRest(__VLS_66));
let __VLS_69;
let __VLS_70;
let __VLS_71;
const __VLS_72 = {
    onOk: (__VLS_ctx.handleSave)
};
__VLS_68.slots.default;
const __VLS_73 = {}.AForm;
/** @type {[typeof __VLS_components.AForm, typeof __VLS_components.aForm, typeof __VLS_components.AForm, typeof __VLS_components.aForm, ]} */ ;
// @ts-ignore
const __VLS_74 = __VLS_asFunctionalComponent(__VLS_73, new __VLS_73({
    model: (__VLS_ctx.form),
    layout: "vertical",
}));
const __VLS_75 = __VLS_74({
    model: (__VLS_ctx.form),
    layout: "vertical",
}, ...__VLS_functionalComponentArgsRest(__VLS_74));
__VLS_76.slots.default;
const __VLS_77 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_78 = __VLS_asFunctionalComponent(__VLS_77, new __VLS_77({
    label: "模块编码",
}));
const __VLS_79 = __VLS_78({
    label: "模块编码",
}, ...__VLS_functionalComponentArgsRest(__VLS_78));
__VLS_80.slots.default;
const __VLS_81 = {}.AInput;
/** @type {[typeof __VLS_components.AInput, typeof __VLS_components.aInput, ]} */ ;
// @ts-ignore
const __VLS_82 = __VLS_asFunctionalComponent(__VLS_81, new __VLS_81({
    value: (__VLS_ctx.form.moduleCode),
}));
const __VLS_83 = __VLS_82({
    value: (__VLS_ctx.form.moduleCode),
}, ...__VLS_functionalComponentArgsRest(__VLS_82));
var __VLS_80;
const __VLS_85 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_86 = __VLS_asFunctionalComponent(__VLS_85, new __VLS_85({
    label: "翻译编码",
}));
const __VLS_87 = __VLS_86({
    label: "翻译编码",
}, ...__VLS_functionalComponentArgsRest(__VLS_86));
__VLS_88.slots.default;
const __VLS_89 = {}.AInput;
/** @type {[typeof __VLS_components.AInput, typeof __VLS_components.aInput, ]} */ ;
// @ts-ignore
const __VLS_90 = __VLS_asFunctionalComponent(__VLS_89, new __VLS_89({
    value: (__VLS_ctx.form.langCode),
}));
const __VLS_91 = __VLS_90({
    value: (__VLS_ctx.form.langCode),
}, ...__VLS_functionalComponentArgsRest(__VLS_90));
var __VLS_88;
const __VLS_93 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_94 = __VLS_asFunctionalComponent(__VLS_93, new __VLS_93({
    label: "翻译文本",
}));
const __VLS_95 = __VLS_94({
    label: "翻译文本",
}, ...__VLS_functionalComponentArgsRest(__VLS_94));
__VLS_96.slots.default;
const __VLS_97 = {}.AInput;
/** @type {[typeof __VLS_components.AInput, typeof __VLS_components.aInput, ]} */ ;
// @ts-ignore
const __VLS_98 = __VLS_asFunctionalComponent(__VLS_97, new __VLS_97({
    value: (__VLS_ctx.form.langText),
}));
const __VLS_99 = __VLS_98({
    value: (__VLS_ctx.form.langText),
}, ...__VLS_functionalComponentArgsRest(__VLS_98));
var __VLS_96;
const __VLS_101 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_102 = __VLS_asFunctionalComponent(__VLS_101, new __VLS_101({
    label: "语言类型",
}));
const __VLS_103 = __VLS_102({
    label: "语言类型",
}, ...__VLS_functionalComponentArgsRest(__VLS_102));
__VLS_104.slots.default;
const __VLS_105 = {}.ASelect;
/** @type {[typeof __VLS_components.ASelect, typeof __VLS_components.aSelect, typeof __VLS_components.ASelect, typeof __VLS_components.aSelect, ]} */ ;
// @ts-ignore
const __VLS_106 = __VLS_asFunctionalComponent(__VLS_105, new __VLS_105({
    value: (__VLS_ctx.form.langType),
}));
const __VLS_107 = __VLS_106({
    value: (__VLS_ctx.form.langType),
}, ...__VLS_functionalComponentArgsRest(__VLS_106));
__VLS_108.slots.default;
const __VLS_109 = {}.ASelectOption;
/** @type {[typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, ]} */ ;
// @ts-ignore
const __VLS_110 = __VLS_asFunctionalComponent(__VLS_109, new __VLS_109({
    value: "zh_CN",
}));
const __VLS_111 = __VLS_110({
    value: "zh_CN",
}, ...__VLS_functionalComponentArgsRest(__VLS_110));
__VLS_112.slots.default;
var __VLS_112;
const __VLS_113 = {}.ASelectOption;
/** @type {[typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, ]} */ ;
// @ts-ignore
const __VLS_114 = __VLS_asFunctionalComponent(__VLS_113, new __VLS_113({
    value: "en_US",
}));
const __VLS_115 = __VLS_114({
    value: "en_US",
}, ...__VLS_functionalComponentArgsRest(__VLS_114));
__VLS_116.slots.default;
var __VLS_116;
const __VLS_117 = {}.ASelectOption;
/** @type {[typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, ]} */ ;
// @ts-ignore
const __VLS_118 = __VLS_asFunctionalComponent(__VLS_117, new __VLS_117({
    value: "ja",
}));
const __VLS_119 = __VLS_118({
    value: "ja",
}, ...__VLS_functionalComponentArgsRest(__VLS_118));
__VLS_120.slots.default;
var __VLS_120;
var __VLS_108;
var __VLS_104;
var __VLS_76;
var __VLS_68;
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
            query: query,
            form: form,
            columns: columns,
            loadData: loadData,
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
