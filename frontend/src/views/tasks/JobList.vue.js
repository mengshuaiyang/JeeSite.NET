import { ref, reactive, onMounted } from 'vue';
import { jobApi } from '@/api';
import { message } from 'ant-design-vue';
const loading = ref(false);
const saving = ref(false);
const modalOpen = ref(false);
const isEdit = ref(false);
const jobs = ref([]);
const pageNo = ref(1);
const pageSize = ref(10);
const total = ref(0);
const form = reactive({ jobName: '', jobGroup: 'DEFAULT', cronExpression: '', assemblyName: '', className: '', description: '', jobId: undefined });
const logDrawer = ref(false);
const logs = ref([]);
const columns = [
    { title: '任务名称', dataIndex: 'jobName' },
    { title: '分组', dataIndex: 'jobGroup', width: 100 },
    { title: 'Cron', dataIndex: 'cronExpression', width: 140 },
    { title: '运行状态', key: 'runStatus', width: 80 },
    { title: '状态', key: 'status', width: 60 },
    { title: '描述', dataIndex: 'description', ellipsis: true },
    { title: '操作', key: 'action', width: 280 }
];
const logColumns = [
    { title: '运行时间', dataIndex: 'runDate', width: 160 },
    { title: '结果', key: 'result', width: 80 },
    { title: '耗时(ms)', dataIndex: 'duration', width: 80 },
    { title: '错误信息', dataIndex: 'errorMessage', ellipsis: true }
];
async function load() {
    loading.value = true;
    const res = await jobApi.list({ pageNo: pageNo.value, pageSize: pageSize.value });
    if (res.data) {
        jobs.value = res.data.list;
        total.value = res.data.total;
    }
    loading.value = false;
}
function showAdd() { isEdit.value = false; form.jobId = undefined; form.jobName = ''; form.jobGroup = 'DEFAULT'; form.cronExpression = ''; form.assemblyName = ''; form.className = ''; form.description = ''; modalOpen.value = true; }
function showEdit(r) { isEdit.value = true; Object.assign(form, r); modalOpen.value = true; }
async function handleSave() {
    saving.value = true;
    await jobApi.save({ ...form });
    message.success('保存成功');
    modalOpen.value = false;
    saving.value = false;
    load();
}
async function deleteJob(r) { await jobApi.delete(r.jobId); message.success('已删除'); load(); }
async function startJob(r) { await jobApi.start(r.jobId); message.success('已启动'); load(); }
async function stopJob(r) { await jobApi.stop(r.jobId); message.success('已停止'); load(); }
async function runOnce(r) { await jobApi.runOnce(r.jobId); message.success('已触发'); load(); }
async function showLogs(r) {
    const res = await jobApi.logs(r.jobId);
    if (res.data)
        logs.value = res.data;
    logDrawer.value = true;
}
onMounted(load);
debugger; /* PartiallyEnd: #3632/scriptSetup.vue */
const __VLS_ctx = {};
let __VLS_components;
let __VLS_directives;
const __VLS_0 = {}.ACard;
/** @type {[typeof __VLS_components.ACard, typeof __VLS_components.aCard, typeof __VLS_components.ACard, typeof __VLS_components.aCard, ]} */ ;
// @ts-ignore
const __VLS_1 = __VLS_asFunctionalComponent(__VLS_0, new __VLS_0({
    title: "任务调度",
}));
const __VLS_2 = __VLS_1({
    title: "任务调度",
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
    dataSource: (__VLS_ctx.jobs),
    columns: (__VLS_ctx.columns),
    rowKey: "jobId",
    loading: (__VLS_ctx.loading),
    pagination: ({ current: __VLS_ctx.pageNo, pageSize: __VLS_ctx.pageSize, total: __VLS_ctx.total, onChange: (p) => { __VLS_ctx.pageNo = p; __VLS_ctx.load(); } }),
}));
const __VLS_15 = __VLS_14({
    dataSource: (__VLS_ctx.jobs),
    columns: (__VLS_ctx.columns),
    rowKey: "jobId",
    loading: (__VLS_ctx.loading),
    pagination: ({ current: __VLS_ctx.pageNo, pageSize: __VLS_ctx.pageSize, total: __VLS_ctx.total, onChange: (p) => { __VLS_ctx.pageNo = p; __VLS_ctx.load(); } }),
}, ...__VLS_functionalComponentArgsRest(__VLS_14));
__VLS_16.slots.default;
{
    const { bodyCell: __VLS_thisSlot } = __VLS_16.slots;
    const [{ record, column }] = __VLS_getSlotParams(__VLS_thisSlot);
    if (column.key === 'runStatus') {
        const __VLS_17 = {}.ATag;
        /** @type {[typeof __VLS_components.ATag, typeof __VLS_components.aTag, typeof __VLS_components.ATag, typeof __VLS_components.aTag, ]} */ ;
        // @ts-ignore
        const __VLS_18 = __VLS_asFunctionalComponent(__VLS_17, new __VLS_17({
            color: (record.runStatus === 'running' ? 'green' : 'default'),
        }));
        const __VLS_19 = __VLS_18({
            color: (record.runStatus === 'running' ? 'green' : 'default'),
        }, ...__VLS_functionalComponentArgsRest(__VLS_18));
        __VLS_20.slots.default;
        (record.runStatus === 'running' ? '运行中' : '停止');
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
        if (record.runStatus !== 'running') {
            const __VLS_29 = {}.APopconfirm;
            /** @type {[typeof __VLS_components.APopconfirm, typeof __VLS_components.aPopconfirm, typeof __VLS_components.APopconfirm, typeof __VLS_components.aPopconfirm, ]} */ ;
            // @ts-ignore
            const __VLS_30 = __VLS_asFunctionalComponent(__VLS_29, new __VLS_29({
                ...{ 'onConfirm': {} },
                title: "启动?",
            }));
            const __VLS_31 = __VLS_30({
                ...{ 'onConfirm': {} },
                title: "启动?",
            }, ...__VLS_functionalComponentArgsRest(__VLS_30));
            let __VLS_33;
            let __VLS_34;
            let __VLS_35;
            const __VLS_36 = {
                onConfirm: (...[$event]) => {
                    if (!(column.key === 'action'))
                        return;
                    if (!(record.runStatus !== 'running'))
                        return;
                    __VLS_ctx.startJob(record);
                }
            };
            __VLS_32.slots.default;
            __VLS_asFunctionalElement(__VLS_intrinsicElements.a, __VLS_intrinsicElements.a)({
                ...{ style: {} },
            });
            var __VLS_32;
        }
        else {
            const __VLS_37 = {}.APopconfirm;
            /** @type {[typeof __VLS_components.APopconfirm, typeof __VLS_components.aPopconfirm, typeof __VLS_components.APopconfirm, typeof __VLS_components.aPopconfirm, ]} */ ;
            // @ts-ignore
            const __VLS_38 = __VLS_asFunctionalComponent(__VLS_37, new __VLS_37({
                ...{ 'onConfirm': {} },
                title: "停止?",
            }));
            const __VLS_39 = __VLS_38({
                ...{ 'onConfirm': {} },
                title: "停止?",
            }, ...__VLS_functionalComponentArgsRest(__VLS_38));
            let __VLS_41;
            let __VLS_42;
            let __VLS_43;
            const __VLS_44 = {
                onConfirm: (...[$event]) => {
                    if (!(column.key === 'action'))
                        return;
                    if (!!(record.runStatus !== 'running'))
                        return;
                    __VLS_ctx.stopJob(record);
                }
            };
            __VLS_40.slots.default;
            __VLS_asFunctionalElement(__VLS_intrinsicElements.a, __VLS_intrinsicElements.a)({
                ...{ style: {} },
            });
            var __VLS_40;
        }
        __VLS_asFunctionalElement(__VLS_intrinsicElements.a, __VLS_intrinsicElements.a)({
            ...{ onClick: (...[$event]) => {
                    if (!(column.key === 'action'))
                        return;
                    __VLS_ctx.runOnce(record);
                } },
        });
        __VLS_asFunctionalElement(__VLS_intrinsicElements.a, __VLS_intrinsicElements.a)({
            ...{ onClick: (...[$event]) => {
                    if (!(column.key === 'action'))
                        return;
                    __VLS_ctx.showLogs(record);
                } },
        });
        const __VLS_45 = {}.APopconfirm;
        /** @type {[typeof __VLS_components.APopconfirm, typeof __VLS_components.aPopconfirm, typeof __VLS_components.APopconfirm, typeof __VLS_components.aPopconfirm, ]} */ ;
        // @ts-ignore
        const __VLS_46 = __VLS_asFunctionalComponent(__VLS_45, new __VLS_45({
            ...{ 'onConfirm': {} },
            title: "确定删除?",
        }));
        const __VLS_47 = __VLS_46({
            ...{ 'onConfirm': {} },
            title: "确定删除?",
        }, ...__VLS_functionalComponentArgsRest(__VLS_46));
        let __VLS_49;
        let __VLS_50;
        let __VLS_51;
        const __VLS_52 = {
            onConfirm: (...[$event]) => {
                if (!(column.key === 'action'))
                    return;
                __VLS_ctx.deleteJob(record);
            }
        };
        __VLS_48.slots.default;
        __VLS_asFunctionalElement(__VLS_intrinsicElements.a, __VLS_intrinsicElements.a)({
            ...{ style: {} },
        });
        var __VLS_48;
        var __VLS_28;
    }
}
var __VLS_16;
const __VLS_53 = {}.AModal;
/** @type {[typeof __VLS_components.AModal, typeof __VLS_components.aModal, typeof __VLS_components.AModal, typeof __VLS_components.aModal, ]} */ ;
// @ts-ignore
const __VLS_54 = __VLS_asFunctionalComponent(__VLS_53, new __VLS_53({
    ...{ 'onOk': {} },
    open: (__VLS_ctx.modalOpen),
    title: (__VLS_ctx.isEdit ? '编辑任务' : '新增任务'),
    confirmLoading: (__VLS_ctx.saving),
}));
const __VLS_55 = __VLS_54({
    ...{ 'onOk': {} },
    open: (__VLS_ctx.modalOpen),
    title: (__VLS_ctx.isEdit ? '编辑任务' : '新增任务'),
    confirmLoading: (__VLS_ctx.saving),
}, ...__VLS_functionalComponentArgsRest(__VLS_54));
let __VLS_57;
let __VLS_58;
let __VLS_59;
const __VLS_60 = {
    onOk: (__VLS_ctx.handleSave)
};
__VLS_56.slots.default;
const __VLS_61 = {}.AForm;
/** @type {[typeof __VLS_components.AForm, typeof __VLS_components.aForm, typeof __VLS_components.AForm, typeof __VLS_components.aForm, ]} */ ;
// @ts-ignore
const __VLS_62 = __VLS_asFunctionalComponent(__VLS_61, new __VLS_61({
    layout: "vertical",
}));
const __VLS_63 = __VLS_62({
    layout: "vertical",
}, ...__VLS_functionalComponentArgsRest(__VLS_62));
__VLS_64.slots.default;
const __VLS_65 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_66 = __VLS_asFunctionalComponent(__VLS_65, new __VLS_65({
    label: "任务名称",
    required: true,
}));
const __VLS_67 = __VLS_66({
    label: "任务名称",
    required: true,
}, ...__VLS_functionalComponentArgsRest(__VLS_66));
__VLS_68.slots.default;
const __VLS_69 = {}.AInput;
/** @type {[typeof __VLS_components.AInput, typeof __VLS_components.aInput, ]} */ ;
// @ts-ignore
const __VLS_70 = __VLS_asFunctionalComponent(__VLS_69, new __VLS_69({
    value: (__VLS_ctx.form.jobName),
}));
const __VLS_71 = __VLS_70({
    value: (__VLS_ctx.form.jobName),
}, ...__VLS_functionalComponentArgsRest(__VLS_70));
var __VLS_68;
const __VLS_73 = {}.ARow;
/** @type {[typeof __VLS_components.ARow, typeof __VLS_components.aRow, typeof __VLS_components.ARow, typeof __VLS_components.aRow, ]} */ ;
// @ts-ignore
const __VLS_74 = __VLS_asFunctionalComponent(__VLS_73, new __VLS_73({
    gutter: (16),
}));
const __VLS_75 = __VLS_74({
    gutter: (16),
}, ...__VLS_functionalComponentArgsRest(__VLS_74));
__VLS_76.slots.default;
const __VLS_77 = {}.ACol;
/** @type {[typeof __VLS_components.ACol, typeof __VLS_components.aCol, typeof __VLS_components.ACol, typeof __VLS_components.aCol, ]} */ ;
// @ts-ignore
const __VLS_78 = __VLS_asFunctionalComponent(__VLS_77, new __VLS_77({
    span: (12),
}));
const __VLS_79 = __VLS_78({
    span: (12),
}, ...__VLS_functionalComponentArgsRest(__VLS_78));
__VLS_80.slots.default;
const __VLS_81 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_82 = __VLS_asFunctionalComponent(__VLS_81, new __VLS_81({
    label: "任务分组",
}));
const __VLS_83 = __VLS_82({
    label: "任务分组",
}, ...__VLS_functionalComponentArgsRest(__VLS_82));
__VLS_84.slots.default;
const __VLS_85 = {}.AInput;
/** @type {[typeof __VLS_components.AInput, typeof __VLS_components.aInput, ]} */ ;
// @ts-ignore
const __VLS_86 = __VLS_asFunctionalComponent(__VLS_85, new __VLS_85({
    value: (__VLS_ctx.form.jobGroup),
    placeholder: "DEFAULT",
}));
const __VLS_87 = __VLS_86({
    value: (__VLS_ctx.form.jobGroup),
    placeholder: "DEFAULT",
}, ...__VLS_functionalComponentArgsRest(__VLS_86));
var __VLS_84;
var __VLS_80;
const __VLS_89 = {}.ACol;
/** @type {[typeof __VLS_components.ACol, typeof __VLS_components.aCol, typeof __VLS_components.ACol, typeof __VLS_components.aCol, ]} */ ;
// @ts-ignore
const __VLS_90 = __VLS_asFunctionalComponent(__VLS_89, new __VLS_89({
    span: (12),
}));
const __VLS_91 = __VLS_90({
    span: (12),
}, ...__VLS_functionalComponentArgsRest(__VLS_90));
__VLS_92.slots.default;
const __VLS_93 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_94 = __VLS_asFunctionalComponent(__VLS_93, new __VLS_93({
    label: "Cron 表达式",
    required: true,
}));
const __VLS_95 = __VLS_94({
    label: "Cron 表达式",
    required: true,
}, ...__VLS_functionalComponentArgsRest(__VLS_94));
__VLS_96.slots.default;
const __VLS_97 = {}.AInput;
/** @type {[typeof __VLS_components.AInput, typeof __VLS_components.aInput, ]} */ ;
// @ts-ignore
const __VLS_98 = __VLS_asFunctionalComponent(__VLS_97, new __VLS_97({
    value: (__VLS_ctx.form.cronExpression),
    placeholder: "0/5 * * * * ?",
}));
const __VLS_99 = __VLS_98({
    value: (__VLS_ctx.form.cronExpression),
    placeholder: "0/5 * * * * ?",
}, ...__VLS_functionalComponentArgsRest(__VLS_98));
var __VLS_96;
var __VLS_92;
var __VLS_76;
const __VLS_101 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_102 = __VLS_asFunctionalComponent(__VLS_101, new __VLS_101({
    label: "程序集",
}));
const __VLS_103 = __VLS_102({
    label: "程序集",
}, ...__VLS_functionalComponentArgsRest(__VLS_102));
__VLS_104.slots.default;
const __VLS_105 = {}.AInput;
/** @type {[typeof __VLS_components.AInput, typeof __VLS_components.aInput, ]} */ ;
// @ts-ignore
const __VLS_106 = __VLS_asFunctionalComponent(__VLS_105, new __VLS_105({
    value: (__VLS_ctx.form.assemblyName),
    placeholder: "JeeSiteNET.Modules.Tasks",
}));
const __VLS_107 = __VLS_106({
    value: (__VLS_ctx.form.assemblyName),
    placeholder: "JeeSiteNET.Modules.Tasks",
}, ...__VLS_functionalComponentArgsRest(__VLS_106));
var __VLS_104;
const __VLS_109 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_110 = __VLS_asFunctionalComponent(__VLS_109, new __VLS_109({
    label: "类名",
}));
const __VLS_111 = __VLS_110({
    label: "类名",
}, ...__VLS_functionalComponentArgsRest(__VLS_110));
__VLS_112.slots.default;
const __VLS_113 = {}.AInput;
/** @type {[typeof __VLS_components.AInput, typeof __VLS_components.aInput, ]} */ ;
// @ts-ignore
const __VLS_114 = __VLS_asFunctionalComponent(__VLS_113, new __VLS_113({
    value: (__VLS_ctx.form.className),
    placeholder: "JeeSiteNET.Modules.Tasks.Jobs.SampleJob",
}));
const __VLS_115 = __VLS_114({
    value: (__VLS_ctx.form.className),
    placeholder: "JeeSiteNET.Modules.Tasks.Jobs.SampleJob",
}, ...__VLS_functionalComponentArgsRest(__VLS_114));
var __VLS_112;
const __VLS_117 = {}.AFormItem;
/** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
// @ts-ignore
const __VLS_118 = __VLS_asFunctionalComponent(__VLS_117, new __VLS_117({
    label: "描述",
}));
const __VLS_119 = __VLS_118({
    label: "描述",
}, ...__VLS_functionalComponentArgsRest(__VLS_118));
__VLS_120.slots.default;
const __VLS_121 = {}.ATextarea;
/** @type {[typeof __VLS_components.ATextarea, typeof __VLS_components.aTextarea, ]} */ ;
// @ts-ignore
const __VLS_122 = __VLS_asFunctionalComponent(__VLS_121, new __VLS_121({
    value: (__VLS_ctx.form.description),
    rows: (2),
}));
const __VLS_123 = __VLS_122({
    value: (__VLS_ctx.form.description),
    rows: (2),
}, ...__VLS_functionalComponentArgsRest(__VLS_122));
var __VLS_120;
var __VLS_64;
var __VLS_56;
const __VLS_125 = {}.ADrawer;
/** @type {[typeof __VLS_components.ADrawer, typeof __VLS_components.aDrawer, typeof __VLS_components.ADrawer, typeof __VLS_components.aDrawer, ]} */ ;
// @ts-ignore
const __VLS_126 = __VLS_asFunctionalComponent(__VLS_125, new __VLS_125({
    open: (__VLS_ctx.logDrawer),
    title: "执行日志",
    placement: "right",
    width: "640px",
}));
const __VLS_127 = __VLS_126({
    open: (__VLS_ctx.logDrawer),
    title: "执行日志",
    placement: "right",
    width: "640px",
}, ...__VLS_functionalComponentArgsRest(__VLS_126));
__VLS_128.slots.default;
const __VLS_129 = {}.ATable;
/** @type {[typeof __VLS_components.ATable, typeof __VLS_components.aTable, typeof __VLS_components.ATable, typeof __VLS_components.aTable, ]} */ ;
// @ts-ignore
const __VLS_130 = __VLS_asFunctionalComponent(__VLS_129, new __VLS_129({
    dataSource: (__VLS_ctx.logs),
    columns: (__VLS_ctx.logColumns),
    rowKey: "logId",
    pagination: (false),
    size: "small",
}));
const __VLS_131 = __VLS_130({
    dataSource: (__VLS_ctx.logs),
    columns: (__VLS_ctx.logColumns),
    rowKey: "logId",
    pagination: (false),
    size: "small",
}, ...__VLS_functionalComponentArgsRest(__VLS_130));
__VLS_132.slots.default;
{
    const { bodyCell: __VLS_thisSlot } = __VLS_132.slots;
    const [{ record, column }] = __VLS_getSlotParams(__VLS_thisSlot);
    if (column.key === 'result') {
        const __VLS_133 = {}.ATag;
        /** @type {[typeof __VLS_components.ATag, typeof __VLS_components.aTag, typeof __VLS_components.ATag, typeof __VLS_components.aTag, ]} */ ;
        // @ts-ignore
        const __VLS_134 = __VLS_asFunctionalComponent(__VLS_133, new __VLS_133({
            color: (record.result === 'success' ? 'green' : 'red'),
        }));
        const __VLS_135 = __VLS_134({
            color: (record.result === 'success' ? 'green' : 'red'),
        }, ...__VLS_functionalComponentArgsRest(__VLS_134));
        __VLS_136.slots.default;
        (record.result);
        var __VLS_136;
    }
}
var __VLS_132;
var __VLS_128;
var __VLS_3;
var __VLS_dollars;
const __VLS_self = (await import('vue')).defineComponent({
    setup() {
        return {
            loading: loading,
            saving: saving,
            modalOpen: modalOpen,
            isEdit: isEdit,
            jobs: jobs,
            pageNo: pageNo,
            pageSize: pageSize,
            total: total,
            form: form,
            logDrawer: logDrawer,
            logs: logs,
            columns: columns,
            logColumns: logColumns,
            load: load,
            showAdd: showAdd,
            showEdit: showEdit,
            handleSave: handleSave,
            deleteJob: deleteJob,
            startJob: startJob,
            stopJob: stopJob,
            runOnce: runOnce,
            showLogs: showLogs,
        };
    },
});
export default (await import('vue')).defineComponent({
    setup() {
        return {};
    },
});
; /* PartiallyEnd: #4569/main.vue */
