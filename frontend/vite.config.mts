import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import path from 'path'

export default defineConfig({
  plugins: [react()],
  resolve: {
    alias: {
      '@': path.resolve(__dirname, './src'),
    },
  },
  server: {
    port: 5173,
    proxy: {
      '/login':      { target: 'https://localhost:7232', changeOrigin: true, secure: false },
      '/me':         { target: 'https://localhost:7232', changeOrigin: true, secure: false },
      '/muestras':   { target: 'https://localhost:7232', changeOrigin: true, secure: false },
      '/ensayos':    { target: 'https://localhost:7232', changeOrigin: true, secure: false },
      '/resultados': { target: 'https://localhost:7232', changeOrigin: true, secure: false },
      '/documentos': { target: 'https://localhost:7232', changeOrigin: true, secure: false },
      '/admin':      { target: 'https://localhost:7232', changeOrigin: true, secure: false },
      '/swagger':    { target: 'https://localhost:7232', changeOrigin: true, secure: false },
    },
  },
})
