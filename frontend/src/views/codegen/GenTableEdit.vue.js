import { ref, reactive, onMounted, computed } from 'vue';
import { codegenApi } from '@/api';
import { useRoute, useRouter } from 'vue-router';
import { message } from 'ant-design-vue';
const route = useRoute();
const router = useRouter();
const isNew = computed(() => !route.query.tableName);
const saving = ref(false);
const loaded = ref(false);
const form = reactive({ tableName: '', className: '', moduleCode: '', functionName: '', functionAuthor: '', businessName: '', tableComment: '', columns: [] });
const colColumns = [
    { title: '列名', dataIndex: 'columnName', width: 100 },
    { title: '属性名', dataIndex: 'propertyName', width: 100 },
    { title: '类型', dataIndex: 'netType', width: 70 },
    { title: '主键', key: 'isPk', width: 40 },
    { title: '新增', key: 'isInsert', width: 40 },
    { title: '编辑', key: 'isEdit', width: 40 },
    { title: '列表', key: 'isList', width: 40 },
    { title: '查询', key: 'isQuery', width: 40 },
    { title: '查询方式', key: 'queryType', width: 100 },
    { title: '显示类型', key: 'htmlType', width: 120 },
    { title: '字典', key: 'dictType', width: 120 }
];
async function load() {
    const tn = route.query.tableName;
    if (!tn) {
        loaded.value = true;
        return;
    }
    const res = await codegenApi.get(tn);
    if (res.data) {
        form.tableName = res.data.tableName;
        form.className = res.data.className;
        form.moduleCode = res.data.moduleCode;
        form.functionName = res.data.functionName || '';
        form.functionAuthor = res.data.functionAuthor || '';
        form.businessName = res.data.businessName || '';
        form.tableComment = res.data.tableComment || '';
        form.columns = res.data.columns || [];
    }
    loaded.value = true;
}
async function handleSave() {
    saving.value = true;
    await codegenApi.save({ ...form });
    message.success('保存成功');
    router.push('/codegen/table');
    saving.value = false;
}
onMounted(load);
debugger; /* PartiallyEnd: #3632/scriptSetup.vue */
const __VLS_ctx = {};
let __VLS_components;
let __VLS_directives;
if (__VLS_ctx.loaded) {
    const __VLS_0 = {}.ACard;
    /** @type {[typeof __VLS_components.ACard, typeof __VLS_components.aCard, typeof __VLS_components.ACard, typeof __VLS_components.aCard, ]} */ ;
    // @ts-ignore
    const __VLS_1 = __VLS_asFunctionalComponent(__VLS_0, new __VLS_0({
        title: (__VLS_ctx.isNew ? '新建表配置' : '编辑表配置'),
    }));
    const __VLS_2 = __VLS_1({
        title: (__VLS_ctx.isNew ? '新建表配置' : '编辑表配置'),
    }, ...__VLS_functionalComponentArgsRest(__VLS_1));
    var __VLS_4 = {};
    __VLS_3.slots.default;
    const __VLS_5 = {}.AForm;
    /** @type {[typeof __VLS_components.AForm, typeof __VLS_components.aForm, typeof __VLS_components.AForm, typeof __VLS_components.aForm, ]} */ ;
    // @ts-ignore
    const __VLS_6 = __VLS_asFunctionalComponent(__VLS_5, new __VLS_5({
        layout: "vertical",
    }));
    const __VLS_7 = __VLS_6({
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
        span: (6),
    }));
    const __VLS_15 = __VLS_14({
        span: (6),
    }, ...__VLS_functionalComponentArgsRest(__VLS_14));
    __VLS_16.slots.default;
    const __VLS_17 = {}.AFormItem;
    /** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
    // @ts-ignore
    const __VLS_18 = __VLS_asFunctionalComponent(__VLS_17, new __VLS_17({
        label: "表名",
        required: true,
    }));
    const __VLS_19 = __VLS_18({
        label: "表名",
        required: true,
    }, ...__VLS_functionalComponentArgsRest(__VLS_18));
    __VLS_20.slots.default;
    const __VLS_21 = {}.AInput;
    /** @type {[typeof __VLS_components.AInput, typeof __VLS_components.aInput, ]} */ ;
    // @ts-ignore
    const __VLS_22 = __VLS_asFunctionalComponent(__VLS_21, new __VLS_21({
        value: (__VLS_ctx.form.tableName),
        disabled: (!__VLS_ctx.isNew),
    }));
    const __VLS_23 = __VLS_22({
        value: (__VLS_ctx.form.tableName),
        disabled: (!__VLS_ctx.isNew),
    }, ...__VLS_functionalComponentArgsRest(__VLS_22));
    var __VLS_20;
    var __VLS_16;
    const __VLS_25 = {}.ACol;
    /** @type {[typeof __VLS_components.ACol, typeof __VLS_components.aCol, typeof __VLS_components.ACol, typeof __VLS_components.aCol, ]} */ ;
    // @ts-ignore
    const __VLS_26 = __VLS_asFunctionalComponent(__VLS_25, new __VLS_25({
        span: (6),
    }));
    const __VLS_27 = __VLS_26({
        span: (6),
    }, ...__VLS_functionalComponentArgsRest(__VLS_26));
    __VLS_28.slots.default;
    const __VLS_29 = {}.AFormItem;
    /** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
    // @ts-ignore
    const __VLS_30 = __VLS_asFunctionalComponent(__VLS_29, new __VLS_29({
        label: "类名",
        required: true,
    }));
    const __VLS_31 = __VLS_30({
        label: "类名",
        required: true,
    }, ...__VLS_functionalComponentArgsRest(__VLS_30));
    __VLS_32.slots.default;
    const __VLS_33 = {}.AInput;
    /** @type {[typeof __VLS_components.AInput, typeof __VLS_components.aInput, ]} */ ;
    // @ts-ignore
    const __VLS_34 = __VLS_asFunctionalComponent(__VLS_33, new __VLS_33({
        value: (__VLS_ctx.form.className),
    }));
    const __VLS_35 = __VLS_34({
        value: (__VLS_ctx.form.className),
    }, ...__VLS_functionalComponentArgsRest(__VLS_34));
    var __VLS_32;
    var __VLS_28;
    const __VLS_37 = {}.ACol;
    /** @type {[typeof __VLS_components.ACol, typeof __VLS_components.aCol, typeof __VLS_components.ACol, typeof __VLS_components.aCol, ]} */ ;
    // @ts-ignore
    const __VLS_38 = __VLS_asFunctionalComponent(__VLS_37, new __VLS_37({
        span: (6),
    }));
    const __VLS_39 = __VLS_38({
        span: (6),
    }, ...__VLS_functionalComponentArgsRest(__VLS_38));
    __VLS_40.slots.default;
    const __VLS_41 = {}.AFormItem;
    /** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
    // @ts-ignore
    const __VLS_42 = __VLS_asFunctionalComponent(__VLS_41, new __VLS_41({
        label: "模块",
        required: true,
    }));
    const __VLS_43 = __VLS_42({
        label: "模块",
        required: true,
    }, ...__VLS_functionalComponentArgsRest(__VLS_42));
    __VLS_44.slots.default;
    const __VLS_45 = {}.AInput;
    /** @type {[typeof __VLS_components.AInput, typeof __VLS_components.aInput, ]} */ ;
    // @ts-ignore
    const __VLS_46 = __VLS_asFunctionalComponent(__VLS_45, new __VLS_45({
        value: (__VLS_ctx.form.moduleCode),
        placeholder: "如 Sys",
    }));
    const __VLS_47 = __VLS_46({
        value: (__VLS_ctx.form.moduleCode),
        placeholder: "如 Sys",
    }, ...__VLS_functionalComponentArgsRest(__VLS_46));
    var __VLS_44;
    var __VLS_40;
    const __VLS_49 = {}.ACol;
    /** @type {[typeof __VLS_components.ACol, typeof __VLS_components.aCol, typeof __VLS_components.ACol, typeof __VLS_components.aCol, ]} */ ;
    // @ts-ignore
    const __VLS_50 = __VLS_asFunctionalComponent(__VLS_49, new __VLS_49({
        span: (6),
    }));
    const __VLS_51 = __VLS_50({
        span: (6),
    }, ...__VLS_functionalComponentArgsRest(__VLS_50));
    __VLS_52.slots.default;
    const __VLS_53 = {}.AFormItem;
    /** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
    // @ts-ignore
    const __VLS_54 = __VLS_asFunctionalComponent(__VLS_53, new __VLS_53({
        label: "功能名",
    }));
    const __VLS_55 = __VLS_54({
        label: "功能名",
    }, ...__VLS_functionalComponentArgsRest(__VLS_54));
    __VLS_56.slots.default;
    const __VLS_57 = {}.AInput;
    /** @type {[typeof __VLS_components.AInput, typeof __VLS_components.aInput, ]} */ ;
    // @ts-ignore
    const __VLS_58 = __VLS_asFunctionalComponent(__VLS_57, new __VLS_57({
        value: (__VLS_ctx.form.functionName),
        placeholder: "如 用户管理",
    }));
    const __VLS_59 = __VLS_58({
        value: (__VLS_ctx.form.functionName),
        placeholder: "如 用户管理",
    }, ...__VLS_functionalComponentArgsRest(__VLS_58));
    var __VLS_56;
    var __VLS_52;
    var __VLS_12;
    const __VLS_61 = {}.ARow;
    /** @type {[typeof __VLS_components.ARow, typeof __VLS_components.aRow, typeof __VLS_components.ARow, typeof __VLS_components.aRow, ]} */ ;
    // @ts-ignore
    const __VLS_62 = __VLS_asFunctionalComponent(__VLS_61, new __VLS_61({
        gutter: (16),
    }));
    const __VLS_63 = __VLS_62({
        gutter: (16),
    }, ...__VLS_functionalComponentArgsRest(__VLS_62));
    __VLS_64.slots.default;
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
        label: "作者",
    }));
    const __VLS_71 = __VLS_70({
        label: "作者",
    }, ...__VLS_functionalComponentArgsRest(__VLS_70));
    __VLS_72.slots.default;
    const __VLS_73 = {}.AInput;
    /** @type {[typeof __VLS_components.AInput, typeof __VLS_components.aInput, ]} */ ;
    // @ts-ignore
    const __VLS_74 = __VLS_asFunctionalComponent(__VLS_73, new __VLS_73({
        value: (__VLS_ctx.form.functionAuthor),
    }));
    const __VLS_75 = __VLS_74({
        value: (__VLS_ctx.form.functionAuthor),
    }, ...__VLS_functionalComponentArgsRest(__VLS_74));
    var __VLS_72;
    var __VLS_68;
    const __VLS_77 = {}.ACol;
    /** @type {[typeof __VLS_components.ACol, typeof __VLS_components.aCol, typeof __VLS_components.ACol, typeof __VLS_components.aCol, ]} */ ;
    // @ts-ignore
    const __VLS_78 = __VLS_asFunctionalComponent(__VLS_77, new __VLS_77({
        span: (8),
    }));
    const __VLS_79 = __VLS_78({
        span: (8),
    }, ...__VLS_functionalComponentArgsRest(__VLS_78));
    __VLS_80.slots.default;
    const __VLS_81 = {}.AFormItem;
    /** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
    // @ts-ignore
    const __VLS_82 = __VLS_asFunctionalComponent(__VLS_81, new __VLS_81({
        label: "业务名",
    }));
    const __VLS_83 = __VLS_82({
        label: "业务名",
    }, ...__VLS_functionalComponentArgsRest(__VLS_82));
    __VLS_84.slots.default;
    const __VLS_85 = {}.AInput;
    /** @type {[typeof __VLS_components.AInput, typeof __VLS_components.aInput, ]} */ ;
    // @ts-ignore
    const __VLS_86 = __VLS_asFunctionalComponent(__VLS_85, new __VLS_85({
        value: (__VLS_ctx.form.businessName),
        placeholder: "如 user",
    }));
    const __VLS_87 = __VLS_86({
        value: (__VLS_ctx.form.businessName),
        placeholder: "如 user",
    }, ...__VLS_functionalComponentArgsRest(__VLS_86));
    var __VLS_84;
    var __VLS_80;
    const __VLS_89 = {}.ACol;
    /** @type {[typeof __VLS_components.ACol, typeof __VLS_components.aCol, typeof __VLS_components.ACol, typeof __VLS_components.aCol, ]} */ ;
    // @ts-ignore
    const __VLS_90 = __VLS_asFunctionalComponent(__VLS_89, new __VLS_89({
        span: (8),
    }));
    const __VLS_91 = __VLS_90({
        span: (8),
    }, ...__VLS_functionalComponentArgsRest(__VLS_90));
    __VLS_92.slots.default;
    const __VLS_93 = {}.AFormItem;
    /** @type {[typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, typeof __VLS_components.AFormItem, typeof __VLS_components.aFormItem, ]} */ ;
    // @ts-ignore
    const __VLS_94 = __VLS_asFunctionalComponent(__VLS_93, new __VLS_93({
        label: "备注",
    }));
    const __VLS_95 = __VLS_94({
        label: "备注",
    }, ...__VLS_functionalComponentArgsRest(__VLS_94));
    __VLS_96.slots.default;
    const __VLS_97 = {}.AInput;
    /** @type {[typeof __VLS_components.AInput, typeof __VLS_components.aInput, ]} */ ;
    // @ts-ignore
    const __VLS_98 = __VLS_asFunctionalComponent(__VLS_97, new __VLS_97({
        value: (__VLS_ctx.form.tableComment),
    }));
    const __VLS_99 = __VLS_98({
        value: (__VLS_ctx.form.tableComment),
    }, ...__VLS_functionalComponentArgsRest(__VLS_98));
    var __VLS_96;
    var __VLS_92;
    var __VLS_64;
    var __VLS_8;
    __VLS_asFunctionalElement(__VLS_intrinsicElements.h4, __VLS_intrinsicElements.h4)({
        ...{ style: {} },
    });
    const __VLS_101 = {}.ATable;
    /** @type {[typeof __VLS_components.ATable, typeof __VLS_components.aTable, typeof __VLS_components.ATable, typeof __VLS_components.aTable, ]} */ ;
    // @ts-ignore
    const __VLS_102 = __VLS_asFunctionalComponent(__VLS_101, new __VLS_101({
        dataSource: (__VLS_ctx.form.columns),
        columns: (__VLS_ctx.colColumns),
        rowKey: "columnName",
        pagination: (false),
        size: "small",
    }));
    const __VLS_103 = __VLS_102({
        dataSource: (__VLS_ctx.form.columns),
        columns: (__VLS_ctx.colColumns),
        rowKey: "columnName",
        pagination: (false),
        size: "small",
    }, ...__VLS_functionalComponentArgsRest(__VLS_102));
    __VLS_104.slots.default;
    {
        const { bodyCell: __VLS_thisSlot } = __VLS_104.slots;
        const [{ record, column }] = __VLS_getSlotParams(__VLS_thisSlot);
        if (column.key === 'isPk') {
            const __VLS_105 = {}.ACheckbox;
            /** @type {[typeof __VLS_components.ACheckbox, typeof __VLS_components.aCheckbox, ]} */ ;
            // @ts-ignore
            const __VLS_106 = __VLS_asFunctionalComponent(__VLS_105, new __VLS_105({
                ...{ 'onChange': {} },
                checked: (record.isPk === '1'),
            }));
            const __VLS_107 = __VLS_106({
                ...{ 'onChange': {} },
                checked: (record.isPk === '1'),
            }, ...__VLS_functionalComponentArgsRest(__VLS_106));
            let __VLS_109;
            let __VLS_110;
            let __VLS_111;
            const __VLS_112 = {
                onChange: (...[$event]) => {
                    if (!(__VLS_ctx.loaded))
                        return;
                    if (!(column.key === 'isPk'))
                        return;
                    record.isPk = $event.target.checked ? '1' : '0';
                }
            };
            var __VLS_108;
        }
        if (column.key === 'isInsert') {
            const __VLS_113 = {}.ACheckbox;
            /** @type {[typeof __VLS_components.ACheckbox, typeof __VLS_components.aCheckbox, ]} */ ;
            // @ts-ignore
            const __VLS_114 = __VLS_asFunctionalComponent(__VLS_113, new __VLS_113({
                ...{ 'onChange': {} },
                checked: (record.isInsert !== '0'),
            }));
            const __VLS_115 = __VLS_114({
                ...{ 'onChange': {} },
                checked: (record.isInsert !== '0'),
            }, ...__VLS_functionalComponentArgsRest(__VLS_114));
            let __VLS_117;
            let __VLS_118;
            let __VLS_119;
            const __VLS_120 = {
                onChange: (...[$event]) => {
                    if (!(__VLS_ctx.loaded))
                        return;
                    if (!(column.key === 'isInsert'))
                        return;
                    record.isInsert = $event.target.checked ? '1' : '0';
                }
            };
            var __VLS_116;
        }
        if (column.key === 'isEdit') {
            const __VLS_121 = {}.ACheckbox;
            /** @type {[typeof __VLS_components.ACheckbox, typeof __VLS_components.aCheckbox, ]} */ ;
            // @ts-ignore
            const __VLS_122 = __VLS_asFunctionalComponent(__VLS_121, new __VLS_121({
                ...{ 'onChange': {} },
                checked: (record.isEdit !== '0'),
            }));
            const __VLS_123 = __VLS_122({
                ...{ 'onChange': {} },
                checked: (record.isEdit !== '0'),
            }, ...__VLS_functionalComponentArgsRest(__VLS_122));
            let __VLS_125;
            let __VLS_126;
            let __VLS_127;
            const __VLS_128 = {
                onChange: (...[$event]) => {
                    if (!(__VLS_ctx.loaded))
                        return;
                    if (!(column.key === 'isEdit'))
                        return;
                    record.isEdit = $event.target.checked ? '1' : '0';
                }
            };
            var __VLS_124;
        }
        if (column.key === 'isList') {
            const __VLS_129 = {}.ACheckbox;
            /** @type {[typeof __VLS_components.ACheckbox, typeof __VLS_components.aCheckbox, ]} */ ;
            // @ts-ignore
            const __VLS_130 = __VLS_asFunctionalComponent(__VLS_129, new __VLS_129({
                ...{ 'onChange': {} },
                checked: (record.isList !== '0'),
            }));
            const __VLS_131 = __VLS_130({
                ...{ 'onChange': {} },
                checked: (record.isList !== '0'),
            }, ...__VLS_functionalComponentArgsRest(__VLS_130));
            let __VLS_133;
            let __VLS_134;
            let __VLS_135;
            const __VLS_136 = {
                onChange: (...[$event]) => {
                    if (!(__VLS_ctx.loaded))
                        return;
                    if (!(column.key === 'isList'))
                        return;
                    record.isList = $event.target.checked ? '1' : '0';
                }
            };
            var __VLS_132;
        }
        if (column.key === 'isQuery') {
            const __VLS_137 = {}.ACheckbox;
            /** @type {[typeof __VLS_components.ACheckbox, typeof __VLS_components.aCheckbox, ]} */ ;
            // @ts-ignore
            const __VLS_138 = __VLS_asFunctionalComponent(__VLS_137, new __VLS_137({
                ...{ 'onChange': {} },
                checked: (record.isQuery === '1'),
            }));
            const __VLS_139 = __VLS_138({
                ...{ 'onChange': {} },
                checked: (record.isQuery === '1'),
            }, ...__VLS_functionalComponentArgsRest(__VLS_138));
            let __VLS_141;
            let __VLS_142;
            let __VLS_143;
            const __VLS_144 = {
                onChange: (...[$event]) => {
                    if (!(__VLS_ctx.loaded))
                        return;
                    if (!(column.key === 'isQuery'))
                        return;
                    record.isQuery = $event.target.checked ? '1' : '0';
                }
            };
            var __VLS_140;
        }
        if (column.key === 'queryType') {
            const __VLS_145 = {}.ASelect;
            /** @type {[typeof __VLS_components.ASelect, typeof __VLS_components.aSelect, typeof __VLS_components.ASelect, typeof __VLS_components.aSelect, ]} */ ;
            // @ts-ignore
            const __VLS_146 = __VLS_asFunctionalComponent(__VLS_145, new __VLS_145({
                value: (record.queryType),
                ...{ style: {} },
                size: "small",
            }));
            const __VLS_147 = __VLS_146({
                value: (record.queryType),
                ...{ style: {} },
                size: "small",
            }, ...__VLS_functionalComponentArgsRest(__VLS_146));
            __VLS_148.slots.default;
            const __VLS_149 = {}.ASelectOption;
            /** @type {[typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, ]} */ ;
            // @ts-ignore
            const __VLS_150 = __VLS_asFunctionalComponent(__VLS_149, new __VLS_149({
                value: "EQ",
            }));
            const __VLS_151 = __VLS_150({
                value: "EQ",
            }, ...__VLS_functionalComponentArgsRest(__VLS_150));
            __VLS_152.slots.default;
            var __VLS_152;
            const __VLS_153 = {}.ASelectOption;
            /** @type {[typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, ]} */ ;
            // @ts-ignore
            const __VLS_154 = __VLS_asFunctionalComponent(__VLS_153, new __VLS_153({
                value: "LIKE",
            }));
            const __VLS_155 = __VLS_154({
                value: "LIKE",
            }, ...__VLS_functionalComponentArgsRest(__VLS_154));
            __VLS_156.slots.default;
            var __VLS_156;
            const __VLS_157 = {}.ASelectOption;
            /** @type {[typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, ]} */ ;
            // @ts-ignore
            const __VLS_158 = __VLS_asFunctionalComponent(__VLS_157, new __VLS_157({
                value: "GT",
            }));
            const __VLS_159 = __VLS_158({
                value: "GT",
            }, ...__VLS_functionalComponentArgsRest(__VLS_158));
            __VLS_160.slots.default;
            var __VLS_160;
            const __VLS_161 = {}.ASelectOption;
            /** @type {[typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, ]} */ ;
            // @ts-ignore
            const __VLS_162 = __VLS_asFunctionalComponent(__VLS_161, new __VLS_161({
                value: "LT",
            }));
            const __VLS_163 = __VLS_162({
                value: "LT",
            }, ...__VLS_functionalComponentArgsRest(__VLS_162));
            __VLS_164.slots.default;
            var __VLS_164;
            const __VLS_165 = {}.ASelectOption;
            /** @type {[typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, ]} */ ;
            // @ts-ignore
            const __VLS_166 = __VLS_asFunctionalComponent(__VLS_165, new __VLS_165({
                value: "BETWEEN",
            }));
            const __VLS_167 = __VLS_166({
                value: "BETWEEN",
            }, ...__VLS_functionalComponentArgsRest(__VLS_166));
            __VLS_168.slots.default;
            var __VLS_168;
            var __VLS_148;
        }
        if (column.key === 'htmlType') {
            const __VLS_169 = {}.ASelect;
            /** @type {[typeof __VLS_components.ASelect, typeof __VLS_components.aSelect, typeof __VLS_components.ASelect, typeof __VLS_components.aSelect, ]} */ ;
            // @ts-ignore
            const __VLS_170 = __VLS_asFunctionalComponent(__VLS_169, new __VLS_169({
                value: (record.htmlType),
                ...{ style: {} },
                size: "small",
            }));
            const __VLS_171 = __VLS_170({
                value: (record.htmlType),
                ...{ style: {} },
                size: "small",
            }, ...__VLS_functionalComponentArgsRest(__VLS_170));
            __VLS_172.slots.default;
            const __VLS_173 = {}.ASelectOption;
            /** @type {[typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, ]} */ ;
            // @ts-ignore
            const __VLS_174 = __VLS_asFunctionalComponent(__VLS_173, new __VLS_173({
                value: "input",
            }));
            const __VLS_175 = __VLS_174({
                value: "input",
            }, ...__VLS_functionalComponentArgsRest(__VLS_174));
            __VLS_176.slots.default;
            var __VLS_176;
            const __VLS_177 = {}.ASelectOption;
            /** @type {[typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, ]} */ ;
            // @ts-ignore
            const __VLS_178 = __VLS_asFunctionalComponent(__VLS_177, new __VLS_177({
                value: "textarea",
            }));
            const __VLS_179 = __VLS_178({
                value: "textarea",
            }, ...__VLS_functionalComponentArgsRest(__VLS_178));
            __VLS_180.slots.default;
            var __VLS_180;
            const __VLS_181 = {}.ASelectOption;
            /** @type {[typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, ]} */ ;
            // @ts-ignore
            const __VLS_182 = __VLS_asFunctionalComponent(__VLS_181, new __VLS_181({
                value: "select",
            }));
            const __VLS_183 = __VLS_182({
                value: "select",
            }, ...__VLS_functionalComponentArgsRest(__VLS_182));
            __VLS_184.slots.default;
            var __VLS_184;
            const __VLS_185 = {}.ASelectOption;
            /** @type {[typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, ]} */ ;
            // @ts-ignore
            const __VLS_186 = __VLS_asFunctionalComponent(__VLS_185, new __VLS_185({
                value: "radio",
            }));
            const __VLS_187 = __VLS_186({
                value: "radio",
            }, ...__VLS_functionalComponentArgsRest(__VLS_186));
            __VLS_188.slots.default;
            var __VLS_188;
            const __VLS_189 = {}.ASelectOption;
            /** @type {[typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, ]} */ ;
            // @ts-ignore
            const __VLS_190 = __VLS_asFunctionalComponent(__VLS_189, new __VLS_189({
                value: "checkbox",
            }));
            const __VLS_191 = __VLS_190({
                value: "checkbox",
            }, ...__VLS_functionalComponentArgsRest(__VLS_190));
            __VLS_192.slots.default;
            var __VLS_192;
            const __VLS_193 = {}.ASelectOption;
            /** @type {[typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, ]} */ ;
            // @ts-ignore
            const __VLS_194 = __VLS_asFunctionalComponent(__VLS_193, new __VLS_193({
                value: "datepicker",
            }));
            const __VLS_195 = __VLS_194({
                value: "datepicker",
            }, ...__VLS_functionalComponentArgsRest(__VLS_194));
            __VLS_196.slots.default;
            var __VLS_196;
            const __VLS_197 = {}.ASelectOption;
            /** @type {[typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, ]} */ ;
            // @ts-ignore
            const __VLS_198 = __VLS_asFunctionalComponent(__VLS_197, new __VLS_197({
                value: "datetimepicker",
            }));
            const __VLS_199 = __VLS_198({
                value: "datetimepicker",
            }, ...__VLS_functionalComponentArgsRest(__VLS_198));
            __VLS_200.slots.default;
            var __VLS_200;
            const __VLS_201 = {}.ASelectOption;
            /** @type {[typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, ]} */ ;
            // @ts-ignore
            const __VLS_202 = __VLS_asFunctionalComponent(__VLS_201, new __VLS_201({
                value: "image",
            }));
            const __VLS_203 = __VLS_202({
                value: "image",
            }, ...__VLS_functionalComponentArgsRest(__VLS_202));
            __VLS_204.slots.default;
            var __VLS_204;
            const __VLS_205 = {}.ASelectOption;
            /** @type {[typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, ]} */ ;
            // @ts-ignore
            const __VLS_206 = __VLS_asFunctionalComponent(__VLS_205, new __VLS_205({
                value: "file",
            }));
            const __VLS_207 = __VLS_206({
                value: "file",
            }, ...__VLS_functionalComponentArgsRest(__VLS_206));
            __VLS_208.slots.default;
            var __VLS_208;
            const __VLS_209 = {}.ASelectOption;
            /** @type {[typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, typeof __VLS_components.ASelectOption, typeof __VLS_components.aSelectOption, ]} */ ;
            // @ts-ignore
            const __VLS_210 = __VLS_asFunctionalComponent(__VLS_209, new __VLS_209({
                value: "editor",
            }));
            const __VLS_211 = __VLS_210({
                value: "editor",
            }, ...__VLS_functionalComponentArgsRest(__VLS_210));
            __VLS_212.slots.default;
            var __VLS_212;
            var __VLS_172;
        }
        if (column.key === 'dictType') {
            const __VLS_213 = {}.AInput;
            /** @type {[typeof __VLS_components.AInput, typeof __VLS_components.aInput, ]} */ ;
            // @ts-ignore
            const __VLS_214 = __VLS_asFunctionalComponent(__VLS_213, new __VLS_213({
                value: (record.dictType),
                size: "small",
                ...{ style: {} },
            }));
            const __VLS_215 = __VLS_214({
                value: (record.dictType),
                size: "small",
                ...{ style: {} },
            }, ...__VLS_functionalComponentArgsRest(__VLS_214));
        }
    }
    var __VLS_104;
    __VLS_asFunctionalElement(__VLS_intrinsicElements.div, __VLS_intrinsicElements.div)({
        ...{ style: {} },
    });
    const __VLS_217 = {}.AButton;
    /** @type {[typeof __VLS_components.AButton, typeof __VLS_components.aButton, typeof __VLS_components.AButton, typeof __VLS_components.aButton, ]} */ ;
    // @ts-ignore
    const __VLS_218 = __VLS_asFunctionalComponent(__VLS_217, new __VLS_217({
        ...{ 'onClick': {} },
        type: "primary",
        loading: (__VLS_ctx.saving),
    }));
    const __VLS_219 = __VLS_218({
        ...{ 'onClick': {} },
        type: "primary",
        loading: (__VLS_ctx.saving),
    }, ...__VLS_functionalComponentArgsRest(__VLS_218));
    let __VLS_221;
    let __VLS_222;
    let __VLS_223;
    const __VLS_224 = {
        onClick: (__VLS_ctx.handleSave)
    };
    __VLS_220.slots.default;
    var __VLS_220;
    const __VLS_225 = {}.AButton;
    /** @type {[typeof __VLS_components.AButton, typeof __VLS_components.aButton, typeof __VLS_components.AButton, typeof __VLS_components.aButton, ]} */ ;
    // @ts-ignore
    const __VLS_226 = __VLS_asFunctionalComponent(__VLS_225, new __VLS_225({
        ...{ 'onClick': {} },
        ...{ style: {} },
    }));
    const __VLS_227 = __VLS_226({
        ...{ 'onClick': {} },
        ...{ style: {} },
    }, ...__VLS_functionalComponentArgsRest(__VLS_226));
    let __VLS_229;
    let __VLS_230;
    let __VLS_231;
    const __VLS_232 = {
        onClick: (...[$event]) => {
            if (!(__VLS_ctx.loaded))
                return;
            __VLS_ctx.$router.back();
        }
    };
    __VLS_228.slots.default;
    var __VLS_228;
    var __VLS_3;
}
var __VLS_dollars;
const __VLS_self = (await import('vue')).defineComponent({
    setup() {
        return {
            isNew: isNew,
            saving: saving,
            loaded: loaded,
            form: form,
            colColumns: colColumns,
            handleSave: handleSave,
        };
    },
});
export default (await import('vue')).defineComponent({
    setup() {
        return {};
    },
});
; /* PartiallyEnd: #4569/main.vue */
