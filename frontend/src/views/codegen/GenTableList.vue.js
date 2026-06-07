import { ref, reactive, onMounted } from 'vue';
import { codegenApi } from '@/api';
import { useRouter } from 'vue-router';
import { message } from 'ant-design-vue';
const router = useRouter();
const loading = ref(false);
const list = ref([]);
const pageNo = ref(1);
const pageSize = ref(10);
const total = ref(0);
const query = reactive({ tableName: '' });
const columns = [
    { title: '表名', dataIndex: 'tableName' }, { title: '类名', dataIndex: 'className' },
    { title: '模块', dataIndex: 'moduleCode' }, { title: '功能', dataIndex: 'functionName' },
    { title: '说明', dataIndex: 'tableComment', ellipsis: true },
    { title: '操作', key: 'action', width: 200 }
];
const importModal = ref(false);
const importing = ref(false);
const dbLoading = ref(false);
const dbTables = ref([]);
const selectedRowKeys = ref([]);
const previewModal = ref(false);
const previewTab = ref('');
const previewItems = ref([]);
async function load() {
    loading.value = true;
    const res = await codegenApi.list({ pageNo: pageNo.value, pageSize: pageSize.value, entity: query });
    if (res.data) {
        list.value = res.data.list;
        total.value = res.data.total;
    }
    loading.value = false;
}
async function showImport() {
    importModal.value = true;
    dbLoading.value = true;
    selectedRowKeys.value = [];
    const res = await codegenApi.dbTables();
    if (res.data)
        dbTables.value = res.data;
    dbLoading.value = false;
}
async function doImport() {
    if (!selectedRowKeys.value.length) {
        message.warning('请选择表');
        return;
    }
    importing.value = true;
    await codegenApi.importTables(selectedRowKeys.value);
    message.success('导入成功');
    importModal.value = false;
    importing.value = false;
    load();
}
async function previewDbColumns(record) {
    const res = await codegenApi.dbColumns(record.tableName);
    const cols = res.data?.map((c) => `${c.columnName} (${c.columnType} → ${c.netType})`).join('\n') || '';
    message.info(`表 ${record.tableName} 的字段:\n${cols}`, 6);
}
function editTable(record) { router.push(`/codegen/table/edit?tableName=${record.tableName}`); }
async function previewCode(record) {
    const res = await codegenApi.preview(record.tableName);
    if (res.data) {
        previewItems.value = res.data;
        previewTab.value = res.data[0]?.fileName || '';
        previewModal.value = true;
    }
}
async function del(record) { await codegenApi.delete(record.tableName); message.success('已删除'); load(); }
onMounted(load);
debugger; /* PartiallyEnd: #3632/scriptSetup.vue */
const __VLS_ctx = {};
let __VLS_components;
let __VLS_directives;
const __VLS_0 = {}.ACard;
/** @type {[typeof __VLS_components.ACard, typeof __VLS_components.aCard, typeof __VLS_components.ACard, typeof __VLS_components.aCard, ]} */ ;
// @ts-ignore
const __VLS_1 = __VLS_asFunctionalComponent(__VLS_0, new __VLS_0({
    title: "代码生成",
}));
const __VLS_2 = __VLS_1({
    title: "代码生成",
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
    label: "表名",
}));
const __VLS_11 = __VLS_10({
    label: "表名",
}, ...__VLS_functionalComponentArgsRest(__VLS_10));
__VLS_12.slots.default;
const __VLS_13 = {}.AInput;
/** @type {[typeof __VLS_components.AInput, typeof __VLS_components.aInput, ]} */ ;
// @ts-ignore
const __VLS_14 = __VLS_asFunctionalComponent(__VLS_13, new __VLS_13({
    value: (__VLS_ctx.query.tableName),
    placeholder: "搜索",
}));
const __VLS_15 = __VLS_14({
    value: (__VLS_ctx.query.tableName),
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
}));
const __VLS_23 = __VLS_22({
    ...{ 'onClick': {} },
}, ...__VLS_functionalComponentArgsRest(__VLS_22));
let __VLS_25;
let __VLS_26;
let __VLS_27;
const __VLS_28 = {
    onClick: (__VLS_ctx.load)
};
__VLS_24.slots.default;
var __VLS_24;
var __VLS_20;
const __VLS_29 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_30 = __VLS_asFunctionalComponent(__VLS_29, new __VLS_29({}));
const __VLS_31 = __VLS_30({}, ...__VLS_functionalComponentArgsRest(__VLS_30));
__VLS_32.slots.default;
const __VLS_33 = {}.AButton;
/** @type {[typeof __VLS_components.AButton, typeof __VLS_components.aButton, typeof __VLS_components.AButton, typeof __VLS_components.aButton, ]} */ ;
// @ts-ignore
const __VLS_34 = __VLS_asFunctionalComponent(__VLS_33, new __VLS_33({
    ...{ 'onClick': {} },
    type: "primary",
}));
const __VLS_35 = __VLS_34({
    ...{ 'onClick': {} },
    type: "primary",
}, ...__VLS_functionalComponentArgsRest(__VLS_34));
let __VLS_37;
let __VLS_38;
let __VLS_39;
const __VLS_40 = {
    onClick: (__VLS_ctx.showImport)
};
__VLS_36.slots.default;
var __VLS_36;
var __VLS_32;
var __VLS_8;
const __VLS_41 = {}.ATable;
/** @type {[typeof __VLS_components.ATable, typeof __VLS_components.aTable, typeof __VLS_components.ATable, typeof __VLS_components.aTable, ]} */ ;
// @ts-ignore
const __VLS_42 = __VLS_asFunctionalComponent(__VLS_41, new __VLS_41({
    dataSource: (__VLS_ctx.list),
    columns: (__VLS_ctx.columns),
    rowKey: "tableName",
    loading: (__VLS_ctx.loading),
    pagination: ({ current: __VLS_ctx.pageNo, pageSize: __VLS_ctx.pageSize, total: __VLS_ctx.total, onChange: (p) => { __VLS_ctx.pageNo = p; __VLS_ctx.load(); } }),
}));
const __VLS_43 = __VLS_42({
    dataSource: (__VLS_ctx.list),
    columns: (__VLS_ctx.columns),
    rowKey: "tableName",
    loading: (__VLS_ctx.loading),
    pagination: ({ current: __VLS_ctx.pageNo, pageSize: __VLS_ctx.pageSize, total: __VLS_ctx.total, onChange: (p) => { __VLS_ctx.pageNo = p; __VLS_ctx.load(); } }),
}, ...__VLS_functionalComponentArgsRest(__VLS_42));
__VLS_44.slots.default;
{
    const { bodyCell: __VLS_thisSlot } = __VLS_44.slots;
    const [{ record, column }] = __VLS_getSlotParams(__VLS_thisSlot);
    if (column.key === 'action') {
        const __VLS_45 = {}.ASpace;
        /** @type {[typeof __VLS_components.ASpace, typeof __VLS_components.aSpace, typeof __VLS_components.ASpace, typeof __VLS_components.aSpace, ]} */ ;
        // @ts-ignore
        const __VLS_46 = __VLS_asFunctionalComponent(__VLS_45, new __VLS_45({}));
        const __VLS_47 = __VLS_46({}, ...__VLS_functionalComponentArgsRest(__VLS_46));
        __VLS_48.slots.default;
        __VLS_asFunctionalElement(__VLS_intrinsicElements.a, __VLS_intrinsicElements.a)({
            ...{ onClick: (...[$event]) => {
                    if (!(column.key === 'action'))
                        return;
                    __VLS_ctx.editTable(record);
                } },
        });
        __VLS_asFunctionalElement(__VLS_intrinsicElements.a, __VLS_intrinsicElements.a)({
            ...{ onClick: (...[$event]) => {
                    if (!(column.key === 'action'))
                        return;
                    __VLS_ctx.previewCode(record);
                } },
        });
        const __VLS_49 = {}.APopconfirm;
        /** @type {[typeof __VLS_components.APopconfirm, typeof __VLS_components.aPopconfirm, typeof __VLS_components.APopconfirm, typeof __VLS_components.aPopconfirm, ]} */ ;
        // @ts-ignore
        const __VLS_50 = __VLS_asFunctionalComponent(__VLS_49, new __VLS_49({
            ...{ 'onConfirm': {} },
            title: "确定删除?",
        }));
        const __VLS_51 = __VLS_50({
            ...{ 'onConfirm': {} },
            title: "确定删除?",
        }, ...__VLS_functionalComponentArgsRest(__VLS_50));
        let __VLS_53;
        let __VLS_54;
        let __VLS_55;
        const __VLS_56 = {
            onConfirm: (...[$event]) => {
                if (!(column.key === 'action'))
                    return;
                __VLS_ctx.del(record);
            }
        };
        __VLS_52.slots.default;
        __VLS_asFunctionalElement(__VLS_intrinsicElements.a, __VLS_intrinsicElements.a)({
            ...{ style: {} },
        });
        var __VLS_52;
        var __VLS_48;
    }
}
var __VLS_44;
const __VLS_57 = {}.AModal;
/** @type {[typeof __VLS_components.AModal, typeof __VLS_components.aModal, typeof __VLS_components.AModal, typeof __VLS_components.aModal, ]} */ ;
// @ts-ignore
const __VLS_58 = __VLS_asFunctionalComponent(__VLS_57, new __VLS_57({
    ...{ 'onOk': {} },
    open: (__VLS_ctx.importModal),
    title: "从数据库导入",
    width: "800px",
    confirmLoading: (__VLS_ctx.importing),
}));
const __VLS_59 = __VLS_58({
    ...{ 'onOk': {} },
    open: (__VLS_ctx.importModal),
    title: "从数据库导入",
    width: "800px",
    confirmLoading: (__VLS_ctx.importing),
}, ...__VLS_functionalComponentArgsRest(__VLS_58));
let __VLS_61;
let __VLS_62;
let __VLS_63;
const __VLS_64 = {
    onOk: (__VLS_ctx.doImport)
};
__VLS_60.slots.default;
const __VLS_65 = {}.ATable;
/** @type {[typeof __VLS_components.ATable, typeof __VLS_components.aTable, typeof __VLS_components.ATable, typeof __VLS_components.aTable, ]} */ ;
// @ts-ignore
const __VLS_66 = __VLS_asFunctionalComponent(__VLS_65, new __VLS_65({
    dataSource: (__VLS_ctx.dbTables),
    rowKey: "tableName",
    loading: (__VLS_ctx.dbLoading),
    pagination: (false),
    size: "small",
    rowSelection: ({ selectedRowKeys: __VLS_ctx.selectedRowKeys, onChange: (keys) => { __VLS_ctx.selectedRowKeys = keys; } }),
}));
const __VLS_67 = __VLS_66({
    dataSource: (__VLS_ctx.dbTables),
    rowKey: "tableName",
    loading: (__VLS_ctx.dbLoading),
    pagination: (false),
    size: "small",
    rowSelection: ({ selectedRowKeys: __VLS_ctx.selectedRowKeys, onChange: (keys) => { __VLS_ctx.selectedRowKeys = keys; } }),
}, ...__VLS_functionalComponentArgsRest(__VLS_66));
__VLS_68.slots.default;
{
    const { bodyCell: __VLS_thisSlot } = __VLS_68.slots;
    const [{ record, column }] = __VLS_getSlotParams(__VLS_thisSlot);
    if (column.key === 'action') {
        __VLS_asFunctionalElement(__VLS_intrinsicElements.a, __VLS_intrinsicElements.a)({
            ...{ onClick: (...[$event]) => {
                    if (!(column.key === 'action'))
                        return;
                    __VLS_ctx.previewDbColumns(record);
                } },
        });
    }
}
var __VLS_68;
var __VLS_60;
const __VLS_69 = {}.AModal;
/** @type {[typeof __VLS_components.AModal, typeof __VLS_components.aModal, typeof __VLS_components.AModal, typeof __VLS_components.aModal, ]} */ ;
// @ts-ignore
const __VLS_70 = __VLS_asFunctionalComponent(__VLS_69, new __VLS_69({
    ...{ 'onOk': {} },
    open: (__VLS_ctx.previewModal),
    title: "代码预览",
    width: "90%",
    footer: (null),
}));
const __VLS_71 = __VLS_70({
    ...{ 'onOk': {} },
    open: (__VLS_ctx.previewModal),
    title: "代码预览",
    width: "90%",
    footer: (null),
}, ...__VLS_functionalComponentArgsRest(__VLS_70));
let __VLS_73;
let __VLS_74;
let __VLS_75;
const __VLS_76 = {
    onOk: (...[$event]) => {
        __VLS_ctx.previewModal = false;
    }
};
__VLS_72.slots.default;
const __VLS_77 = {}.ATabs;
/** @type {[typeof __VLS_components.ATabs, typeof __VLS_components.aTabs, typeof __VLS_components.ATabs, typeof __VLS_components.aTabs, ]} */ ;
// @ts-ignore
const __VLS_78 = __VLS_asFunctionalComponent(__VLS_77, new __VLS_77({
    activeKey: (__VLS_ctx.previewTab),
}));
const __VLS_79 = __VLS_78({
    activeKey: (__VLS_ctx.previewTab),
}, ...__VLS_functionalComponentArgsRest(__VLS_78));
__VLS_80.slots.default;
for (const [item] of __VLS_getVForSourceType((__VLS_ctx.previewItems))) {
    const __VLS_81 = {}.ATabPane;
    /** @type {[typeof __VLS_components.ATabPane, typeof __VLS_components.aTabPane, typeof __VLS_components.ATabPane, typeof __VLS_components.aTabPane, ]} */ ;
    // @ts-ignore
    const __VLS_82 = __VLS_asFunctionalComponent(__VLS_81, new __VLS_81({
        key: (item.fileName),
        tab: (item.fileName.split('/').pop() || item.fileName),
    }));
    const __VLS_83 = __VLS_82({
        key: (item.fileName),
        tab: (item.fileName.split('/').pop() || item.fileName),
    }, ...__VLS_functionalComponentArgsRest(__VLS_82));
    __VLS_84.slots.default;
    __VLS_asFunctionalElement(__VLS_intrinsicElements.pre, __VLS_intrinsicElements.pre)({
        ...{ style: {} },
    });
    __VLS_asFunctionalElement(__VLS_intrinsicElements.code, __VLS_intrinsicElements.code)({});
    (item.content);
    var __VLS_84;
}
var __VLS_80;
var __VLS_72;
var __VLS_3;
var __VLS_dollars;
const __VLS_self = (await import('vue')).defineComponent({
    setup() {
        return {
            loading: loading,
            list: list,
            pageNo: pageNo,
            pageSize: pageSize,
            total: total,
            query: query,
            columns: columns,
            importModal: importModal,
            importing: importing,
            dbLoading: dbLoading,
            dbTables: dbTables,
            selectedRowKeys: selectedRowKeys,
            previewModal: previewModal,
            previewTab: previewTab,
            previewItems: previewItems,
            load: load,
            showImport: showImport,
            doImport: doImport,
            previewDbColumns: previewDbColumns,
            editTable: editTable,
            previewCode: previewCode,
            del: del,
        };
    },
});
export default (await import('vue')).defineComponent({
    setup() {
        return {};
    },
});
; /* PartiallyEnd: #4569/main.vue */
