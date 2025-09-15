import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

export default defineConfig({
  plugins: [react()],
  resolve: {
    alias: {
      '@': '/src',
    },
  },
  server: {
    port: 5173,
    proxy: {
  '/login':      { target: 'http://localhost:5062', changeOrigin: true, secure: false },
  '/me':         { target: 'http://localhost:5062', changeOrigin: true, secure: false },
  '/muestras':   { target: 'http://localhost:5062', changeOrigin: true, secure: false },
  '/ensayos':    { target: 'http://localhost:5062', changeOrigin: true, secure: false },
  '/resultados': { target: 'http://localhost:5062', changeOrigin: true, secure: false },
  '/documentos': { target: 'http://localhost:5062', changeOrigin: true, secure: false },
  '/admin':      { target: 'http://localhost:5062', changeOrigin: true, secure: false },
  '/swagger':    { target: 'http://localhost:5062', changeOrigin: true, secure: false },
    },
  },
})
