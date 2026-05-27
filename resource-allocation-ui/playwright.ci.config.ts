import { defineConfig, devices } from '@playwright/test';

export default defineConfig({

  testDir: './tests',

  timeout: 60000,

  expect: {
    timeout: 10000
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
    baseURL: 'http://127.0.0.1:4200',
    headless: true,
    screenshot: 'only-on-failure',
    video: 'retain-on-failure',
    trace: 'retain-on-failure'
  },

  webServer: {
    command: 'npm start',
    url: 'http://127.0.0.1:4200',
    reuseExistingServer: false,
    timeout: 300000,
    stdout: 'pipe',
    stderr: 'pipe'
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