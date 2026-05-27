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
    baseURL: 'http://localhost:7193',
    headless: true,
    screenshot: 'only-on-failure',
    video: 'retain-on-failure',
    trace: 'retain-on-failure'
  },

  webServer: {
    command: 'npm start',
    url: 'http://127.0.0.1:7193',
    reuseExistingServer: !process.env['CI'],
    timeout: 120000
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