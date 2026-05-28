import { Page, expect } from '@playwright/test';

export async function login(page: Page, username: string, password: string): Promise<void> {
    await page.goto('/login');
    await expect(page.locator('id=username')).toBeVisible();
    await page.fill('id=username', username);
    await page.fill('id=password', password);
    await page.click('id=loginButton');
    // Debugging
    console.log(await page.url());
    // Print page content if login fails
    if (page.url().includes('/login')) {
        console.log(await page.locator('body').textContent());
    }
    await expect(page).toHaveURL('/dashboard');
}