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
      <a-row :gutter="24">
        <!-- Sidebar -->
        <a-col :xs="24" :md="6">
          <!-- Category filter -->
          <a-card title="栏目分类" size="small" style="margin-bottom:16px">
            <a-tree v-if="categories.length" :tree-data="treeData" :default-expand-all="true"
              @select="onCategorySelect" :selected-keys="selectedCategory ? [selectedCategory] : []" />
            <a-empty v-else description="暂无栏目" />
          </a-card>

          <!-- Tag cloud -->
          <a-card title="标签云" size="small" style="margin-bottom:16px">
            <div v-if="tagCloud.length" class="tag-cloud">
              <a-tag v-for="tag in tagCloud" :key="tag.tagName"
                :color="tagColor(tag.articleCount)"
                class="tag-item"
                :style="{ fontSize: tagSize(tag.articleCount) + 'px', cursor: 'pointer' }"
                @click="onTagClick(tag.tagName)">
                {{ tag.tagName }}
              </a-tag>
            </div>
            <a-empty v-else description="暂无标签" />
          </a-card>
        </a-col>

        <!-- Main content -->
        <a-col :xs="24" :md="18">
          <!-- Filter bar -->
          <a-card style="margin-bottom:16px">
            <a-space wrap>
              <a-select v-model:value="filterRecommend" placeholder="推荐" style="width:100px" allow-clear @change="doSearch">
                <a-select-option value="1">推荐</a-select-option>
                <a-select-option value="0">非推荐</a-select-option>
              </a-select>
              <a-select v-model:value="filterHot" placeholder="热门" style="width:100px" allow-clear @change="doSearch">
                <a-select-option value="1">热门</a-select-option>
                <a-select-option value="0">非热门</a-select-option>
              </a-select>
              <a-select v-model:value="sortField" style="width:140px" @change="doSearch">
                <a-select-option value="newest">最新发布</a-select-option>
                <a-select-option value="oldest">最早发布</a-select-option>
                <a-select-option value="hottest">最多阅读</a-select-option>
              </a-select>
              <a-tag v-if="selectedCategory" closable @close="clearCategory">{{ selectedCategoryName || selectedCategory }}</a-tag>
              <a-tag v-if="selectedTag" closable @close="clearTag">标签: {{ selectedTag }}</a-tag>
            </a-space>
          </a-card>

          <div v-if="searched" class="cms-content">
            <a-list item-layout="vertical" :data-source="articles" :loading="loading">
              <template #renderItem="{ item }">
                <a-list-item>
                  <a-list-item-meta>
                    <template #title>
                      <a @click="$router.push(`/cms/article/${item.articleCode}`)" v-html="highlight(item.title)" />
                    </template>
                    <template #description>
                      <span v-if="item.categoryName">[{{ item.categoryName }}] &nbsp;</span>
                      <span v-if="item.author">作者: {{ item.author }} &nbsp;</span>
                      <span v-if="item.publishDate">{{ dayjs(item.publishDate).format('YYYY-MM-DD') }} &nbsp;</span>
                      <span v-if="item.clickCount != null">阅读: {{ item.clickCount }}</span>
                      <template v-if="item.tags">
                        <a-tag v-for="t in item.tags.split(',')" :key="t" class="article-tag" @click.stop="onTagClick(t)">{{ t }}</a-tag>
                      </template>
                    </template>
                  </a-list-item-meta>
                  <div v-if="item.summary" class="cms-summary" v-html="highlight(item.summary)" />
                </a-list-item>
              </template>
              <template v-if="articles.length === 0 && !loading">
                <a-empty :description="`未找到与 ${keyword} 相关的文章`" />
              </template>
            </a-list>
            <a-pagination v-if="total > pageSize" v-model:current="pageNo" :total="total" :page-size="pageSize" @change="doSearch" style="margin-top:16px;text-align:right" />
          </div>
          <div v-else class="cms-welcome">
            <a-empty description="输入关键词搜索文章" />
          </div>
        </a-col>
      </a-row>
    </main>

    <footer class="cms-footer">
      <p>JeeSite CMS © {{ new Date().getFullYear() }}</p>
    </footer>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, computed } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { cmsFrontApi } from '@/api/cmsFront'
import type { ArticleDto, CategoryDto, TagDto } from '@/types/api'
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

// Sidebar data
const categories = ref<CategoryDto[]>([])
const tagCloud = ref<TagDto[]>([])
const selectedCategory = ref('')
const selectedCategoryName = ref('')
const selectedTag = ref('')

// Filters
const filterRecommend = ref<string | undefined>()
const filterHot = ref<string | undefined>()
const sortField = ref('newest')

const treeData = computed(() => formatTree(categories.value, '0'))

function formatTree(list: CategoryDto[], parentCode: string): any[] {
  return list.filter(c => c.parentCode === parentCode).map(c => ({
    title: c.categoryName,
    key: c.categoryCode,
    children: formatTree(list, c.categoryCode)
  }))
}

function tagColor(count?: number): string {
  if (!count) return 'default'
  if (count >= 10) return 'red'
  if (count >= 5) return 'orange'
  if (count >= 3) return 'blue'
  return 'default'
}

function tagSize(count?: number): number {
  if (!count) return 12
  if (count >= 10) return 18
  if (count >= 5) return 15
  if (count >= 3) return 14
  return 12
}

function onCategorySelect(keys: any[]) {
  if (keys.length) {
    selectedCategory.value = keys[0] as string
    const cat = categories.value.find(c => c.categoryCode === keys[0])
    selectedCategoryName.value = cat?.categoryName || ''
    pageNo.value = 1
    doSearch()
  }
}

function clearCategory() {
  selectedCategory.value = ''
  selectedCategoryName.value = ''
  pageNo.value = 1
  doSearch()
}

function onTagClick(tagName: string) {
  selectedTag.value = selectedTag.value === tagName ? '' : tagName
  keyword.value = ''
  pageNo.value = 1
  doSearch()
}

function clearTag() {
  selectedTag.value = ''
  pageNo.value = 1
  doSearch()
}

const highlight = (text: string) => {
  if (!keyword.value || !text) return text
  const k = keyword.value.replace(/[.*+?^${}()|[\]\\]/g, '\\$&')
  return text.replace(new RegExp(k, 'gi'), m => `<span style="color:#1890ff;font-weight:bold">${m}</span>`)
}

const buildEntity = () => {
  const entity: Record<string, any> = { status: '0' }
  if (keyword.value.trim()) entity.title = keyword.value.trim()
  if (selectedCategory.value) entity.categoryCode = selectedCategory.value
  if (selectedTag.value) entity.tags = selectedTag.value
  if (filterRecommend.value) entity.isRecommend = filterRecommend.value
  if (filterHot.value) entity.isHot = filterHot.value
  return entity
}

const doSearch = async (val?: string) => {
  if (val !== undefined) { keyword.value = val; pageNo.value = 1 }
  if (!keyword.value.trim() && !selectedCategory.value && !selectedTag.value) { searched.value = true; return }
  loading.value = true; searched.value = true
  const entity = buildEntity()
  const res = await cmsFrontApi.articleSearch({ pageNo: pageNo.value, pageSize: pageSize.value, entity })
  if (res.code === 200 && res.data) {
    let list = res.data.list || []
    if (sortField.value === 'oldest') list = list.reverse()
    if (sortField.value === 'hottest') list = [...list].sort((a, b) => (b.clickCount || 0) - (a.clickCount || 0))
    articles.value = list
    total.value = res.data.total || 0
  }
  loading.value = false
  const q: Record<string, string> = {}
  if (keyword.value.trim()) q.keyword = keyword.value.trim()
  if (selectedCategory.value) q.categoryCode = selectedCategory.value
  if (selectedTag.value) q.tag = selectedTag.value
  router.replace({ query: q })
}

onMounted(async () => {
  try {
    const [catRes, tagRes] = await Promise.all([
      cmsFrontApi.getCategories(''),
      cmsFrontApi.tagCloud()
    ])
    if (catRes.data) categories.value = catRes.data
    if (tagRes.data) tagCloud.value = tagRes.data
  } catch (_) { /* ignore */ }

  const q = route.query
  if (q.keyword) { keyword.value = q.keyword as string }
  if (q.categoryCode) { selectedCategory.value = q.categoryCode as string }
  if (q.tag) { selectedTag.value = q.tag as string }
  if (q.keyword || q.categoryCode || q.tag) doSearch()
})
</script>

<style scoped>
.cms-site { min-height: 100vh; background: #f0f2f5; }
.cms-header { background: #fff; border-bottom: 1px solid #e8e8e8; padding: 16px 0; }
.cms-header-inner { max-width: 1200px; margin: 0 auto; padding: 0 24px; }
.cms-search-box { max-width: 600px; margin: 16px auto 0; }
.cms-main { max-width: 1200px; margin: 24px auto; padding: 0 24px; }
.cms-content { background: #fff; padding: 24px; border-radius: 4px; }
.cms-summary { color: #666; margin-top: 8px; }
.cms-welcome { background: #fff; padding: 64px; text-align: center; border-radius: 4px; }
.cms-footer { background: #fff; border-top: 1px solid #e8e8e8; padding: 16px; text-align: center; color: #999; margin-top: 24px; }
.tag-cloud { line-height: 2; }
.tag-item { margin: 4px; transition: transform 0.2s; }
.tag-item:hover { transform: scale(1.1); }
.article-tag { cursor: pointer; }
</style>
