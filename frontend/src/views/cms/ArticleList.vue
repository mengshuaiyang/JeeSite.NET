<template>
  <a-card title="文章管理">
    <a-form layout="inline" style="margin-bottom:16px">
      <a-form-item label="标题"><a-input v-model:value="query.title" placeholder="搜索标题" style="width:200px" /></a-form-item>
      <a-form-item label="栏目">
        <a-tree-select v-model:value="query.categoryCode" :treeData="categoryTree" allowClear placeholder="全部"
          treeDefaultExpandAll style="width:180px" fieldNames="{label:'categoryName',value:'categoryCode',children:'children'}" />
      </a-form-item>
      <a-form-item label="状态">
        <a-select v-model:value="query.status" allowClear placeholder="全部" style="width:100px">
          <a-select-option value="draft">草稿</a-select-option>
          <a-select-option value="publish">已发布</a-select-option>
        </a-select>
      </a-form-item>
      <a-form-item><a-button @click="loadArticles">查询</a-button></a-form-item>
      <a-form-item><a-button type="primary" @click="addArticle">写文章</a-button></a-form-item>
    </a-form>
    <a-table :dataSource="articles" :columns="columns" rowKey="articleCode" :loading="loading"
      :pagination="{ current: pageNo, pageSize, total, onChange: (p: number) => { pageNo = p; loadArticles() } }">
      <template #bodyCell="{ record, column }">
        <template v-if="column.key==='image'">
          <a-image v-if="record.image" :src="record.image" width="36" height="36" style="object-fit:cover;border-radius:2px" />
          <span v-else>-</span>
        </template>
        <template v-if="column.key==='status'">
          <a-tag :color="record.status==='publish'?'green':'orange'">{{ record.status==='publish'?'已发布':'草稿' }}</a-tag>
        </template>
        <template v-if="column.key==='action'">
          <a-space>
            <a @click="editArticle(record)">编辑</a>
            <a-popconfirm title="确定删除?" @confirm="deleteArticle(record)"><a style="color:red">删除</a></a-popconfirm>
          </a-space>
        </template>
      </template>
    </a-table>
  </a-card>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { articleApi, categoryApi } from '@/api'
import { useRouter } from 'vue-router'
import { message } from 'ant-design-vue'
import type { ArticleDto, CategoryDto } from '@/types/api'

const router = useRouter()
const loading = ref(false); const articles = ref<ArticleDto[]>([])
const categoryTree = ref<CategoryDto[]>([])
const pageNo = ref(1); const pageSize = ref(10); const total = ref(0)
const query = ref<{ title?: string; categoryCode?: string; status?: string }>({})

const columns = [
  { title: '标题', dataIndex: 'title', ellipsis: true },
  { title: '栏目', dataIndex: 'categoryName', width: 120 },
  { title: '作者', dataIndex: 'author', width: 100 },
  { title: '封面', key: 'image', width: 50 },
  { title: '点击', dataIndex: 'clickCount', width: 60 },
  { title: '状态', key: 'status', width: 80 },
  { title: '发布时间', dataIndex: 'publishDate', width: 160 },
  { title: '操作', key: 'action', width: 120 }
]

async function loadArticles() {
  loading.value = true
  const res = await articleApi.list({ pageNo: pageNo.value, pageSize: pageSize.value, entity: query.value })
  if (res.data) { articles.value = res.data.list; total.value = res.data.total }
  loading.value = false
}
async function loadCategoryTree() {
  const res = await categoryApi.tree()
  if (res.data) categoryTree.value = res.data
}
function addArticle() { router.push('/cms/article/edit') }
function editArticle(record: ArticleDto) { router.push(`/cms/article/edit?articleCode=${record.articleCode}`) }
async function deleteArticle(record: ArticleDto) {
  await articleApi.delete(record.articleCode)
  message.success('已删除'); await loadArticles()
}
onMounted(async () => { await loadCategoryTree(); await loadArticles() })
</script>
