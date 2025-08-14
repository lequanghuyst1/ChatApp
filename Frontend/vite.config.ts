import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vite.dev/config/
import path from "path";

export default defineConfig({
  plugins: [react()],
  resolve: {
    alias: {
      "@apis": path.resolve(__dirname, "src/apis"),
      "@components": path.resolve(__dirname, "src/components"),
      "@pages": path.resolve(__dirname, "src/pages"),
      "@types": path.resolve(__dirname, "src/types"),
      "@utils": path.resolve(__dirname, "src/utils"),
      "@": path.resolve(__dirname, "src"),
    },
  },
});
