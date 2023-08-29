import { createApp } from 'vue';
import { createPinia } from 'pinia';

import App from './App.vue';
import router from './router';
import 'primeicons/primeicons.css';
import PrimeVue from 'primevue/config';
// use primevue base css for now until I feel like using unstyled with cirrus.
import 'primevue/resources/themes/soho-dark/theme.css';
import 'primeflex/primeflex.css';
import ToastService from 'primevue/toastservice';
import '@/assets/main.css';

const app = createApp(App);

app.use(PrimeVue);
app.use(createPinia());
app.use(router);
app.use(ToastService);

app.mount('#app');
