<div align="right">
  <a href="Home">← 返回首页</a>
</div>

---

# 25 Vditor编辑器

> 所见即所得 Markdown 富文本编辑器集成，图片/附件上传、AI 帮写、数学公式、流程图。
>
> **适用角色**：前端开发人员
> **阅读时间**：约 10 分钟
> **相关文档**：[12-HTML清洗](12-HTML清洗) · [20-AI智能问答](20-AI智能问答)
> 最后更新: 2026-06-13

---

## 📋 目录

- [一、编辑器概述](#一、编辑器概述)
- [二、在 Vue 组件中集成 Vditor](#二、在-vue-组件中集成-vditor)
  - [安装与依赖](#安装与依赖)
  - [Vue 3 封装组件（VditorEditor.vue）](#vue-3-封装组件（vditoreditorvue）)
  - [关键要点说明](#关键要点说明)
- [三、在 ArticleEdit.vue 中的使用示例](#三、在-articleeditvue-中的使用示例)
- [四、三种编辑模式](#四、三种编辑模式)
- [五、图片上传与文件服务](#五、图片上传与文件服务)
  - [上传流程](#上传流程)
  - [后端 API 说明](#后端-api-说明)
  - [安全与限制](#安全与限制)
  - [扩展：上传非图片文件](#扩展：上传非图片文件)
- [六、AI 写作助手（Vditor 扩展）](#六、ai-写作助手（vditor-扩展）)
  - [调用示例](#调用示例)
  - [支持的写作模式](#支持的写作模式)
  - [后端对接](#后端对接)
- [七、安全考量](#七、安全考量)
  - [1. HTML 清洗（后端）](#1-html-清洗（后端）)
  - [2. 图片 URL 同源校验](#2-图片-url-同源校验)
  - [3. CSP Header（深度防御）](#3-csp-header（深度防御）)
  - [4. 用户输入长度限制](#4-用户输入长度限制)
  - [5. CSS 隔离](#5-css-隔离)
- [八、编辑器个性化配置](#八、编辑器个性化配置)
  - [工具栏配置](#工具栏配置)
  - [主题](#主题)
  - [字号 / 字体](#字号-字体)
  - [自动保存（草稿）](#自动保存（草稿）)
- [九、常见问题](#九、常见问题)

---


## 一、编辑器概述

Vditor 是一款浏览器端所见即所得（WYSIWYG）Markdown 编辑器，由 B3log 开源社区维护，具有以下核心特性：

- **三种编辑模式**：所见即所得（WYSIWYG）、即时渲染（IR）、分屏预览（SV）
- **原生 TypeScript 支持**：与 Vue 3 无缝集成，类型定义完整
- **Markdown 原生解析**：产出标准 Markdown 文本，同时提供 HTML 渲染结果
- **丰富的工具栏**：支持标题、列表、表格、代码块、引用、上传、表情等
- **图片/文件上传**：内置上传机制，可对接任意后端文件服务 API
- **扩展性强**：支持自定义 Toolbar 按钮、快捷键、AI 写作扩展等
- **中文优化**：针对中文用户体验做了特殊优化（如自动空格、术语纠错）

在 JeeSite.NET 中的使用位置：

| 位置 | 文件路径 | 用途 |
|------|---------|------|
| CMS 文章编辑 | `frontend/src/views/cms/ArticleEdit.vue` | 编辑文章正文 |
| 审批意见 | `frontend/src/views/bpm/PendingApproval.vue` | 审批节点填写意见 |
| 站内信回复 | `frontend/src/views/sys/MsgInbox.vue` | 回复站内消息 |
| 自定义模块 | CodeGen 模板生成 | 任何需要富文本编辑的业务模块（可通过 CodeGen 配置包含 Vditor） |

---

## 二、在 Vue 组件中集成 Vditor

### 安装与依赖

```bash
cd frontend
pnpm add vditor
```

Vditor 为纯前端库，无其他依赖。已在 `frontend/package.json` 中默认包含。

### Vue 3 封装组件（VditorEditor.vue）

在 `<script setup>` 中使用，完整封装：

```vue
<template>
  <div ref="vditorRef" class="vditor-container"></div>
</template>

<script setup lang="ts">
import { ref, onMounted, onBeforeUnmount, watch } from 'vue';
import Vditor from 'vditor';
import 'vditor/dist/index.css';

const props = defineProps<{
  modelValue: string;
  placeholder?: string;
  height?: number;
  toolbarConfig?: string[]; // 自定义工具栏
  mode?: 'wysiwyg' | 'ir' | 'sv';
}>();

const emit = defineEmits<{
  'update:modelValue': [value: string];
}>();

let vditorInstance: Vditor | null = null;
const vditorRef = ref<HTMLDivElement | null>(null);

onMounted(() => {
  vditorInstance = new Vditor(vditorRef.value as HTMLElement, {
    height: props.height || 500,
    placeholder: props.placeholder || '请输入内容...',
    mode: props.mode || 'wysiwyg', // 'wysiwyg' | 'ir' | 'sv'
    toolbar: props.toolbarConfig || [
      'emoji', 'headings', 'bold', 'italic', 'strike',
      '|', 'link', 'list', 'ordered-list', 'check', 'outdent', 'indent',
      '|', 'quote', 'line', 'code', 'inline-code', 'insert-before', 'insert-after',
      '|', 'upload', 'table',
      '|', 'undo', 'redo',
      '|', 'fullscreen', 'edit-mode'
    ],
    // 图片上传: 走 JeeSite.NET 文件上传 API
    upload: {
      url: '/api/v1/sys/file/upload',
      fieldName: 'file',
      token: localStorage.getItem('token') || '',
      accept: 'image/*',
      max: 10 * 1024 * 1024, // 10MB
      multiple: false,
      // 上传成功后，将服务器返回的 URL 填入 img 标签
      success(editor: HTMLElement, msg: string) {
        try {
          const result = JSON.parse(msg);
          if (result.code === 0 && result.data?.url) {
            Vditor.singletonRenderedImage(editor, result.data.url);
          } else {
            console.error('上传失败:', result.message);
          }
        } catch (e) {
          console.error('上传响应解析失败:', e);
        }
      },
      error(editor: HTMLElement, msg: string) {
        console.error('上传错误:', msg);
      }
    },
    preview: {
      markdown: {
        toc: true,
        mark: true,
        footnotes: true,
        autoSpace: true,
        fixTermTypo: true
      },
      hljs: { style: 'github' }
    },
    cache: {
      enable: false // 禁用浏览器本地缓存，保存统一走后端
    },
    input(val: string) {
      emit('update:modelValue', val);
    },
    after() {
      if (props.modelValue) {
        vditorInstance?.setValue(props.modelValue);
      }
    }
  });
});

// 父组件 v-model 变化 → 更新编辑器内容
watch(
  () => props.modelValue,
  (newVal) => {
    if (vditorInstance && newVal !== vditorInstance.getValue()) {
      vditorInstance.setValue(newVal);
    }
  }
);

onBeforeUnmount(() => {
  if (vditorInstance) {
    vditorInstance.destroy();
    vditorInstance = null;
  }
});

// 对外暴露方法供父组件调用
defineExpose({
  getValue: () => vditorInstance?.getValue() || '',
  setValue: (val: string) => vditorInstance?.setValue(val),
  focus: () => vditorInstance?.focus(),
  insertText: (text: string) => vditorInstance?.insertValue(text)
});
</script>

<style>
.vditor-container {
  border: 1px solid #d9d9d9;
  border-radius: 6px;
}

.vditor-toolbar {
  border-bottom: 1px solid #d9d9d9;
}
</style>
```

### 关键要点说明

1. **`modelValue` 双向绑定**：通过 `input` 事件 + `watch` 实现父组件 v-model 与编辑器内容同步
2. **`upload.url`**：指向 `SysFileController.UploadAsync`，使用项目统一的文件上传接口
3. **`upload.token`**：从 `localStorage` 读取 JWT，通过 multipart header 传递给后端鉴权
4. **`cache.enable = false`**：Vditor 默认会在 localStorage 中缓存草稿内容，此处禁用以避免与后端保存逻辑冲突
5. **`defineExpose`**：对外暴露 `getValue` / `setValue` / `focus` / `insertText` 方法，供父组件或 AI 助手调用
6. **`onBeforeUnmount`**：必须调用 `destroy()` 释放 DOM 与事件监听，否则切换路由会导致内存泄漏

---

## 三、在 ArticleEdit.vue 中的使用示例

`views/cms/ArticleEdit.vue` 是 CMS 模块中的文章编辑页，使用方式如下：

```vue
<template>
  <a-form :model="formState" layout="vertical">
    <a-row :gutter="16">
      <a-col :span="12">
        <a-form-item label="文章标题" required>
          <a-input v-model:value="formState.title" placeholder="请输入文章标题" />
        </a-form-item>
      </a-col>
      <a-col :span="12">
        <a-form-item label="文章分类">
          <a-select v-model:value="formState.categoryCode" :options="categoryOptions" />
        </a-form-item>
      </a-col>
    </a-row>

    <a-form-item label="文章摘要">
      <a-textarea v-model:value="formState.description" :rows="3" placeholder="简短描述这篇文章..." />
    </a-form-item>

    <a-form-item label="文章内容">
      <div class="editor-toolbar-actions">
        <a-button @click="aiHelp">✨ AI 帮写</a-button>
        <a-button @click="previewArticle">📄 预览</a-button>
      </div>
      <!-- Vditor 编辑器 -->
      <VditorEditor
        ref="vditorEditorRef"
        v-model="formState.content"
        :height="600"
        mode="wysiwyg"
      />
    </a-form-item>

    <a-form-item>
      <a-space>
        <a-button type="primary" @click="saveArticle">保存文章</a-button>
        <a-button @click="publishArticle">发布</a-button>
        <a-button @click="cancelEdit">取消</a-button>
      </a-space>
    </a-form-item>
  </a-form>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue';
import { message } from 'ant-design-vue';
import VditorEditor from '@/components/VditorEditor.vue'; // 上一节封装的组件
import * as articleApi from '@/api/article';

const vditorEditorRef = ref<InstanceType<typeof VditorEditor> | null>(null);

const formState = reactive({
  id: '',
  title: '',
  categoryCode: '',
  description: '',
  content: ''
});

async function saveArticle() {
  // formState.content 由 VditorEditor 通过 v-model 同步，已包含完整 HTML（WYSIWYG 模式）
  // 后端 ArticleService.SaveAsync → HtmlSanitizerUtil.SanitizeRich → 存储
  const result = await articleApi.save(formState);
  if (result.code === 0) {
    message.success('保存成功');
  }
}

function publishArticle() {
  formState.status = 1; // 1 = 已发布
  saveArticle();
}

function cancelEdit() {
  history.back();
}

function previewArticle() {
  // 弹出模态，渲染 formState.content
  // ...
}

async function aiHelp() {
  // 见第六节 AI 写作助手
}

onMounted(() => {
  // 加载已有文章数据
});
</script>

<style scoped>
.editor-toolbar-actions {
  margin-bottom: 8px;
}
</style>
```

在 `PendingApproval.vue`（审批意见）和 `MsgInbox.vue`（站内信回复）中的使用方式类似，区别是：
- `PendingApproval.vue` 中 `:height="250"`，工具栏只保留基本文本按钮（bold/italic/list/quote）
- `MsgInbox.vue` 中 `:height="200"`，同样使用精简工具栏

---

## 四、三种编辑模式

Vditor 支持三种不同的编辑模式，对应不同用户群体的使用习惯：

| 模式 | 常量值 | 说明 | 适用场景 |
|------|--------|------|---------|
| 所见即所得 | `'wysiwyg'` | 类 Word 的富文本体验，编辑区就是最终渲染效果，产出 HTML | CMS 文章、消息通知、审批意见（非技术人员） |
| 即时渲染 | `'ir'` | Instant Rendering，输入 Markdown 语法后即时渲染为格式文本（类似 Typora） | 技术文档、个人博客、内部 Wiki |
| 分屏预览 | `'sv'` | Split View，左侧编辑 Markdown 源码，右侧实时渲染预览 | 开发文档、代码说明、Markdown 爱好者 |

**JeeSite.NET CMS 模块默认使用 `wysiwyg` 模式**，以保证非技术用户（如内容编辑、运营人员）获得最佳编辑体验。如需切换模式，可通过右上角「编辑模式」按钮或在 `VditorEditor` 的 `mode` prop 中传入。

三种模式产出的数据格式差异：

- `wysiwyg` 模式：`getValue()` 返回 **HTML 字符串**（可直接用于前端渲染）
- `ir` / `sv` 模式：`getValue()` 返回 **Markdown 字符串**（需 `Vditor.previewElement()` 渲染为 HTML）

后端 ArticleService 会根据存储方式对内容进行处理，`wysiwyg` 模式输出的 HTML 会经过 `HtmlSanitizerUtil.SanitizeRich` 清洗后再入库。

---

## 五、图片上传与文件服务

### 上传流程

```
┌─────────────────┐   multipart/form-data   ┌────────────────────┐
│ Vditor 编辑器    │ ──────────────────────▶ │ FileController     │
│ (upload config)  │                          │ UploadAsync        │
└─────────────────┘                          └──────────┬─────────┘
                                                         │
                        ┌────────────────────────────────┘
                        ▼
           ┌─────────────────────────────┐
           │ FileSecurityUtil            │
           │  - 扩展名白名单校验          │
           │  - 文件签名（Magic Number）   │
           │  - 文件大小限制              │
           │  - 重命名为随机文件名         │
           └──────────────┬───────────────┘
                          ▼
           ┌─────────────────────────────┐
           │ 返回 JSON                    │
           │ { code:0, data:{ url:.. } }  │
           └──────────────┬───────────────┘
                          ▼
              Vditor.singletonRenderedImage()
                    插入 img 标签
```

### 后端 API 说明

- **接口**：`POST /api/v1/sys/file/upload`
- **鉴权**：通过 `upload.token` 传递 JWT（Header: `Authorization: Bearer {token}`），FileController 自动从 Header 读取
- **请求**：`multipart/form-data`，字段名 `file`（由 `upload.fieldName` 配置）
- **响应**：

  ```json
  {
    "code": 0,
    "message": "ok",
    "data": {
      "url": "/upload/2026/06/2d4c9e8f-12ab-43cd-a567.png",
      "fileCode": "20260612-0001",
      "fileSize": 458321
    }
  }
  ```

### 安全与限制

| 限制项 | 配置位置 | 默认值 | 说明 |
|--------|---------|--------|------|
| 最大文件大小 | `ConfigService["upload.maxSize"]` | 10 MB | 超过返回 413 或 code=500 |
| 允许的扩展名 | `FileSecurityUtil.AllowedExtensions` | `.jpg/.jpeg/.png/.gif/.webp/.bmp` | 上传其他扩展名直接拒绝 |
| 文件签名校验 | `FileSecurityUtil.CheckFileSignature` | — 对文件头字节与扩展名对照，防"改扩展名绕过" |
| 存储路径 | `ConfigService["upload.path"]` | `./wwwroot/upload/{yyyy}/{MM}/` | 按年月分目录 |
| URL 同源策略 | `HtmlSanitizerUtil.SanitizeRich` | 仅允许 `/upload/` 或 `appsettings.json` 中 `Cms:TrustedImageHosts` 配置的域名 |

### 扩展：上传非图片文件

如需在 Vditor 中支持 PDF、DOCX 等附件上传，调整以下配置：

```javascript
upload: {
  accept: 'image/*,.pdf,.doc,.docx',
  // Vditor 的 upload 仅处理图片自动插入 <img>；
  // 非图片文件需要自定义 file 按钮，通过 FileController.UploadAsync 返回 URL，
  // 手动拼接 `<a href="...">文件名</a>` 插入到编辑器中
}
```

---

## 六、AI 写作助手（Vditor 扩展）

在 `ArticleEdit.vue` 顶部工具栏提供「✨ AI 帮写」按钮，通过对接 `AiChatService` 为用户提供智能写作能力。

### 调用示例

```vue
<template>
  <!-- ... -->
  <a-space>
    <a-button @click="aiHelp('summary')">📝 摘要</a-button>
    <a-button @click="aiHelp('expand')">✏️ 扩写</a-button>
    <a-button @click="aiHelp('polish')">💎 润色</a-button>
    <a-button @click="aiHelp('translate')">🌐 翻译</a-button>
    <a-button @click="aiHelp('continue')">✍️ 续写</a-button>
  </a-space>
  <VditorEditor ref="vditorEditorRef" v-model="formState.content" :height="600" />
</template>

<script setup lang="ts">
import { ref, reactive } from 'vue';
import { message } from 'ant-design-vue';
import VditorEditor from '@/components/VditorEditor.vue';
import * as aiApi from '@/api/cms';

const vditorEditorRef = ref<InstanceType<typeof VditorEditor> | null>(null);
const formState = reactive({ title: '', content: '' });

// 模式到 Prompt 模板的映射
const promptTemplates = {
  summary: '请为以下标题生成 100-200 字的文章摘要：',
  expand: '请基于以下内容扩充成更丰富的段落（300-500 字）：',
  polish: '请润色以下内容，优化语法与用词，保持原意：',
  translate: '请将以下中文内容翻译成英文：',
  continue: '请基于以下上下文继续写作：'
};

async function aiHelp(mode: keyof typeof promptTemplates) {
  if (!formState.title && !formState.content) {
    message.warning('请先输入标题或内容');
    return;
  }

  const currentContent = formState.content || formState.title;
  const fullPrompt = `${promptTemplates[mode]}\n\n${currentContent}`;

  try {
    const result = await aiApi.aiChatSend({
      message: fullPrompt,
      sessionId: `article-${Date.now()}`,
      stream: false
    });

    if (result.code === 0 && result.data?.answer) {
      const answer = result.data.answer;
      // 聚焦编辑器并插入文本（在光标位置或末尾）
      vditorEditorRef.value?.focus();
      vditorEditorRef.value?.insertText(`\n\n${answer}\n\n`);
      message.success('AI 内容已插入编辑器');
    } else {
      message.error(result.message || 'AI 调用失败');
    }
  } catch (e) {
    console.error('AI 调用异常', e);
    message.error('AI 服务暂时不可用');
  }
}
</script>
```

### 支持的写作模式

| 模式 | 说明 | 适用场景 | 字数建议 |
|------|------|---------|---------|
| **摘要生成** | 从长文章中提炼 100-200 字摘要 | 新闻/公告/SEO 描述 | 100-200 |
| **扩写** | 基于现有简短内容扩充为完整段落 | 草稿完善、内容丰富 | 300-500 |
| **润色** | 优化语法、用词、逻辑结构 | 正式发布前润色 | 保持原长 |
| **翻译** | 中↔英双向翻译 | 多语言站点内容 | 保持原长 |
| **续写** | 基于当前内容继续向下写作 | 长文分段创作 | 200-400 |

### 后端对接

上述示例调用的是 `POST /api/v1/cms/ai-chat/send`，由 `CmsAiChatController` 转发给 `AiChatService`。`AiChatService` 会根据 `appsettings.json` 中 `Ai:Provider` 的配置（支持 OpenAI / Azure OpenAI / DeepSeek / Ollama 等）调用对应的 LLM API。

配置示例：

```json
{
  "Ai": {
    "Provider": "OpenAI",
    "ApiKey": "sk-xxxxxx",
    "BaseAddress": "https://api.openai.com/v1",
    "ChatModel": "gpt-4o-mini"
  }
}
```

---

## 七、安全考量

富文本编辑器是 XSS 攻击的高频入口，JeeSite.NET 在 Vditor 的使用中实施了多层安全防御：

### 1. HTML 清洗（后端）

在 `ArticleService.SaveAsync` / `MsgService.SaveAsync` 等业务服务中，保存前调用：

```csharp
// JeeSiteNET.Core.Utils.HtmlSanitizerUtil
var safeHtml = HtmlSanitizerUtil.SanitizeRich(rawHtmlFromVditor);
// 用 safeHtml 替换原始 content 入库
```

`SanitizeRich` 的白名单策略：

| 项目 | 策略 | 示例 |
|------|------|------|
| 允许的标签 | `<div> <p> <span> <br> <hr> <h1>~<h6> <strong> <em> <b> <i> <u> <s> <strike> <blockquote> <pre> <code> <ul> <ol> <li> <a> <img> <table> <thead> <tbody> <tr> <th> <td> <figure> <figcaption>` | — |
| 允许的属性 | `href`（仅限 http/https/相对路径）、`src`（仅限 `/upload/` 及可信域名）、`alt`、`title`、`class`、`style`（限安全 CSS 属性）、`width`、`height`、`target="_blank"` | `src="javascript:..."` 被移除 |
| 协议白名单 | `http:` `https:` `mailto:` `tel:` | `javascript:` `data:` `vbscript:` 全部拒绝 |
| 事件属性 | 全部移除 | `onclick` `onload` `onerror` 等 |
| 危险标签 | 全部移除 | `<script> <iframe> <object> <embed> <form> <textarea> <input> <button>` |
| CSS 表达式 | 移除 `expression()`、`url()` 等 | 防 CSS 注入 |

> 🔒 注意：`SanitizeRich` 是**相对宽松**的富文本白名单。如果需要更严格的清洗（如用户评论），请使用 `SanitizeStrict`（仅保留最基础标签）。

### 2. 图片 URL 同源校验

`HtmlSanitizerUtil.SanitizeRich` 内部对 `<img src>` 做二次校验：

- 允许 `/upload/` 开头的相对路径（经 FileController 上传的本地文件）
- 允许 `appsettings.json` 中 `Cms:TrustedImageHosts` 配置的域名（如 CDN 域名）
- 其他域名的图片一律替换为默认占位图或移除

### 3. CSP Header（深度防御）

在 `Program.cs` 中通过中间件设置 Content-Security-Policy：

```csharp
// Program.cs
app.Use(async (context, next) =>
{
    context.Response.Headers["Content-Security-Policy"] =
        "default-src 'self'; " +
        "img-src 'self' data: https://cdn.example.com; " +
        "script-src 'self'; " +
        "style-src 'self' 'unsafe-inline'; " +
        "frame-ancestors 'self'";
    await next();
});
```

即使清洗逻辑有漏网之鱼，CSP 也能阻止 `<script>` 的执行以及非法资源加载。

### 4. 用户输入长度限制

`Article.Content` 字段的数据库类型为 `nvarchar(max)`，但在 Controller / Service 层对输入长度做限制：

```csharp
// ArticleService.SaveAsync
if (Encoding.UTF8.GetByteCount(article.Content ?? "") > 500_000)
{
    throw new UserFriendlyException("文章内容过长（上限 500,000 字符）");
}
```

防止超大内容导致数据库膨胀、内存溢出或正则表达式 DoS 攻击。

### 5. CSS 隔离

Vditor 的样式不污染全局：

- `vditor/dist/index.css` 仅定义 `.vditor-*` 命名空间下的样式
- VditorEditor 容器外层使用 `.vditor-container` 类包裹
- 业务页面中使用 `<style scoped>`，避免与 Ant Design Vue 样式冲突

---

## 八、编辑器个性化配置

### 工具栏配置

`VditorEditor.vue` 的默认工具栏是经过用户体验优化的推荐配置。如需自定义，可通过 `toolbarConfig` prop 传入：

```vue
<VditorEditor
  v-model="formState.comment"
  :height="200"
  :toolbar-config="[
    'bold', 'italic', 'strike',
    '|', 'list', 'ordered-list',
    '|', 'link', 'upload'
  ]"
/>
```

完整的 Vditor Toolbar 可用按钮：

| 按钮名 | 功能 |
|--------|------|
| `emoji` | 表情选择 |
| `headings` | 标题下拉 |
| `bold` `italic` `strike` | 粗体 / 斜体 / 删除线 |
| `link` | 超链接 |
| `list` `ordered-list` `check` | 无序列表 / 有序列表 / 任务列表 |
| `outdent` `indent` | 减少 / 增加缩进 |
| `quote` `line` | 引用 / 分割线 |
| `code` `inline-code` | 代码块 / 行内代码 |
| `insert-before` `insert-after` | 在当前块前后插入新行 |
| `upload` | 图片上传 |
| `table` | 表格 |
| `undo` `redo` | 撤销 / 重做 |
| `fullscreen` | 全屏 |
| `edit-mode` | 切换编辑模式 |
| `\|` | 分隔符 |

### 主题

Vditor 支持 light / dark 主题，跟随系统或由用户选择：

```javascript
new Vditor(el, {
  theme: 'classic',       // 主题色 'classic' | 'dark'
  preview: { theme: { path: '/css/content-theme', type: 'light' } },
  // ...
});
```

在 JeeSite.NET 中，主题跟随前端 `appStore.theme`（light / dark），在创建 Vditor 实例时动态传入。

### 字号 / 字体

可在编辑器设置中调整，保存在用户偏好（`sys_config` 或前端 `localStorage`）：

```javascript
new Vditor(el, {
  preview: {
    markdown: {
      // 代码块字体、字号由 hljs 配置控制
    }
  }
});
```

### 自动保存（草稿）

可选启用。`VditorEditor.vue` 默认 `cache.enable = false`（由后端统一保存）。如需浏览器本地草稿：

```javascript
cache: {
  enable: true,
  id: 'article-' + articleId   // 每篇文章独立缓存 key
}
```

> 注意：启用本地缓存后，用户关闭浏览器再打开可恢复内容，但需要额外处理「编辑中覆盖已有内容」的冲突提醒。

---

## 九、常见问题

| 问题 | 可能原因 | 解决办法 |
|------|---------|---------|
| **粘贴的图片不显示** | 浏览器安全策略禁止直接读取本地文件；或粘贴的是文件路径而非文件对象 | 使用编辑器的「📎 上传图片」按钮；或在支持的浏览器中直接拖拽图片到编辑区 |
| **工具栏按钮不生效** | Vditor 版本与 Vue 3 不兼容；或工具栏按钮名拼写错误 | 确保 `package.json` 中 `vditor >= 3.10.0`；核对工具栏按钮名清单（见第八节） |
| **保存后格式丢失** | 后端使用了过于严格的 `SanitizeStrict` 而非 `SanitizeRich`，导致部分标签被过滤 | 检查 `ArticleService` 中调用的是 `SanitizeRich`；如需保留特定样式标签/属性，在 `HtmlSanitizerUtil` 的白名单中补充 |
| **图片上传返回 401** | JWT Token 已过期或未正确传递 | 检查 `localStorage.getItem('token')` 是否有效；检查 FileController 的 `[Authorize]` 策略 |
| **上传大文件失败** | 超过 `upload.maxSize` 限制；或 Kestrel / IIS / Nginx 的请求体大小限制 | 在 `appsettings.json` 中调整 `upload.maxSize`；同时调整 `Program.cs` 中的 `builder.WebHost.ConfigureKestrel(options => options.Limits.MaxRequestBodySize = ...)` |
| **AI 帮写按钮无响应** | `appsettings.json` 的 `Ai:Provider` / `ApiKey` 未配置；或网络无法访问 LLM API | 检查配置项是否正确；检查后端日志 `AiChatService` 的异常；如使用代理，在 `HttpClient` 中配置代理 |
| **切换路由后页面卡死** | Vditor 实例未在 `onBeforeUnmount` 中调用 `destroy()`，导致 DOM 节点泄漏 | 按第二节示例，务必在 `onBeforeUnmount` 中 `vditorInstance.destroy()` |
| **wysiwyg 模式内容与 HTML 渲染不一致** | WYSIWYG 模式产生的 HTML 使用了 Vditor 内部 class，需要在预览区引入同样的 CSS | 在文章详情页 `ArticleDetail.vue` 中引入 `vditor/dist/css/content-theme/vditor.css`（或 `vditor.previewElement()` 渲染） |
| **首次加载内容为空** | 父组件 `modelValue` 初始值为 `undefined`，而 `after()` 回调时 vditor 已就绪但内容未传入 | 保证 `formState.content` 初始值为 `''` 而非 `undefined`；或在 `watch` 中处理 `newVal != null` 的情况 |
| **分屏模式看不到预览** | 「sv」模式需要左右两栏宽度，容器最小宽度约 768px | 检查父容器是否有足够宽度；在窄屏设备上默认回退到 wysiwyg |

---

> 💡 **提示**：Vditor 的完整 API 文档可参考 [官方 GitHub 仓库](https://github.com/Vanessa219/vditor)。JeeSite.NET 中对 Vditor 的封装以「尽量少修改上游源码」为原则，所有定制化均通过配置项和外层组件实现，便于后续升级 Vditor 版本。
---

<div align="center">
  <small>本文档最后更新: 2026-06-12 · JeeSite.NET Wiki</small>
</div>

---

## 💡 快速参考

### 核心类/文件清单

| 类型 | 文件/名称 | 说明 |
|------|----------|------|
| Component | `VditorEditor.vue` | `frontend/src/components/VditorEditor.vue`（所见即所得 Markdown 富文本编辑器封装） |
| API | `POST /api/v1/sys/file/upload` | 文件上传接口（图片/附件） |
| API | `POST /api/v1/cms/ai-chat/send` | AI 写作助手（摘要/扩写/润色/翻译/续写） |

### 常用 API 速查

| API | 说明 |
|-----|------|
| `editor.getValue()` | 获取编辑器内容（wysiwyg 模式返回 HTML，ir/sv 模式返回 Markdown） |
| `editor.setValue(value)` | 设置编辑器内容 |
| `editor.insertText(text)` | 在光标位置插入文本 |
| `editor.focus()` | 聚焦编辑器 |
| `editor.destroy()` | 销毁实例，释放 DOM 节点（切换路由务必调用） |
| `VditorEditor`（组件 ref） | `getValue()` / `setValue()` / `focus()` / `insertText()` |

### 最小工作示例（Vue 3）

```vue
<template>
  <a-form :model="formState" layout="vertical">
    <a-form-item label="文章标题" required>
      <a-input v-model:value="formState.title" placeholder="请输入文章标题" />
    </a-form-item>

    <a-form-item label="文章摘要">
      <a-textarea v-model:value="formState.description" :rows="2" />
    </a-form-item>

    <a-form-item label="正文">
      <!-- 工具栏 -->
      <a-space style="margin-bottom: 8px">
        <a-button @click="aiHelp('summary')">📝 摘要</a-button>
        <a-button @click="aiHelp('expand')">✏️ 扩写</a-button>
        <a-button @click="aiHelp('polish')">💎 润色</a-button>
        <a-button @click="aiHelp('translate')">🌐 翻译</a-button>
        <a-button @click="aiHelp('continue')">✍️ 续写</a-button>
      </a-space>

      <!-- Vditor 编辑器 -->
      <VditorEditor
        ref="editorRef"
        v-model="formState.content"
        :height="500"
        mode="wysiwyg"
        placeholder="请输入文章内容..."
      />
    </a-form-item>

    <a-form-item>
      <a-space>
        <a-button type="primary" @click="saveArticle">保存</a-button>
        <a-button @click="publishArticle">发布</a-button>
        <a-button @click="previewArticle">预览</a-button>
      </a-space>
    </a-form-item>
  </a-form>
</template>

<script setup lang="ts">
import { ref, reactive } from 'vue';
import { message } from 'ant-design-vue';
import VditorEditor from '@/components/VditorEditor.vue';
import * as articleApi from '@/api/article';
import * as aiApi from '@/api/cms';

const editorRef = ref<InstanceType<typeof VditorEditor> | null>(null);
const formState = reactive({
  id: '',
  title: '',
  description: '',
  content: ''
});

async function saveArticle() {
  const result = await articleApi.save(formState);
  if (result.code === 0) message.success('保存成功');
}

function publishArticle() {
  (formState as any).status = 1;
  saveArticle();
}

function previewArticle() {
  // 弹出模态，渲染 formState.content
}

async function aiHelp(mode: 'summary' | 'expand' | 'polish' | 'translate' | 'continue') {
  try {
    const result = await aiApi.aiChatSend({
      message: `${mode}: ${formState.title}\n${formState.content}`,
      sessionId: `article-${Date.now()}`,
      stream: false
    });
    if (result.code === 0 && result.data?.answer) {
      editorRef.value?.focus();
      editorRef.value?.insertText(`\n\n${result.data.answer}\n\n`);
      message.success('AI 内容已插入');
    }
  } catch (e) {
    message.error('AI 服务暂时不可用');
  }
}
</script>
```

### 三种编辑模式

| 模式 | `mode` 值 | 说明 | `getValue()` 返回 |
|------|-----------|------|-------------------|
| 所见即所得（WYSIWYG） | `'wysiwyg'` | 类 Word 富文本体验（默认） | HTML 字符串 |
| 即时渲染（IR） | `'ir'` | 输入 Markdown 语法后即时渲染 | Markdown 字符串 |
| 分屏预览（SV） | `'sv'` | 左侧 Markdown 源码 / 右侧实时预览 | Markdown 字符串 |

### 文件上传

| 项目 | 值 |
|------|-----|
| 接口 | `POST /api/v1/sys/file/upload` |
| 文件大小限制 | 默认 `10 MB`（可在 `appsettings.json` 中调整 `Upload:MaxSize`） |
| 允许扩展名 | `.jpg/.jpeg/.png/.gif/.webp/.bmp`（非图片需单独扩展） |
| 安全策略 | 文件头 Magic Number 校验 + 重命名为随机文件名 |
| 返回 | `{ code: 0, data: { url, fileCode, fileSize } }` |

---

## ❓ 常见问题

**Q1：粘贴的图片不显示？**
- 浏览器安全策略禁止直接读取本地文件（粘贴图片本质是读取文件）。
- 使用编辑器的「📎 上传图片」按钮；或拖拽图片到编辑区域（已支持）。

**Q2：工具栏按钮不生效（如加粗/列表无反应）？**
- 检查 `package.json` 中 `vditor` 版本是否与 Vue 3 兼容（`>= 3.10.0`）。
- 确认 `vditor/dist/index.css` 已在 `main.ts` 或组件内引入。

**Q3：保存后格式丢失？**
- 后端 `HtmlSanitizerUtil.SanitizeRich` 是相对宽松的富文本白名单，保留了 `div/p/span/img/a/table/...` 等标签。
- 若仍有标签丢失，在白名单配置中补充即可；或使用 `ir`/`sv` 模式保存 Markdown。

**Q4：图片上传返回 401？**
- JWT Token 过期或未正确传递。
- 检查 `localStorage.getItem('token')` 是否有效；检查 `FileController` 的 `[Authorize]` 策略。

**Q5：切换路由后页面卡死？**
- Vditor 实例未在 `onBeforeUnmount` 中 `destroy()`，导致 DOM 节点泄漏。
- 务必在组件 `onBeforeUnmount(() => editorInstance?.destroy())` 中释放。

**Q6：wysiwyg 模式内容与 HTML 渲染不一致？**
- WYSIWYG 模式产生的 HTML 使用 Vditor 内部 class，需要在预览区引入同样的 CSS。
- 在文章详情页引入 `vditor/dist/css/content-theme/vditor.css` 或直接使用 `Vditor.previewElement()`。

---

## 📚 相关文档

- [12-HTML清洗](12-HTML清洗) — 后端 `HtmlSanitizerUtil` 白名单策略（XSS 防护必读）
- [04-CMS内容管理](04-CMS内容管理) — 文章/栏目/分类的完整业务流程
- [20-AI智能问答](20-AI智能问答) — AI 写作助手的底层服务
- [26-AI-Tools开发](26-AI-Tools开发) — 如何将编辑器功能扩展为 AI Tool
- Home: [Wiki 首页](Home)

---

## 🚀 下一步

1. **在 CMS 文章编辑页使用 VditorEditor**：参考上述最小示例，替换当前使用的纯 textarea。
2. **启用 AI 写作助手**：确保 `Ai:Provider` + `ApiKey` 已配置，测试 5 种模式（摘要/扩写/润色/翻译/续写）。
3. **配置自定义词库与同义词**：在 `HtmlSanitizerUtil` 中补充业务场景的安全标签（如代码高亮、视频等）。
4. **启用本地草稿**：如需浏览器本地草稿，将 `VditorEditor` 中 `cache: { enable: true, id: 'article-' + articleId }` 打开。
5. **移动端适配**：窄屏设备默认回退到 `wysiwyg` 模式，确保样式与 PC 一致。

<div align="center">
  <small>本文档最后更新: 2026-06-13 · JeeSite.NET Wiki</small>
</div>