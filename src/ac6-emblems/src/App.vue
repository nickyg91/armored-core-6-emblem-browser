<script setup lang="ts">
import { RouterView } from 'vue-router';
</script>

<template>
  <section class="p-panel">
    <div class="p-panel-content">
      <Suspense>
        <RouterView />
      </Suspense>
    </div>
  </section>
</template>
<style>
.nav {
  height: 1.5em;
}
</style>
