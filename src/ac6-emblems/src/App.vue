<script setup lang="ts">
import { RouterView } from 'vue-router';
import Toast from 'primevue/toast';
</script>

<template>
  <section>
    <div class="p-5">
      <Suspense>
        <RouterView />
      </Suspense>
    </div>
  </section>
  <Toast />
</template>
<style></style>
