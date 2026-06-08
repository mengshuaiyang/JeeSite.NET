<template>
  <div class="cms-site">
    <header class="cms-header">
      <div class="cms-header-inner">
        <a-button type="link" @click="$router.push('/cms')">&lt; 返回首页</a-button>
        <div class="cms-search-box">
          <a-input-search v-model:value="keyword" placeholder="搜索文章..." enter-button size="large" @search="doSearch" />
        </div>
      </div>
    </header>

    <main class="cms-main">
      <div v-if="searched" class="cms-content">
        <a-list item-layout="vertical" :data-source="articles" :loading="loading">
          <template #renderItem="{ item }">
            <a-list-item>
              <a-list-item-meta>
                <template #title>
                  <a @click="$router.push(`/cms/article/${item.articleCode}`)" v-html="highlight(item.title)" />
                </template>
                <template #description>
                  <span v-if="item.author">作者: {{ item.author }} &nbsp;</span>
                  <span v-if="item.publishDate">{{ dayjs(item.publishDate).format('YYYY-MM-DD') }} &nbsp;</span>
                  <span v-if="item.clickCount != null">阅读: {{ item.clickCount }}</span>
                </template>
              </a-list-item-meta>
              <div v-if="item.summary" class="cms-summary" v-html="highlight(item.summary)" />
            </a-list-item>
          </template>
          <template v-if="articles.length === 0 && !loading">
            <a-empty :description="'未找到与 "' + keyword + '" 相关的文章'" />
          </template>
        </a-list>
        <a-pagination v-if="total > pageSize" v-model:current="pageNo" :total="total" :page-size="pageSize" @change="doSearch" style="margin-top:16px;text-align:right" />
      </div>
      <div v-else class="cms-welcome">
        <a-empty description="输入关键词搜索文章" />
      </div>
    </main>

    <footer class="cms-footer">
      <p>JeeSite CMS © {{ new Date().getFullYear() }}</p>
    </footer>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { cmsFrontApi } from '@/api/cmsFront'
import type { ArticleDto } from '@/types/api'
import dayjs from 'dayjs'

const route = useRoute()
const router = useRouter()
const keyword = ref('')
const articles = ref<ArticleDto[]>([])
const loading = ref(false)
const searched = ref(false)
const pageNo = ref(1)
const pageSize = ref(10)
const total = ref(0)

const highlight = (text: string) => {
  if (!keyword.value || !text) return text
  const k = keyword.value.replace(/[.*+?^${}()|[\]\\]/g, '\\$&')
  return text.replace(new RegExp(k, 'gi'), m => `<span style="color:#1890ff;font-weight:bold">${m}</span>`)
}

const doSearch = async (val?: string) => {
  if (val !== undefined) { keyword.value = val; pageNo.value = 1 }
  if (!keyword.value.trim()) return
  loading.value = true; searched.value = true
  const res = await cmsFrontApi.articleSearch({ pageNo: pageNo.value, pageSize: pageSize.value, entity: { title: keyword.value.trim(), status: '0' } })
  if (res.code === 200 && res.data) {
    articles.value = res.data.list || []
    total.value = res.data.total || 0
  }
  loading.value = false
  router.replace({ query: { keyword: keyword.value } })
}

onMounted(() => {
  const q = route.query.keyword as string
  if (q) { keyword.value = q; doSearch() }
})
</script>

<style scoped>
.cms-site { min-height: 100vh; background: #f0f2f5; }
.cms-header { background: #fff; border-bottom: 1px solid #e8e8e8; padding: 16px 0; }
.cms-header-inner { max-width: 900px; margin: 0 auto; padding: 0 24px; }
.cms-search-box { max-width: 600px; margin: 16px auto 0; }
.cms-main { max-width: 900px; margin: 24px auto; padding: 0 24px; }
.cms-content { background: #fff; padding: 24px; border-radius: 4px; }
.cms-summary { color: #666; margin-top: 8px; }
.cms-welcome { background: #fff; padding: 64px; text-align: center; border-radius: 4px; }
.cms-footer { background: #fff; border-top: 1px solid #e8e8e8; padding: 16px; text-align: center; color: #999; margin-top: 24px; }
</style>
