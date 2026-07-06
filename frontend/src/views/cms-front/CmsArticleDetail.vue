<template>
  <div class="cms-site">
    <header class="cms-header">
      <div class="cms-header-inner">
        <a-button type="link" @click="goBack">&lt; 返回</a-button>
      </div>
    </header>

    <main class="cms-main">
      <div v-if="loading" style="text-align:center;padding:64px"><a-spin /></div>
      <div v-else-if="article" class="cms-article">
        <h1 class="cms-title">{{ article.title }}</h1>
        <div class="cms-meta">
          <span v-if="article.author">作者: {{ article.author }} &nbsp;</span>
          <span v-if="article.publishDate">发布于: {{ dayjs(article.publishDate).format('YYYY-MM-DD HH:mm') }} &nbsp;</span>
          <span v-if="article.clickCount != null">阅读: {{ article.clickCount }}</span>
          <span v-if="article.categoryName"> &nbsp; 栏目: {{ article.categoryName }}</span>
        </div>
        <div v-if="article.summary" class="cms-summary">{{ article.summary }}</div>
        <div class="cms-content-body" v-html="sanitize(article.articleData?.content)" />
        <div v-if="article.tags" class="cms-tags">
          <a-tag v-for="tag in article.tags.split(',').map(t=>t.trim()).filter(Boolean)" :key="tag" color="blue">{{ tag }}</a-tag>
        </div>
      </div>
      <a-result v-else status="404" title="404" sub-title="文章不存在" />

      <div v-if="article" class="cms-comments">
        <a-comment v-for="c in comments" :key="c.commentCode">
          <template #author><a>{{ c.name }}</a></template>
          <template #content><p>{{ c.content }}</p></template>
          <template #datetime>{{ dayjs(c.createDate).format('YYYY-MM-DD HH:mm') }}</template>
        </a-comment>
        <a-empty v-if="comments.length === 0" description="暂无评论" />
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
import DOMPurify from 'dompurify'
import { cmsFrontApi } from '@/api/cmsFront'
import type { ArticleDto } from '@/types/api'
import dayjs from 'dayjs'

const route = useRoute()
const router = useRouter()
const article = ref<ArticleDto>()
const comments = ref<any[]>([])
const loading = ref(true)

// 服务端返回的文章正文为原始 HTML，渲染前用 DOMPurify 白名单消毒，防止存储型 XSS
const sanitize = (html?: string) => DOMPurify.sanitize(html || '')

const goBack = () => router.back()

const loadArticle = async () => {
  const code = route.params.articleCode as string
  if (!code) { loading.value = false; return }
  const res = await cmsFrontApi.articleGet(code)
  if (res.code === 200) article.value = res.data
  loading.value = false
}

onMounted(loadArticle)
</script>

<style scoped>
.cms-site { min-height: 100vh; background: #f0f2f5; }
.cms-header { background: #fff; border-bottom: 1px solid #e8e8e8; padding: 12px 0; }
.cms-header-inner { max-width: 900px; margin: 0 auto; padding: 0 24px; }
.cms-main { max-width: 900px; margin: 24px auto; padding: 0 24px; }
.cms-article { background: #fff; padding: 24px 32px; border-radius: 4px; }
.cms-title { font-size: 24px; margin-bottom: 12px; }
.cms-meta { color: #999; font-size: 13px; margin-bottom: 16px; padding-bottom: 12px; border-bottom: 1px solid #e8e8e8; }
.cms-summary { background: #fafafa; padding: 12px 16px; border-left: 3px solid #1890ff; color: #666; margin-bottom: 20px; }
.cms-content-body { line-height: 1.8; font-size: 15px; }
.cms-content-body :deep(img) { max-width: 100%; }
.cms-tags { margin-top: 24px; padding-top: 16px; border-top: 1px solid #e8e8e8; }
.cms-comments { background: #fff; padding: 24px 32px; border-radius: 4px; margin-top: 16px; }
.cms-footer { background: #fff; border-top: 1px solid #e8e8e8; padding: 16px; text-align: center; color: #999; margin-top: 24px; }
</style>
