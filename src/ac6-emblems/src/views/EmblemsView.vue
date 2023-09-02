<script setup lang="ts">
import EmblemCard from '@/components/EmblemCard.vue';
import { useEmblemStore } from '@/stores/emblems.store';
import InputText from 'primevue/inputtext';
import ProgressSpinner from 'primevue/progressspinner';

const store = useEmblemStore();
await store.getEmblems();
const emblems = store.emblems;
</script>
<template>
  <section>
    <template v-if="!store.isLoading">
      <div class="flex align-content-start">
        <InputText style="width: 100%" placeholder="Search Name, ShareId, Platform"></InputText>
      </div>
      <div class="mt-2 grid">
        <div v-for="emblem in emblems" :key="emblem.id" class="p-2 md:col-6 lg:col-3 sm:col-1">
          <EmblemCard :emblem="emblem"></EmblemCard>
        </div>
      </div>
    </template>
    <template v-else>
      <div class="loading-container flex align-items-center justify-content-center">
        <ProgressSpinner></ProgressSpinner>
      </div>
    </template>
  </section>
</template>
<style scoped>
.loading-container {
  height: 100vh;
}
</style>
