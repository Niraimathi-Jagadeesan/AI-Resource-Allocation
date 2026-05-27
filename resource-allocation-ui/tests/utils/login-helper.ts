import { Page, expect } from '@playwright/test';

export async function login(page: Page, username: string, password: string): Promise<void> {
    await page.goto('/login');
    await expect(page.locator('id=username')).toBeVisible();
    await page.fill('id=username', username);
    await page.fill('id=password', password);
    await page.click('id=loginButton');
    await page.waitForTimeout(3000);
    await expect(page).toHaveURL('/dashboard');
}