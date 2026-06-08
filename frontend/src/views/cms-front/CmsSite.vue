<template>
  <div class="cms-site">
    <header class="cms-header">
      <div class="cms-header-inner">
        <h1 v-if="site">{{ site.siteName }}</h1>
        <p v-if="site" class="cms-desc">{{ site.description }}</p>
        <div class="cms-nav">
          <a-button type="link" @click="$router.push('/cms')">首页</a-button>
          <a-button type="link" @click="$router.push('/cms/search')">搜索</a-button>
        </div>
      </div>
    </header>

    <main class="cms-main">
      <div class="cms-sidebar">
        <a-card title="栏目分类" size="small">
          <a-menu mode="inline" :selectedKeys="[]" @click="handleCategoryClick">
            <a-menu-item v-for="cat in categories" :key="cat.categoryCode">
              <template #icon><FolderOutlined /></template>
              {{ cat.categoryName }}
            </a-menu-item>
          </a-menu>
        </a-card>
        <a-card title="热门标签" size="small" style="margin-top:16px">
          <a-tag v-for="tag in hotTags" :key="tag" color="blue" style="cursor:pointer;margin-bottom:8px" @click="searchTag(tag)">
            {{ tag }}
          </a-tag>
          <a-empty v-if="hotTags.length === 0" description="暂无标签" />
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
                  <span v-if="item.publishDate">{{ formatDate(item.publishDate) }} &nbsp;</span>
                  <span v-if="item.clickCount != null">阅读: {{ item.clickCount }}</span>
                </template>
              </a-list-item-meta>
              <div v-if="item.summary" class="cms-summary">{{ item.summary }}</div>
            </a-list-item>
          </template>
        </a-list>
        <a-pagination v-if="total > pageSize" v-model:current="pageNo" :total="total" :page-size="pageSize" @change="loadArticles" show-size-changer />
      </div>
    </main>

    <footer class="cms-footer">
      <p v-if="site">{{ site.copyright || site.siteName + ' © ' + new Date().getFullYear() }}</p>
    </footer>
  </div>
</template>

<script setup lang="ts">
import { FolderOutlined } from '@ant-design/icons-vue'
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { cmsFrontApi, type SiteDto } from '@/api/cmsFront'
import type { CategoryDto, ArticleDto } from '@/types/api'
import dayjs from 'dayjs'

const router = useRouter()
const site = ref<SiteDto>()
const categories = ref<CategoryDto[]>([])
const articles = ref<ArticleDto[]>([])
const loading = ref(false)
const pageNo = ref(1)
const pageSize = ref(10)
const total = ref(0)
const hotTags = ref<string[]>([])

const formatDate = (d: string) => d ? dayjs(d).format('YYYY-MM-DD') : ''

const loadSite = async () => {
  const res = await cmsFrontApi.getSites()
  if (res.code === 200 && res.data && res.data.length > 0) {
    site.value = res.data[0]
    loadCategories(res.data[0].siteCode)
    loadArticles()
  }
}

const loadCategories = async (siteCode: string) => {
  const res = await cmsFrontApi.getCategories(siteCode)
  if (res.code === 200 && res.data) categories.value = res.data
}

const loadArticles = async () => {
  loading.value = true
  const res = await cmsFrontApi.articleList({ pageNo: pageNo.value, pageSize: pageSize.value, entity: { status: '0' } })
  if (res.code === 200 && res.data) {
    articles.value = res.data.list || []
    total.value = res.data.total || 0
    const allTags = articles.value.flatMap(a => (a.tags || '').split(',').map(t => t.trim()).filter(Boolean))
    hotTags.value = [...new Set(allTags)].slice(0, 10)
  }
  loading.value = false
}

const handleCategoryClick = ({ key }: { key: string }) => {
  router.push(`/cms/category/${key}`)
}

const searchTag = (tag: string) => {
  router.push(`/cms/search?keyword=${encodeURIComponent(tag)}`)
}

onMounted(loadSite)
</script>

<style scoped>
.cms-site { min-height: 100vh; background: #f0f2f5; }
.cms-header { background: #fff; border-bottom: 1px solid #e8e8e8; padding: 24px 0 12px; }
.cms-header-inner { max-width: 1200px; margin: 0 auto; padding: 0 24px; }
.cms-header h1 { margin: 0; font-size: 28px; }
.cms-desc { color: #666; margin: 4px 0 0; }
.cms-nav { margin-top: 8px; }
.cms-main { max-width: 1200px; margin: 24px auto; padding: 0 24px; display: flex; gap: 24px; }
.cms-sidebar { width: 260px; flex-shrink: 0; }
.cms-content { flex: 1; }
.cms-summary { color: #666; margin-top: 8px; }
.cms-footer { background: #fff; border-top: 1px solid #e8e8e8; padding: 16px; text-align: center; color: #999; }
</style>
