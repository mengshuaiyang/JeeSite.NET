<template>
  <a-card :title="isEdit ? '编辑文章' : '写文章'">
    <a-form :model="form" layout="vertical">
      <a-row :gutter="16">
        <a-col :span="16">
          <a-form-item label="标题" required><a-input v-model:value="form.title" placeholder="文章标题" /></a-form-item>
        </a-col>
        <a-col :span="8">
          <a-form-item label="栏目" required>
            <a-tree-select v-model:value="form.categoryCode" :treeData="categoryTree" placeholder="选择栏目"
              fieldNames="{label:'categoryName',value:'categoryCode',children:'children'}" style="width:100%" />
          </a-form-item>
        </a-col>
      </a-row>
      <a-row :gutter="16">
        <a-col :span="8">
          <a-form-item label="作者"><a-input v-model:value="form.author" placeholder="作者" /></a-form-item>
        </a-col>
        <a-col :span="8">
          <a-form-item label="来源"><a-input v-model:value="form.source" placeholder="来源" /></a-form-item>
        </a-col>
        <a-col :span="8">
          <a-form-item label="发布时间"><a-date-picker v-model:value="form.publishDate" style="width:100%" /></a-form-item>
        </a-col>
      </a-row>
      <a-form-item label="副标题"><a-input v-model:value="form.subtitle" placeholder="副标题（可选）" /></a-form-item>
      <a-form-item label="摘要"><a-textarea v-model:value="form.summary" :rows="2" placeholder="文章摘要" /></a-form-item>
      <a-form-item label="正文内容">
        <a-textarea v-model:value="form.content" :rows="12" placeholder="支持 HTML 内容，可集成富文本编辑器" />
      </a-form-item>
      <a-row :gutter="16">
        <a-col :span="8">
          <a-form-item label="标签"><a-input v-model:value="form.tags" placeholder="逗号分隔" /></a-form-item>
        </a-col>
        <a-col :span="8">
          <a-form-item label="封面图">
            <a-upload :beforeUpload="handleImageUpload" accept="image/*" :showUploadList="false">
              <a-button><upload-outlined /> 上传封面</a-button>
            </a-upload>
            <a-image v-if="form.image" :src="form.image" width="80" style="margin-top:8px;border-radius:4px" />
          </a-form-item>
        </a-col>
        <a-col :span="8">
          <a-form-item label="属性">
            <a-space>
              <a-switch v-model:checked="isTopVal" checked-children="置顶" un-checked-children="置顶" />
              <a-switch v-model:checked="isRecommendVal" checked-children="推荐" un-checked-children="推荐" />
              <a-switch v-model:checked="isHotVal" checked-children="热门" un-checked-children="热门" />
            </a-space>
          </a-form-item>
        </a-col>
      </a-row>
      <a-form-item>
        <a-space>
          <a-button type="primary" @click="save('draft')" :loading="saving">保存草稿</a-button>
          <a-button type="primary" danger @click="save('publish')" :loading="saving">发布</a-button>
          <a-button @click="$router.back()">取消</a-button>
        </a-space>
      </a-form-item>
    </a-form>
  </a-card>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted, computed } from 'vue'
import { articleApi, categoryApi, fileApi } from '@/api'
import { useRoute, useRouter } from 'vue-router'
import { message } from 'ant-design-vue'
import { UploadOutlined } from '@ant-design/icons-vue'
import type { CategoryDto } from '@/types/api'
import dayjs from 'dayjs'

const route = useRoute(); const router = useRouter()
const isEdit = computed(() => !!route.query.articleCode)
const saving = ref(false)
const categoryTree = ref<CategoryDto[]>([])
const isTopVal = ref(false); const isRecommendVal = ref(false); const isHotVal = ref(false)

const form = reactive({
  articleCode: '', categoryCode: '', title: '', subtitle: '', summary: '',
  content: '', author: '', source: '', image: '', tags: '',
  isTop: '0', isRecommend: '0', isHot: '0', publishDate: undefined as any
})

async function loadCategoryTree() {
  const res = await categoryApi.tree()
  if (res.data) categoryTree.value = res.data
}
async function loadArticle() {
  const code = route.query.articleCode as string
  if (!code) return
  const res = await articleApi.get(code)
  if (res.data) {
    const d = res.data
    form.articleCode = d.articleCode; form.categoryCode = d.categoryCode
    form.title = d.title; form.subtitle = d.subtitle || ''
    form.summary = d.summary || ''; form.content = d.articleData?.content || ''
    form.author = d.author || ''; form.source = d.source || ''
    form.image = d.image || ''; form.tags = d.tags || ''
    form.publishDate = d.publishDate ? dayjs(d.publishDate) : undefined
    isTopVal.value = d.isTop === '1'; isRecommendVal.value = d.isRecommend === '1'; isHotVal.value = d.isHot === '1'
  }
}
async function handleImageUpload(file: File) {
  const res = await fileApi.upload(file, 'cms_article', form.articleCode || 'new')
  if (res.data) form.image = `/api/v1/sys/file/download/${res.data.uploadId}`
  return false
}
async function save(status: string) {
  if (!form.title) { message.warning('请输入标题'); return }
  if (!form.categoryCode) { message.warning('请选择栏目'); return }
  saving.value = true
  form.isTop = isTopVal.value ? '1' : '0'
  form.isRecommend = isRecommendVal.value ? '1' : '0'
  form.isHot = isHotVal.value ? '1' : '0'
  await articleApi.save({ ...form, publishDate: form.publishDate ? form.publishDate.toISOString() : undefined } as any)
  message.success(status === 'publish' ? '已发布' : '草稿已保存')
  router.push('/cms/article')
  saving.value = false
}
onMounted(async () => { await loadCategoryTree(); await loadArticle() })
</script>
