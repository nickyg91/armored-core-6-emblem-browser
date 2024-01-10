<script setup lang="ts">
const store = useEmblemsStore();
const isAddSlideoutShown = ref(false);

onMounted(async () => {
  await store.fetchEmblems();
  await store.fetchTags();
});

const onAddComplete = async () => {
  await store.fetchTags();
  isAddSlideoutShown.value = false;
};
</script>
<template>
  <UContainer>
    <USkeleton v-if="store.pending" />
    <div
      v-else
      class="mt-5"
    >
      <div>
        <SearchEmblems @add-clicked="isAddSlideoutShown = true"></SearchEmblems>
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
      v-model="isAddSlideoutShown"
      :prevent-close="true"
    >
      <UCard>
        <template #header>
          <div class="flex justify-end">
            <UButton
              :ui="{ rounded: 'rounded-full' }"
              icon="i-heroicons-x-mark"
              @click="isAddSlideoutShown = false"
            ></UButton>
          </div>
        </template>
        <AddEmblemForm @add-complete="onAddComplete"></AddEmblemForm>
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
