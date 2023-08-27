<template>
  <div>Hello Emblems!</div>
  <div v-for="emblem in emblems" :key="emblem.id">
    <EmblemCard :emblem="emblem" />
  </div>
</template>

<script setup lang="ts">
import EmblemCard from '@/components/EmblemCard.vue';
import { useEmblemStore } from '@/stores/emblems.store';

const store = useEmblemStore();
await store.getEmblems();
const emblems = store.emblems;
</script>

<style scoped></style>
