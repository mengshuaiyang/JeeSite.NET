import { ref, onMounted } from 'vue';
import { articleApi, categoryApi } from '@/api';
import { useRouter } from 'vue-router';
import { message } from 'ant-design-vue';
const router = useRouter();
const loading = ref(false);
const articles = ref([]);
const categoryTree = ref([]);
const pageNo = ref(1);
const pageSize = ref(10);
const total = ref(0);
const query = ref({});
const columns = [
    { title: '标题', dataIndex: 'title', ellipsis: true },
    { title: '栏目', dataIndex: 'categoryName', width: 120 },
    { title: '作者', dataIndex: 'author', width: 100 },
    { title: '封面', key: 'image', width: 50 },
    { title: '点击', dataIndex: 'clickCount', width: 60 },
    { title: '状态', key: 'status', width: 80 },
    { title: '发布时间', dataIndex: 'publishDate', width: 160 },
    { title: '操作', key: 'action', width: 120 }
];
async function loadArticles() {
    loading.value = true;
    const res = await articleApi.list({ pageNo: pageNo.value, pageSize: pageSize.value, entity: query.value });
    if (res.data) {
        articles.value = res.data.list;
        total.value = res.data.total;
    }
    loading.value = false;
}
async function loadCategoryTree() {
    const res = await categoryApi.tree();
    if (res.data)
        categoryTree.value = res.data;
}
function addArticle() { router.push('/cms/article/edit'); }
function editArticle(record) { router.push(`/cms/article/edit?articleCode=${record.articleCode}`); }
async function deleteArticle(record) {
    await articleApi.delete(record.articleCode);
    message.success('已删除');
    await loadArticles();
}
onMounted(async () => { await loadCategoryTree(); await loadArticles(); });
debugger; /* PartiallyEnd: #3632/scriptSetup.vue */
const __VLS_ctx = {};
let __VLS_components;
let __VLS_directives;
const __VLS_0 = {}.ACard;
/** @type {[typeof __VLS_components.ACard, typeof __VLS_components.aCard, typeof __VLS_components.ACard, typeof __VLS_components.aCard, ]} */ ;
// @ts-ignore
const __VLS_1 = __VLS_asFunctionalComponent(__VLS_0, new __VLS_0({
    title: "文章管理",
}));
const __VLS_2 = __VLS_1({
    title: "文章管理",
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
    label: "标题",
}));
const __VLS_11 = __VLS_10({
    label: "标题",
}, ...__VLS_functionalComponentArgsRest(__VLS_10));
__VLS_12.slots.default;
const __VLS_13 = {}.AInput;
/** @type {[typeof __VLS_components.AInput, typeof __VLS_components.aInput, ]} */ ;
// @ts-ignore
const __VLS_14 = __VLS_asFunctionalComponent(__VLS_13, new __VLS_13({
    value: (__VLS_ctx.query.title),
    placeholder: "搜索标题",
    ...{ style: {} },
}));
const __VLS_15 = __VLS_14({
    value: (__VLS_ctx.query.title),
    placeholder: "搜索标题",
    ...{ style: {} },
}, ...__VLS_functionalComponentArgsRest(__VLS_14));
var __VLS_12;
const __VLS_17 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_18 = __VLS_asFunctionalComponent(__VLS_17, new __VLS_17({
    label: "栏目",
}));
const __VLS_19 = __VLS_18({
    label: "栏目",
}, ...__VLS_functionalComponentArgsRest(__VLS_18));
__VLS_20.slots.default;
const __VLS_21 = {}.ATreeSelect;
/** @type {[typeof __VLS_components.ATreeSelect, typeof __VLS_components.aTreeSelect, ]} */ ;
// @ts-ignore
const __VLS_22 = __VLS_asFunctionalComponent(__VLS_21, new __VLS_21({
    value: (__VLS_ctx.query.categoryCode),
    treeData: (__VLS_ctx.categoryTree),
    allowClear: true,
    placeholder: "全部",
    treeDefaultExpandAll: true,
    ...{ style: {} },
    fieldNames: "{label:'categoryName',value:'categoryCode',children:'children'}",
}));
const __VLS_23 = __VLS_22({
    value: (__VLS_ctx.query.categoryCode),
    treeData: (__VLS_ctx.categoryTree),
    allowClear: true,
    placeholder: "全部",
    treeDefaultExpandAll: true,
    ...{ style: {} },
    fieldNames: "{label:'categoryName',value:'categoryCode',children:'children'}",
}, ...__VLS_functionalComponentArgsRest(__VLS_22));
var __VLS_20;
const __VLS_25 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_26 = __VLS_asFunctionalComponent(__VLS_25, new __VLS_25({
    label: "状态",
}));
const __VLS_27 = __VLS_26({
    label: "状态",
}, ...__VLS_functionalComponentArgsRest(__VLS_26));
__VLS_28.slots.default;
const __VLS_29 = {}.ASelect;
/** @type {[typeof __VLS_components.ASelect, typeof __VLS_components.aSelect, typeof __VLS_components.ASelect, typeof __VLS_components.aSelect, ]} */ ;
// @ts-ignore
const __VLS_30 = __VLS_asFunctionalComponent(__VLS_29, new __VLS_29({
    value: (__VLS_ctx.query.status),
    allowClear: true,
    placeholder: "全部",
    ...{ style: {} },
}));
const __VLS_31 = __VLS_30({
    value: (__VLS_ctx.query.status),
    allowClear: true,
    placeholder: "全部",
    ...{ style: {} },
}, ...__VLS_functionalComponentArgsRest(__VLS_30));
__VLS_32.slots.default;
const __VLS_33 = {}.ASelectOption;
/** @type {[typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, ]} */ ;
// @ts-ignore
const __VLS_34 = __VLS_asFunctionalComponent(__VLS_33, new __VLS_33({
    value: "draft",
}));
const __VLS_35 = __VLS_34({
    value: "draft",
}, ...__VLS_functionalComponentArgsRest(__VLS_34));
__VLS_36.slots.default;
var __VLS_36;
const __VLS_37 = {}.ASelectOption;
/** @type {[typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, ]} */ ;
// @ts-ignore
const __VLS_38 = __VLS_asFunctionalComponent(__VLS_37, new __VLS_37({
    value: "publish",
}));
const __VLS_39 = __VLS_38({
    value: "publish",
}, ...__VLS_functionalComponentArgsRest(__VLS_38));
__VLS_40.slots.default;
var __VLS_40;
var __VLS_32;
var __VLS_28;
const __VLS_41 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_42 = __VLS_asFunctionalComponent(__VLS_41, new __VLS_41({}));
const __VLS_43 = __VLS_42({}, ...__VLS_functionalComponentArgsRest(__VLS_42));
__VLS_44.slots.default;
const __VLS_45 = {}.AButton;
/** @type {[typeof __VLS_components.AButton, typeof __VLS_components.aButton, typeof __VLS_components.AButton, typeof __VLS_components.aButton, ]} */ ;
// @ts-ignore
const __VLS_46 = __VLS_asFunctionalComponent(__VLS_45, new __VLS_45({
    ...{ 'onClick': {} },
}));
const __VLS_47 = __VLS_46({
    ...{ 'onClick': {} },
}, ...__VLS_functionalComponentArgsRest(__VLS_46));
let __VLS_49;
let __VLS_50;
let __VLS_51;
const __VLS_52 = {
    onClick: (__VLS_ctx.loadArticles)
};
__VLS_48.slots.default;
var __VLS_48;
var __VLS_44;
const __VLS_53 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_54 = __VLS_asFunctionalComponent(__VLS_53, new __VLS_53({}));
const __VLS_55 = __VLS_54({}, ...__VLS_functionalComponentArgsRest(__VLS_54));
__VLS_56.slots.default;
const __VLS_57 = {}.AButton;
/** @type {[typeof __VLS_components.AButton, typeof __VLS_components.aButton, typeof __VLS_components.AButton, typeof __VLS_components.aButton, ]} */ ;
// @ts-ignore
const __VLS_58 = __VLS_asFunctionalComponent(__VLS_57, new __VLS_57({
    ...{ 'onClick': {} },
    type: "primary",
}));
const __VLS_59 = __VLS_58({
    ...{ 'onClick': {} },
    type: "primary",
}, ...__VLS_functionalComponentArgsRest(__VLS_58));
let __VLS_61;
let __VLS_62;
let __VLS_63;
const __VLS_64 = {
    onClick: (__VLS_ctx.addArticle)
};
__VLS_60.slots.default;
var __VLS_60;
var __VLS_56;
var __VLS_8;
const __VLS_65 = {}.ATable;
/** @type {[typeof __VLS_components.ATable, typeof __VLS_components.aTable, typeof __VLS_components.ATable, typeof __VLS_components.aTable, ]} */ ;
// @ts-ignore
const __VLS_66 = __VLS_asFunctionalComponent(__VLS_65, new __VLS_65({
    dataSource: (__VLS_ctx.articles),
    columns: (__VLS_ctx.columns),
    rowKey: "articleCode",
    loading: (__VLS_ctx.loading),
    pagination: ({ current: __VLS_ctx.pageNo, pageSize: __VLS_ctx.pageSize, total: __VLS_ctx.total, onChange: (p) => { __VLS_ctx.pageNo = p; __VLS_ctx.loadArticles(); } }),
}));
const __VLS_67 = __VLS_66({
    dataSource: (__VLS_ctx.articles),
    columns: (__VLS_ctx.columns),
    rowKey: "articleCode",
    loading: (__VLS_ctx.loading),
    pagination: ({ current: __VLS_ctx.pageNo, pageSize: __VLS_ctx.pageSize, total: __VLS_ctx.total, onChange: (p) => { __VLS_ctx.pageNo = p; __VLS_ctx.loadArticles(); } }),
}, ...__VLS_functionalComponentArgsRest(__VLS_66));
__VLS_68.slots.default;
{
    const { bodyCell: __VLS_thisSlot } = __VLS_68.slots;
    const [{ record, column }] = __VLS_getSlotParams(__VLS_thisSlot);
    if (column.key === 'image') {
        if (record.image) {
            const __VLS_69 = {}.AImage;
            /** @type {[typeof __VLS_components.AImage, typeof __VLS_components.aImage, ]} */ ;
            // @ts-ignore
            const __VLS_70 = __VLS_asFunctionalComponent(__VLS_69, new __VLS_69({
                src: (record.image),
                width: "36",
                height: "36",
                ...{ style: {} },
            }));
            const __VLS_71 = __VLS_70({
                src: (record.image),
                width: "36",
                height: "36",
                ...{ style: {} },
            }, ...__VLS_functionalComponentArgsRest(__VLS_70));
        }
        else {
            __VLS_asFunctionalElement(__VLS_intrinsicElements.span, __VLS_intrinsicElements.span)({});
        }
    }
    if (column.key === 'status') {
        const __VLS_73 = {}.ATag;
        /** @type {[typeof __VLS_components.ATag, typeof __VLS_components.aTag, typeof __VLS_components.ATag, typeof __VLS_components.aTag, ]} */ ;
        // @ts-ignore
        const __VLS_74 = __VLS_asFunctionalComponent(__VLS_73, new __VLS_73({
            color: (record.status === 'publish' ? 'green' : 'orange'),
        }));
        const __VLS_75 = __VLS_74({
            color: (record.status === 'publish' ? 'green' : 'orange'),
        }, ...__VLS_functionalComponentArgsRest(__VLS_74));
        __VLS_76.slots.default;
        (record.status === 'publish' ? '已发布' : '草稿');
        var __VLS_76;
    }
    if (column.key === 'action') {
        const __VLS_77 = {}.ASpace;
        /** @type {[typeof __VLS_components.ASpace, typeof __VLS_components.aSpace, typeof __VLS_components.ASpace, typeof __VLS_components.aSpace, ]} */ ;
        // @ts-ignore
        const __VLS_78 = __VLS_asFunctionalComponent(__VLS_77, new __VLS_77({}));
        const __VLS_79 = __VLS_78({}, ...__VLS_functionalComponentArgsRest(__VLS_78));
        __VLS_80.slots.default;
        __VLS_asFunctionalElement(__VLS_intrinsicElements.a, __VLS_intrinsicElements.a)({
            ...{ onClick: (...[$event]) => {
                    if (!(column.key === 'action'))
                        return;
                    __VLS_ctx.editArticle(record);
                } },
        });
        const __VLS_81 = {}.APopconfirm;
        /** @type {[typeof __VLS_components.APopconfirm, typeof __VLS_components.aPopconfirm, typeof __VLS_components.APopconfirm, typeof __VLS_components.aPopconfirm, ]} */ ;
        // @ts-ignore
        const __VLS_82 = __VLS_asFunctionalComponent(__VLS_81, new __VLS_81({
            ...{ 'onConfirm': {} },
            title: "确定删除?",
        }));
        const __VLS_83 = __VLS_82({
            ...{ 'onConfirm': {} },
            title: "确定删除?",
        }, ...__VLS_functionalComponentArgsRest(__VLS_82));
        let __VLS_85;
        let __VLS_86;
        let __VLS_87;
        const __VLS_88 = {
            onConfirm: (...[$event]) => {
                if (!(column.key === 'action'))
                    return;
                __VLS_ctx.deleteArticle(record);
            }
        };
        __VLS_84.slots.default;
        __VLS_asFunctionalElement(__VLS_intrinsicElements.a, __VLS_intrinsicElements.a)({
            ...{ style: {} },
        });
        var __VLS_84;
        var __VLS_80;
    }
}
var __VLS_68;
var __VLS_3;
var __VLS_dollars;
const __VLS_self = (await import('vue')).defineComponent({
    setup() {
        return {
            loading: loading,
            articles: articles,
            categoryTree: categoryTree,
            pageNo: pageNo,
            pageSize: pageSize,
            total: total,
            query: query,
            columns: columns,
            loadArticles: loadArticles,
            addArticle: addArticle,
            editArticle: editArticle,
            deleteArticle: deleteArticle,
        };
    },
});
export default (await import('vue')).defineComponent({
    setup() {
        return {};
    },
});
; /* PartiallyEnd: #4569/main.vue */
