import { test, expect } from '@playwright/test';
import { login } from '../utils/login-helper';

test.describe('Dashboard Tests', () => {
  test('dashboard loads successfully after login', async ({ page }) => {
    await login(page, 'admin', 'Admin@123');

    await expect(page).toHaveURL(/dashboard/);
    await expect(page.locator('#dashboardTitle')).toBeVisible();
  });

  test('summary cards are visible', async ({ page }) => {
    await login(page, 'admin', 'Admin@123');

    await expect(page.locator('#employeeCountCard')).toBeVisible();
    await expect(page.locator('#projectCountCard')).toBeVisible();
    await expect(page.locator('#recommendationCard')).toBeVisible();
    await expect(page.locator('#riskCard')).toBeVisible();
  });  

  test('navigation links are visible', async ({ page }) => {
    await login(page, 'admin', 'Admin@123');

    await expect(page.getByRole('heading', { name: 'Employees' })).toBeVisible();
    await expect(page.getByRole('heading', { name: 'Projects' })).toBeVisible();
    await expect(page.getByRole('heading', { name: 'Recommendations' })).toBeVisible();
    await expect(page.getByRole('heading', { name: 'Risk Analysis' })).toBeVisible();
    });
});