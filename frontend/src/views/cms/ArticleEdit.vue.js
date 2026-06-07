import { ref, reactive, onMounted, computed } from 'vue';
import { articleApi, categoryApi, fileApi } from '@/api';
import { useRoute, useRouter } from 'vue-router';
import { message } from 'ant-design-vue';
import { UploadOutlined } from '@ant-design/icons-vue';
import dayjs from 'dayjs';
const route = useRoute();
const router = useRouter();
const isEdit = computed(() => !!route.query.articleCode);
const saving = ref(false);
const categoryTree = ref([]);
const isTopVal = ref(false);
const isRecommendVal = ref(false);
const isHotVal = ref(false);
const form = reactive({
    articleCode: '', categoryCode: '', title: '', subtitle: '', summary: '',
    content: '', author: '', source: '', image: '', tags: '',
    isTop: '0', isRecommend: '0', isHot: '0', publishDate: undefined
});
async function loadCategoryTree() {
    const res = await categoryApi.tree();
    if (res.data)
        categoryTree.value = res.data;
}
async function loadArticle() {
    const code = route.query.articleCode;
    if (!code)
        return;
    const res = await articleApi.get(code);
    if (res.data) {
        const d = res.data;
        form.articleCode = d.articleCode;
        form.categoryCode = d.categoryCode;
        form.title = d.title;
        form.subtitle = d.subtitle || '';
        form.summary = d.summary || '';
        form.content = d.articleData?.content || '';
        form.author = d.author || '';
        form.source = d.source || '';
        form.image = d.image || '';
        form.tags = d.tags || '';
        form.publishDate = d.publishDate ? dayjs(d.publishDate) : undefined;
        isTopVal.value = d.isTop === '1';
        isRecommendVal.value = d.isRecommend === '1';
        isHotVal.value = d.isHot === '1';
    }
}
async function handleImageUpload(file) {
    const res = await fileApi.upload(file, 'cms_article', form.articleCode || 'new');
    if (res.data)
        form.image = `/api/v1/sys/file/download/${res.data.uploadId}`;
    return false;
}
async function save(status) {
    if (!form.title) {
        message.warning('请输入标题');
        return;
    }
    if (!form.categoryCode) {
        message.warning('请选择栏目');
        return;
    }
    saving.value = true;
    form.isTop = isTopVal.value ? '1' : '0';
    form.isRecommend = isRecommendVal.value ? '1' : '0';
    form.isHot = isHotVal.value ? '1' : '0';
    await articleApi.save({ ...form, publishDate: form.publishDate ? form.publishDate.toISOString() : undefined });
    message.success(status === 'publish' ? '已发布' : '草稿已保存');
    router.push('/cms/article');
    saving.value = false;
}
onMounted(async () => { await loadCategoryTree(); await loadArticle(); });
debugger; /* PartiallyEnd: #3632/scriptSetup.vue */
const __VLS_ctx = {};
let __VLS_components;
let __VLS_directives;
const __VLS_0 = {}.ACard;
/** @type {[typeof __VLS_components.ACard, typeof __VLS_components.aCard, typeof __VLS_components.ACard, typeof __VLS_components.aCard, ]} */ ;
// @ts-ignore
const __VLS_1 = __VLS_asFunctionalComponent(__VLS_0, new __VLS_0({
    title: (__VLS_ctx.isEdit ? '编辑文章' : '写文章'),
}));
const __VLS_2 = __VLS_1({
    title: (__VLS_ctx.isEdit ? '编辑文章' : '写文章'),
}, ...__VLS_functionalComponentArgsRest(__VLS_1));
var __VLS_4 = {};
__VLS_3.slots.default;
const __VLS_5 = {}.AForm;
/** @type {[typeof __VLS_components.AForm, typeof __VLS_components.aForm, typeof __VLS_components.AForm, typeof __VLS_components.aForm, ]} */ ;
// @ts-ignore
const __VLS_6 = __VLS_asFunctionalComponent(__VLS_5, new __VLS_5({
    model: (__VLS_ctx.form),
    layout: "vertical",
}));
const __VLS_7 = __VLS_6({
    model: (__VLS_ctx.form),
    layout: "vertical",
}, ...__VLS_functionalComponentArgsRest(__VLS_6));
__VLS_8.slots.default;
const __VLS_9 = {}.ARow;
/** @type {[typeof __VLS_components.ARow, typeof __VLS_components.aRow, typeof __VLS_components.ARow, typeof __VLS_components.aRow, ]} */ ;
// @ts-ignore
const __VLS_10 = __VLS_asFunctionalComponent(__VLS_9, new __VLS_9({
    gutter: (16),
}));
const __VLS_11 = __VLS_10({
    gutter: (16),
}, ...__VLS_functionalComponentArgsRest(__VLS_10));
__VLS_12.slots.default;
const __VLS_13 = {}.ACol;
/** @type {[typeof __VLS_components.ACol, typeof __VLS_components.aCol, typeof __VLS_components.ACol, typeof __VLS_components.aCol, ]} */ ;
// @ts-ignore
const __VLS_14 = __VLS_asFunctionalComponent(__VLS_13, new __VLS_13({
    span: (16),
}));
const __VLS_15 = __VLS_14({
    span: (16),
}, ...__VLS_functionalComponentArgsRest(__VLS_14));
__VLS_16.slots.default;
const __VLS_17 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_18 = __VLS_asFunctionalComponent(__VLS_17, new __VLS_17({
    label: "标题",
    required: true,
}));
const __VLS_19 = __VLS_18({
    label: "标题",
    required: true,
}, ...__VLS_functionalComponentArgsRest(__VLS_18));
__VLS_20.slots.default;
const __VLS_21 = {}.AInput;
/** @type {[typeof __VLS_components.AInput, typeof __VLS_components.aInput, ]} */ ;
// @ts-ignore
const __VLS_22 = __VLS_asFunctionalComponent(__VLS_21, new __VLS_21({
    value: (__VLS_ctx.form.title),
    placeholder: "文章标题",
}));
const __VLS_23 = __VLS_22({
    value: (__VLS_ctx.form.title),
    placeholder: "文章标题",
}, ...__VLS_functionalComponentArgsRest(__VLS_22));
var __VLS_20;
var __VLS_16;
const __VLS_25 = {}.ACol;
/** @type {[typeof __VLS_components.ACol, typeof __VLS_components.aCol, typeof __VLS_components.ACol, typeof __VLS_components.aCol, ]} */ ;
// @ts-ignore
const __VLS_26 = __VLS_asFunctionalComponent(__VLS_25, new __VLS_25({
    span: (8),
}));
const __VLS_27 = __VLS_26({
    span: (8),
}, ...__VLS_functionalComponentArgsRest(__VLS_26));
__VLS_28.slots.default;
const __VLS_29 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_30 = __VLS_asFunctionalComponent(__VLS_29, new __VLS_29({
    label: "栏目",
    required: true,
}));
const __VLS_31 = __VLS_30({
    label: "栏目",
    required: true,
}, ...__VLS_functionalComponentArgsRest(__VLS_30));
__VLS_32.slots.default;
const __VLS_33 = {}.ATreeSelect;
/** @type {[typeof __VLS_components.ATreeSelect, typeof __VLS_components.aTreeSelect, ]} */ ;
// @ts-ignore
const __VLS_34 = __VLS_asFunctionalComponent(__VLS_33, new __VLS_33({
    value: (__VLS_ctx.form.categoryCode),
    treeData: (__VLS_ctx.categoryTree),
    placeholder: "选择栏目",
    fieldNames: "{label:'categoryName',value:'categoryCode',children:'children'}",
    ...{ style: {} },
}));
const __VLS_35 = __VLS_34({
    value: (__VLS_ctx.form.categoryCode),
    treeData: (__VLS_ctx.categoryTree),
    placeholder: "选择栏目",
    fieldNames: "{label:'categoryName',value:'categoryCode',children:'children'}",
    ...{ style: {} },
}, ...__VLS_functionalComponentArgsRest(__VLS_34));
var __VLS_32;
var __VLS_28;
var __VLS_12;
const __VLS_37 = {}.ARow;
/** @type {[typeof __VLS_components.ARow, typeof __VLS_components.aRow, typeof __VLS_components.ARow, typeof __VLS_components.aRow, ]} */ ;
// @ts-ignore
const __VLS_38 = __VLS_asFunctionalComponent(__VLS_37, new __VLS_37({
    gutter: (16),
}));
const __VLS_39 = __VLS_38({
    gutter: (16),
}, ...__VLS_functionalComponentArgsRest(__VLS_38));
__VLS_40.slots.default;
const __VLS_41 = {}.ACol;
/** @type {[typeof __VLS_components.ACol, typeof __VLS_components.aCol, typeof __VLS_components.ACol, typeof __VLS_components.aCol, ]} */ ;
// @ts-ignore
const __VLS_42 = __VLS_asFunctionalComponent(__VLS_41, new __VLS_41({
    span: (8),
}));
const __VLS_43 = __VLS_42({
    span: (8),
}, ...__VLS_functionalComponentArgsRest(__VLS_42));
__VLS_44.slots.default;
const __VLS_45 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_46 = __VLS_asFunctionalComponent(__VLS_45, new __VLS_45({
    label: "作者",
}));
const __VLS_47 = __VLS_46({
    label: "作者",
}, ...__VLS_functionalComponentArgsRest(__VLS_46));
__VLS_48.slots.default;
const __VLS_49 = {}.AInput;
/** @type {[typeof __VLS_components.AInput, typeof __VLS_components.aInput, ]} */ ;
// @ts-ignore
const __VLS_50 = __VLS_asFunctionalComponent(__VLS_49, new __VLS_49({
    value: (__VLS_ctx.form.author),
    placeholder: "作者",
}));
const __VLS_51 = __VLS_50({
    value: (__VLS_ctx.form.author),
    placeholder: "作者",
}, ...__VLS_functionalComponentArgsRest(__VLS_50));
var __VLS_48;
var __VLS_44;
const __VLS_53 = {}.ACol;
/** @type {[typeof __VLS_components.ACol, typeof __VLS_components.aCol, typeof __VLS_components.ACol, typeof __VLS_components.aCol, ]} */ ;
// @ts-ignore
const __VLS_54 = __VLS_asFunctionalComponent(__VLS_53, new __VLS_53({
    span: (8),
}));
const __VLS_55 = __VLS_54({
    span: (8),
}, ...__VLS_functionalComponentArgsRest(__VLS_54));
__VLS_56.slots.default;
const __VLS_57 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_58 = __VLS_asFunctionalComponent(__VLS_57, new __VLS_57({
    label: "来源",
}));
const __VLS_59 = __VLS_58({
    label: "来源",
}, ...__VLS_functionalComponentArgsRest(__VLS_58));
__VLS_60.slots.default;
const __VLS_61 = {}.AInput;
/** @type {[typeof __VLS_components.AInput, typeof __VLS_components.aInput, ]} */ ;
// @ts-ignore
const __VLS_62 = __VLS_asFunctionalComponent(__VLS_61, new __VLS_61({
    value: (__VLS_ctx.form.source),
    placeholder: "来源",
}));
const __VLS_63 = __VLS_62({
    value: (__VLS_ctx.form.source),
    placeholder: "来源",
}, ...__VLS_functionalComponentArgsRest(__VLS_62));
var __VLS_60;
var __VLS_56;
const __VLS_65 = {}.ACol;
/** @type {[typeof __VLS_components.ACol, typeof __VLS_components.aCol, typeof __VLS_components.ACol, typeof __VLS_components.aCol, ]} */ ;
// @ts-ignore
const __VLS_66 = __VLS_asFunctionalComponent(__VLS_65, new __VLS_65({
    span: (8),
}));
const __VLS_67 = __VLS_66({
    span: (8),
}, ...__VLS_functionalComponentArgsRest(__VLS_66));
__VLS_68.slots.default;
const __VLS_69 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_70 = __VLS_asFunctionalComponent(__VLS_69, new __VLS_69({
    label: "发布时间",
}));
const __VLS_71 = __VLS_70({
    label: "发布时间",
}, ...__VLS_functionalComponentArgsRest(__VLS_70));
__VLS_72.slots.default;
const __VLS_73 = {}.ADatePicker;
/** @type {[typeof __VLS_components.ADatePicker, typeof __VLS_components.aDatePicker, ]} */ ;
// @ts-ignore
const __VLS_74 = __VLS_asFunctionalComponent(__VLS_73, new __VLS_73({
    value: (__VLS_ctx.form.publishDate),
    ...{ style: {} },
}));
const __VLS_75 = __VLS_74({
    value: (__VLS_ctx.form.publishDate),
    ...{ style: {} },
}, ...__VLS_functionalComponentArgsRest(__VLS_74));
var __VLS_72;
var __VLS_68;
var __VLS_40;
const __VLS_77 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_78 = __VLS_asFunctionalComponent(__VLS_77, new __VLS_77({
    label: "副标题",
}));
const __VLS_79 = __VLS_78({
    label: "副标题",
}, ...__VLS_functionalComponentArgsRest(__VLS_78));
__VLS_80.slots.default;
const __VLS_81 = {}.AInput;
/** @type {[typeof __VLS_components.AInput, typeof __VLS_components.aInput, ]} */ ;
// @ts-ignore
const __VLS_82 = __VLS_asFunctionalComponent(__VLS_81, new __VLS_81({
    value: (__VLS_ctx.form.subtitle),
    placeholder: "副标题（可选）",
}));
const __VLS_83 = __VLS_82({
    value: (__VLS_ctx.form.subtitle),
    placeholder: "副标题（可选）",
}, ...__VLS_functionalComponentArgsRest(__VLS_82));
var __VLS_80;
const __VLS_85 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_86 = __VLS_asFunctionalComponent(__VLS_85, new __VLS_85({
    label: "摘要",
}));
const __VLS_87 = __VLS_86({
    label: "摘要",
}, ...__VLS_functionalComponentArgsRest(__VLS_86));
__VLS_88.slots.default;
const __VLS_89 = {}.ATextarea;
/** @type {[typeof __VLS_components.ATextarea, typeof __VLS_components.aTextarea, ]} */ ;
// @ts-ignore
const __VLS_90 = __VLS_asFunctionalComponent(__VLS_89, new __VLS_89({
    value: (__VLS_ctx.form.summary),
    rows: (2),
    placeholder: "文章摘要",
}));
const __VLS_91 = __VLS_90({
    value: (__VLS_ctx.form.summary),
    rows: (2),
    placeholder: "文章摘要",
}, ...__VLS_functionalComponentArgsRest(__VLS_90));
var __VLS_88;
const __VLS_93 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_94 = __VLS_asFunctionalComponent(__VLS_93, new __VLS_93({
    label: "正文内容",
}));
const __VLS_95 = __VLS_94({
    label: "正文内容",
}, ...__VLS_functionalComponentArgsRest(__VLS_94));
__VLS_96.slots.default;
const __VLS_97 = {}.ATextarea;
/** @type {[typeof __VLS_components.ATextarea, typeof __VLS_components.aTextarea, ]} */ ;
// @ts-ignore
const __VLS_98 = __VLS_asFunctionalComponent(__VLS_97, new __VLS_97({
    value: (__VLS_ctx.form.content),
    rows: (12),
    placeholder: "支持 HTML 内容，可集成富文本编辑器",
}));
const __VLS_99 = __VLS_98({
    value: (__VLS_ctx.form.content),
    rows: (12),
    placeholder: "支持 HTML 内容，可集成富文本编辑器",
}, ...__VLS_functionalComponentArgsRest(__VLS_98));
var __VLS_96;
const __VLS_101 = {}.ARow;
/** @type {[typeof __VLS_components.ARow, typeof __VLS_components.aRow, typeof __VLS_components.ARow, typeof __VLS_components.aRow, ]} */ ;
// @ts-ignore
const __VLS_102 = __VLS_asFunctionalComponent(__VLS_101, new __VLS_101({
    gutter: (16),
}));
const __VLS_103 = __VLS_102({
    gutter: (16),
}, ...__VLS_functionalComponentArgsRest(__VLS_102));
__VLS_104.slots.default;
const __VLS_105 = {}.ACol;
/** @type {[typeof __VLS_components.ACol, typeof __VLS_components.aCol, typeof __VLS_components.ACol, typeof __VLS_components.aCol, ]} */ ;
// @ts-ignore
const __VLS_106 = __VLS_asFunctionalComponent(__VLS_105, new __VLS_105({
    span: (8),
}));
const __VLS_107 = __VLS_106({
    span: (8),
}, ...__VLS_functionalComponentArgsRest(__VLS_106));
__VLS_108.slots.default;
const __VLS_109 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_110 = __VLS_asFunctionalComponent(__VLS_109, new __VLS_109({
    label: "标签",
}));
const __VLS_111 = __VLS_110({
    label: "标签",
}, ...__VLS_functionalComponentArgsRest(__VLS_110));
__VLS_112.slots.default;
const __VLS_113 = {}.AInput;
/** @type {[typeof __VLS_components.AInput, typeof __VLS_components.aInput, ]} */ ;
// @ts-ignore
const __VLS_114 = __VLS_asFunctionalComponent(__VLS_113, new __VLS_113({
    value: (__VLS_ctx.form.tags),
    placeholder: "逗号分隔",
}));
const __VLS_115 = __VLS_114({
    value: (__VLS_ctx.form.tags),
    placeholder: "逗号分隔",
}, ...__VLS_functionalComponentArgsRest(__VLS_114));
var __VLS_112;
var __VLS_108;
const __VLS_117 = {}.ACol;
/** @type {[typeof __VLS_components.ACol, typeof __VLS_components.aCol, typeof __VLS_components.ACol, typeof __VLS_components.aCol, ]} */ ;
// @ts-ignore
const __VLS_118 = __VLS_asFunctionalComponent(__VLS_117, new __VLS_117({
    span: (8),
}));
const __VLS_119 = __VLS_118({
    span: (8),
}, ...__VLS_functionalComponentArgsRest(__VLS_118));
__VLS_120.slots.default;
const __VLS_121 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_122 = __VLS_asFunctionalComponent(__VLS_121, new __VLS_121({
    label: "封面图",
}));
const __VLS_123 = __VLS_122({
    label: "封面图",
}, ...__VLS_functionalComponentArgsRest(__VLS_122));
__VLS_124.slots.default;
const __VLS_125 = {}.AUpload;
/** @type {[typeof __VLS_components.AUpload, typeof __VLS_components.aUpload, typeof __VLS_components.AUpload, typeof __VLS_components.aUpload, ]} */ ;
// @ts-ignore
const __VLS_126 = __VLS_asFunctionalComponent(__VLS_125, new __VLS_125({
    beforeUpload: (__VLS_ctx.handleImageUpload),
    accept: "image/*",
    showUploadList: (false),
}));
const __VLS_127 = __VLS_126({
    beforeUpload: (__VLS_ctx.handleImageUpload),
    accept: "image/*",
    showUploadList: (false),
}, ...__VLS_functionalComponentArgsRest(__VLS_126));
__VLS_128.slots.default;
const __VLS_129 = {}.AButton;
/** @type {[typeof __VLS_components.AButton, typeof __VLS_components.aButton, typeof __VLS_components.AButton, typeof __VLS_components.aButton, ]} */ ;
// @ts-ignore
const __VLS_130 = __VLS_asFunctionalComponent(__VLS_129, new __VLS_129({}));
const __VLS_131 = __VLS_130({}, ...__VLS_functionalComponentArgsRest(__VLS_130));
__VLS_132.slots.default;
const __VLS_133 = {}.UploadOutlined;
/** @type {[typeof __VLS_components.UploadOutlined, typeof __VLS_components.uploadOutlined, ]} */ ;
// @ts-ignore
const __VLS_134 = __VLS_asFunctionalComponent(__VLS_133, new __VLS_133({}));
const __VLS_135 = __VLS_134({}, ...__VLS_functionalComponentArgsRest(__VLS_134));
var __VLS_132;
var __VLS_128;
if (__VLS_ctx.form.image) {
    const __VLS_137 = {}.AImage;
    /** @type {[typeof __VLS_components.AImage, typeof __VLS_components.aImage, ]} */ ;
    // @ts-ignore
    const __VLS_138 = __VLS_asFunctionalComponent(__VLS_137, new __VLS_137({
        src: (__VLS_ctx.form.image),
        width: "80",
        ...{ style: {} },
    }));
    const __VLS_139 = __VLS_138({
        src: (__VLS_ctx.form.image),
        width: "80",
        ...{ style: {} },
    }, ...__VLS_functionalComponentArgsRest(__VLS_138));
}
var __VLS_124;
var __VLS_120;
const __VLS_141 = {}.ACol;
/** @type {[typeof __VLS_components.ACol, typeof __VLS_components.aCol, typeof __VLS_components.ACol, typeof __VLS_components.aCol, ]} */ ;
// @ts-ignore
const __VLS_142 = __VLS_asFunctionalComponent(__VLS_141, new __VLS_141({
    span: (8),
}));
const __VLS_143 = __VLS_142({
    span: (8),
}, ...__VLS_functionalComponentArgsRest(__VLS_142));
__VLS_144.slots.default;
const __VLS_145 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_146 = __VLS_asFunctionalComponent(__VLS_145, new __VLS_145({
    label: "属性",
}));
const __VLS_147 = __VLS_146({
    label: "属性",
}, ...__VLS_functionalComponentArgsRest(__VLS_146));
__VLS_148.slots.default;
const __VLS_149 = {}.ASpace;
/** @type {[typeof __VLS_components.ASpace, typeof __VLS_components.aSpace, typeof __VLS_components.ASpace, typeof __VLS_components.aSpace, ]} */ ;
// @ts-ignore
const __VLS_150 = __VLS_asFunctionalComponent(__VLS_149, new __VLS_149({}));
const __VLS_151 = __VLS_150({}, ...__VLS_functionalComponentArgsRest(__VLS_150));
__VLS_152.slots.default;
const __VLS_153 = {}.ASwitch;
/** @type {[typeof __VLS_components.ASwitch, typeof __VLS_components.aSwitch, ]} */ ;
// @ts-ignore
const __VLS_154 = __VLS_asFunctionalComponent(__VLS_153, new __VLS_153({
    checked: (__VLS_ctx.isTopVal),
    checkedChildren: "置顶",
    unCheckedChildren: "置顶",
}));
const __VLS_155 = __VLS_154({
    checked: (__VLS_ctx.isTopVal),
    checkedChildren: "置顶",
    unCheckedChildren: "置顶",
}, ...__VLS_functionalComponentArgsRest(__VLS_154));
const __VLS_157 = {}.ASwitch;
/** @type {[typeof __VLS_components.ASwitch, typeof __VLS_components.aSwitch, ]} */ ;
// @ts-ignore
const __VLS_158 = __VLS_asFunctionalComponent(__VLS_157, new __VLS_157({
    checked: (__VLS_ctx.isRecommendVal),
    checkedChildren: "推荐",
    unCheckedChildren: "推荐",
}));
const __VLS_159 = __VLS_158({
    checked: (__VLS_ctx.isRecommendVal),
    checkedChildren: "推荐",
    unCheckedChildren: "推荐",
}, ...__VLS_functionalComponentArgsRest(__VLS_158));
const __VLS_161 = {}.ASwitch;
/** @type {[typeof __VLS_components.ASwitch, typeof __VLS_components.aSwitch, ]} */ ;
// @ts-ignore
const __VLS_162 = __VLS_asFunctionalComponent(__VLS_161, new __VLS_161({
    checked: (__VLS_ctx.isHotVal),
    checkedChildren: "热门",
    unCheckedChildren: "热门",
}));
const __VLS_163 = __VLS_162({
    checked: (__VLS_ctx.isHotVal),
    checkedChildren: "热门",
    unCheckedChildren: "热门",
}, ...__VLS_functionalComponentArgsRest(__VLS_162));
var __VLS_152;
var __VLS_148;
var __VLS_144;
var __VLS_104;
const __VLS_165 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_166 = __VLS_asFunctionalComponent(__VLS_165, new __VLS_165({}));
const __VLS_167 = __VLS_166({}, ...__VLS_functionalComponentArgsRest(__VLS_166));
__VLS_168.slots.default;
const __VLS_169 = {}.ASpace;
/** @type {[typeof __VLS_components.ASpace, typeof __VLS_components.aSpace, typeof __VLS_components.ASpace, typeof __VLS_components.aSpace, ]} */ ;
// @ts-ignore
const __VLS_170 = __VLS_asFunctionalComponent(__VLS_169, new __VLS_169({}));
const __VLS_171 = __VLS_170({}, ...__VLS_functionalComponentArgsRest(__VLS_170));
__VLS_172.slots.default;
const __VLS_173 = {}.AButton;
/** @type {[typeof __VLS_components.AButton, typeof __VLS_components.aButton, typeof __VLS_components.AButton, typeof __VLS_components.aButton, ]} */ ;
// @ts-ignore
const __VLS_174 = __VLS_asFunctionalComponent(__VLS_173, new __VLS_173({
    ...{ 'onClick': {} },
    type: "primary",
    loading: (__VLS_ctx.saving),
}));
const __VLS_175 = __VLS_174({
    ...{ 'onClick': {} },
    type: "primary",
    loading: (__VLS_ctx.saving),
}, ...__VLS_functionalComponentArgsRest(__VLS_174));
let __VLS_177;
let __VLS_178;
let __VLS_179;
const __VLS_180 = {
    onClick: (...[$event]) => {
        __VLS_ctx.save('draft');
    }
};
__VLS_176.slots.default;
var __VLS_176;
const __VLS_181 = {}.AButton;
/** @type {[typeof __VLS_components.AButton, typeof __VLS_components.aButton, typeof __VLS_components.AButton, typeof __VLS_components.aButton, ]} */ ;
// @ts-ignore
const __VLS_182 = __VLS_asFunctionalComponent(__VLS_181, new __VLS_181({
    ...{ 'onClick': {} },
    type: "primary",
    danger: true,
    loading: (__VLS_ctx.saving),
}));
const __VLS_183 = __VLS_182({
    ...{ 'onClick': {} },
    type: "primary",
    danger: true,
    loading: (__VLS_ctx.saving),
}, ...__VLS_functionalComponentArgsRest(__VLS_182));
let __VLS_185;
let __VLS_186;
let __VLS_187;
const __VLS_188 = {
    onClick: (...[$event]) => {
        __VLS_ctx.save('publish');
    }
};
__VLS_184.slots.default;
var __VLS_184;
const __VLS_189 = {}.AButton;
/** @type {[typeof __VLS_components.AButton, typeof __VLS_components.aButton, typeof __VLS_components.AButton, typeof __VLS_components.aButton, ]} */ ;
// @ts-ignore
const __VLS_190 = __VLS_asFunctionalComponent(__VLS_189, new __VLS_189({
    ...{ 'onClick': {} },
}));
const __VLS_191 = __VLS_190({
    ...{ 'onClick': {} },
}, ...__VLS_functionalComponentArgsRest(__VLS_190));
let __VLS_193;
let __VLS_194;
let __VLS_195;
const __VLS_196 = {
    onClick: (...[$event]) => {
        __VLS_ctx.$router.back();
    }
};
__VLS_192.slots.default;
var __VLS_192;
var __VLS_172;
var __VLS_168;
var __VLS_8;
var __VLS_3;
var __VLS_dollars;
const __VLS_self = (await import('vue')).defineComponent({
    setup() {
        return {
            UploadOutlined: UploadOutlined,
            isEdit: isEdit,
            saving: saving,
            categoryTree: categoryTree,
            isTopVal: isTopVal,
            isRecommendVal: isRecommendVal,
            isHotVal: isHotVal,
            form: form,
            handleImageUpload: handleImageUpload,
            save: save,
        };
    },
});
export default (await import('vue')).defineComponent({
    setup() {
        return {};
    },
});
; /* PartiallyEnd: #4569/main.vue */
