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
        <div style="flex:1" />
        <a-tooltip :title="app.darkMode ? '切换亮色' : '切换暗色'">
          <a-button type="text" @click="app.toggleDarkMode" :icon="app.darkMode ? SunOutlined : MoonOutlined" />
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
import { ref, watch } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { useAppStore } from '@/stores/app'
import { useUserStore } from '@/stores/user'
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
  HeatMapOutlined, SlidersOutlined, AntDesignOutlined,
  SunOutlined, MoonOutlined
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

watch(() => route.path, (p) => { selectedKeys.value = [p] })

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
