import { defineConfig, devices } from '@playwright/test';

export default defineConfig({
  testDir: './tests',

  timeout: 60_000,

  expect: {
    timeout: 10_000
  },

  fullyParallel: false,

  retries: 0,

  workers: 1,

  reporter: [
    ['dot'],
    ['html'],
    ['list']
  ],

  use: {
    baseURL: 'https://purple-cliff-0ed652900.7.azurestaticapps.net',
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