<template>
  <a-card title="文件管理">
    <a-form layout="inline" style="margin-bottom:16px">
      <a-form-item label="业务类型"><a-input v-model:value="bizType" placeholder="如 cms_article" style="width:160px" /></a-form-item>
      <a-form-item label="业务主键"><a-input v-model:value="bizKey" placeholder="业务ID" style="width:160px" /></a-form-item>
      <a-form-item><a-button @click="loadFiles">查询</a-button></a-form-item>
      <a-form-item><a-button type="primary" @click="showUpload = true">上传文件</a-button></a-form-item>
    </a-form>
    <a-table :dataSource="files" :columns="columns" :loading="loading" rowKey="uploadId" :pagination="{ pageSize: 20 }">
      <template #bodyCell="{ record, column }">
        <template v-if="column.key==='preview'">
          <a-image v-if="isImage(record.fileName)" :src="fileApi.downloadUrl(record.uploadId)" width="48" height="48" style="object-fit:cover;border-radius:4px" />
          <a-tag v-else color="blue">{{ getExt(record.fileName) }}</a-tag>
        </template>
        <template v-if="column.key==='fileSize'">{{ formatSize(record.fileSize) }}</template>
        <template v-if="column.key==='action'">
          <a-space>
            <a :href="fileApi.downloadUrl(record.uploadId)" target="_blank">下载</a>
            <a-popconfirm title="确定删除?" @confirm="deleteFile(record)"><a style="color:red">删除</a></a-popconfirm>
          </a-space>
        </template>
      </template>
    </a-table>
    <a-modal v-model:open="showUpload" title="上传文件" :footer="null" width="480px">
      <a-upload-dragger :beforeUpload="handleUpload" accept="*" :showUploadList="false">
        <p style="font-size:48px;color:#1890ff"><upload-outlined /></p>
        <p class="ant-upload-text">点击或拖拽文件到此处上传</p>
      </a-upload-dragger>
    </a-modal>
  </a-card>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { fileApi } from '@/api'
import { message } from 'ant-design-vue'
import { UploadOutlined } from '@ant-design/icons-vue'
import type { FileUploadDto } from '@/types/api'

const loading = ref(false); const showUpload = ref(false)
const files = ref<FileUploadDto[]>([])
const bizType = ref(''); const bizKey = ref('')

const columns = [
  { title: '预览', key: 'preview' },
  { title: '文件名', dataIndex: 'fileName', ellipsis: true },
  { title: '文件大小', key: 'fileSize', width: 100 },
  { title: '业务类型', dataIndex: 'bizType', width: 120 },
  { title: '业务主键', dataIndex: 'bizKey', width: 120 },
  { title: '操作', key: 'action', width: 120 }
]

function isImage(name: string) { return /\.(png|jpe?g|gif|webp|bmp|svg)$/i.test(name) }
function getExt(name: string) { return name.split('.').pop()?.toUpperCase() || '?' }
function formatSize(bytes: number) {
  if (!bytes) return '-'
  const u = ['B','KB','MB','GB']; let i = 0
  while (bytes >= 1024 && i < u.length-1) { bytes /= 1024; i++ }
  return `${bytes.toFixed(1)} ${u[i]}`
}
async function loadFiles() {
  loading.value = true
  const res = await fileApi.list(bizType.value, bizKey.value)
  if (res.data) files.value = res.data
  loading.value = false
}
async function handleUpload(file: File) {
  const res = await fileApi.upload(file, bizType.value, bizKey.value)
  if (res.code === 0) {
    message.success(`${file.name} 上传成功`)
    showUpload.value = false
    await loadFiles()
  } else message.error('上传失败')
  return false
}
async function deleteFile(record: FileUploadDto) {
  await fileApi.delete(record.uploadId)
  message.success('已删除')
  await loadFiles()
}
onMounted(loadFiles)
</script>
