import { test, expect } from '@playwright/test';
test.describe('Login Page', () => {
    test.beforeEach(async ({ page }) => {
        await page.goto('/login');
    });
    test('renders login form', async ({ page }) => {
        await expect(page.locator('.login-container')).toBeVisible();
        await expect(page.locator('.ant-card-head-title')).toContainText('JeeSite.NET');
        await expect(page.locator('button[type="submit"]')).toContainText('登 录');
    });
    test('shows validation errors on empty submit', async ({ page }) => {
        await page.fill('input[placeholder="登录名"]', '');
        await page.fill('input[placeholder="密码"]', '');
        await page.click('button[type="submit"]');
        await expect(page.locator('.ant-form-item-explain-error')).toHaveCount(2);
    });
    test('shows captcha after failed login', async ({ page }) => {
        await page.route('**/api/v1/sys/auth/login', async (route) => {
            await route.fulfill({
                status: 200,
                contentType: 'application/json',
                body: JSON.stringify({ code: 401, message: '登录失败' }),
            });
        });
        await page.fill('input[placeholder="登录名"]', 'admin');
        await page.fill('input[placeholder="密码"]', 'wrong');
        await page.click('button[type="submit"]');
        await expect(page.locator('.ant-message')).toContainText('登录失败');
    });
    test('login form has default values', async ({ page }) => {
        await expect(page.locator('input[placeholder="登录名"]')).toHaveValue('admin');
        await expect(page.locator('input[placeholder="密码"]')).toHaveValue('admin');
    });
});
