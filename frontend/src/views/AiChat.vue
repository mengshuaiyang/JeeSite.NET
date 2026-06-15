<template>
  <div class="ai-chat-layout">
    <a-card title="AI 智能助手" style="flex:1;display:flex;flex-direction:column">
      <a-tabs v-model:activeKey="activeTab" size="small" style="margin-bottom:8px">
        <a-tab-pane key="chat" tab="对话" />
        <a-tab-pane key="json" tab="JSON 提取" />
        <a-tab-pane key="entity" tab="实体抽取" />
      </a-tabs>
      <div class="chat-messages" ref="messagesRef">
        <div v-for="(msg, i) in messages" :key="i" :class="['message', msg.role]">
          <a-avatar :icon="msg.role === 'user' ? 'user' : 'robot'" :style="{ background: msg.role === 'user' ? '#1890ff' : '#52c41a' }" />
          <div class="bubble">
            <div v-if="msg.sources?.length" class="sources">
              参考文章: <a v-for="s in msg.sources" :key="s" :href="`/cms/article/${s}`" target="_blank">{{ s }}</a>
            </div>
            <div class="content" v-html="renderMarkdown(msg.content)"></div>
          </div>
        </div>
        <div v-if="loading" class="message assistant">
          <a-avatar icon="robot" style="background:#52c41a" />
          <div class="bubble"><a-spin size="small" /> 思考中...</div>
        </div>
      </div>
      <div v-if="jsonResult" class="json-result">
        <a-divider>结构化结果</a-divider>
        <pre>{{ JSON.stringify(jsonResult, null, 2) }}</pre>
      </div>
      <a-divider style="margin:12px 0" />
      <div class="chat-input">
        <a-input
          v-if="activeTab === 'entity'"
          v-model:value="entityType"
          placeholder="实体类型"
          style="width:200px;margin-right:8px;margin-bottom:8px"
        />
        <a-input-search
          v-model:value="input"
          placeholder="输入您的问题..."
          enter-button="发送"
          size="large"
          @search="send"
          :loading="loading"
        />
      </div>
    </a-card>
  </div>
</template>

<script setup lang="ts">
import { ref, nextTick } from 'vue'
import { aiChatApi } from '@/api'

interface ChatMsg { role: string; content: string; sources?: string[] }

const input = ref('')
const loading = ref(false)
const activeTab = ref('chat')
const entityType = ref('object')
const jsonResult = ref<any>(null)
const messages = ref<ChatMsg[]>([
  { role: 'assistant', content: '您好！我是 JeeSite CMS 智能助手，可以回答关于网站内容的问题。' }
])
const messagesRef = ref<HTMLElement>()

function renderMarkdown(text: string) {
  const escaped = text.replace(/</g, '&lt;').replace(/>/g, '&gt;')
  return escaped
    .replace(/### (.+)/g, '<h3>$1</h3>')
    .replace(/\*\*(.+?)\*\*/g, '<strong>$1</strong>')
    .replace(/\n/g, '<br>')
}

async function send() {
  const msg = input.value.trim()
  if (!msg || loading.value) return
  input.value = ''

  messages.value.push({ role: 'user', content: msg })
  loading.value = true
  jsonResult.value = null
  scrollToBottom()

  try {
    let res: any
    if (activeTab.value === 'json') {
      res = await aiChatApi.chatJson({
        message: msg,
        history: messages.value.slice(0, -1).map(m => ({ role: m.role, content: m.content }))
      })
    } else if (activeTab.value === 'entity') {
      res = await aiChatApi.chatEntity({
        message: msg,
        entityType: entityType.value
      })
    } else {
      res = await aiChatApi.chat({
        message: msg,
        history: messages.value.slice(0, -1).map(m => ({ role: m.role, content: m.content }))
      })
    }
    if (res.code === 200 && res.data) {
      if (activeTab.value !== 'chat') {
        jsonResult.value = res.data
        messages.value.push({ role: 'assistant', content: '结构化提取完成，请查看下方结果。' })
      } else {
        messages.value.push({
          role: 'assistant',
          content: res.data.reply || '抱歉，我暂时无法回答这个问题。',
          sources: res.data.sourceArticles,
        })
      }
    } else {
      messages.value.push({ role: 'assistant', content: '服务异常，请稍后再试。' })
    }
  } catch {
    messages.value.push({ role: 'assistant', content: '网络错误，请检查连接。' })
  }
  loading.value = false
  scrollToBottom()
}

function scrollToBottom() {
  nextTick(() => {
    if (messagesRef.value) messagesRef.value.scrollTop = messagesRef.value.scrollHeight
  })
}
</script>

<style scoped>
.ai-chat-layout { height: calc(100vh - 120px); display: flex; padding: 16px; }
.chat-messages { flex: 1; overflow-y: auto; padding: 8px; }
.message { display: flex; gap: 12px; margin-bottom: 16px; align-items: flex-start; }
.message.user { flex-direction: row-reverse; }
.bubble { max-width: 70%; padding: 10px 14px; border-radius: 8px; background: #f5f5f5; }
.message.user .bubble { background: #e6f7ff; }
.sources { font-size: 12px; color: #1890ff; margin-bottom: 4px; }
.sources a { margin-right: 8px; }
.chat-input { padding: 0 8px; }
.json-result { padding: 8px; max-height: 300px; overflow-y: auto; }
.json-result pre { background: #f6f8fa; padding: 12px; border-radius: 6px; font-size: 13px; overflow-x: auto; }
</style>
