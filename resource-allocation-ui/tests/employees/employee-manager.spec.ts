import { test, expect } from '@playwright/test';
import { login } from '../utils/login-helper';

test('manager should not see Employees card on dashboard', async ({ page }) => {
  await login(page, 'manager', 'Manager@123');

  await expect(page.locator('text=Employees')).toHaveCount(0);
});

test('manager should not access employees page directly', async ({ page }) => {
  await login(page, 'manager', 'Manager@123');

  await page.goto('/employees');

  // Adjust this based on your application behavior:
  // redirected to dashboard, access denied page, or unauthorized page.
  await expect(page).not.toHaveURL(/.*employees/);
});