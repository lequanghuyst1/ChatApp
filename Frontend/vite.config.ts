import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vite.dev/config/
import path from "path";

export default defineConfig({
  plugins: [react()],
  // Base path cho GitHub Pages
  // Nếu deploy lên https://username.github.io/ChatApp, dùng '/ChatApp/'
  // Nếu deploy lên https://username.github.io, dùng '/'
  base: process.env.GITHUB_PAGES ? '/ChatApp/' : '/',
  resolve: {
    alias: {
      "@": path.resolve(__dirname, "src"),
      "@/apis": path.resolve(__dirname, "src/apis"),
      "@/components": path.resolve(__dirname, "src/components"),
      "@/pages": path.resolve(__dirname, "src/pages"),
      "@/types": path.resolve(__dirname, "src/types"),
      "@/utils": path.resolve(__dirname, "src/utils"),
      "@/routes": path.resolve(__dirname, "src/routes"),
      "@/stores": path.resolve(__dirname, "src/stores"),
      "@/hooks": path.resolve(__dirname, "src/hooks"),
    },
  },
  build: {
    // Tối ưu cho production
    sourcemap: false,
  },
});
