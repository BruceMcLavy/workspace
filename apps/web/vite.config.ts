import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// Vite runs inside the web container.
// The API is reachable by service name "api" on port 5000.
// We proxy /api/* requests there so the browser stays same-origin.
export default defineConfig({
  plugins: [react()],
  server: {
    host: true,          // 0.0.0.0
    port: 5173,
    strictPort: true,
    proxy: {
      '/api': {
        target: 'http://api:5000',
        changeOrigin: true,
      },
    },
  },
})