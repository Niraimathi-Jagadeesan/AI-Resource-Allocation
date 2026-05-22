import { test, expect } from '@playwright/test';
import { login } from '../utils/login-helper';

test.beforeEach(async ({ page }) => {
  await login(page, 'admin', 'Admin@123');
});

test('should navigate to recommendations page', async ({ page }) => {
  await page.goto('/dashboard');

  await page.getByRole('link', { name: 'Recommendations' }).click();

  await expect(page).toHaveURL(/.*recommendations/);
  await expect(
    page.getByRole('heading', { name: /AI Resource Recommendations/i })
  ).toBeVisible();
});

test('should load project dropdown', async ({ page }) => {
  await page.goto('/recommendations');  

  const dropdown = page.locator('select');
  await expect(dropdown).toBeVisible();

  // Wait until dropdown has more than 1 option
  await expect.poll(async () => {
    return await dropdown.locator('option').count();
  }).toBeGreaterThan(1);
  
  const optionCount = await dropdown.locator('option').count();
  expect(optionCount).toBeGreaterThan(1);
});

test('should generate recommendations', async ({ page }) => {
  await page.goto('/recommendations');

  const dropdown = page.locator('select');

  await dropdown.selectOption({ index: 1 });

  await page.getByRole('button', {
    name: 'Get Recommendations'
  }).click();

  await expect(page.locator('table')).toBeVisible();
  await expect(page.locator('tbody tr').first()).toBeVisible();
});

test('should show ranked employees', async ({ page }) => {
  await page.goto('/recommendations');

  await page.locator('select').selectOption({ index: 1 });

  await page.getByRole('button', {
    name: 'Get Recommendations'
  }).click();

  const firstRow = page.locator('tbody tr').first();

  await expect(firstRow).toContainText('#1');
});

test('should display recommendation explanation', async ({ page }) => {
  await page.goto('/recommendations');

  await page.locator('select').selectOption({ index: 1 });

  await page.getByRole('button', {
    name: 'Get Recommendations'
  }).click();

  const reasonCell = page.locator('tbody tr').first().locator('td').last();

  await expect(reasonCell).not.toBeEmpty();
});

test('should display recommendation score', async ({ page }) => {
  await page.goto('/recommendations');

  await page.locator('select').selectOption({ index: 1 });

  await page.getByRole('button', {
    name: 'Get Recommendations'
  }).click();

  const scoreCell = page.locator('tbody tr').first().locator('td').nth(2);

  await expect(scoreCell).not.toBeEmpty();
});