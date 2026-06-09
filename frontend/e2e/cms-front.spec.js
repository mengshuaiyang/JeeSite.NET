import { test, expect } from '@playwright/test';
const mockSites = [
    { siteCode: 'site01', siteName: '官网', domain: '', status: '0' }
];
const mockCategories = [
    { categoryCode: 'cat01', categoryName: '新闻动态', parentCode: '0', siteCode: 'site01', treeSort: 1 },
    { categoryCode: 'cat02', categoryName: '行业资讯', parentCode: '0', siteCode: 'site01', treeSort: 2 },
];
const mockArticle = {
    articleCode: 'art001', categoryCode: 'cat01', title: '测试文章标题',
    summary: '这是一篇测试文章摘要', author: '管理员', clickCount: 42,
    publishDate: '2026-06-01T10:00:00', categoryName: '新闻动态',
    tags: '技术,测试', articleData: { content: '<p>文章正文内容</p>' }
};
const mockPageResult = { code: 200, data: { list: [mockArticle], total: 1, pageNo: 1, pageSize: 10 } };
async function setupMocks(page) {
    await page.route('**/api/v1/cms/front/site', async (route) => {
        await route.fulfill({ status: 200, contentType: 'application/json', body: JSON.stringify({ code: 200, data: mockSites }) });
    });
    await page.route('**/api/v1/cms/front/category/list/**', async (route) => {
        await route.fulfill({ status: 200, contentType: 'application/json', body: JSON.stringify({ code: 200, data: mockCategories }) });
    });
    await page.route('**/api/v1/cms/front/article/list', async (route) => {
        await route.fulfill({ status: 200, contentType: 'application/json', body: JSON.stringify(mockPageResult) });
    });
    await page.route('**/api/v1/cms/front/tag/cloud', async (route) => {
        await route.fulfill({ status: 200, contentType: 'application/json', body: JSON.stringify({ code: 200, data: [{ tagName: '技术', articleCount: 5 }] }) });
    });
    await page.route('**/api/v1/cms/front/article/get/**', async (route) => {
        await route.fulfill({ status: 200, contentType: 'application/json', body: JSON.stringify({ code: 200, data: mockArticle }) });
    });
    await page.route('**/api/v1/cms/front/article/search', async (route) => {
        await route.fulfill({ status: 200, contentType: 'application/json', body: JSON.stringify(mockPageResult) });
    });
}
test.describe('CMS Front Pages', () => {
    test('CMS site home page renders', async ({ page }) => {
        await setupMocks(page);
        await page.goto('/cms');
        await page.waitForLoadState('networkidle');
        await expect(page.locator('.cms-site')).toBeVisible();
        await expect(page.locator('.cms-header')).toBeVisible();
        await expect(page.locator('.cms-main')).toBeVisible();
    });
    test('article list page shows articles', async ({ page }) => {
        await setupMocks(page);
        await page.goto('/cms/category/cat01');
        await page.waitForLoadState('networkidle');
        await expect(page.locator('.cms-site')).toBeVisible();
        await expect(page.locator('.ant-list-item')).toHaveCount(1);
        await expect(page.locator('.ant-list-item')).toContainText('测试文章标题');
    });
    test('article detail page shows content', async ({ page }) => {
        await setupMocks(page);
        await page.goto('/cms/article/art001');
        await page.waitForLoadState('networkidle');
        await expect(page.locator('.cms-article')).toBeVisible();
        await expect(page.locator('.cms-title')).toContainText('测试文章标题');
        await expect(page.locator('.cms-meta')).toContainText('阅读: 42');
        await expect(page.locator('.cms-summary')).toContainText('这是一篇测试文章摘要');
        await expect(page.locator('.cms-content-body')).toContainText('文章正文内容');
    });
    test('search page is accessible', async ({ page }) => {
        await setupMocks(page);
        await page.goto('/cms/search');
        await page.waitForLoadState('networkidle');
        await expect(page.locator('.cms-site')).toBeVisible();
        await expect(page.locator('.cms-search-box')).toBeVisible();
    });
    test('404 for non-existent article', async ({ page }) => {
        await setupMocks(page);
        await page.route('**/api/v1/cms/front/article/get/nonexistent', async (route) => {
            await route.fulfill({ status: 200, contentType: 'application/json', body: JSON.stringify({ code: 404, data: null }) });
        });
        await page.goto('/cms/article/nonexistent');
        await page.waitForLoadState('networkidle');
        await expect(page.locator('.ant-result')).toBeVisible();
        await expect(page.locator('.ant-result-title')).toContainText('404');
    });
    test('login page redirects when not authenticated', async ({ page }) => {
        await page.goto('/');
        await expect(page).toHaveURL('/login');
    });
});
