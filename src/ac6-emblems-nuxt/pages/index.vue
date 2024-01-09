<script setup lang="ts">
const store = useEmblemsStore();
const isAddModalShown = ref(false);

onMounted(async () => {
  await store.fetchEmblems();
});
</script>
<template>
  <UContainer>
    <USkeleton v-if="store.pending" />
    <div v-else>
      <div>
        <UButton
          label="Add Emblem"
          @click="isAddModalShown = true"
        ></UButton>
      </div>
      <div class="mt-5 grid grid-cols-4 grid-flow-col gap-5">
        <EmblemCard
          v-for="emblem in store.emblems"
          :key="emblem.id"
          :emblem="emblem"
        ></EmblemCard>
      </div>
    </div>
    <USlideover
      v-model="isAddModalShown"
      :prevent-close="true"
    >
      <UCard>
        <template #header>
          <div class="flex justify-end">
            <UButton
              :ui="{ rounded: 'rounded-full' }"
              icon="i-heroicons-x-mark"
              @click="isAddModalShown = false"
            ></UButton>
          </div>
        </template>
        <AddEmblemForm @add-complete="isAddModalShown = false"></AddEmblemForm>
      </UCard>
    </USlideover>
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
