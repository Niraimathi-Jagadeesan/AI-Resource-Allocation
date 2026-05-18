import { test, expect } from '@playwright/test';
import { login } from '../utils/login-helper';

test.describe('Dashboard Navigation Tests', () => {
  test.beforeEach(async ({ page }) => {
    await login(page, 'admin', 'Admin@123');
    await expect(page).toHaveURL(/dashboard/);
  });

  test('Employees card redirects to Employees page', async ({ page }) => {
    await page.locator('#btnEmployeesOpen').click();
    await expect(page).toHaveURL(/employees/);
    await expect(page.getByRole('heading', { name: 'Employees' }))
      .toBeVisible();
  });

  test('Projects card redirects to Projects page', async ({ page }) => {
    await page.locator('#btnProjectsOpen').click();
    await expect(page).toHaveURL(/projects/);
    await expect(page.getByRole('heading', { name: 'Projects' }))
      .toBeVisible();
  });

  test('Recommendations card redirects to Recommendations page', async ({ page }) => {
    await page.locator('#btnRecommendationsOpen').click();
    await expect(page).toHaveURL(/recommendations/);
    await expect(page.getByRole('heading', { name: /recommendations/i }))
      .toBeVisible();
  });

  test('Risk Analysis card redirects to Risk page', async ({ page }) => {
    await page.locator('#btnRiskOpen').click();
    await expect(page).toHaveURL(/risk/);
    await expect(page.getByRole('heading', { name: /risk analysis/i }))
      .toBeVisible();
  });
});