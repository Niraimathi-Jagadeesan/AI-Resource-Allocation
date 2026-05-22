import { test, expect } from '@playwright/test';
import { login } from '../utils/login-helper';

test.describe('Employee CRUD Tests', () => {
  test.beforeEach(async ({ page }) => {
    await login(page, 'admin', 'Admin@123');
    await page.goto('/employees');
  });

  test('employees page loads successfully', async ({ page }) => {
    await expect(
      page.getByRole('heading', { name: 'Employees' })
    ).toBeVisible();
  });

  test('admin can add a new employee', async ({ page }) => {
    const employeeName = `Test Employee ${Date.now()}`;

    await page.locator('#btnAddEmployee').click();

    await page.locator('#txtName').fill(employeeName);
    await page.locator('#txtPrimaryRole').fill('Automation Engineer');
    await page.locator('#txtSkills').fill('Playwright,TypeScript');
    await page.locator('#txtExperience').fill('5');
    await page.locator('#txtPerformanceScore').fill('95');
    await page.locator('#txtAllocation').fill('20');
    await page.locator('#txtSuccessRate').fill('98');

    await page.locator('#chkAvailable').check();

    await page.locator('#btnSaveEmployee').click();

    await expect(page.getByText(employeeName)).toBeVisible();
  });

  test('cancel button closes employee form', async ({ page }) => {
    await page.locator('#btnAddEmployee').click();
    await page.locator('#btnCancelEmployee').click();

    await expect(page.locator('#txtName')).toHaveCount(0);
  });

  test('admin can edit employee', async ({ page }) => {
    const row = page.locator('table tbody tr').first();

    await row.getByRole('button', { name: 'Edit' }).click();

    await page.locator('#txtPrimaryRole').fill('Senior Developer');

    await page.locator('#btnSaveEmployee').click();

    await expect(page.getByText('Senior Developer')).toBeVisible();
  });

  test('admin can delete employee', async ({ page }) => {    
    // Navigate to Employees page
    await page.goto('/employees');
    
    // Wait for table rows to load
    await page.waitForSelector('tbody tr');
    
    // Get employee count before delete
    const rowsBefore = await page.locator('tbody tr').count();

    // Register dialog handler BEFORE clicking (confirm fires immediately on click)
    page.once('dialog', async dialog => {
      expect(dialog.message()).toContain('Delete this employee?');
      await dialog.accept();
    });

    // Click the Delete button of the last row
    await page.locator('tbody tr').last().getByRole('button', { name: 'Delete' }).click();

    // Wait for table to refresh
    await page.waitForTimeout(1000);

    // Verify row count decreased by 1
    const rowsAfter = await page.locator('tbody tr').count();
    expect(rowsAfter).toBe(rowsBefore - 1);
    });
});