<script setup lang="ts">
import { toArrayOfEnumDescriptions } from '~/shared/enum-functions';
import { PlatformType } from '~/types/platform-type.enum';

const toast = useToast();
const emits = defineEmits<{
  (e: 'addClicked'): void;
  (e: 'searchLoading', isLoading: boolean): void;
}>();
const store = useEmblemsStore();
const platforms = toArrayOfEnumDescriptions(PlatformType);
const filterCriteria = reactive({
  platforms: new Array<PlatformType>(),
  searchQuery: '',
  tags: new Array<string>(),
});

watchDebounced(
  filterCriteria,
  async () => {
    emits('searchLoading', true);
    try {
      if (!filterCriteria.searchQuery && filterCriteria.platforms.length === 0 && filterCriteria.tags.length === 0) {
        store.resetEmblems();
        await store.fetchEmblems();
      } else {
        store.resetEmblems();
        await store.fetchFilteredEmblems(filterCriteria.platforms, filterCriteria.searchQuery, filterCriteria.tags);
      }
    } catch (error) {
      toast.add({
        timeout: 5000,
        color: 'red',
        icon: 'i-heroicons-x-mark',
        title: 'Error!',
        description: 'Your search failed! Try again later.',
      });
    } finally {
      emits('searchLoading', false);
    }
  },
  { debounce: 500, maxWait: 1000, deep: true },
);

function onCheckboxClicked(val: number) {
  const idx = filterCriteria.platforms.indexOf(val);
  if (idx > -1) {
    filterCriteria.platforms.splice(idx, 1);
  } else {
    filterCriteria.platforms.push(val);
  }
}
</script>
<template>
  <UCard>
    <div class="flex">
      <div class="flex flex-grow mr-3">
        <UInput
          v-model="filterCriteria.searchQuery"
          class="w-full"
          placeholder="Search Name, Tags, or Share ID"
        ></UInput>
      </div>
      <div class="flex justify-end">
        <UButton
          label="Add Emblem"
          @click="emits('addClicked')"
        ></UButton>
      </div>
    </div>
    <div class="flex justify-between mt-5">
      <div
        v-for="item in platforms"
        :key="item.value + '-filter'"
        class="flex align-items-center mr-1 p-1"
      >
        <UCheckbox
          :id="item.key + '-filter'"
          :name="item.key + '-filter'"
          :label="item.key"
          @click="onCheckboxClicked(item.value)"
        ></UCheckbox>
      </div>
    </div>
    <div class="flex flex-grow align-items-center mt-5">
      <USelectMenu
        v-model="filterCriteria.tags"
        :options="store.tags"
        class="w-full"
        placeholder="Tags"
        multiple
        searchable
      ></USelectMenu>
    </div>
  </UCard>
</template>
<style scoped></style>
