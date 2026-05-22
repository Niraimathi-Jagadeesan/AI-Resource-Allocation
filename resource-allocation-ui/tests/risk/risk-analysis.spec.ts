import { test, expect } from '@playwright/test';
import { login } from '../utils/login-helper';

test.beforeEach(async ({ page }) => {
  await login(page, 'admin', 'Admin@123');
});

test('should navigate to risk analysis page', async ({ page }) => {
  await page.goto('/dashboard');

  await page.getByRole('link', { name: 'Risk Analysis' }).click();

  await expect(page).toHaveURL(/.*risk/);

  await expect(
    page.getByRole('heading', { name: /AI Project Risk Analysis/i })
  ).toBeVisible();
});

test('should load projects in dropdown', async ({ page }) => {
  await page.goto('/risk');

  const dropdown = page.locator('select');
  await expect(dropdown).toBeVisible();

  // Wait until dropdown has more than 1 option
  await expect.poll(async () => {
    return await dropdown.locator('option').count();
  }).toBeGreaterThan(1);

  const optionCount = await dropdown.locator('option').count();
  expect(optionCount).toBeGreaterThan(1);
});

test('should analyze project risk', async ({ page }) => {
  await page.goto('/risk');

  await page.locator('select').selectOption({ index: 1 });

  await page.getByRole('button', { name: 'Analyze Risk' }).click();

  await expect(page.locator('text=Risk Level:')).toBeVisible();
  await expect(page.locator('text=Summary:')).toBeVisible();
});

test('should display risk score percentage', async ({ page }) => {
  await page.goto('/risk');

  await page.locator('select').selectOption({ index: 1 });

  await page.getByRole('button', { name: 'Analyze Risk' }).click();

  await expect(page.locator('text=Risk Score:')).toBeVisible();

  const scoreText = await page.locator('#riskScore').textContent();
  expect(scoreText).toContain('%');
});

test('should display valid risk level', async ({ page }) => {
  await page.goto('/risk');

  await page.locator('select').selectOption({ index: 1 });

  await page.getByRole('button', { name: 'Analyze Risk' }).click();

  const bodyText = await page.locator('#riskLevel').textContent();

  expect(
    bodyText?.includes('Low') ||
    bodyText?.includes('Medium') ||
    bodyText?.includes('High')
  ).toBeTruthy();
});

test('should display risk factors', async ({ page }) => {
  await page.goto('/risk');

  await page.locator('select').selectOption({ index: 1 });

  await page.getByRole('button', { name: 'Analyze Risk' }).click();

  await expect(
    page.locator('text=Risk Factors')
  ).toBeVisible();
});

test('should display mitigation recommendations', async ({ page }) => {
  await page.goto('/risk');

  await page.locator('select').selectOption({ index: 1 });

  await page.getByRole('button', { name: 'Analyze Risk' }).click();

  await expect(
    page.locator('text=Recommendations')
  ).toBeVisible();
});

test('should display analysis summary', async ({ page }) => {
  await page.goto('/risk');

  await page.locator('select').selectOption({ index: 1 });

  await page.getByRole('button', { name: 'Analyze Risk' }).click();

  const summarySection = page.locator('text=Summary:');
  await expect(summarySection).toBeVisible();
});

test('should analyze multiple projects', async ({ page }) => {
  await page.goto('/risk');

  const dropdown = page.locator('select');
  const optionCount = await dropdown.locator('option').count();

  const maxProjects = Math.min(optionCount - 1, 3);

  for (let i = 1; i <= maxProjects; i++) {
    await dropdown.selectOption({ index: i });

    await page.getByRole('button', {
      name: 'Analyze Risk'
    }).click();

    await expect(page.locator('text=Risk Level:')).toBeVisible();
  }
});