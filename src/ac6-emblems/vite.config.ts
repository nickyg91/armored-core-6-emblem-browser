import { fileURLToPath, URL } from 'url';
import { defineConfig } from 'vite';
import vue from '@vitejs/plugin-vue';
import { join } from 'path';
import { readFileSync } from 'fs';

export default defineConfig(({ command }) => {
  if (command === 'serve') {
    const baseFolder =
      process.env.APPDATA !== undefined && process.env.APPDATA !== ''
        ? `${process.env.APPDATA}/ASP.NET/https`
        : `${process.env.HOME}/.aspnet/https`;
    const certificateName = process.env.npm_package_name;

    const certFilePath = join(baseFolder, `${certificateName}.pem`);
    const keyFilePath = join(baseFolder, `${certificateName}.key`);
    return {
      plugins: [vue()],
      server: {
        https: {
          key: readFileSync(keyFilePath),
          cert: readFileSync(certFilePath)
        },
        port: 5002,
        strictPort: true,
        proxy: {
          '/api': {
            target: 'https://localhost:7111/',
            changeOrigin: true,
            secure: false
          }
        }
      },
      resolve: {
        alias: {
          '@': fileURLToPath(new URL('./src', import.meta.url))
        }
      }
    };
  } else {
    return {
      plugins: [vue()],
      resolve: {
        alias: {
          '@': fileURLToPath(new URL('./src', import.meta.url))
        }
      }
    };
  }
});
