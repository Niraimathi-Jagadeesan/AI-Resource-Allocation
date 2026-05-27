import { defineConfig, devices } from '@playwright/test';

export default defineConfig({

  testDir: './tests',

  timeout: 60000,

  expect: {
    timeout: 10000
  },

  fullyParallel: false,

  retries: 1,

  workers: 1,

  reporter: [
    ['dot'],
    ['html'],
    ['list']
  ],

  use: {
    baseURL: process.env['PLAYWRIGHT_BASE_URL'],
    headless: true,
    screenshot: 'only-on-failure',
    video: 'retain-on-failure',
    trace: 'retain-on-failure'
  },

  projects: [
    {
      name: 'chromium',
      use: {
        ...devices['Desktop Chrome']
      }
    }
  ]
});