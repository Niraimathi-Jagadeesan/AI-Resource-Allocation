import { test, expect } from '@playwright/test';
import { login } from '../utils/login-helper';

test.describe('Authentication - Logout', () => {
  test('user can log out successfully', async ({ page }) => {
    // Step 1: Login
    await login(page, 'admin', 'Admin@123');

    // Step 2: Click Logout
    await page.locator('.btn-logout').first().click();

    // Step 3: Verify redirect to login page
    await expect(page).toHaveURL(/login/);

    // Step 4: Verify login title is visible
    await expect(page.locator('#loginTitle')).toBeVisible();

    // Step 5: Verify token is removed from localStorage
    const token = await page.evaluate(() =>
      localStorage.getItem('token')
    );

    expect(token).toBeNull();
  });

  test('protected pages are inaccessible after logout', async ({ page }) => {
    await login(page, 'admin', 'Admin@123');

    await page.locator('.btn-logout').first().click();

    await page.goto('/dashboard');

    await expect(page).toHaveURL(/login/);
    });
});