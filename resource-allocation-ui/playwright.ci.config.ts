import { defineConfig, devices } from '@playwright/test';

export default defineConfig({

  // ONLY PLAYWRIGHT TESTS
  testDir: './tests',

  // ONLY THESE FILES
  testMatch: '**/*.spec.ts',

  // EXCLUDE ANGULAR UNIT TESTS
  testIgnore: [
    '**/src/**/*.spec.ts',
    '**/node_modules/**'
  ],

  timeout: 60000,

  expect: {
    timeout: 10000
  },

  fullyParallel: false,

  retries: 1,

  workers: 1,

  reporter: [
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
    command: 'npm run start:ci',
    url: 'http://127.0.0.1:4200',
    reuseExistingServer: false,
    timeout: 180000
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