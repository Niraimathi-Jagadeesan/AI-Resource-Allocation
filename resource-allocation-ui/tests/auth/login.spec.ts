import { test, expect } from '@playwright/test';
import { login } from '../utils/login-helper';

test.describe('Authentication - Login', () => {
    test('Admin user can log in successfully', async ({ page }) => {
        await login(page, 'admin', 'Admin@123');

        await expect(page).toHaveURL('/dashboard'); 
    });

    test('Manager user can log in successfully', async ({ page }) => {
        await login(page, 'manager', 'Manager@123');

        await expect(page).toHaveURL('/dashboard');            
    });

    test('Invalid credentials show an error message', async ({ page }) => {
        await page.goto('/login');
        await page.fill('id=username', 'invalidUser');
        await page.fill('id=password', 'wrongPassword');
        await page.click('id=loginButton');
        await expect(page.locator('id=loginError').textContent()).resolves.toContain('Invalid username or password');
    });
});