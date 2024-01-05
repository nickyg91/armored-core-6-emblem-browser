// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  devtools: { enabled: true },
  css: ['~/assets/css/main.css'],
  modules: ['@nuxt/ui', '@pinia/nuxt', '@nuxtjs/tailwindcss', '@nuxtjs/eslint-module', '@vueuse/nuxt'],
  postcss: {
    plugins: {
      tailwindcss: {},
      autoprefixer: {},
    },
  },
  nitro: {
    routeRules: {
      '/api/**': {
        proxy: {
          to: 'https://localhost:7111/api/**',
        },
      },
    },
  },
});
