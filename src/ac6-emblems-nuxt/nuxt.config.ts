// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  devtools: { enabled: true },
  modules: ["@nuxt/ui", "@pinia/nuxt"],
  nitro: {
    routeRules: {
      // "/api/**": {
      //   proxy: "https://localhost:7111/",
      // },
      "/api/**": {
        proxy: {
          to: "https://localhost:7111/api/**",
        },
      },
    },
  },
});
