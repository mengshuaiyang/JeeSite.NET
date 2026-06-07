import { ref, reactive, onMounted } from 'vue';
import { categoryApi } from '@/api';
import { message } from 'ant-design-vue';
const loading = ref(false);
const saving = ref(false);
const modalOpen = ref(false);
const isEdit = ref(false);
const treeData = ref([]);
const form = reactive({ categoryName: '', categoryType: 'article', treeSort: 1000, parentCode: '0', link: '', keywords: '', description: '', isShowBool: '1', categoryCode: undefined });
const typeMap = { article: '文章', link: '链接', picture: '图片' };
const columns = [
    { title: '栏目名称', dataIndex: 'categoryName' },
    { title: '类型', key: 'type', width: 70 },
    { title: '排序', dataIndex: 'treeSort', width: 60 },
    { title: '显示', key: 'isShow', width: 60 },
    { title: '状态', key: 'status', width: 60 },
    { title: '操作', key: 'action', width: 200 }
];
async function loadData() {
    loading.value = true;
    const res = await categoryApi.tree();
    if (res.data)
        treeData.value = res.data;
    loading.value = false;
}
function showAdd() { isEdit.value = false; form.categoryCode = undefined; form.categoryName = ''; form.categoryType = 'article'; form.treeSort = 1000; form.parentCode = '0'; form.link = ''; form.keywords = ''; form.description = ''; form.isShowBool = '1'; modalOpen.value = true; }
function showEdit(r) { isEdit.value = true; form.categoryCode = r.categoryCode; form.categoryName = r.categoryName; form.categoryType = r.categoryType || 'article'; form.treeSort = r.treeSort; form.parentCode = r.parentCode; form.link = r.link || ''; form.keywords = r.keywords || ''; form.description = r.description || ''; form.isShowBool = r.isShow || '1'; modalOpen.value = true; }
function showAddChild(parent) { isEdit.value = false; form.categoryCode = undefined; form.categoryName = ''; form.categoryType = 'article'; form.treeSort = 1000; form.parentCode = parent.categoryCode; form.link = ''; form.keywords = ''; form.description = ''; form.isShowBool = '1'; modalOpen.value = true; }
async function handleSave() {
    saving.value = true;
    await categoryApi.save({ categoryCode: form.categoryCode, categoryName: form.categoryName, categoryType: form.categoryType, parentCode: form.parentCode, treeSort: form.treeSort, link: form.link, keywords: form.keywords, description: form.description, isShow: form.isShowBool });
    message.success('保存成功');
    modalOpen.value = false;
    saving.value = false;
    loadData();
}
async function handleDelete(code) { await categoryApi.delete(code); message.success('删除成功'); loadData(); }
onMounted(loadData);
debugger; /* PartiallyEnd: #3632/scriptSetup.vue */
const __VLS_ctx = {};
let __VLS_components;
let __VLS_directives;
const __VLS_0 = {}.ACard;
/** @type {[typeof __VLS_components.ACard, typeof __VLS_components.aCard, typeof __VLS_components.ACard, typeof __VLS_components.aCard, ]} */ ;
// @ts-ignore
const __VLS_1 = __VLS_asFunctionalComponent(__VLS_0, new __VLS_0({
    title: "栏目管理",
}));
const __VLS_2 = __VLS_1({
    title: "栏目管理",
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
    dataSource: (__VLS_ctx.treeData),
    columns: (__VLS_ctx.columns),
    loading: (__VLS_ctx.loading),
    rowKey: "categoryCode",
    defaultExpandAllRows: true,
    pagination: (false),
}));
const __VLS_15 = __VLS_14({
    dataSource: (__VLS_ctx.treeData),
    columns: (__VLS_ctx.columns),
    loading: (__VLS_ctx.loading),
    rowKey: "categoryCode",
    defaultExpandAllRows: true,
    pagination: (false),
}, ...__VLS_functionalComponentArgsRest(__VLS_14));
__VLS_16.slots.default;
{
    const { bodyCell: __VLS_thisSlot } = __VLS_16.slots;
    const [{ record, column }] = __VLS_getSlotParams(__VLS_thisSlot);
    if (column.key === 'type') {
        (__VLS_ctx.typeMap[record.categoryType || ''] || record.categoryType);
    }
    if (column.key === 'isShow') {
        const __VLS_17 = {}.ATag;
        /** @type {[typeof __VLS_components.ATag, typeof __VLS_components.aTag, typeof __VLS_components.ATag, typeof __VLS_components.aTag, ]} */ ;
        // @ts-ignore
        const __VLS_18 = __VLS_asFunctionalComponent(__VLS_17, new __VLS_17({
            color: (record.isShow === '1' ? 'green' : 'default'),
        }));
        const __VLS_19 = __VLS_18({
            color: (record.isShow === '1' ? 'green' : 'default'),
        }, ...__VLS_functionalComponentArgsRest(__VLS_18));
        __VLS_20.slots.default;
        (record.isShow === '1' ? '显示' : '隐藏');
        var __VLS_20;
    }
    if (column.key === 'status') {
        const __VLS_21 = {}.ATag;
        /** @type {[typeof __VLS_components.ATag, typeof __VLS_components.aTag, typeof __VLS_components.ATag, typeof __VLS_components.aTag, ]} */ ;
        // @ts-ignore
        const __VLS_22 = __VLS_asFunctionalComponent(__VLS_21, new __VLS_21({
            color: (record.status === '0' ? 'green' : 'red'),
        }));
        const __VLS_23 = __VLS_22({
            color: (record.status === '0' ? 'green' : 'red'),
        }, ...__VLS_functionalComponentArgsRest(__VLS_22));
        __VLS_24.slots.default;
        (record.status === '0' ? '正常' : '停用');
        var __VLS_24;
    }
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
                    __VLS_ctx.showEdit(record);
                } },
        });
        __VLS_asFunctionalElement(__VLS_intrinsicElements.a, __VLS_intrinsicElements.a)({
            ...{ onClick: (...[$event]) => {
                    if (!(column.key === 'action'))
                        return;
                    __VLS_ctx.showAddChild(record);
                } },
        });
        const __VLS_29 = {}.APopconfirm;
        /** @type {[typeof __VLS_components.APopconfirm, typeof __VLS_components.aPopconfirm, typeof __VLS_components.APopconfirm, typeof __VLS_components.aPopconfirm, ]} */ ;
        // @ts-ignore
        const __VLS_30 = __VLS_asFunctionalComponent(__VLS_29, new __VLS_29({
            ...{ 'onConfirm': {} },
            title: "确定删除?",
        }));
        const __VLS_31 = __VLS_30({
            ...{ 'onConfirm': {} },
            title: "确定删除?",
        }, ...__VLS_functionalComponentArgsRest(__VLS_30));
        let __VLS_33;
        let __VLS_34;
        let __VLS_35;
        const __VLS_36 = {
            onConfirm: (...[$event]) => {
                if (!(column.key === 'action'))
                    return;
                __VLS_ctx.handleDelete(record.categoryCode);
            }
        };
        __VLS_32.slots.default;
        __VLS_asFunctionalElement(__VLS_intrinsicElements.a, __VLS_intrinsicElements.a)({
            ...{ style: {} },
        });
        var __VLS_32;
        var __VLS_28;
    }
}
var __VLS_16;
const __VLS_37 = {}.AModal;
/** @type {[typeof __VLS_components.AModal, typeof __VLS_components.aModal, typeof __VLS_components.AModal, typeof __VLS_components.aModal, ]} */ ;
// @ts-ignore
const __VLS_38 = __VLS_asFunctionalComponent(__VLS_37, new __VLS_37({
    ...{ 'onOk': {} },
    open: (__VLS_ctx.modalOpen),
    title: (__VLS_ctx.isEdit ? '编辑栏目' : '新增栏目'),
    confirmLoading: (__VLS_ctx.saving),
    width: "640px",
}));
const __VLS_39 = __VLS_38({
    ...{ 'onOk': {} },
    open: (__VLS_ctx.modalOpen),
    title: (__VLS_ctx.isEdit ? '编辑栏目' : '新增栏目'),
    confirmLoading: (__VLS_ctx.saving),
    width: "640px",
}, ...__VLS_functionalComponentArgsRest(__VLS_38));
let __VLS_41;
let __VLS_42;
let __VLS_43;
const __VLS_44 = {
    onOk: (__VLS_ctx.handleSave)
};
__VLS_40.slots.default;
const __VLS_45 = {}.AForm;
/** @type {[typeof __VLS_components.AForm, typeof __VLS_components.aForm, typeof __VLS_components.AForm, typeof __VLS_components.aForm, ]} */ ;
// @ts-ignore
const __VLS_46 = __VLS_asFunctionalComponent(__VLS_45, new __VLS_45({
    model: (__VLS_ctx.form),
    layout: "vertical",
}));
const __VLS_47 = __VLS_46({
    model: (__VLS_ctx.form),
    layout: "vertical",
}, ...__VLS_functionalComponentArgsRest(__VLS_46));
__VLS_48.slots.default;
const __VLS_49 = {}.ARow;
/** @type {[typeof __VLS_components.ARow, typeof __VLS_components.aRow, typeof __VLS_components.ARow, typeof __VLS_components.aRow, ]} */ ;
// @ts-ignore
const __VLS_50 = __VLS_asFunctionalComponent(__VLS_49, new __VLS_49({
    gutter: (16),
}));
const __VLS_51 = __VLS_50({
    gutter: (16),
}, ...__VLS_functionalComponentArgsRest(__VLS_50));
__VLS_52.slots.default;
const __VLS_53 = {}.ACol;
/** @type {[typeof __VLS_components.ACol, typeof __VLS_components.aCol, typeof __VLS_components.ACol, typeof __VLS_components.aCol, ]} */ ;
// @ts-ignore
const __VLS_54 = __VLS_asFunctionalComponent(__VLS_53, new __VLS_53({
    span: (12),
}));
const __VLS_55 = __VLS_54({
    span: (12),
}, ...__VLS_functionalComponentArgsRest(__VLS_54));
__VLS_56.slots.default;
const __VLS_57 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_58 = __VLS_asFunctionalComponent(__VLS_57, new __VLS_57({
    label: "栏目名",
    required: true,
}));
const __VLS_59 = __VLS_58({
    label: "栏目名",
    required: true,
}, ...__VLS_functionalComponentArgsRest(__VLS_58));
__VLS_60.slots.default;
const __VLS_61 = {}.AInput;
/** @type {[typeof __VLS_components.AInput, typeof __VLS_components.aInput, ]} */ ;
// @ts-ignore
const __VLS_62 = __VLS_asFunctionalComponent(__VLS_61, new __VLS_61({
    value: (__VLS_ctx.form.categoryName),
}));
const __VLS_63 = __VLS_62({
    value: (__VLS_ctx.form.categoryName),
}, ...__VLS_functionalComponentArgsRest(__VLS_62));
var __VLS_60;
var __VLS_56;
const __VLS_65 = {}.ACol;
/** @type {[typeof __VLS_components.ACol, typeof __VLS_components.aCol, typeof __VLS_components.ACol, typeof __VLS_components.aCol, ]} */ ;
// @ts-ignore
const __VLS_66 = __VLS_asFunctionalComponent(__VLS_65, new __VLS_65({
    span: (12),
}));
const __VLS_67 = __VLS_66({
    span: (12),
}, ...__VLS_functionalComponentArgsRest(__VLS_66));
__VLS_68.slots.default;
const __VLS_69 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_70 = __VLS_asFunctionalComponent(__VLS_69, new __VLS_69({
    label: "类型",
}));
const __VLS_71 = __VLS_70({
    label: "类型",
}, ...__VLS_functionalComponentArgsRest(__VLS_70));
__VLS_72.slots.default;
const __VLS_73 = {}.ASelect;
/** @type {[typeof __VLS_components.ASelect, typeof __VLS_components.aSelect, typeof __VLS_components.ASelect, typeof __VLS_components.aSelect, ]} */ ;
// @ts-ignore
const __VLS_74 = __VLS_asFunctionalComponent(__VLS_73, new __VLS_73({
    value: (__VLS_ctx.form.categoryType),
}));
const __VLS_75 = __VLS_74({
    value: (__VLS_ctx.form.categoryType),
}, ...__VLS_functionalComponentArgsRest(__VLS_74));
__VLS_76.slots.default;
const __VLS_77 = {}.ASelectOption;
/** @type {[typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, ]} */ ;
// @ts-ignore
const __VLS_78 = __VLS_asFunctionalComponent(__VLS_77, new __VLS_77({
    value: "article",
}));
const __VLS_79 = __VLS_78({
    value: "article",
}, ...__VLS_functionalComponentArgsRest(__VLS_78));
__VLS_80.slots.default;
var __VLS_80;
const __VLS_81 = {}.ASelectOption;
/** @type {[typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, ]} */ ;
// @ts-ignore
const __VLS_82 = __VLS_asFunctionalComponent(__VLS_81, new __VLS_81({
    value: "link",
}));
const __VLS_83 = __VLS_82({
    value: "link",
}, ...__VLS_functionalComponentArgsRest(__VLS_82));
__VLS_84.slots.default;
var __VLS_84;
const __VLS_85 = {}.ASelectOption;
/** @type {[typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, ]} */ ;
// @ts-ignore
const __VLS_86 = __VLS_asFunctionalComponent(__VLS_85, new __VLS_85({
    value: "picture",
}));
const __VLS_87 = __VLS_86({
    value: "picture",
}, ...__VLS_functionalComponentArgsRest(__VLS_86));
__VLS_88.slots.default;
var __VLS_88;
var __VLS_76;
var __VLS_72;
var __VLS_68;
var __VLS_52;
const __VLS_89 = {}.ARow;
/** @type {[typeof __VLS_components.ARow, typeof __VLS_components.aRow, typeof __VLS_components.ARow, typeof __VLS_components.aRow, ]} */ ;
// @ts-ignore
const __VLS_90 = __VLS_asFunctionalComponent(__VLS_89, new __VLS_89({
    gutter: (16),
}));
const __VLS_91 = __VLS_90({
    gutter: (16),
}, ...__VLS_functionalComponentArgsRest(__VLS_90));
__VLS_92.slots.default;
const __VLS_93 = {}.ACol;
/** @type {[typeof __VLS_components.ACol, typeof __VLS_components.aCol, typeof __VLS_components.ACol, typeof __VLS_components.aCol, ]} */ ;
// @ts-ignore
const __VLS_94 = __VLS_asFunctionalComponent(__VLS_93, new __VLS_93({
    span: (12),
}));
const __VLS_95 = __VLS_94({
    span: (12),
}, ...__VLS_functionalComponentArgsRest(__VLS_94));
__VLS_96.slots.default;
const __VLS_97 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_98 = __VLS_asFunctionalComponent(__VLS_97, new __VLS_97({
    label: "排序",
}));
const __VLS_99 = __VLS_98({
    label: "排序",
}, ...__VLS_functionalComponentArgsRest(__VLS_98));
__VLS_100.slots.default;
const __VLS_101 = {}.AInputNumber;
/** @type {[typeof __VLS_components.AInputNumber, typeof __VLS_components.aInputNumber, ]} */ ;
// @ts-ignore
const __VLS_102 = __VLS_asFunctionalComponent(__VLS_101, new __VLS_101({
    value: (__VLS_ctx.form.treeSort),
    ...{ style: {} },
}));
const __VLS_103 = __VLS_102({
    value: (__VLS_ctx.form.treeSort),
    ...{ style: {} },
}, ...__VLS_functionalComponentArgsRest(__VLS_102));
var __VLS_100;
var __VLS_96;
const __VLS_105 = {}.ACol;
/** @type {[typeof __VLS_components.ACol, typeof __VLS_components.aCol, typeof __VLS_components.ACol, typeof __VLS_components.aCol, ]} */ ;
// @ts-ignore
const __VLS_106 = __VLS_asFunctionalComponent(__VLS_105, new __VLS_105({
    span: (12),
}));
const __VLS_107 = __VLS_106({
    span: (12),
}, ...__VLS_functionalComponentArgsRest(__VLS_106));
__VLS_108.slots.default;
const __VLS_109 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_110 = __VLS_asFunctionalComponent(__VLS_109, new __VLS_109({
    label: "显示",
}));
const __VLS_111 = __VLS_110({
    label: "显示",
}, ...__VLS_functionalComponentArgsRest(__VLS_110));
__VLS_112.slots.default;
const __VLS_113 = {}.ASwitch;
/** @type {[typeof __VLS_components.ASwitch, typeof __VLS_components.aSwitch, ]} */ ;
// @ts-ignore
const __VLS_114 = __VLS_asFunctionalComponent(__VLS_113, new __VLS_113({
    checked: (__VLS_ctx.form.isShowBool),
    checkedValue: "1",
    unCheckedValue: "0",
}));
const __VLS_115 = __VLS_114({
    checked: (__VLS_ctx.form.isShowBool),
    checkedValue: "1",
    unCheckedValue: "0",
}, ...__VLS_functionalComponentArgsRest(__VLS_114));
var __VLS_112;
var __VLS_108;
var __VLS_92;
const __VLS_117 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_118 = __VLS_asFunctionalComponent(__VLS_117, new __VLS_117({
    label: "链接",
}));
const __VLS_119 = __VLS_118({
    label: "链接",
}, ...__VLS_functionalComponentArgsRest(__VLS_118));
__VLS_120.slots.default;
const __VLS_121 = {}.AInput;
/** @type {[typeof __VLS_components.AInput, typeof __VLS_components.aInput, ]} */ ;
// @ts-ignore
const __VLS_122 = __VLS_asFunctionalComponent(__VLS_121, new __VLS_121({
    value: (__VLS_ctx.form.link),
    placeholder: "外部链接",
}));
const __VLS_123 = __VLS_122({
    value: (__VLS_ctx.form.link),
    placeholder: "外部链接",
}, ...__VLS_functionalComponentArgsRest(__VLS_122));
var __VLS_120;
const __VLS_125 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_126 = __VLS_asFunctionalComponent(__VLS_125, new __VLS_125({
    label: "关键字",
}));
const __VLS_127 = __VLS_126({
    label: "关键字",
}, ...__VLS_functionalComponentArgsRest(__VLS_126));
__VLS_128.slots.default;
const __VLS_129 = {}.AInput;
/** @type {[typeof __VLS_components.AInput, typeof __VLS_components.aInput, ]} */ ;
// @ts-ignore
const __VLS_130 = __VLS_asFunctionalComponent(__VLS_129, new __VLS_129({
    value: (__VLS_ctx.form.keywords),
    placeholder: "逗号分隔",
}));
const __VLS_131 = __VLS_130({
    value: (__VLS_ctx.form.keywords),
    placeholder: "逗号分隔",
}, ...__VLS_functionalComponentArgsRest(__VLS_130));
var __VLS_128;
const __VLS_133 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_134 = __VLS_asFunctionalComponent(__VLS_133, new __VLS_133({
    label: "描述",
}));
const __VLS_135 = __VLS_134({
    label: "描述",
}, ...__VLS_functionalComponentArgsRest(__VLS_134));
__VLS_136.slots.default;
const __VLS_137 = {}.ATextarea;
/** @type {[typeof __VLS_components.ATextarea, typeof __VLS_components.aTextarea, ]} */ ;
// @ts-ignore
const __VLS_138 = __VLS_asFunctionalComponent(__VLS_137, new __VLS_137({
    value: (__VLS_ctx.form.description),
    rows: (2),
}));
const __VLS_139 = __VLS_138({
    value: (__VLS_ctx.form.description),
    rows: (2),
}, ...__VLS_functionalComponentArgsRest(__VLS_138));
var __VLS_136;
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
            isEdit: isEdit,
            treeData: treeData,
            form: form,
            typeMap: typeMap,
            columns: columns,
            showAdd: showAdd,
            showEdit: showEdit,
            showAddChild: showAddChild,
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
