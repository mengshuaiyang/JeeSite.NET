<template>
  <div class="cms-site">
    <header class="cms-header">
      <div class="cms-header-inner">
        <a-button type="link" @click="$router.push('/cms')">&lt; 返回首页</a-button>
        <h2 v-if="category">{{ category.categoryName }}</h2>
        <p v-if="category" class="cms-desc">{{ category.description }}</p>
      </div>
    </header>

    <main class="cms-main">
      <div class="cms-sidebar">
        <a-card title="栏目分类" size="small">
          <a-menu mode="inline" :selectedKeys="[categoryCode]">
            <a-menu-item v-for="cat in categories" :key="cat.categoryCode" @click="$router.push(`/cms/category/${cat.categoryCode}`)">
              <FolderOutlined /> {{ cat.categoryName }}
            </a-menu-item>
          </a-menu>
        </a-card>
      </div>

      <div class="cms-content">
        <a-list item-layout="vertical" :data-source="articles" :loading="loading">
          <template #renderItem="{ item }">
            <a-list-item>
              <a-list-item-meta>
                <template #title>
                  <a @click="$router.push(`/cms/article/${item.articleCode}`)">{{ item.title }}</a>
                </template>
                <template #description>
                  <span v-if="item.author">作者: {{ item.author }} &nbsp;</span>
                  <span v-if="item.publishDate">{{ dayjs(item.publishDate).format('YYYY-MM-DD') }}</span>
                </template>
              </a-list-item-meta>
              <div v-if="item.summary" class="cms-summary">{{ item.summary }}</div>
            </a-list-item>
          </template>
          <template v-if="articles.length === 0 && !loading">
            <a-empty description="暂无文章" />
          </template>
        </a-list>
        <a-pagination v-if="total > pageSize" v-model:current="pageNo" :total="total" :page-size="pageSize" @change="loadArticles" style="margin-top:16px;text-align:right" />
      </div>
    </main>

    <footer class="cms-footer">
      <p>JeeSite CMS © {{ new Date().getFullYear() }}</p>
    </footer>
  </div>
</template>

<script setup lang="ts">
import { FolderOutlined } from '@ant-design/icons-vue'
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { cmsFrontApi } from '@/api/cmsFront'
import type { ArticleDto, CategoryDto } from '@/types/api'
import dayjs from 'dayjs'

const route = useRoute()
const router = useRouter()
const categoryCode = ref((route.params.categoryCode as string) || '')
const category = ref<CategoryDto>()
const categories = ref<CategoryDto[]>([])
const articles = ref<ArticleDto[]>([])
const loading = ref(false)
const pageNo = ref(1)
const pageSize = ref(10)
const total = ref(0)

const loadCategories = async () => {
  const siteRes = await cmsFrontApi.getSites()
  if (siteRes.code === 200 && siteRes.data && siteRes.data.length > 0) {
    const siteCode = siteRes.data[0].siteCode
    const res = await cmsFrontApi.getCategories(siteCode)
    if (res.code === 200 && res.data) {
      categories.value = res.data
      category.value = res.data.find(c => c.categoryCode === categoryCode.value)
    }
  }
}

const loadArticles = async () => {
  loading.value = true
  const res = await cmsFrontApi.articleList({ pageNo: pageNo.value, pageSize: pageSize.value, entity: { categoryCode: categoryCode.value, status: '0' } })
  if (res.code === 200 && res.data) {
    articles.value = res.data.list || []
    total.value = res.data.total || 0
  }
  loading.value = false
}

onMounted(() => { loadCategories(); loadArticles() })
</script>

<style scoped>
.cms-site { min-height: 100vh; background: #f0f2f5; }
.cms-header { background: #fff; border-bottom: 1px solid #e8e8e8; padding: 16px 0 12px; }
.cms-header-inner { max-width: 1200px; margin: 0 auto; padding: 0 24px; }
.cms-header h2 { margin: 8px 0 0; }
.cms-desc { color: #666; margin: 4px 0 0; }
.cms-main { max-width: 1200px; margin: 24px auto; padding: 0 24px; display: flex; gap: 24px; }
.cms-sidebar { width: 260px; flex-shrink: 0; }
.cms-content { flex: 1; }
.cms-summary { color: #666; margin-top: 8px; }
.cms-footer { background: #fff; border-top: 1px solid #e8e8e8; padding: 16px; text-align: center; color: #999; }
</style>
