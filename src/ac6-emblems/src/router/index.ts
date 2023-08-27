import EmblemsView from '@/views/EmblemsView.vue';
import { createRouter, createWebHistory } from 'vue-router';

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'emblems',
      component: EmblemsView
    }
  ]
});

export default router;
