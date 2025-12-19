import { defineConfig } from 'vite'
import { resolve } from 'path'

import vue from '@vitejs/plugin-vue'
import tailwindcss from '@tailwindcss/vite'



// https://vitejs.dev/config/
export default defineConfig({
  plugins: [
    vue(),
    tailwindcss()
  ],
  base: './',
  
  // Build configuration
  build: {
    outDir: '../unity/QuestSLAM-ros2/Assets/StreamingAssets/ui',
    emptyOutDir: true,
    assetsDir: 'assets',
    rollupOptions: {
      input: {
        main: resolve(__dirname, 'index.html')
      },
      output: {
        // Use consistent filenames (no hash) for easier Unity integration
        // These filenames are referenced in WebServerManager.cs for Android APK extraction
        // If you change these, update WebServerManager.cs ExtractAndroidUIFiles() method
        entryFileNames: 'assets/[name].js',
        chunkFileNames: 'assets/[name].js',
        assetFileNames: 'assets/[name].[ext]'
      }
    }
  },
  
  // Development server configuration
  server: {
    port: 5173,
  },
  
  // Path resolution
  resolve: {
    alias: {
      '@': resolve(__dirname, 'src')
    }
  }
})