import { ref, reactive, onMounted } from 'vue';
import { dictApi } from '@/api';
import { message } from 'ant-design-vue';
const loading = ref(false);
const loadingData = ref(false);
const typeData = reactive({ list: [], total: 0, pageNo: 1, pageSize: 20 });
const dataData = reactive({ list: [], total: 0, pageNo: 1, pageSize: 20 });
const dataQuery = reactive({ dictType: '' });
const typeColumns = [{ title: '编码', dataIndex: 'dictTypeCode' }, { title: '名称', dataIndex: 'dictName' }, { title: '系统', dataIndex: 'isSys' }, { title: '操作', key: 'action' }];
const dataColumns = [{ title: '类型', dataIndex: 'dictType' }, { title: '标签', dataIndex: 'dictLabel' }, { title: '值', dataIndex: 'dictValue' }, { title: '操作', key: 'action' }];
const typeModalVisible = ref(false);
const typeModalTitle = ref('新增字典类型');
const typeForm = reactive({ dictTypeCode: '', dictName: '', isSysBool: '0', sort: 0 });
const dataModalVisible = ref(false);
const dataForm = reactive({ dictCode: undefined, dictType: '', dictLabel: '', dictValue: '', sort: 0 });
async function loadType() { loading.value = true; const r = await dictApi.typeList({ pageNo: typeData.pageNo, pageSize: typeData.pageSize, entity: {} }); if (r.data) {
    typeData.list = r.data.list;
    typeData.total = r.data.total;
} loading.value = false; }
async function loadData() { loadingData.value = true; const r = await dictApi.dataList({ pageNo: dataData.pageNo, pageSize: dataData.pageSize, entity: { dictType: dataQuery.dictType } }); if (r.data) {
    dataData.list = r.data.list;
    dataData.total = r.data.total;
} loadingData.value = false; }
function onTypePageChange(p, s) { typeData.pageNo = p; typeData.pageSize = s; loadType(); }
function onDataPageChange(p, s) { dataData.pageNo = p; dataData.pageSize = s; loadData(); }
function showTypeAdd() { typeModalTitle.value = '新增字典类型'; typeForm.dictTypeCode = ''; typeForm.dictName = ''; typeForm.isSysBool = '0'; typeForm.sort = 0; typeModalVisible.value = true; }
function showTypeEdit(r) { typeModalTitle.value = '编辑字典类型'; typeForm.dictTypeCode = r.dictTypeCode; typeForm.dictName = r.dictName; typeForm.isSysBool = r.isSys || '0'; typeForm.sort = r.sort || 0; typeModalVisible.value = true; }
async function saveType() { await dictApi.typeSave({ dictTypeCode: typeForm.dictTypeCode || undefined, dictName: typeForm.dictName, isSys: typeForm.isSysBool, sort: typeForm.sort }); message.success('保存成功'); typeModalVisible.value = false; loadType(); }
function showDataAdd() { dataForm.dictCode = undefined; dataForm.dictType = ''; dataForm.dictLabel = ''; dataForm.dictValue = ''; dataForm.sort = 0; dataModalVisible.value = true; }
function showDataEdit(r) { dataForm.dictCode = r.dictCode; dataForm.dictType = r.dictType; dataForm.dictLabel = r.dictLabel; dataForm.dictValue = r.dictValue; dataForm.sort = r.sort || 0; dataModalVisible.value = true; }
async function saveData() { await dictApi.dataSave({ dictCode: dataForm.dictCode, dictType: dataForm.dictType, dictLabel: dataForm.dictLabel, dictValue: dataForm.dictValue, sort: dataForm.sort }); message.success('保存成功'); dataModalVisible.value = false; loadData(); }
async function handleTypeDelete(c) { await dictApi.typeDelete(c); message.success('删除成功'); loadType(); }
async function handleDataDelete(c) { await dictApi.dataDelete(c); message.success('删除成功'); loadData(); }
onMounted(() => { loadType(); loadData(); });
debugger; /* PartiallyEnd: #3632/scriptSetup.vue */
const __VLS_ctx = {};
let __VLS_components;
let __VLS_directives;
const __VLS_0 = {}.ACard;
/** @type {[typeof __VLS_components.ACard, typeof __VLS_components.aCard, typeof __VLS_components.ACard, typeof __VLS_components.aCard, ]} */ ;
// @ts-ignore
const __VLS_1 = __VLS_asFunctionalComponent(__VLS_0, new __VLS_0({
    title: "字典管理",
}));
const __VLS_2 = __VLS_1({
    title: "字典管理",
}, ...__VLS_functionalComponentArgsRest(__VLS_1));
var __VLS_4 = {};
__VLS_3.slots.default;
const __VLS_5 = {}.ATabs;
/** @type {[typeof __VLS_components.ATabs, typeof __VLS_components.aTabs, typeof __VLS_components.ATabs, typeof __VLS_components.aTabs, ]} */ ;
// @ts-ignore
const __VLS_6 = __VLS_asFunctionalComponent(__VLS_5, new __VLS_5({}));
const __VLS_7 = __VLS_6({}, ...__VLS_functionalComponentArgsRest(__VLS_6));
__VLS_8.slots.default;
const __VLS_9 = {}.ATabPane;
/** @type {[typeof __VLS_components.ATabPane, typeof __VLS_components.aTabPane, typeof __VLS_components.ATabPane, typeof __VLS_components.aTabPane, ]} */ ;
// @ts-ignore
const __VLS_10 = __VLS_asFunctionalComponent(__VLS_9, new __VLS_9({
    key: "type",
    tab: "字典类型",
}));
const __VLS_11 = __VLS_10({
    key: "type",
    tab: "字典类型",
}, ...__VLS_functionalComponentArgsRest(__VLS_10));
__VLS_12.slots.default;
const __VLS_13 = {}.AButton;
/** @type {[typeof __VLS_components.AButton, typeof __VLS_components.aButton, typeof __VLS_components.AButton, typeof __VLS_components.aButton, ]} */ ;
// @ts-ignore
const __VLS_14 = __VLS_asFunctionalComponent(__VLS_13, new __VLS_13({
    ...{ 'onClick': {} },
    type: "primary",
    ...{ style: {} },
}));
const __VLS_15 = __VLS_14({
    ...{ 'onClick': {} },
    type: "primary",
    ...{ style: {} },
}, ...__VLS_functionalComponentArgsRest(__VLS_14));
let __VLS_17;
let __VLS_18;
let __VLS_19;
const __VLS_20 = {
    onClick: (__VLS_ctx.showTypeAdd)
};
__VLS_16.slots.default;
var __VLS_16;
const __VLS_21 = {}.ATable;
/** @type {[typeof __VLS_components.ATable, typeof __VLS_components.aTable, typeof __VLS_components.ATable, typeof __VLS_components.aTable, ]} */ ;
// @ts-ignore
const __VLS_22 = __VLS_asFunctionalComponent(__VLS_21, new __VLS_21({
    dataSource: (__VLS_ctx.typeData.list),
    columns: (__VLS_ctx.typeColumns),
    loading: (__VLS_ctx.loading),
    rowKey: "dictTypeCode",
    pagination: ({ current: __VLS_ctx.typeData.pageNo, pageSize: __VLS_ctx.typeData.pageSize, total: __VLS_ctx.typeData.total, onChange: __VLS_ctx.onTypePageChange }),
}));
const __VLS_23 = __VLS_22({
    dataSource: (__VLS_ctx.typeData.list),
    columns: (__VLS_ctx.typeColumns),
    loading: (__VLS_ctx.loading),
    rowKey: "dictTypeCode",
    pagination: ({ current: __VLS_ctx.typeData.pageNo, pageSize: __VLS_ctx.typeData.pageSize, total: __VLS_ctx.typeData.total, onChange: __VLS_ctx.onTypePageChange }),
}, ...__VLS_functionalComponentArgsRest(__VLS_22));
__VLS_24.slots.default;
{
    const { bodyCell: __VLS_thisSlot } = __VLS_24.slots;
    const [{ record, column }] = __VLS_getSlotParams(__VLS_thisSlot);
    if (column.key === 'action') {
        const __VLS_25 = {}.ASpace;
        /** @type {[typeof __VLS_components.ASpace, typeof __VLS_components.aSpace, typeof __VLS_components.ASpace, typeof __VLS_components.aSpace, ]} */ ;
        // @ts-ignore
        const __VLS_26 = __VLS_asFunctionalComponent(__VLS_25, new __VLS_25({}));
        const __VLS_27 = __VLS_26({}, ...__VLS_functionalComponentArgsRest(__VLS_26));
        __VLS_28.slots.default;
        __VLS_asFunctionalElement(__VLS_intrinsicElements.a, __VLS_intrinsicElements.a)({
            ...{ onClick: (...[$event]) => {
                    if (!(column.key === 'action'))
                        return;
                    __VLS_ctx.showTypeEdit(record);
                } },
        });
        __VLS_asFunctionalElement(__VLS_intrinsicElements.a, __VLS_intrinsicElements.a)({
            ...{ onClick: (...[$event]) => {
                    if (!(column.key === 'action'))
                        return;
                    __VLS_ctx.handleTypeDelete(record.dictTypeCode);
                } },
        });
        var __VLS_28;
    }
}
var __VLS_24;
var __VLS_12;
const __VLS_29 = {}.ATabPane;
/** @type {[typeof __VLS_components.ATabPane, typeof __VLS_components.aTabPane, typeof __VLS_components.ATabPane, typeof __VLS_components.aTabPane, ]} */ ;
// @ts-ignore
const __VLS_30 = __VLS_asFunctionalComponent(__VLS_29, new __VLS_29({
    key: "data",
    tab: "字典数据",
}));
const __VLS_31 = __VLS_30({
    key: "data",
    tab: "字典数据",
}, ...__VLS_functionalComponentArgsRest(__VLS_30));
__VLS_32.slots.default;
const __VLS_33 = {}.AForm;
/** @type {[typeof __VLS_components.AForm, typeof __VLS_components.aForm, typeof __VLS_components.AForm, typeof __VLS_components.aForm, ]} */ ;
// @ts-ignore
const __VLS_34 = __VLS_asFunctionalComponent(__VLS_33, new __VLS_33({
    layout: "inline",
    ...{ style: {} },
}));
const __VLS_35 = __VLS_34({
    layout: "inline",
    ...{ style: {} },
}, ...__VLS_functionalComponentArgsRest(__VLS_34));
__VLS_36.slots.default;
const __VLS_37 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_38 = __VLS_asFunctionalComponent(__VLS_37, new __VLS_37({
    label: "字典类型",
}));
const __VLS_39 = __VLS_38({
    label: "字典类型",
}, ...__VLS_functionalComponentArgsRest(__VLS_38));
__VLS_40.slots.default;
const __VLS_41 = {}.AInput;
/** @type {[typeof __VLS_components.AInput, typeof __VLS_components.aInput, ]} */ ;
// @ts-ignore
const __VLS_42 = __VLS_asFunctionalComponent(__VLS_41, new __VLS_41({
    value: (__VLS_ctx.dataQuery.dictType),
    placeholder: "类型编码",
}));
const __VLS_43 = __VLS_42({
    value: (__VLS_ctx.dataQuery.dictType),
    placeholder: "类型编码",
}, ...__VLS_functionalComponentArgsRest(__VLS_42));
var __VLS_40;
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
    onClick: (__VLS_ctx.loadData)
};
__VLS_52.slots.default;
var __VLS_52;
var __VLS_48;
const __VLS_57 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_58 = __VLS_asFunctionalComponent(__VLS_57, new __VLS_57({}));
const __VLS_59 = __VLS_58({}, ...__VLS_functionalComponentArgsRest(__VLS_58));
__VLS_60.slots.default;
const __VLS_61 = {}.AButton;
/** @type {[typeof __VLS_components.AButton, typeof __VLS_components.aButton, typeof __VLS_components.AButton, typeof __VLS_components.aButton, ]} */ ;
// @ts-ignore
const __VLS_62 = __VLS_asFunctionalComponent(__VLS_61, new __VLS_61({
    ...{ 'onClick': {} },
    type: "primary",
}));
const __VLS_63 = __VLS_62({
    ...{ 'onClick': {} },
    type: "primary",
}, ...__VLS_functionalComponentArgsRest(__VLS_62));
let __VLS_65;
let __VLS_66;
let __VLS_67;
const __VLS_68 = {
    onClick: (__VLS_ctx.showDataAdd)
};
__VLS_64.slots.default;
var __VLS_64;
var __VLS_60;
var __VLS_36;
const __VLS_69 = {}.ATable;
/** @type {[typeof __VLS_components.ATable, typeof __VLS_components.aTable, typeof __VLS_components.ATable, typeof __VLS_components.aTable, ]} */ ;
// @ts-ignore
const __VLS_70 = __VLS_asFunctionalComponent(__VLS_69, new __VLS_69({
    dataSource: (__VLS_ctx.dataData.list),
    columns: (__VLS_ctx.dataColumns),
    loading: (__VLS_ctx.loadingData),
    rowKey: "dictCode",
    pagination: ({ current: __VLS_ctx.dataData.pageNo, pageSize: __VLS_ctx.dataData.pageSize, total: __VLS_ctx.dataData.total, onChange: __VLS_ctx.onDataPageChange }),
}));
const __VLS_71 = __VLS_70({
    dataSource: (__VLS_ctx.dataData.list),
    columns: (__VLS_ctx.dataColumns),
    loading: (__VLS_ctx.loadingData),
    rowKey: "dictCode",
    pagination: ({ current: __VLS_ctx.dataData.pageNo, pageSize: __VLS_ctx.dataData.pageSize, total: __VLS_ctx.dataData.total, onChange: __VLS_ctx.onDataPageChange }),
}, ...__VLS_functionalComponentArgsRest(__VLS_70));
__VLS_72.slots.default;
{
    const { bodyCell: __VLS_thisSlot } = __VLS_72.slots;
    const [{ record, column }] = __VLS_getSlotParams(__VLS_thisSlot);
    if (column.key === 'action') {
        const __VLS_73 = {}.ASpace;
        /** @type {[typeof __VLS_components.ASpace, typeof __VLS_components.aSpace, typeof __VLS_components.ASpace, typeof __VLS_components.aSpace, ]} */ ;
        // @ts-ignore
        const __VLS_74 = __VLS_asFunctionalComponent(__VLS_73, new __VLS_73({}));
        const __VLS_75 = __VLS_74({}, ...__VLS_functionalComponentArgsRest(__VLS_74));
        __VLS_76.slots.default;
        __VLS_asFunctionalElement(__VLS_intrinsicElements.a, __VLS_intrinsicElements.a)({
            ...{ onClick: (...[$event]) => {
                    if (!(column.key === 'action'))
                        return;
                    __VLS_ctx.showDataEdit(record);
                } },
        });
        __VLS_asFunctionalElement(__VLS_intrinsicElements.a, __VLS_intrinsicElements.a)({
            ...{ onClick: (...[$event]) => {
                    if (!(column.key === 'action'))
                        return;
                    __VLS_ctx.handleDataDelete(record.dictCode);
                } },
        });
        var __VLS_76;
    }
}
var __VLS_72;
var __VLS_32;
var __VLS_8;
const __VLS_77 = {}.AModal;
/** @type {[typeof __VLS_components.AModal, typeof __VLS_components.aModal, typeof __VLS_components.AModal, typeof __VLS_components.aModal, ]} */ ;
// @ts-ignore
const __VLS_78 = __VLS_asFunctionalComponent(__VLS_77, new __VLS_77({
    ...{ 'onOk': {} },
    open: (__VLS_ctx.typeModalVisible),
    title: (__VLS_ctx.typeModalTitle),
}));
const __VLS_79 = __VLS_78({
    ...{ 'onOk': {} },
    open: (__VLS_ctx.typeModalVisible),
    title: (__VLS_ctx.typeModalTitle),
}, ...__VLS_functionalComponentArgsRest(__VLS_78));
let __VLS_81;
let __VLS_82;
let __VLS_83;
const __VLS_84 = {
    onOk: (__VLS_ctx.saveType)
};
__VLS_80.slots.default;
const __VLS_85 = {}.AForm;
/** @type {[typeof __VLS_components.AForm, typeof __VLS_components.aForm, typeof __VLS_components.AForm, typeof __VLS_components.aForm, ]} */ ;
// @ts-ignore
const __VLS_86 = __VLS_asFunctionalComponent(__VLS_85, new __VLS_85({
    layout: "vertical",
}));
const __VLS_87 = __VLS_86({
    layout: "vertical",
}, ...__VLS_functionalComponentArgsRest(__VLS_86));
__VLS_88.slots.default;
const __VLS_89 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_90 = __VLS_asFunctionalComponent(__VLS_89, new __VLS_89({
    label: "编码",
}));
const __VLS_91 = __VLS_90({
    label: "编码",
}, ...__VLS_functionalComponentArgsRest(__VLS_90));
__VLS_92.slots.default;
const __VLS_93 = {}.AInput;
/** @type {[typeof __VLS_components.AInput, typeof __VLS_components.aInput, ]} */ ;
// @ts-ignore
const __VLS_94 = __VLS_asFunctionalComponent(__VLS_93, new __VLS_93({
    value: (__VLS_ctx.typeForm.dictTypeCode),
}));
const __VLS_95 = __VLS_94({
    value: (__VLS_ctx.typeForm.dictTypeCode),
}, ...__VLS_functionalComponentArgsRest(__VLS_94));
var __VLS_92;
const __VLS_97 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_98 = __VLS_asFunctionalComponent(__VLS_97, new __VLS_97({
    label: "名称",
}));
const __VLS_99 = __VLS_98({
    label: "名称",
}, ...__VLS_functionalComponentArgsRest(__VLS_98));
__VLS_100.slots.default;
const __VLS_101 = {}.AInput;
/** @type {[typeof __VLS_components.AInput, typeof __VLS_components.aInput, ]} */ ;
// @ts-ignore
const __VLS_102 = __VLS_asFunctionalComponent(__VLS_101, new __VLS_101({
    value: (__VLS_ctx.typeForm.dictName),
}));
const __VLS_103 = __VLS_102({
    value: (__VLS_ctx.typeForm.dictName),
}, ...__VLS_functionalComponentArgsRest(__VLS_102));
var __VLS_100;
const __VLS_105 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_106 = __VLS_asFunctionalComponent(__VLS_105, new __VLS_105({
    label: "系统",
}));
const __VLS_107 = __VLS_106({
    label: "系统",
}, ...__VLS_functionalComponentArgsRest(__VLS_106));
__VLS_108.slots.default;
const __VLS_109 = {}.ASwitch;
/** @type {[typeof __VLS_components.ASwitch, typeof __VLS_components.aSwitch, ]} */ ;
// @ts-ignore
const __VLS_110 = __VLS_asFunctionalComponent(__VLS_109, new __VLS_109({
    checked: (__VLS_ctx.typeForm.isSysBool),
    checkedValue: "1",
    unCheckedValue: "0",
}));
const __VLS_111 = __VLS_110({
    checked: (__VLS_ctx.typeForm.isSysBool),
    checkedValue: "1",
    unCheckedValue: "0",
}, ...__VLS_functionalComponentArgsRest(__VLS_110));
var __VLS_108;
const __VLS_113 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_114 = __VLS_asFunctionalComponent(__VLS_113, new __VLS_113({
    label: "排序",
}));
const __VLS_115 = __VLS_114({
    label: "排序",
}, ...__VLS_functionalComponentArgsRest(__VLS_114));
__VLS_116.slots.default;
const __VLS_117 = {}.AInputNumber;
/** @type {[typeof __VLS_components.AInputNumber, typeof __VLS_components.aInputNumber, ]} */ ;
// @ts-ignore
const __VLS_118 = __VLS_asFunctionalComponent(__VLS_117, new __VLS_117({
    value: (__VLS_ctx.typeForm.sort),
}));
const __VLS_119 = __VLS_118({
    value: (__VLS_ctx.typeForm.sort),
}, ...__VLS_functionalComponentArgsRest(__VLS_118));
var __VLS_116;
var __VLS_88;
var __VLS_80;
const __VLS_121 = {}.AModal;
/** @type {[typeof __VLS_components.AModal, typeof __VLS_components.aModal, typeof __VLS_components.AModal, typeof __VLS_components.aModal, ]} */ ;
// @ts-ignore
const __VLS_122 = __VLS_asFunctionalComponent(__VLS_121, new __VLS_121({
    ...{ 'onOk': {} },
    open: (__VLS_ctx.dataModalVisible),
    title: "字典数据",
}));
const __VLS_123 = __VLS_122({
    ...{ 'onOk': {} },
    open: (__VLS_ctx.dataModalVisible),
    title: "字典数据",
}, ...__VLS_functionalComponentArgsRest(__VLS_122));
let __VLS_125;
let __VLS_126;
let __VLS_127;
const __VLS_128 = {
    onOk: (__VLS_ctx.saveData)
};
__VLS_124.slots.default;
const __VLS_129 = {}.AForm;
/** @type {[typeof __VLS_components.AForm, typeof __VLS_components.aForm, typeof __VLS_components.AForm, typeof __VLS_components.aForm, ]} */ ;
// @ts-ignore
const __VLS_130 = __VLS_asFunctionalComponent(__VLS_129, new __VLS_129({
    layout: "vertical",
}));
const __VLS_131 = __VLS_130({
    layout: "vertical",
}, ...__VLS_functionalComponentArgsRest(__VLS_130));
__VLS_132.slots.default;
const __VLS_133 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_134 = __VLS_asFunctionalComponent(__VLS_133, new __VLS_133({
    label: "类型",
}));
const __VLS_135 = __VLS_134({
    label: "类型",
}, ...__VLS_functionalComponentArgsRest(__VLS_134));
__VLS_136.slots.default;
const __VLS_137 = {}.AInput;
/** @type {[typeof __VLS_components.AInput, typeof __VLS_components.aInput, ]} */ ;
// @ts-ignore
const __VLS_138 = __VLS_asFunctionalComponent(__VLS_137, new __VLS_137({
    value: (__VLS_ctx.dataForm.dictType),
}));
const __VLS_139 = __VLS_138({
    value: (__VLS_ctx.dataForm.dictType),
}, ...__VLS_functionalComponentArgsRest(__VLS_138));
var __VLS_136;
const __VLS_141 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_142 = __VLS_asFunctionalComponent(__VLS_141, new __VLS_141({
    label: "标签",
}));
const __VLS_143 = __VLS_142({
    label: "标签",
}, ...__VLS_functionalComponentArgsRest(__VLS_142));
__VLS_144.slots.default;
const __VLS_145 = {}.AInput;
/** @type {[typeof __VLS_components.AInput, typeof __VLS_components.aInput, ]} */ ;
// @ts-ignore
const __VLS_146 = __VLS_asFunctionalComponent(__VLS_145, new __VLS_145({
    value: (__VLS_ctx.dataForm.dictLabel),
}));
const __VLS_147 = __VLS_146({
    value: (__VLS_ctx.dataForm.dictLabel),
}, ...__VLS_functionalComponentArgsRest(__VLS_146));
var __VLS_144;
const __VLS_149 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_150 = __VLS_asFunctionalComponent(__VLS_149, new __VLS_149({
    label: "值",
}));
const __VLS_151 = __VLS_150({
    label: "值",
}, ...__VLS_functionalComponentArgsRest(__VLS_150));
__VLS_152.slots.default;
const __VLS_153 = {}.AInput;
/** @type {[typeof __VLS_components.AInput, typeof __VLS_components.aInput, ]} */ ;
// @ts-ignore
const __VLS_154 = __VLS_asFunctionalComponent(__VLS_153, new __VLS_153({
    value: (__VLS_ctx.dataForm.dictValue),
}));
const __VLS_155 = __VLS_154({
    value: (__VLS_ctx.dataForm.dictValue),
}, ...__VLS_functionalComponentArgsRest(__VLS_154));
var __VLS_152;
const __VLS_157 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_158 = __VLS_asFunctionalComponent(__VLS_157, new __VLS_157({
    label: "排序",
}));
const __VLS_159 = __VLS_158({
    label: "排序",
}, ...__VLS_functionalComponentArgsRest(__VLS_158));
__VLS_160.slots.default;
const __VLS_161 = {}.AInputNumber;
/** @type {[typeof __VLS_components.AInputNumber, typeof __VLS_components.aInputNumber, ]} */ ;
// @ts-ignore
const __VLS_162 = __VLS_asFunctionalComponent(__VLS_161, new __VLS_161({
    value: (__VLS_ctx.dataForm.sort),
}));
const __VLS_163 = __VLS_162({
    value: (__VLS_ctx.dataForm.sort),
}, ...__VLS_functionalComponentArgsRest(__VLS_162));
var __VLS_160;
var __VLS_132;
var __VLS_124;
var __VLS_3;
var __VLS_dollars;
const __VLS_self = (await import('vue')).defineComponent({
    setup() {
        return {
            loading: loading,
            loadingData: loadingData,
            typeData: typeData,
            dataData: dataData,
            dataQuery: dataQuery,
            typeColumns: typeColumns,
            dataColumns: dataColumns,
            typeModalVisible: typeModalVisible,
            typeModalTitle: typeModalTitle,
            typeForm: typeForm,
            dataModalVisible: dataModalVisible,
            dataForm: dataForm,
            loadData: loadData,
            onTypePageChange: onTypePageChange,
            onDataPageChange: onDataPageChange,
            showTypeAdd: showTypeAdd,
            showTypeEdit: showTypeEdit,
            saveType: saveType,
            showDataAdd: showDataAdd,
            showDataEdit: showDataEdit,
            saveData: saveData,
            handleTypeDelete: handleTypeDelete,
            handleDataDelete: handleDataDelete,
        };
    },
});
export default (await import('vue')).defineComponent({
    setup() {
        return {};
    },
});
; /* PartiallyEnd: #4569/main.vue */
