module.exports = {
    preset: 'ts-jest',
    testEnvironment: 'node',
    moduleNameMapper: {
      '^@/(.+)': '<rootDir>/src/$1', // Map @/ to src/ directory
    },
    testMatch: ['**/?(*.)+(spec|test).ts?(x)'], // Match *.test.ts files
  };