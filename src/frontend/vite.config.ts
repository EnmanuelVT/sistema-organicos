import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

export default defineConfig({
  plugins: [react()],
  resolve: {
    alias: {
      '@': '/src',
      '@components': '/src/../components',
    },
  },
  server: {
    port: 5173,
    proxy: {
      '/api': {
        target: 'http://localhost:5062',
        changeOrigin: true,
        secure: false,
      },
      '/login': {
        target: 'http://localhost:5062',
        changeOrigin: true,
        secure: false,
      },
      '/register': {
        target: 'http://localhost:5062',
        changeOrigin: true,
        secure: false,
      },
      '/refresh': {
        target: 'http://localhost:5062',
        changeOrigin: true,
        secure: false,
      },
    },
  },
})