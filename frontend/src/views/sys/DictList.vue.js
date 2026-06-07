import { ref, reactive, onMounted, computed } from 'vue';
import { dictApi } from '@/api';
import { message } from 'ant-design-vue';
const activeTab = ref('type');
const loading = ref(false);
const loadingData = ref(false);
const typeSaving = ref(false);
const dataSaving = ref(false);
const typeData = reactive({ list: [], total: 0, pageNo: 1, pageSize: 20 });
const dataData = reactive({ list: [], total: 0, pageNo: 1, pageSize: 20 });
const dataQuery = reactive({ dictType: '', dictLabel: '' });
const typeColumns = [
    { title: '编码', dataIndex: 'dictTypeCode' },
    { title: '名称', dataIndex: 'dictName' },
    { title: '系统', dataIndex: 'isSys', width: 60 },
    { title: '状态', key: 'status', width: 70 },
    { title: '排序', dataIndex: 'sort', width: 60 },
    { title: '操作', key: 'action', width: 200 }
];
const dataColumns = [
    { title: '标签', dataIndex: 'dictLabel' },
    { title: '数据值', dataIndex: 'dictValue' },
    { title: '排序', dataIndex: 'sort', width: 60 },
    { title: '状态', key: 'status', width: 70 },
    { title: '操作', key: 'action', width: 200 }
];
const typeModalVisible = ref(false);
const typeModalTitle = ref('新增字典类型');
const isTypeEdit = computed(() => typeModalTitle.value === '编辑字典类型');
const typeForm = reactive({ dictTypeCode: '', dictName: '', isSysBool: '0', sort: 0 });
const dataModalVisible = ref(false);
const dataModalTitle = ref('新增字典数据');
const dataForm = reactive({ dictCode: undefined, dictType: '', dictLabel: '', dictValue: '', parentCode: undefined, sort: 0 });
async function loadType() { loading.value = true; const r = await dictApi.typeList({ pageNo: typeData.pageNo, pageSize: typeData.pageSize, entity: {} }); if (r.data) {
    typeData.list = r.data.list;
    typeData.total = r.data.total;
} loading.value = false; }
async function loadData() { if (!dataQuery.dictType) {
    dataData.list = [];
    dataData.total = 0;
    return;
} loadingData.value = true; const r = await dictApi.dataList({ pageNo: dataData.pageNo, pageSize: dataData.pageSize, entity: { dictType: dataQuery.dictType, dictLabel: dataQuery.dictLabel } }); if (r.data) {
    dataData.list = r.data.list;
    dataData.total = r.data.total;
} loadingData.value = false; }
function onTypePageChange(p, s) { typeData.pageNo = p; typeData.pageSize = s; loadType(); }
function onDataPageChange(p, s) { dataData.pageNo = p; dataData.pageSize = s; loadData(); }
function selectTypeAndSwitch(code) { dataQuery.dictType = code; activeTab.value = 'data'; loadData(); }
function showTypeAdd() { typeModalTitle.value = '新增字典类型'; typeForm.dictTypeCode = ''; typeForm.dictName = ''; typeForm.isSysBool = '0'; typeForm.sort = 0; typeModalVisible.value = true; }
function showTypeEdit(r) { typeModalTitle.value = '编辑字典类型'; typeForm.dictTypeCode = r.dictTypeCode; typeForm.dictName = r.dictName; typeForm.isSysBool = r.isSys || '0'; typeForm.sort = r.sort || 0; typeModalVisible.value = true; }
async function saveType() { typeSaving.value = true; await dictApi.typeSave({ dictTypeCode: typeForm.dictTypeCode || undefined, dictName: typeForm.dictName, isSys: typeForm.isSysBool, sort: typeForm.sort }); message.success('保存成功'); typeModalVisible.value = false; typeSaving.value = false; loadType(); }
function showDataAdd() { dataModalTitle.value = '新增字典数据'; resetDataForm(); dataForm.dictType = dataQuery.dictType; dataModalVisible.value = true; }
function showDataAddChild(parent) { dataModalTitle.value = '新增字典数据'; resetDataForm(); dataForm.dictType = parent.dictType; dataForm.parentCode = parent.dictCode; dataModalVisible.value = true; }
function showDataEdit(r) { dataModalTitle.value = '编辑字典数据'; dataForm.dictCode = r.dictCode; dataForm.dictType = r.dictType; dataForm.dictLabel = r.dictLabel; dataForm.dictValue = r.dictValue; dataForm.parentCode = r.parentCode || undefined; dataForm.sort = r.sort || 0; dataModalVisible.value = true; }
function resetDataForm() { dataForm.dictCode = undefined; dataForm.dictType = ''; dataForm.dictLabel = ''; dataForm.dictValue = ''; dataForm.parentCode = undefined; dataForm.sort = 0; }
async function saveData() { dataSaving.value = true; await dictApi.dataSave({ dictCode: dataForm.dictCode, dictType: dataForm.dictType, dictLabel: dataForm.dictLabel, dictValue: dataForm.dictValue, parentCode: dataForm.parentCode, sort: dataForm.sort }); message.success('保存成功'); dataModalVisible.value = false; dataSaving.value = false; loadData(); }
async function handleTypeDelete(c) { await dictApi.typeDelete(c); message.success('删除成功'); loadType(); }
async function handleDataDelete(c) { await dictApi.dataDelete(c); message.success('删除成功'); loadData(); }
onMounted(loadType);
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
const __VLS_6 = __VLS_asFunctionalComponent(__VLS_5, new __VLS_5({
    activeKey: (__VLS_ctx.activeTab),
}));
const __VLS_7 = __VLS_6({
    activeKey: (__VLS_ctx.activeTab),
}, ...__VLS_functionalComponentArgsRest(__VLS_6));
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
    if (column.key === 'status') {
        const __VLS_25 = {}.ATag;
        /** @type {[typeof __VLS_components.ATag, typeof __VLS_components.aTag, typeof __VLS_components.ATag, typeof __VLS_components.aTag, ]} */ ;
        // @ts-ignore
        const __VLS_26 = __VLS_asFunctionalComponent(__VLS_25, new __VLS_25({
            color: (record.status === '0' ? 'green' : 'red'),
        }));
        const __VLS_27 = __VLS_26({
            color: (record.status === '0' ? 'green' : 'red'),
        }, ...__VLS_functionalComponentArgsRest(__VLS_26));
        __VLS_28.slots.default;
        (record.status === '0' ? '正常' : '停用');
        var __VLS_28;
    }
    if (column.key === 'action') {
        const __VLS_29 = {}.ASpace;
        /** @type {[typeof __VLS_components.ASpace, typeof __VLS_components.aSpace, typeof __VLS_components.ASpace, typeof __VLS_components.aSpace, ]} */ ;
        // @ts-ignore
        const __VLS_30 = __VLS_asFunctionalComponent(__VLS_29, new __VLS_29({}));
        const __VLS_31 = __VLS_30({}, ...__VLS_functionalComponentArgsRest(__VLS_30));
        __VLS_32.slots.default;
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
                    __VLS_ctx.selectTypeAndSwitch(record.dictTypeCode);
                } },
        });
        const __VLS_33 = {}.APopconfirm;
        /** @type {[typeof __VLS_components.APopconfirm, typeof __VLS_components.aPopconfirm, typeof __VLS_components.APopconfirm, typeof __VLS_components.aPopconfirm, ]} */ ;
        // @ts-ignore
        const __VLS_34 = __VLS_asFunctionalComponent(__VLS_33, new __VLS_33({
            ...{ 'onConfirm': {} },
            title: "确定删除?",
        }));
        const __VLS_35 = __VLS_34({
            ...{ 'onConfirm': {} },
            title: "确定删除?",
        }, ...__VLS_functionalComponentArgsRest(__VLS_34));
        let __VLS_37;
        let __VLS_38;
        let __VLS_39;
        const __VLS_40 = {
            onConfirm: (...[$event]) => {
                if (!(column.key === 'action'))
                    return;
                __VLS_ctx.handleTypeDelete(record.dictTypeCode);
            }
        };
        __VLS_36.slots.default;
        __VLS_asFunctionalElement(__VLS_intrinsicElements.a, __VLS_intrinsicElements.a)({
            ...{ style: {} },
        });
        var __VLS_36;
        var __VLS_32;
    }
}
var __VLS_24;
var __VLS_12;
const __VLS_41 = {}.ATabPane;
/** @type {[typeof __VLS_components.ATabPane, typeof __VLS_components.aTabPane, typeof __VLS_components.ATabPane, typeof __VLS_components.aTabPane, ]} */ ;
// @ts-ignore
const __VLS_42 = __VLS_asFunctionalComponent(__VLS_41, new __VLS_41({
    key: "data",
    tab: "字典数据",
    forceRender: true,
}));
const __VLS_43 = __VLS_42({
    key: "data",
    tab: "字典数据",
    forceRender: true,
}, ...__VLS_functionalComponentArgsRest(__VLS_42));
__VLS_44.slots.default;
const __VLS_45 = {}.AForm;
/** @type {[typeof __VLS_components.AForm, typeof __VLS_components.aForm, typeof __VLS_components.AForm, typeof __VLS_components.aForm, ]} */ ;
// @ts-ignore
const __VLS_46 = __VLS_asFunctionalComponent(__VLS_45, new __VLS_45({
    layout: "inline",
    ...{ style: {} },
}));
const __VLS_47 = __VLS_46({
    layout: "inline",
    ...{ style: {} },
}, ...__VLS_functionalComponentArgsRest(__VLS_46));
__VLS_48.slots.default;
const __VLS_49 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_50 = __VLS_asFunctionalComponent(__VLS_49, new __VLS_49({
    label: "字典类型",
}));
const __VLS_51 = __VLS_50({
    label: "字典类型",
}, ...__VLS_functionalComponentArgsRest(__VLS_50));
__VLS_52.slots.default;
const __VLS_53 = {}.ASelect;
/** @type {[typeof __VLS_components.ASelect, typeof __VLS_components.aSelect, typeof __VLS_components.ASelect, typeof __VLS_components.aSelect, ]} */ ;
// @ts-ignore
const __VLS_54 = __VLS_asFunctionalComponent(__VLS_53, new __VLS_53({
    ...{ 'onChange': {} },
    value: (__VLS_ctx.dataQuery.dictType),
    ...{ style: {} },
    allowClear: true,
    placeholder: "选择字典类型",
}));
const __VLS_55 = __VLS_54({
    ...{ 'onChange': {} },
    value: (__VLS_ctx.dataQuery.dictType),
    ...{ style: {} },
    allowClear: true,
    placeholder: "选择字典类型",
}, ...__VLS_functionalComponentArgsRest(__VLS_54));
let __VLS_57;
let __VLS_58;
let __VLS_59;
const __VLS_60 = {
    onChange: (__VLS_ctx.loadData)
};
__VLS_56.slots.default;
for (const [t] of __VLS_getVForSourceType((__VLS_ctx.typeData.list))) {
    const __VLS_61 = {}.ASelectOption;
    /** @type {[typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, ]} */ ;
    // @ts-ignore
    const __VLS_62 = __VLS_asFunctionalComponent(__VLS_61, new __VLS_61({
        key: (t.dictTypeCode),
        value: (t.dictTypeCode),
    }));
    const __VLS_63 = __VLS_62({
        key: (t.dictTypeCode),
        value: (t.dictTypeCode),
    }, ...__VLS_functionalComponentArgsRest(__VLS_62));
    __VLS_64.slots.default;
    (t.dictTypeCode);
    (t.dictName);
    var __VLS_64;
}
var __VLS_56;
var __VLS_52;
const __VLS_65 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_66 = __VLS_asFunctionalComponent(__VLS_65, new __VLS_65({
    label: "标签",
}));
const __VLS_67 = __VLS_66({
    label: "标签",
}, ...__VLS_functionalComponentArgsRest(__VLS_66));
__VLS_68.slots.default;
const __VLS_69 = {}.AInput;
/** @type {[typeof __VLS_components.AInput, typeof __VLS_components.aInput, ]} */ ;
// @ts-ignore
const __VLS_70 = __VLS_asFunctionalComponent(__VLS_69, new __VLS_69({
    value: (__VLS_ctx.dataQuery.dictLabel),
    placeholder: "搜索标签",
}));
const __VLS_71 = __VLS_70({
    value: (__VLS_ctx.dataQuery.dictLabel),
    placeholder: "搜索标签",
}, ...__VLS_functionalComponentArgsRest(__VLS_70));
var __VLS_68;
const __VLS_73 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_74 = __VLS_asFunctionalComponent(__VLS_73, new __VLS_73({}));
const __VLS_75 = __VLS_74({}, ...__VLS_functionalComponentArgsRest(__VLS_74));
__VLS_76.slots.default;
const __VLS_77 = {}.AButton;
/** @type {[typeof __VLS_components.AButton, typeof __VLS_components.aButton, typeof __VLS_components.AButton, typeof __VLS_components.aButton, ]} */ ;
// @ts-ignore
const __VLS_78 = __VLS_asFunctionalComponent(__VLS_77, new __VLS_77({
    ...{ 'onClick': {} },
}));
const __VLS_79 = __VLS_78({
    ...{ 'onClick': {} },
}, ...__VLS_functionalComponentArgsRest(__VLS_78));
let __VLS_81;
let __VLS_82;
let __VLS_83;
const __VLS_84 = {
    onClick: (__VLS_ctx.loadData)
};
__VLS_80.slots.default;
var __VLS_80;
var __VLS_76;
const __VLS_85 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_86 = __VLS_asFunctionalComponent(__VLS_85, new __VLS_85({}));
const __VLS_87 = __VLS_86({}, ...__VLS_functionalComponentArgsRest(__VLS_86));
__VLS_88.slots.default;
const __VLS_89 = {}.AButton;
/** @type {[typeof __VLS_components.AButton, typeof __VLS_components.aButton, typeof __VLS_components.AButton, typeof __VLS_components.aButton, ]} */ ;
// @ts-ignore
const __VLS_90 = __VLS_asFunctionalComponent(__VLS_89, new __VLS_89({
    ...{ 'onClick': {} },
    type: "primary",
}));
const __VLS_91 = __VLS_90({
    ...{ 'onClick': {} },
    type: "primary",
}, ...__VLS_functionalComponentArgsRest(__VLS_90));
let __VLS_93;
let __VLS_94;
let __VLS_95;
const __VLS_96 = {
    onClick: (__VLS_ctx.showDataAdd)
};
__VLS_92.slots.default;
var __VLS_92;
var __VLS_88;
var __VLS_48;
const __VLS_97 = {}.ATable;
/** @type {[typeof __VLS_components.ATable, typeof __VLS_components.aTable, typeof __VLS_components.ATable, typeof __VLS_components.aTable, ]} */ ;
// @ts-ignore
const __VLS_98 = __VLS_asFunctionalComponent(__VLS_97, new __VLS_97({
    dataSource: (__VLS_ctx.dataData.list),
    columns: (__VLS_ctx.dataColumns),
    loading: (__VLS_ctx.loadingData),
    rowKey: "dictCode",
    pagination: ({ current: __VLS_ctx.dataData.pageNo, pageSize: __VLS_ctx.dataData.pageSize, total: __VLS_ctx.dataData.total, onChange: __VLS_ctx.onDataPageChange }),
    expandable: ({ childrenColumnName: 'children', rowExpandable: (r) => r.children?.length > 0 }),
}));
const __VLS_99 = __VLS_98({
    dataSource: (__VLS_ctx.dataData.list),
    columns: (__VLS_ctx.dataColumns),
    loading: (__VLS_ctx.loadingData),
    rowKey: "dictCode",
    pagination: ({ current: __VLS_ctx.dataData.pageNo, pageSize: __VLS_ctx.dataData.pageSize, total: __VLS_ctx.dataData.total, onChange: __VLS_ctx.onDataPageChange }),
    expandable: ({ childrenColumnName: 'children', rowExpandable: (r) => r.children?.length > 0 }),
}, ...__VLS_functionalComponentArgsRest(__VLS_98));
__VLS_100.slots.default;
{
    const { bodyCell: __VLS_thisSlot } = __VLS_100.slots;
    const [{ record, column }] = __VLS_getSlotParams(__VLS_thisSlot);
    if (column.key === 'status') {
        const __VLS_101 = {}.ATag;
        /** @type {[typeof __VLS_components.ATag, typeof __VLS_components.aTag, typeof __VLS_components.ATag, typeof __VLS_components.aTag, ]} */ ;
        // @ts-ignore
        const __VLS_102 = __VLS_asFunctionalComponent(__VLS_101, new __VLS_101({
            color: (record.status === '0' ? 'green' : 'red'),
        }));
        const __VLS_103 = __VLS_102({
            color: (record.status === '0' ? 'green' : 'red'),
        }, ...__VLS_functionalComponentArgsRest(__VLS_102));
        __VLS_104.slots.default;
        (record.status === '0' ? '正常' : '停用');
        var __VLS_104;
    }
    if (column.key === 'action') {
        const __VLS_105 = {}.ASpace;
        /** @type {[typeof __VLS_components.ASpace, typeof __VLS_components.aSpace, typeof __VLS_components.ASpace, typeof __VLS_components.aSpace, ]} */ ;
        // @ts-ignore
        const __VLS_106 = __VLS_asFunctionalComponent(__VLS_105, new __VLS_105({}));
        const __VLS_107 = __VLS_106({}, ...__VLS_functionalComponentArgsRest(__VLS_106));
        __VLS_108.slots.default;
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
                    __VLS_ctx.showDataAddChild(record);
                } },
        });
        const __VLS_109 = {}.APopconfirm;
        /** @type {[typeof __VLS_components.APopconfirm, typeof __VLS_components.aPopconfirm, typeof __VLS_components.APopconfirm, typeof __VLS_components.aPopconfirm, ]} */ ;
        // @ts-ignore
        const __VLS_110 = __VLS_asFunctionalComponent(__VLS_109, new __VLS_109({
            ...{ 'onConfirm': {} },
            title: "确定删除?",
        }));
        const __VLS_111 = __VLS_110({
            ...{ 'onConfirm': {} },
            title: "确定删除?",
        }, ...__VLS_functionalComponentArgsRest(__VLS_110));
        let __VLS_113;
        let __VLS_114;
        let __VLS_115;
        const __VLS_116 = {
            onConfirm: (...[$event]) => {
                if (!(column.key === 'action'))
                    return;
                __VLS_ctx.handleDataDelete(record.dictCode);
            }
        };
        __VLS_112.slots.default;
        __VLS_asFunctionalElement(__VLS_intrinsicElements.a, __VLS_intrinsicElements.a)({
            ...{ style: {} },
        });
        var __VLS_112;
        var __VLS_108;
    }
}
var __VLS_100;
var __VLS_44;
var __VLS_8;
const __VLS_117 = {}.AModal;
/** @type {[typeof __VLS_components.AModal, typeof __VLS_components.aModal, typeof __VLS_components.AModal, typeof __VLS_components.aModal, ]} */ ;
// @ts-ignore
const __VLS_118 = __VLS_asFunctionalComponent(__VLS_117, new __VLS_117({
    ...{ 'onOk': {} },
    open: (__VLS_ctx.typeModalVisible),
    title: (__VLS_ctx.typeModalTitle),
    confirmLoading: (__VLS_ctx.typeSaving),
}));
const __VLS_119 = __VLS_118({
    ...{ 'onOk': {} },
    open: (__VLS_ctx.typeModalVisible),
    title: (__VLS_ctx.typeModalTitle),
    confirmLoading: (__VLS_ctx.typeSaving),
}, ...__VLS_functionalComponentArgsRest(__VLS_118));
let __VLS_121;
let __VLS_122;
let __VLS_123;
const __VLS_124 = {
    onOk: (__VLS_ctx.saveType)
};
__VLS_120.slots.default;
const __VLS_125 = {}.AForm;
/** @type {[typeof __VLS_components.AForm, typeof __VLS_components.aForm, typeof __VLS_components.AForm, typeof __VLS_components.aForm, ]} */ ;
// @ts-ignore
const __VLS_126 = __VLS_asFunctionalComponent(__VLS_125, new __VLS_125({
    layout: "vertical",
}));
const __VLS_127 = __VLS_126({
    layout: "vertical",
}, ...__VLS_functionalComponentArgsRest(__VLS_126));
__VLS_128.slots.default;
const __VLS_129 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_130 = __VLS_asFunctionalComponent(__VLS_129, new __VLS_129({
    label: "编码",
}));
const __VLS_131 = __VLS_130({
    label: "编码",
}, ...__VLS_functionalComponentArgsRest(__VLS_130));
__VLS_132.slots.default;
const __VLS_133 = {}.AInput;
/** @type {[typeof __VLS_components.AInput, typeof __VLS_components.aInput, ]} */ ;
// @ts-ignore
const __VLS_134 = __VLS_asFunctionalComponent(__VLS_133, new __VLS_133({
    value: (__VLS_ctx.typeForm.dictTypeCode),
    disabled: (__VLS_ctx.isTypeEdit),
}));
const __VLS_135 = __VLS_134({
    value: (__VLS_ctx.typeForm.dictTypeCode),
    disabled: (__VLS_ctx.isTypeEdit),
}, ...__VLS_functionalComponentArgsRest(__VLS_134));
var __VLS_132;
const __VLS_137 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_138 = __VLS_asFunctionalComponent(__VLS_137, new __VLS_137({
    label: "名称",
}));
const __VLS_139 = __VLS_138({
    label: "名称",
}, ...__VLS_functionalComponentArgsRest(__VLS_138));
__VLS_140.slots.default;
const __VLS_141 = {}.AInput;
/** @type {[typeof __VLS_components.AInput, typeof __VLS_components.aInput, ]} */ ;
// @ts-ignore
const __VLS_142 = __VLS_asFunctionalComponent(__VLS_141, new __VLS_141({
    value: (__VLS_ctx.typeForm.dictName),
}));
const __VLS_143 = __VLS_142({
    value: (__VLS_ctx.typeForm.dictName),
}, ...__VLS_functionalComponentArgsRest(__VLS_142));
var __VLS_140;
const __VLS_145 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_146 = __VLS_asFunctionalComponent(__VLS_145, new __VLS_145({
    label: "系统字典",
}));
const __VLS_147 = __VLS_146({
    label: "系统字典",
}, ...__VLS_functionalComponentArgsRest(__VLS_146));
__VLS_148.slots.default;
const __VLS_149 = {}.ASwitch;
/** @type {[typeof __VLS_components.ASwitch, typeof __VLS_components.aSwitch, ]} */ ;
// @ts-ignore
const __VLS_150 = __VLS_asFunctionalComponent(__VLS_149, new __VLS_149({
    checked: (__VLS_ctx.typeForm.isSysBool),
    checkedValue: "1",
    unCheckedValue: "0",
}));
const __VLS_151 = __VLS_150({
    checked: (__VLS_ctx.typeForm.isSysBool),
    checkedValue: "1",
    unCheckedValue: "0",
}, ...__VLS_functionalComponentArgsRest(__VLS_150));
var __VLS_148;
const __VLS_153 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_154 = __VLS_asFunctionalComponent(__VLS_153, new __VLS_153({
    label: "排序",
}));
const __VLS_155 = __VLS_154({
    label: "排序",
}, ...__VLS_functionalComponentArgsRest(__VLS_154));
__VLS_156.slots.default;
const __VLS_157 = {}.AInputNumber;
/** @type {[typeof __VLS_components.AInputNumber, typeof __VLS_components.aInputNumber, ]} */ ;
// @ts-ignore
const __VLS_158 = __VLS_asFunctionalComponent(__VLS_157, new __VLS_157({
    value: (__VLS_ctx.typeForm.sort),
    ...{ style: {} },
}));
const __VLS_159 = __VLS_158({
    value: (__VLS_ctx.typeForm.sort),
    ...{ style: {} },
}, ...__VLS_functionalComponentArgsRest(__VLS_158));
var __VLS_156;
var __VLS_128;
var __VLS_120;
const __VLS_161 = {}.AModal;
/** @type {[typeof __VLS_components.AModal, typeof __VLS_components.aModal, typeof __VLS_components.AModal, typeof __VLS_components.aModal, ]} */ ;
// @ts-ignore
const __VLS_162 = __VLS_asFunctionalComponent(__VLS_161, new __VLS_161({
    ...{ 'onOk': {} },
    open: (__VLS_ctx.dataModalVisible),
    title: (__VLS_ctx.dataModalTitle),
    confirmLoading: (__VLS_ctx.dataSaving),
    width: "600px",
}));
const __VLS_163 = __VLS_162({
    ...{ 'onOk': {} },
    open: (__VLS_ctx.dataModalVisible),
    title: (__VLS_ctx.dataModalTitle),
    confirmLoading: (__VLS_ctx.dataSaving),
    width: "600px",
}, ...__VLS_functionalComponentArgsRest(__VLS_162));
let __VLS_165;
let __VLS_166;
let __VLS_167;
const __VLS_168 = {
    onOk: (__VLS_ctx.saveData)
};
__VLS_164.slots.default;
const __VLS_169 = {}.AForm;
/** @type {[typeof __VLS_components.AForm, typeof __VLS_components.aForm, typeof __VLS_components.AForm, typeof __VLS_components.aForm, ]} */ ;
// @ts-ignore
const __VLS_170 = __VLS_asFunctionalComponent(__VLS_169, new __VLS_169({
    layout: "vertical",
}));
const __VLS_171 = __VLS_170({
    layout: "vertical",
}, ...__VLS_functionalComponentArgsRest(__VLS_170));
__VLS_172.slots.default;
const __VLS_173 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_174 = __VLS_asFunctionalComponent(__VLS_173, new __VLS_173({
    label: "字典类型",
}));
const __VLS_175 = __VLS_174({
    label: "字典类型",
}, ...__VLS_functionalComponentArgsRest(__VLS_174));
__VLS_176.slots.default;
const __VLS_177 = {}.ASelect;
/** @type {[typeof __VLS_components.ASelect, typeof __VLS_components.aSelect, typeof __VLS_components.ASelect, typeof __VLS_components.aSelect, ]} */ ;
// @ts-ignore
const __VLS_178 = __VLS_asFunctionalComponent(__VLS_177, new __VLS_177({
    value: (__VLS_ctx.dataForm.dictType),
    ...{ style: {} },
}));
const __VLS_179 = __VLS_178({
    value: (__VLS_ctx.dataForm.dictType),
    ...{ style: {} },
}, ...__VLS_functionalComponentArgsRest(__VLS_178));
__VLS_180.slots.default;
for (const [t] of __VLS_getVForSourceType((__VLS_ctx.typeData.list))) {
    const __VLS_181 = {}.ASelectOption;
    /** @type {[typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, ]} */ ;
    // @ts-ignore
    const __VLS_182 = __VLS_asFunctionalComponent(__VLS_181, new __VLS_181({
        key: (t.dictTypeCode),
        value: (t.dictTypeCode),
    }));
    const __VLS_183 = __VLS_182({
        key: (t.dictTypeCode),
        value: (t.dictTypeCode),
    }, ...__VLS_functionalComponentArgsRest(__VLS_182));
    __VLS_184.slots.default;
    (t.dictTypeCode);
    var __VLS_184;
}
var __VLS_180;
var __VLS_176;
const __VLS_185 = {}.ARow;
/** @type {[typeof __VLS_components.ARow, typeof __VLS_components.aRow, typeof __VLS_components.ARow, typeof __VLS_components.aRow, ]} */ ;
// @ts-ignore
const __VLS_186 = __VLS_asFunctionalComponent(__VLS_185, new __VLS_185({
    gutter: (16),
}));
const __VLS_187 = __VLS_186({
    gutter: (16),
}, ...__VLS_functionalComponentArgsRest(__VLS_186));
__VLS_188.slots.default;
const __VLS_189 = {}.ACol;
/** @type {[typeof __VLS_components.ACol, typeof __VLS_components.aCol, typeof __VLS_components.ACol, typeof __VLS_components.aCol, ]} */ ;
// @ts-ignore
const __VLS_190 = __VLS_asFunctionalComponent(__VLS_189, new __VLS_189({
    span: (12),
}));
const __VLS_191 = __VLS_190({
    span: (12),
}, ...__VLS_functionalComponentArgsRest(__VLS_190));
__VLS_192.slots.default;
const __VLS_193 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_194 = __VLS_asFunctionalComponent(__VLS_193, new __VLS_193({
    label: "标签",
}));
const __VLS_195 = __VLS_194({
    label: "标签",
}, ...__VLS_functionalComponentArgsRest(__VLS_194));
__VLS_196.slots.default;
const __VLS_197 = {}.AInput;
/** @type {[typeof __VLS_components.AInput, typeof __VLS_components.aInput, ]} */ ;
// @ts-ignore
const __VLS_198 = __VLS_asFunctionalComponent(__VLS_197, new __VLS_197({
    value: (__VLS_ctx.dataForm.dictLabel),
}));
const __VLS_199 = __VLS_198({
    value: (__VLS_ctx.dataForm.dictLabel),
}, ...__VLS_functionalComponentArgsRest(__VLS_198));
var __VLS_196;
var __VLS_192;
const __VLS_201 = {}.ACol;
/** @type {[typeof __VLS_components.ACol, typeof __VLS_components.aCol, typeof __VLS_components.ACol, typeof __VLS_components.aCol, ]} */ ;
// @ts-ignore
const __VLS_202 = __VLS_asFunctionalComponent(__VLS_201, new __VLS_201({
    span: (12),
}));
const __VLS_203 = __VLS_202({
    span: (12),
}, ...__VLS_functionalComponentArgsRest(__VLS_202));
__VLS_204.slots.default;
const __VLS_205 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_206 = __VLS_asFunctionalComponent(__VLS_205, new __VLS_205({
    label: "数据值",
}));
const __VLS_207 = __VLS_206({
    label: "数据值",
}, ...__VLS_functionalComponentArgsRest(__VLS_206));
__VLS_208.slots.default;
const __VLS_209 = {}.AInput;
/** @type {[typeof __VLS_components.AInput, typeof __VLS_components.aInput, ]} */ ;
// @ts-ignore
const __VLS_210 = __VLS_asFunctionalComponent(__VLS_209, new __VLS_209({
    value: (__VLS_ctx.dataForm.dictValue),
}));
const __VLS_211 = __VLS_210({
    value: (__VLS_ctx.dataForm.dictValue),
}, ...__VLS_functionalComponentArgsRest(__VLS_210));
var __VLS_208;
var __VLS_204;
var __VLS_188;
if (__VLS_ctx.dataData.list.length) {
    const __VLS_213 = {}.AFormItem;
    /** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
    // @ts-ignore
    const __VLS_214 = __VLS_asFunctionalComponent(__VLS_213, new __VLS_213({
        label: "父级",
    }));
    const __VLS_215 = __VLS_214({
        label: "父级",
    }, ...__VLS_functionalComponentArgsRest(__VLS_214));
    __VLS_216.slots.default;
    const __VLS_217 = {}.ATreeSelect;
    /** @type {[typeof __VLS_components.ATreeSelect, typeof __VLS_components.aTreeSelect, ]} */ ;
    // @ts-ignore
    const __VLS_218 = __VLS_asFunctionalComponent(__VLS_217, new __VLS_217({
        value: (__VLS_ctx.dataForm.parentCode),
        treeData: (__VLS_ctx.dataData.list),
        allowClear: true,
        placeholder: "顶级",
        fieldNames: ({ label: 'dictLabel', value: 'dictCode', children: 'children' }),
        ...{ style: {} },
    }));
    const __VLS_219 = __VLS_218({
        value: (__VLS_ctx.dataForm.parentCode),
        treeData: (__VLS_ctx.dataData.list),
        allowClear: true,
        placeholder: "顶级",
        fieldNames: ({ label: 'dictLabel', value: 'dictCode', children: 'children' }),
        ...{ style: {} },
    }, ...__VLS_functionalComponentArgsRest(__VLS_218));
    var __VLS_216;
}
const __VLS_221 = {}.ARow;
/** @type {[typeof __VLS_components.ARow, typeof __VLS_components.aRow, typeof __VLS_components.ARow, typeof __VLS_components.aRow, ]} */ ;
// @ts-ignore
const __VLS_222 = __VLS_asFunctionalComponent(__VLS_221, new __VLS_221({
    gutter: (16),
}));
const __VLS_223 = __VLS_222({
    gutter: (16),
}, ...__VLS_functionalComponentArgsRest(__VLS_222));
__VLS_224.slots.default;
const __VLS_225 = {}.ACol;
/** @type {[typeof __VLS_components.ACol, typeof __VLS_components.aCol, typeof __VLS_components.ACol, typeof __VLS_components.aCol, ]} */ ;
// @ts-ignore
const __VLS_226 = __VLS_asFunctionalComponent(__VLS_225, new __VLS_225({
    span: (12),
}));
const __VLS_227 = __VLS_226({
    span: (12),
}, ...__VLS_functionalComponentArgsRest(__VLS_226));
__VLS_228.slots.default;
const __VLS_229 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_230 = __VLS_asFunctionalComponent(__VLS_229, new __VLS_229({
    label: "排序",
}));
const __VLS_231 = __VLS_230({
    label: "排序",
}, ...__VLS_functionalComponentArgsRest(__VLS_230));
__VLS_232.slots.default;
const __VLS_233 = {}.AInputNumber;
/** @type {[typeof __VLS_components.AInputNumber, typeof __VLS_components.aInputNumber, ]} */ ;
// @ts-ignore
const __VLS_234 = __VLS_asFunctionalComponent(__VLS_233, new __VLS_233({
    value: (__VLS_ctx.dataForm.sort),
    ...{ style: {} },
}));
const __VLS_235 = __VLS_234({
    value: (__VLS_ctx.dataForm.sort),
    ...{ style: {} },
}, ...__VLS_functionalComponentArgsRest(__VLS_234));
var __VLS_232;
var __VLS_228;
var __VLS_224;
var __VLS_172;
var __VLS_164;
var __VLS_3;
var __VLS_dollars;
const __VLS_self = (await import('vue')).defineComponent({
    setup() {
        return {
            activeTab: activeTab,
            loading: loading,
            loadingData: loadingData,
            typeSaving: typeSaving,
            dataSaving: dataSaving,
            typeData: typeData,
            dataData: dataData,
            dataQuery: dataQuery,
            typeColumns: typeColumns,
            dataColumns: dataColumns,
            typeModalVisible: typeModalVisible,
            typeModalTitle: typeModalTitle,
            isTypeEdit: isTypeEdit,
            typeForm: typeForm,
            dataModalVisible: dataModalVisible,
            dataModalTitle: dataModalTitle,
            dataForm: dataForm,
            loadData: loadData,
            onTypePageChange: onTypePageChange,
            onDataPageChange: onDataPageChange,
            selectTypeAndSwitch: selectTypeAndSwitch,
            showTypeAdd: showTypeAdd,
            showTypeEdit: showTypeEdit,
            saveType: saveType,
            showDataAdd: showDataAdd,
            showDataAddChild: showDataAddChild,
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
