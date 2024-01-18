<script setup lang="ts">
const store = useEmblemsStore();
const isAddSlideoutShown = ref(false);
const isLoading = ref(false);
const toast = useToast();
onMounted(async () => {
  try {
    await store.fetchEmblems();
    await store.fetchTags();
  } catch (error) {
    // eslint-disable-next-line no-console
    console.error(error);
    toast.add({
      timeout: 5000,
      color: 'red',
      icon: 'i-heroicons-x-mark',
      title: 'Error!',
      description: 'An Error occurred fetching emblems.',
    });
  }
});
const onScrollDebounced = useDebounceFn((e) => {
  watchEffect(async () => {
    const clientHeight = e.target.clientHeight;
    const scrollHeight = e.target.scrollHeight;
    const scrollTop = e.target.scrollTop;
    if (scrollTop + clientHeight >= scrollHeight) {
      store.currentPageNumber++;
      await store.fetchEmblems();
    }
  });
}, 1000);
const onAddComplete = async () => {
  await store.fetchTags();
  isAddSlideoutShown.value = false;
};
</script>
<template>
  <UContainer>
    <div class="mt-5">
      <SearchEmblems
        @search-loading="isLoading = $event"
        @add-clicked="isAddSlideoutShown = true"
      ></SearchEmblems>
    </div>
    <LoadingSpinner v-if="isLoading"></LoadingSpinner>
    <div
      v-else
      class="mt-5 transition-opacity duration-200 opacity-100 scrollable"
      @scroll="onScrollDebounced($event)"
    >
      <div class="grid grid-cols-4 grid-flow-col gap-5">
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
