<template>
  <a-layout style="min-height: 100vh">
    <a-layout-sider v-model:collapsed="app.collapsed" theme="dark" collapsible>
      <div class="logo">{{ app.collapsed ? 'JS' : 'JeeSite.NET' }}</div>
      <a-menu theme="dark" mode="inline" v-model:selectedKeys="selectedKeys" @click="onMenuClick">
        <template v-for="item in app.menus" :key="item.key">
          <a-menu-item v-if="!item.children?.length" :key="item.key">
            <component :is="resolveIcon(item.icon)" v-if="item.icon" />
            {{ item.title }}
          </a-menu-item>
          <a-sub-menu v-else :key="item.key">
            <template #title>
              <component :is="resolveIcon(item.icon)" v-if="item.icon" />
              <span>{{ item.title }}</span>
            </template>
            <a-menu-item v-for="child in item.children" :key="child.key">{{ child.title }}</a-menu-item>
          </a-sub-menu>
        </template>
      </a-menu>
    </a-layout-sider>
    <a-layout>
      <a-layout-header class="header">
        <menu-unfold-outlined v-if="app.collapsed" @click="app.toggleCollapsed" />
        <menu-fold-outlined v-else @click="app.toggleCollapsed" />
        <span style="margin-left: 16px">欢迎, {{ userStore.user?.userName || '用户' }}</span>
        <a-select v-if="app.sysCodes.length>1" v-model:value="app.currentSysCode" style="width:140px;margin-left:12px" @change="app.switchSysCode" size="small">
          <a-select-option v-for="c in app.sysCodes" :key="c" :value="c">{{ c }}</a-select-option>
        </a-select>
        <a-select v-if="companies.length>1" v-model:value="currentCorp" style="width:160px;margin-left:8px" @change="switchCorp" size="small">
          <a-select-option v-for="c in companies" :key="c.companyCode" :value="c.companyCode">{{ c.companyName }}</a-select-option>
        </a-select>
        <div style="flex:1" />
        <a-tooltip :title="app.darkMode ? '切换亮色' : '切换暗色'">
          <a-button type="text" @click="app.toggleDarkMode">
            <template #icon>
              <svg viewBox="0 0 1024 1024" width="1em" height="1em" fill="currentColor" style="font-size:18px">
                <path v-if="app.darkMode" d="M512 256c-141.4 0-256 114.6-256 256s114.6 256 256 256 256-114.6 256-256-114.6-256-256-256zm0 448c-105.9 0-192-86.1-192-192s86.1-192 192-192 192 86.1 192 192-86.1 192-192 192zM512 128c17.7 0 32-14.3 32-32V64c0-17.7-14.3-32-32-32s-32 14.3-32 32v32c0 17.7 14.3 32 32 32zm0 768c-17.7 0-32 14.3-32 32v32c0 17.7 14.3 32 32 32s32-14.3 32-32v-32c0-17.7-14.3-32-32-32zM195.2 195.2c-12.5-12.5-32.8-12.5-45.3 0s-12.5 32.8 0 45.3l22.6 22.6c12.5 12.5 32.8 12.5 45.3 0s12.5-32.8 0-45.3L195.2 195.2zm611.2 633.6c12.5 12.5 32.8 12.5 45.3 0s12.5-32.8 0-45.3l-22.6-22.6c-12.5-12.5-32.8-12.5-45.3 0s-12.5 32.8 0 45.3l22.6 22.6zM128 512c0-17.7-14.3-32-32-32H64c-17.7 0-32 14.3-32 32s14.3 32 32 32h32c17.7 0 32-14.3 32-32zm768 0c0-17.7 14.3-32 32-32h32c17.7 0 32 14.3 32 32s-14.3 32-32 32h-32c-17.7 0-32-14.3-32-32zM195.2 828.8l-22.6 22.6c-12.5 12.5-12.5 32.8 0 45.3s32.8 12.5 45.3 0l22.6-22.6c12.5-12.5 12.5-32.8 0-45.3s-32.8-12.5-45.3 0zM828.8 195.2l22.6-22.6c12.5-12.5 12.5-32.8 0-45.3s-32.8-12.5-45.3 0l-22.6 22.6c-12.5 12.5-12.5 32.8 0 45.3s32.8 12.5 45.3 0z"/>
                <path v-else d="M512 64c-17.7 0-32 14.3-32 32v64c0 17.7 14.3 32 32 32s32-14.3 32-32V96c0-17.7-14.3-32-32-32zm0 768c-17.7 0-32 14.3-32 32v64c0 17.7 14.3 32 32 32s32-14.3 32-32v-64c0-17.7-14.3-32-32-32zM195.2 195.2c-12.5-12.5-32.8-12.5-45.3 0s-12.5 32.8 0 45.3l45.3 45.3c12.5 12.5 32.8 12.5 45.3 0s12.5-32.8 0-45.3l-45.3-45.3zm633.6 633.6c12.5-12.5 12.5-32.8 0-45.3l-45.3-45.3c-12.5-12.5-32.8-12.5-45.3 0s-12.5 32.8 0 45.3l45.3 45.3c12.5 12.5 32.8 12.5 45.3 0zM128 512c0-17.7-14.3-32-32-32H32c-17.7 0-32 14.3-32 32s14.3 32 32 32h64c17.7 0 32-14.3 32-32zm768 0c0-17.7-14.3-32-32-32h-64c-17.7 0-32 14.3-32 32s14.3 32 32 32h64c17.7 0 32-14.3 32-32zM512 320c-106 0-192 86-192 192s86 192 192 192 192-86 192-192-86-192-192-192zm0 320c-70.7 0-128-57.3-128-128s57.3-128 128-128 128 57.3 128 128-57.3 128-128 128z"/>
              </svg>
            </template>
          </a-button>
        </a-tooltip>
        <a-button type="link" @click="handleLogout">退出</a-button>
      </a-layout-header>
      <a-layout-content class="content">
        <router-view />
      </a-layout-content>
    </a-layout>
  </a-layout>
</template>

<script setup lang="ts">
import { ref, watch, onMounted } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { useAppStore } from '@/stores/app'
import { useUserStore } from '@/stores/user'
import { companyApi } from '@/api/company'
import { switchApi } from '@/api/switch'
import {
  PieChartOutlined, SettingOutlined, MenuUnfoldOutlined, MenuFoldOutlined,
  UserOutlined, TeamOutlined, AppstoreOutlined, FileTextOutlined,
  SafetyOutlined, ToolOutlined, FolderOutlined, ProfileOutlined,
  HomeOutlined, MailOutlined, CloudOutlined, DatabaseOutlined,
  FlagOutlined, GlobalOutlined, ApartmentOutlined, LockOutlined,
  KeyOutlined, AuditOutlined, BranchesOutlined, CodeOutlined,
  BugOutlined, BuildOutlined, ThunderboltOutlined, ExperimentOutlined,
  TranslationOutlined, MessageOutlined, NotificationOutlined,
  PaperClipOutlined, PictureOutlined, VideoCameraOutlined,
  DashboardOutlined, TableOutlined, FormOutlined, CheckSquareOutlined,
  BarsOutlined, OrderedListOutlined, EditOutlined, CopyOutlined,
  DeleteOutlined, PlusOutlined, SearchOutlined, DownloadOutlined,
  UploadOutlined, ReloadOutlined, ExportOutlined, ImportOutlined,
  StarOutlined, HeartOutlined, LikeOutlined, DislikeOutlined,
  EyeOutlined, EyeInvisibleOutlined, BookOutlined, ReadOutlined,
  CalendarOutlined, ClockCircleOutlined, EnvironmentOutlined,
  PhoneOutlined, MobileOutlined, LaptopOutlined, MonitorOutlined,
  PrinterOutlined, ScanOutlined, QrcodeOutlined, BarcodeOutlined,
  TagOutlined, TagsOutlined, GiftOutlined, ShoppingOutlined,
  BankOutlined, BuildFilled, TrophyOutlined, CrownOutlined,
  SmileOutlined, FrownOutlined, MehOutlined,
  InboxOutlined, SendOutlined,
  CommentOutlined, CustomerServiceOutlined, DashboardFilled,
  AlertOutlined, WarningOutlined, InfoCircleOutlined,
  QuestionCircleOutlined, ExclamationCircleOutlined,
  CloseCircleOutlined, CheckCircleOutlined, StopOutlined,
  ShakeOutlined, FireOutlined, CoffeeOutlined,
  InsuranceOutlined, SaveOutlined, PrinterFilled,
  RollbackOutlined, ForwardOutlined, StepForwardOutlined, StepBackwardOutlined,
  FastForwardOutlined, FastBackwardOutlined, SwapOutlined,
  SwapLeftOutlined, SwapRightOutlined, VerticalAlignTopOutlined,
  VerticalAlignBottomOutlined, VerticalAlignMiddleOutlined,
  FullscreenOutlined, FullscreenExitOutlined, ColumnWidthOutlined,
  ColumnHeightOutlined, WifiOutlined,
  CarOutlined, RocketOutlined,
  CompassOutlined, AimOutlined, RadiusUpleftOutlined,
  RadiusUprightOutlined, RadiusBottomleftOutlined, RadiusBottomrightOutlined,
  BorderOutlined, BorderInnerOutlined, BorderOuterOutlined,
  BorderLeftOutlined, BorderRightOutlined, BorderTopOutlined,
  BorderBottomOutlined, BorderVerticleOutlined, BorderHorizontalOutlined,
  MinusOutlined, PlusCircleOutlined, MinusCircleOutlined,
  CloseOutlined, CheckOutlined, EllipsisOutlined,
  LoadingOutlined, SyncOutlined, PoweroffOutlined,
  DragOutlined, MenuOutlined, AppstoreAddOutlined,
  UnorderedListOutlined, NumberOutlined,
  FontColorsOutlined, FontSizeOutlined, BoldOutlined,
  ItalicOutlined, UnderlineOutlined, StrikethroughOutlined,
  HighlightOutlined, AlignLeftOutlined, AlignCenterOutlined,
  AlignRightOutlined, BgColorsOutlined, LineHeightOutlined,
  LineOutlined, DashOutlined, SmallDashOutlined,
  SortAscendingOutlined, SortDescendingOutlined,
  FallOutlined, RiseOutlined, StockOutlined,
  AreaChartOutlined, BarChartOutlined, LineChartOutlined,
  PieChartFilled, FundOutlined, RadarChartOutlined,
  HeatMapOutlined, SlidersOutlined, AntDesignOutlined
} from '@ant-design/icons-vue'

const iconMap: Record<string, any> = {
  'pie-chart-outlined': PieChartOutlined, 'setting-outlined': SettingOutlined,
  'user-outlined': UserOutlined, 'team-outlined': TeamOutlined,
  'appstore-outlined': AppstoreOutlined, 'file-text-outlined': FileTextOutlined,
  'safety-outlined': SafetyOutlined, 'tool-outlined': ToolOutlined,
  'folder-outlined': FolderOutlined, 'profile-outlined': ProfileOutlined,
  'home-outlined': HomeOutlined, 'mail-outlined': MailOutlined,
  'cloud-outlined': CloudOutlined, 'database-outlined': DatabaseOutlined,
  'flag-outlined': FlagOutlined, 'global-outlined': GlobalOutlined,
  'apartment-outlined': ApartmentOutlined, 'lock-outlined': LockOutlined,
  'key-outlined': KeyOutlined, 'audit-outlined': AuditOutlined,
  'branches-outlined': BranchesOutlined, 'code-outlined': CodeOutlined,
  'bug-outlined': BugOutlined, 'build-outlined': BuildOutlined,
  'thunderbolt-outlined': ThunderboltOutlined, 'experiment-outlined': ExperimentOutlined,
  'translation-outlined': TranslationOutlined, 'message-outlined': MessageOutlined,
  'notification-outlined': NotificationOutlined, 'paper-clip-outlined': PaperClipOutlined,
  'picture-outlined': PictureOutlined, 'video-camera-outlined': VideoCameraOutlined,
  'dashboard-outlined': DashboardOutlined, 'table-outlined': TableOutlined,
  'form-outlined': FormOutlined, 'check-square-outlined': CheckSquareOutlined,
  'bars-outlined': BarsOutlined, 'ordered-list-outlined': OrderedListOutlined,
  'edit-outlined': EditOutlined, 'copy-outlined': CopyOutlined,
  'delete-outlined': DeleteOutlined, 'plus-outlined': PlusOutlined,
  'search-outlined': SearchOutlined, 'download-outlined': DownloadOutlined,
  'upload-outlined': UploadOutlined, 'reload-outlined': ReloadOutlined,
  'star-outlined': StarOutlined, 'heart-outlined': HeartOutlined,
  'like-outlined': LikeOutlined, 'dislike-outlined': DislikeOutlined,
  'eye-outlined': EyeOutlined, 'eye-invisible-outlined': EyeInvisibleOutlined,
  'book-outlined': BookOutlined, 'read-outlined': ReadOutlined,
  'calendar-outlined': CalendarOutlined, 'clock-circle-outlined': ClockCircleOutlined,
  'environment-outlined': EnvironmentOutlined, 'phone-outlined': PhoneOutlined,
  'mobile-outlined': MobileOutlined, 'laptop-outlined': LaptopOutlined,
  'monitor-outlined': MonitorOutlined, 'tag-outlined': TagOutlined,
  'tags-outlined': TagsOutlined, 'gift-outlined': GiftOutlined,
  'bank-outlined': BankOutlined, 'trophy-outlined': TrophyOutlined,
  'crown-outlined': CrownOutlined, 'smile-outlined': SmileOutlined,
  'inbox-outlined': InboxOutlined, 'send-outlined': SendOutlined,

  'comment-outlined': CommentOutlined, 'customer-service-outlined': CustomerServiceOutlined,
  'dashboard-filled': DashboardFilled,
  'alert-outlined': AlertOutlined, 'warning-outlined': WarningOutlined,
  'info-circle-outlined': InfoCircleOutlined, 'question-circle-outlined': QuestionCircleOutlined,
  'exclamation-circle-outlined': ExclamationCircleOutlined,
  'close-circle-outlined': CloseCircleOutlined, 'check-circle-outlined': CheckCircleOutlined,
  'stop-outlined': StopOutlined, 'fire-outlined': FireOutlined,
  'coffee-outlined': CoffeeOutlined, 'insurance-outlined': InsuranceOutlined,
  'save-outlined': SaveOutlined, 'rollback-outlined': RollbackOutlined,
  'swap-outlined': SwapOutlined, 'car-outlined': CarOutlined,
  'rocket-outlined': RocketOutlined,
  'compass-outlined': CompassOutlined, 'aim-outlined': AimOutlined,
  'loading-outlined': LoadingOutlined, 'sync-outlined': SyncOutlined,
  'poweroff-outlined': PoweroffOutlined, 'menu-outlined': MenuOutlined,
  'appstore-add-outlined': AppstoreAddOutlined,
  'unordered-list-outlined': UnorderedListOutlined,
  'sort-ascending-outlined': SortAscendingOutlined,
  'sort-descending-outlined': SortDescendingOutlined,
  'area-chart-outlined': AreaChartOutlined, 'bar-chart-outlined': BarChartOutlined,
  'line-chart-outlined': LineChartOutlined, 'pie-chart-filled': PieChartFilled,
  'fund-outlined': FundOutlined, 'radar-chart-outlined': RadarChartOutlined,
  'heat-map-outlined': HeatMapOutlined, 'sliders-outlined': SlidersOutlined
}

const router = useRouter()
const route = useRoute()
const app = useAppStore()
const userStore = useUserStore()
const selectedKeys = ref<string[]>([route.path])
const companies = ref<{companyCode:string;companyName:string}[]>([])
const currentCorp = ref<string>('')

watch(() => route.path, (p) => { selectedKeys.value = [p] })

onMounted(async () => {
  try {
    const res = await companyApi.tree()
    if (res.code === 200 && res.data) {
      companies.value = flattenTree(res.data, 'children')
      if (companies.value.length) currentCorp.value = companies.value[0].companyCode
    }
  } catch {}
})

function flattenTree(list: any[], childKey: string, result: any[] = []): any[] {
  for (const item of list) {
    result.push({ companyCode: item.companyCode, companyName: item.companyName })
    if (item[childKey]?.length) flattenTree(item[childKey], childKey, result)
  }
  return result
}

async function switchCorp(corpCode: string) {
  await switchApi.switchCorp(corpCode)
}

function resolveIcon(name?: string) {
  if (!name) return AppstoreOutlined
  const kebab = name.replace(/([a-z])([A-Z])/g, '$1-$2').replace(/([A-Z]+)([A-Z][a-z])/g, '$1-$2').toLowerCase()
  return iconMap[kebab] || iconMap[name] || AppstoreOutlined
}
function onMenuClick({ key }: { key: string }) { router.push(key) }
function handleLogout() { userStore.logout(); router.push('/login') }
</script>

<style scoped>
.logo { height: 64px; line-height: 64px; text-align: center; color: #fff; font-size: 18px; font-weight: bold; }
.header { background: #fff; padding: 0 24px; display: flex; align-items: center; gap: 8px; }
.content { margin: 24px; min-height: 280px; }
</style>
