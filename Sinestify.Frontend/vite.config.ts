import react from '@vitejs/plugin-react'
import { defineConfig } from 'vite'

export default defineConfig(() => {
  const apiTarget = process.env.SINESTIFY_API_URL || 'http://localhost:5000'

  return {
    plugins: [react()],
    server: {
      proxy: {
        '/api': { target: apiTarget, changeOrigin: true },
        '/login': { target: apiTarget, changeOrigin: true },
        '/usuarios': { target: apiTarget, changeOrigin: true },
      },
    },
  }
})
