<script setup lang="ts">
const store = useEmblemsStore();
onMounted(async () => {
  await store.fetchEmblems();
});
</script>
<template>
  <UContainer>
    <USkeleton v-if="store.pending" />
    <UCard v-for="emblem in store.emblemResults?.emblems">
      <template #header>
        {{ emblem.name }}
      </template>
      {{ emblem.platform }}
      <template #footer>
        {{ emblem.imageUrl }}
      </template>
    </UCard>
  </UContainer>
</template>

<style scoped></style>
