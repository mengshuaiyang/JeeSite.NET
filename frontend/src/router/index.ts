import { createRouter, createWebHistory } from 'vue-router'
import type { RouteRecordRaw } from 'vue-router'
import { useAppStore } from '@/stores/app'

declare module 'vue-router' {
  interface RouteMeta {
    permission?: string
    title?: string
    public?: boolean
  }
}

const routes: RouteRecordRaw[] = [
  { path: '/login', name: 'Login', component: () => import('@/views/Login.vue'), meta: { title: '登录', public: true } },
  { path: '/register', name: 'Register', component: () => import('@/views/Register.vue'), meta: { title: '注册', public: true } },
  { path: '/forgot-password', name: 'ForgotPassword', component: () => import('@/views/ForgotPassword.vue'), meta: { title: '找回密码', public: true } },
  { path: '/oauth2/callback', name: 'OAuth2Callback', component: () => import('@/views/OAuth2Callback.vue'), meta: { title: '第三方登录', public: true } },

  // ---- CMS 前端展示 (公开) ----
  { path: '/cms', name: 'CmsSite', component: () => import('@/views/cms-front/CmsSite.vue'), meta: { title: 'CMS 首页', public: true } },
  { path: '/cms/category/:categoryCode', name: 'CmsArticleList', component: () => import('@/views/cms-front/CmsArticleList.vue'), meta: { title: '文章列表', public: true } },
  { path: '/cms/article/:articleCode', name: 'CmsArticleDetail', component: () => import('@/views/cms-front/CmsArticleDetail.vue'), meta: { title: '文章详情', public: true } },
  { path: '/cms/search', name: 'CmsSearch', component: () => import('@/views/cms-front/CmsSearch.vue'), meta: { title: '搜索', public: true } },

  {
    path: '/',
    component: () => import('@/layouts/MainLayout.vue'),
    redirect: '/dashboard',
    children: [
      { path: 'dashboard', name: 'Dashboard', component: () => import('@/views/Dashboard.vue'), meta: { title: '控制台', permission: 'dashboard' } },
      { path: 'sys/user', name: 'User', component: () => import('@/views/sys/UserList.vue'), meta: { title: '用户管理', permission: 'sys:user' } },
      { path: 'sys/role', name: 'Role', component: () => import('@/views/sys/RoleList.vue'), meta: { title: '角色管理', permission: 'sys:role' } },
      { path: 'sys/menu', name: 'Menu', component: () => import('@/views/sys/MenuList.vue'), meta: { title: '菜单管理', permission: 'sys:menu' } },
      { path: 'sys/org', name: 'Organization', component: () => import('@/views/sys/OrgTree.vue'), meta: { title: '机构管理', permission: 'sys:org' } },
      { path: 'sys/post', name: 'Post', component: () => import('@/views/sys/PostList.vue'), meta: { title: '岗位管理', permission: 'sys:post' } },
      { path: 'sys/dict', name: 'Dict', component: () => import('@/views/sys/DictList.vue'), meta: { title: '字典管理', permission: 'sys:dict' } },
      { path: 'sys/config', name: 'Config', component: () => import('@/views/sys/ConfigList.vue'), meta: { title: '参数配置', permission: 'sys:config' } },
      { path: 'sys/module', name: 'Module', component: () => import('@/views/sys/ModuleList.vue'), meta: { title: '模块管理', permission: 'sys:module' } },
      { path: 'sys/log', name: 'Log', component: () => import('@/views/sys/LogList.vue'), meta: { title: '操作日志', permission: 'sys:log' } },
      { path: 'sys/employee', name: 'Employee', component: () => import('@/views/sys/EmployeeList.vue'), meta: { title: '员工管理', permission: 'sys:employee' } },
      { path: 'sys/company', name: 'Company', component: () => import('@/views/sys/CompanyTree.vue'), meta: { title: '公司管理', permission: 'sys:company' } },
      { path: 'sys/area', name: 'Area', component: () => import('@/views/sys/AreaTree.vue'), meta: { title: '行政区划', permission: 'sys:area' } },
      { path: 'sys/msg/inbox', name: 'MsgInbox', component: () => import('@/views/sys/MsgInbox.vue'), meta: { title: '收件箱', permission: 'sys:msg' } },
      { path: 'sys/msg/sent', name: 'MsgSent', component: () => import('@/views/sys/MsgSent.vue'), meta: { title: '发件箱', permission: 'sys:msg' } },
      { path: 'sys/msg/template', name: 'MsgTemplate', component: () => import('@/views/sys/MsgTemplateList.vue'), meta: { title: '消息模板', permission: 'sys:msg' } },
      { path: 'sys/msg/push', name: 'MsgPush', component: () => import('@/views/sys/MsgPushList.vue'), meta: { title: '推送记录', permission: 'sys:msg:push:list' } },
      { path: 'sys/lang', name: 'Lang', component: () => import('@/views/sys/LangList.vue'), meta: { title: '国际化', permission: 'sys:lang' } },
      { path: 'sys/biz-category', name: 'BizCategory', component: () => import('@/views/sys/BizCategoryTree.vue'), meta: { title: '业务分类', permission: 'sys:biz-category' } },
      { path: 'sys/cache', name: 'Cache', component: () => import('@/views/sys/CacheList.vue'), meta: { title: '缓存管理', permission: 'sys:cache:view' } },
      { path: 'sys/online', name: 'OnlineUser', component: () => import('@/views/sys/OnlineUserList.vue'), meta: { title: '在线用户', permission: 'sys:online:view' } },
      { path: 'sys/monitor', name: 'MonitorServer', component: () => import('@/views/sys/MonitorServer.vue'), meta: { title: '系统监控', permission: 'sys:monitor:view' } },
      { path: 'sys/audit', name: 'Audit', component: () => import('@/views/sys/AuditList.vue'), meta: { title: '审计跟踪', permission: 'sys:audit:view' } },
      { path: 'cms/category', name: 'CmsCategory', component: () => import('@/views/cms/CategoryList.vue'), meta: { title: '栏目管理', permission: 'cms:category' } },
      { path: 'cms/article', name: 'CmsArticle', component: () => import('@/views/cms/ArticleList.vue'), meta: { title: '文章管理', permission: 'cms:article' } },
      { path: 'cms/article/edit', name: 'CmsArticleEdit', component: () => import('@/views/cms/ArticleEdit.vue'), meta: { title: '编辑文章', permission: 'cms:article:edit' } },
      { path: 'codegen/table', name: 'Codegen', component: () => import('@/views/codegen/GenTableList.vue'), meta: { title: '代码生成', permission: 'codegen:table:list' } },
      { path: 'codegen/table/edit', name: 'CodegenEdit', component: () => import('@/views/codegen/GenTableEdit.vue'), meta: { title: '编辑表配置', permission: 'codegen:table:edit' } },
      { path: 'tasks/job', name: 'Job', component: () => import('@/views/tasks/JobList.vue'), meta: { title: '任务调度', permission: 'tasks:job:list' } },
      { path: 'cms/ai-chat', name: 'AiChat', component: () => import('@/views/AiChat.vue'), meta: { title: 'AI 助手', permission: 'cms:article' } },
      { path: 'cms/comment', name: 'CmsComment', component: () => import('@/views/cms/CommentList.vue'), meta: { title: '评论管理', permission: 'cms:comment' } },
      { path: 'cms/guestbook', name: 'CmsGuestbook', component: () => import('@/views/cms/GuestbookList.vue'), meta: { title: '留言管理', permission: 'cms:guestbook' } },
      { path: 'app/comment', name: 'AppComment', component: () => import('@/views/app/AppCommentList.vue'), meta: { title: '意见反馈', permission: 'app:comment' } },
      { path: 'app/upgrade', name: 'AppUpgrade', component: () => import('@/views/app/AppUpgradeList.vue'), meta: { title: '版本管理', permission: 'app:upgrade' } },
      { path: 'test/data', name: 'TestData', component: () => import('@/views/test/TestDataList.vue'), meta: { title: '测试数据', permission: 'test:data' } },
      { path: 'test/tree', name: 'TestTree', component: () => import('@/views/test/TestTree.vue'), meta: { title: '测试树表', permission: 'test:tree' } },
      { path: 'sys/profile', name: 'Profile', component: () => import('@/views/sys/Profile.vue'), meta: { title: '个人中心' } },
      { path: 'sys/role-data-scope', name: 'RoleDataScope', component: () => import('@/views/sys/RoleDataScope.vue'), meta: { title: '数据权限', permission: 'sys:role:data-scope' } },
      { path: 'sys/role-field-scope', name: 'RoleFieldScope', component: () => import('@/views/sys/RoleFieldScope.vue'), meta: { title: '字段权限', permission: 'sys:role:field-scope' } },
      { path: 'sys/file', name: 'File', component: () => import('@/views/sys/FileList.vue'), meta: { title: '文件管理', permission: 'sys:file' } },
      { path: 'demo/form', name: 'DemoForm', component: () => import('@/views/demo/DemoForm.vue'), meta: { title: '表单演示', permission: 'test:demo:view' } },
      { path: 'demo/grid', name: 'DemoGrid', component: () => import('@/views/demo/DemoGrid.vue'), meta: { title: '表格演示', permission: 'test:demo:view' } },
      { path: 'cms/file-template', name: 'CmsFileTemplate', component: () => import('@/views/cms/FileTemplateList.vue'), meta: { title: '文件模板', permission: 'cms:file-template:list' } },
      { path: 'bpm/leave', name: 'MyLeave', component: () => import('@/views/bpm/MyLeave.vue'), meta: { title: '我的请假', permission: 'bpm:leave:list' } },
      { path: 'bpm/approval', name: 'PendingApproval', component: () => import('@/views/bpm/PendingApproval.vue'), meta: { title: '待审批', permission: 'bpm:leave:action' } }
    ]
  },
  { path: '/:pathMatch(.*)*', component: () => import('@/views/error/NotFound.vue'), meta: { title: '404' } }
]

const router = createRouter({ history: createWebHistory(), routes })

router.beforeEach((to) => {
  const token = localStorage.getItem('token')

  if (to.meta.public) return

  if (!token) return '/login'

  if (to.meta.permission) {
    const appStore = useAppStore()
    if (!appStore.hasPermission(to.meta.permission)) {
      return '/dashboard'
    }
  }
})

export default router
