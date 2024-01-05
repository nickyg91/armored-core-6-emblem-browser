<script setup lang="ts">
const store = useEmblemsStore();
onMounted(async () => {
  await store.fetchEmblems();
});
</script>
<template>
  <UContainer>
    <USkeleton v-if="store.pending" />
    <div v-else class="mt-5 grid grid-cols-4 grid-flow-col gap-5">
      <EmblemCard v-for="emblem in store.emblems" :key="emblem.id" :emblem="emblem"></EmblemCard>
    </div>
  </UContainer>
</template>

<style scoped>
.scrollable {
  max-height: 1080px;
  width: 100%;
  overflow-y: scroll;
  overflow-x: hidden;
}
</style>
