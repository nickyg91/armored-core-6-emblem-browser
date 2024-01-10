<script setup lang="ts">
import { toArrayOfEnumDescriptions } from '~/shared/enum-functions';
import { PlatformType } from '~/types/platform-type.enum';

const emits = defineEmits<{ (e: 'addClicked'): void }>();
const store = useEmblemsStore();
const platforms = toArrayOfEnumDescriptions(PlatformType);
const filterCriteria = reactive({
  platforms: new Array<PlatformType>(),
  searchQuery: '',
  tags: new Array<string>(),
});

watchDebounced(
  filterCriteria,
  () => {
    if (!filterCriteria.searchQuery && filterCriteria.platforms.length === 0 && filterCriteria.tags.length === 0) {
      // store.resetEmblems();
      // await store.getEmblems();
    } else {
      // store.resetEmblems();
      // await store.getFilteredEmblems(filterCriteria.platforms, filterCriteria.searchQuery, filterCriteria.tags);
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
    <div class="flex justify-evenly mt-5">
      <div
        v-for="item in platforms"
        :key="item.value + '-filter'"
        class="flex align-items-center mr-1"
      >
        <UCheckbox
          :id="item.key + '-filter'"
          :name="item.key + '-filter'"
          :label="item.key"
          @click="onCheckboxClicked(item.value)"
        ></UCheckbox>
      </div>
      <div class="flex align-items-center">
        <USelectMenu
          v-model="filterCriteria.tags"
          class="w-max"
          :options="store.tags"
          placeholder="Tags"
          multiple
          searchable
        >
        </USelectMenu>
      </div>
    </div>
  </UCard>
</template>
<style scoped></style>
