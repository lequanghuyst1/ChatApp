import js from '@eslint/js'
import globals from 'globals'
import reactHooks from 'eslint-plugin-react-hooks'
import reactRefresh from 'eslint-plugin-react-refresh'
import tseslint from 'typescript-eslint'
import { globalIgnores } from 'eslint/config'

export default tseslint.config([
  globalIgnores(['dist']),
  {
    files: ['**/*.{ts,tsx}'],
    extends: [
      js.configs.recommended,
      tseslint.configs.recommended,
      reactHooks.configs['recommended-latest'],
      reactRefresh.configs.vite,
    ],
    languageOptions: {
      ecmaVersion: 2020,
      globals: globals.browser,
    },
    rules: {
      // Tắt các rules quá strict
      'no-useless-catch': 'off', // Cho phép try/catch chỉ throw lại
      '@typescript-eslint/no-explicit-any': 'warn', // Warning thay vì error cho 'any' type
      '@typescript-eslint/no-unused-vars': [
        'error',
        {
          argsIgnorePattern: '^_', // Cho phép unused vars nếu bắt đầu bằng _
          varsIgnorePattern: '^_',
          ignoreRestSiblings: true,
        },
      ],
      '@typescript-eslint/ban-ts-comment': 'warn', // Warning cho @ts-ignore
      'react-refresh/only-export-components': 'warn', // Warning thay vì error
      'react-hooks/exhaustive-deps': 'warn', // Warning cho missing dependencies
    },
  },
])
