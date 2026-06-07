import { ref, onMounted } from 'vue';
import { fileApi } from '@/api';
import { message } from 'ant-design-vue';
import { UploadOutlined } from '@ant-design/icons-vue';
const loading = ref(false);
const showUpload = ref(false);
const files = ref([]);
const bizType = ref('');
const bizKey = ref('');
const columns = [
    { title: '预览', key: 'preview' },
    { title: '文件名', dataIndex: 'fileName', ellipsis: true },
    { title: '文件大小', key: 'fileSize', width: 100 },
    { title: '业务类型', dataIndex: 'bizType', width: 120 },
    { title: '业务主键', dataIndex: 'bizKey', width: 120 },
    { title: '操作', key: 'action', width: 120 }
];
function isImage(name) { return /\.(png|jpe?g|gif|webp|bmp|svg)$/i.test(name); }
function getExt(name) { return name.split('.').pop()?.toUpperCase() || '?'; }
function formatSize(bytes) {
    if (!bytes)
        return '-';
    const u = ['B', 'KB', 'MB', 'GB'];
    let i = 0;
    while (bytes >= 1024 && i < u.length - 1) {
        bytes /= 1024;
        i++;
    }
    return `${bytes.toFixed(1)} ${u[i]}`;
}
async function loadFiles() {
    loading.value = true;
    const res = await fileApi.list(bizType.value, bizKey.value);
    if (res.data)
        files.value = res.data;
    loading.value = false;
}
async function handleUpload(file) {
    const res = await fileApi.upload(file, bizType.value, bizKey.value);
    if (res.code === 0) {
        message.success(`${file.name} 上传成功`);
        showUpload.value = false;
        await loadFiles();
    }
    else
        message.error('上传失败');
    return false;
}
async function deleteFile(record) {
    await fileApi.delete(record.uploadId);
    message.success('已删除');
    await loadFiles();
}
onMounted(loadFiles);
debugger; /* PartiallyEnd: #3632/scriptSetup.vue */
const __VLS_ctx = {};
let __VLS_components;
let __VLS_directives;
const __VLS_0 = {}.ACard;
/** @type {[typeof __VLS_components.ACard, typeof __VLS_components.aCard, typeof __VLS_components.ACard, typeof __VLS_components.aCard, ]} */ ;
// @ts-ignore
const __VLS_1 = __VLS_asFunctionalComponent(__VLS_0, new __VLS_0({
    title: "文件管理",
}));
const __VLS_2 = __VLS_1({
    title: "文件管理",
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
    label: "业务类型",
}));
const __VLS_11 = __VLS_10({
    label: "业务类型",
}, ...__VLS_functionalComponentArgsRest(__VLS_10));
__VLS_12.slots.default;
const __VLS_13 = {}.AInput;
/** @type {[typeof __VLS_components.AInput, typeof __VLS_components.aInput, ]} */ ;
// @ts-ignore
const __VLS_14 = __VLS_asFunctionalComponent(__VLS_13, new __VLS_13({
    value: (__VLS_ctx.bizType),
    placeholder: "如 cms_article",
    ...{ style: {} },
}));
const __VLS_15 = __VLS_14({
    value: (__VLS_ctx.bizType),
    placeholder: "如 cms_article",
    ...{ style: {} },
}, ...__VLS_functionalComponentArgsRest(__VLS_14));
var __VLS_12;
const __VLS_17 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_18 = __VLS_asFunctionalComponent(__VLS_17, new __VLS_17({
    label: "业务主键",
}));
const __VLS_19 = __VLS_18({
    label: "业务主键",
}, ...__VLS_functionalComponentArgsRest(__VLS_18));
__VLS_20.slots.default;
const __VLS_21 = {}.AInput;
/** @type {[typeof __VLS_components.AInput, typeof __VLS_components.aInput, ]} */ ;
// @ts-ignore
const __VLS_22 = __VLS_asFunctionalComponent(__VLS_21, new __VLS_21({
    value: (__VLS_ctx.bizKey),
    placeholder: "业务ID",
    ...{ style: {} },
}));
const __VLS_23 = __VLS_22({
    value: (__VLS_ctx.bizKey),
    placeholder: "业务ID",
    ...{ style: {} },
}, ...__VLS_functionalComponentArgsRest(__VLS_22));
var __VLS_20;
const __VLS_25 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_26 = __VLS_asFunctionalComponent(__VLS_25, new __VLS_25({}));
const __VLS_27 = __VLS_26({}, ...__VLS_functionalComponentArgsRest(__VLS_26));
__VLS_28.slots.default;
const __VLS_29 = {}.AButton;
/** @type {[typeof __VLS_components.AButton, typeof __VLS_components.aButton, typeof __VLS_components.AButton, typeof __VLS_components.aButton, ]} */ ;
// @ts-ignore
const __VLS_30 = __VLS_asFunctionalComponent(__VLS_29, new __VLS_29({
    ...{ 'onClick': {} },
}));
const __VLS_31 = __VLS_30({
    ...{ 'onClick': {} },
}, ...__VLS_functionalComponentArgsRest(__VLS_30));
let __VLS_33;
let __VLS_34;
let __VLS_35;
const __VLS_36 = {
    onClick: (__VLS_ctx.loadFiles)
};
__VLS_32.slots.default;
var __VLS_32;
var __VLS_28;
const __VLS_37 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_38 = __VLS_asFunctionalComponent(__VLS_37, new __VLS_37({}));
const __VLS_39 = __VLS_38({}, ...__VLS_functionalComponentArgsRest(__VLS_38));
__VLS_40.slots.default;
const __VLS_41 = {}.AButton;
/** @type {[typeof __VLS_components.AButton, typeof __VLS_components.aButton, typeof __VLS_components.AButton, typeof __VLS_components.aButton, ]} */ ;
// @ts-ignore
const __VLS_42 = __VLS_asFunctionalComponent(__VLS_41, new __VLS_41({
    ...{ 'onClick': {} },
    type: "primary",
}));
const __VLS_43 = __VLS_42({
    ...{ 'onClick': {} },
    type: "primary",
}, ...__VLS_functionalComponentArgsRest(__VLS_42));
let __VLS_45;
let __VLS_46;
let __VLS_47;
const __VLS_48 = {
    onClick: (...[$event]) => {
        __VLS_ctx.showUpload = true;
    }
};
__VLS_44.slots.default;
var __VLS_44;
var __VLS_40;
var __VLS_8;
const __VLS_49 = {}.ATable;
/** @type {[typeof __VLS_components.ATable, typeof __VLS_components.aTable, typeof __VLS_components.ATable, typeof __VLS_components.aTable, ]} */ ;
// @ts-ignore
const __VLS_50 = __VLS_asFunctionalComponent(__VLS_49, new __VLS_49({
    dataSource: (__VLS_ctx.files),
    columns: (__VLS_ctx.columns),
    loading: (__VLS_ctx.loading),
    rowKey: "uploadId",
    pagination: ({ pageSize: 20 }),
}));
const __VLS_51 = __VLS_50({
    dataSource: (__VLS_ctx.files),
    columns: (__VLS_ctx.columns),
    loading: (__VLS_ctx.loading),
    rowKey: "uploadId",
    pagination: ({ pageSize: 20 }),
}, ...__VLS_functionalComponentArgsRest(__VLS_50));
__VLS_52.slots.default;
{
    const { bodyCell: __VLS_thisSlot } = __VLS_52.slots;
    const [{ record, column }] = __VLS_getSlotParams(__VLS_thisSlot);
    if (column.key === 'preview') {
        if (__VLS_ctx.isImage(record.fileName)) {
            const __VLS_53 = {}.AImage;
            /** @type {[typeof __VLS_components.AImage, typeof __VLS_components.aImage, ]} */ ;
            // @ts-ignore
            const __VLS_54 = __VLS_asFunctionalComponent(__VLS_53, new __VLS_53({
                src: (__VLS_ctx.fileApi.downloadUrl(record.uploadId)),
                width: "48",
                height: "48",
                ...{ style: {} },
            }));
            const __VLS_55 = __VLS_54({
                src: (__VLS_ctx.fileApi.downloadUrl(record.uploadId)),
                width: "48",
                height: "48",
                ...{ style: {} },
            }, ...__VLS_functionalComponentArgsRest(__VLS_54));
        }
        else {
            const __VLS_57 = {}.ATag;
            /** @type {[typeof __VLS_components.ATag, typeof __VLS_components.aTag, typeof __VLS_components.ATag, typeof __VLS_components.aTag, ]} */ ;
            // @ts-ignore
            const __VLS_58 = __VLS_asFunctionalComponent(__VLS_57, new __VLS_57({
                color: "blue",
            }));
            const __VLS_59 = __VLS_58({
                color: "blue",
            }, ...__VLS_functionalComponentArgsRest(__VLS_58));
            __VLS_60.slots.default;
            (__VLS_ctx.getExt(record.fileName));
            var __VLS_60;
        }
    }
    if (column.key === 'fileSize') {
        (__VLS_ctx.formatSize(record.fileSize));
    }
    if (column.key === 'action') {
        const __VLS_61 = {}.ASpace;
        /** @type {[typeof __VLS_components.ASpace, typeof __VLS_components.aSpace, typeof __VLS_components.ASpace, typeof __VLS_components.aSpace, ]} */ ;
        // @ts-ignore
        const __VLS_62 = __VLS_asFunctionalComponent(__VLS_61, new __VLS_61({}));
        const __VLS_63 = __VLS_62({}, ...__VLS_functionalComponentArgsRest(__VLS_62));
        __VLS_64.slots.default;
        __VLS_asFunctionalElement(__VLS_intrinsicElements.a, __VLS_intrinsicElements.a)({
            href: (__VLS_ctx.fileApi.downloadUrl(record.uploadId)),
            target: "_blank",
        });
        const __VLS_65 = {}.APopconfirm;
        /** @type {[typeof __VLS_components.APopconfirm, typeof __VLS_components.aPopconfirm, typeof __VLS_components.APopconfirm, typeof __VLS_components.aPopconfirm, ]} */ ;
        // @ts-ignore
        const __VLS_66 = __VLS_asFunctionalComponent(__VLS_65, new __VLS_65({
            ...{ 'onConfirm': {} },
            title: "确定删除?",
        }));
        const __VLS_67 = __VLS_66({
            ...{ 'onConfirm': {} },
            title: "确定删除?",
        }, ...__VLS_functionalComponentArgsRest(__VLS_66));
        let __VLS_69;
        let __VLS_70;
        let __VLS_71;
        const __VLS_72 = {
            onConfirm: (...[$event]) => {
                if (!(column.key === 'action'))
                    return;
                __VLS_ctx.deleteFile(record);
            }
        };
        __VLS_68.slots.default;
        __VLS_asFunctionalElement(__VLS_intrinsicElements.a, __VLS_intrinsicElements.a)({
            ...{ style: {} },
        });
        var __VLS_68;
        var __VLS_64;
    }
}
var __VLS_52;
const __VLS_73 = {}.AModal;
/** @type {[typeof __VLS_components.AModal, typeof __VLS_components.aModal, typeof __VLS_components.AModal, typeof __VLS_components.aModal, ]} */ ;
// @ts-ignore
const __VLS_74 = __VLS_asFunctionalComponent(__VLS_73, new __VLS_73({
    open: (__VLS_ctx.showUpload),
    title: "上传文件",
    footer: (null),
    width: "480px",
}));
const __VLS_75 = __VLS_74({
    open: (__VLS_ctx.showUpload),
    title: "上传文件",
    footer: (null),
    width: "480px",
}, ...__VLS_functionalComponentArgsRest(__VLS_74));
__VLS_76.slots.default;
const __VLS_77 = {}.AUploadDragger;
/** @type {[typeof __VLS_components.AUploadDragger, typeof __VLS_components.aUploadDragger, typeof __VLS_components.AUploadDragger, typeof __VLS_components.aUploadDragger, ]} */ ;
// @ts-ignore
const __VLS_78 = __VLS_asFunctionalComponent(__VLS_77, new __VLS_77({
    beforeUpload: (__VLS_ctx.handleUpload),
    accept: "*",
    showUploadList: (false),
}));
const __VLS_79 = __VLS_78({
    beforeUpload: (__VLS_ctx.handleUpload),
    accept: "*",
    showUploadList: (false),
}, ...__VLS_functionalComponentArgsRest(__VLS_78));
__VLS_80.slots.default;
__VLS_asFunctionalElement(__VLS_intrinsicElements.p, __VLS_intrinsicElements.p)({
    ...{ style: {} },
});
const __VLS_81 = {}.UploadOutlined;
/** @type {[typeof __VLS_components.UploadOutlined, typeof __VLS_components.uploadOutlined, ]} */ ;
// @ts-ignore
const __VLS_82 = __VLS_asFunctionalComponent(__VLS_81, new __VLS_81({}));
const __VLS_83 = __VLS_82({}, ...__VLS_functionalComponentArgsRest(__VLS_82));
__VLS_asFunctionalElement(__VLS_intrinsicElements.p, __VLS_intrinsicElements.p)({
    ...{ class: "ant-upload-text" },
});
var __VLS_80;
var __VLS_76;
var __VLS_3;
/** @type {__VLS_StyleScopedClasses['ant-upload-text']} */ ;
var __VLS_dollars;
const __VLS_self = (await import('vue')).defineComponent({
    setup() {
        return {
            fileApi: fileApi,
            UploadOutlined: UploadOutlined,
            loading: loading,
            showUpload: showUpload,
            files: files,
            bizType: bizType,
            bizKey: bizKey,
            columns: columns,
            isImage: isImage,
            getExt: getExt,
            formatSize: formatSize,
            loadFiles: loadFiles,
            handleUpload: handleUpload,
            deleteFile: deleteFile,
        };
    },
});
export default (await import('vue')).defineComponent({
    setup() {
        return {};
    },
});
; /* PartiallyEnd: #4569/main.vue */
