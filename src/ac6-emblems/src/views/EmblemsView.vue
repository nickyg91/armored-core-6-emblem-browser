<script setup lang="ts">
import EmblemCard from '@/components/EmblemCard.vue';
import { useEmblemStore } from '@/stores/emblems.store';
import InputText from 'primevue/inputtext';
import ProgressSpinner from 'primevue/progressspinner';
import { onMounted, reactive, ref, watchEffect } from 'vue';
import Button from 'primevue/button';
import Dialog from 'primevue/dialog';
import AddEmblemForm from '@/components/AddEmblemForm.vue';
import { PlatformType } from '@/enums/platform-type.enum';
import { toArrayOfEnumDescriptions } from '@/shared/enum-functions';
import Checkbox from 'primevue/checkbox';
import { useDebounceFn, watchDebounced } from '@vueuse/core';
import Card from 'primevue/card';
import MultiSelect from 'primevue/multiselect';

const store = useEmblemStore();

const tags = store.tags;

const filterCriteria = reactive({
  platforms: new Array<PlatformType>(),
  nameOrShareId: '',
  tags: new Array<string>()
});

const platforms = toArrayOfEnumDescriptions(PlatformType);

const isAddEmblemShown = ref(false);
function showAddEmblemComponent() {
  isAddEmblemShown.value = true;
}

function onCheckboxClicked(val: number) {
  const idx = filterCriteria.platforms.indexOf(val);
  if (idx > -1) {
    filterCriteria.platforms.splice(idx, 1);
  } else {
    filterCriteria.platforms.push(val);
  }
}

watchDebounced(
  filterCriteria,
  async () => {
    if (!filterCriteria.nameOrShareId && filterCriteria.platforms.length == 0) {
      store.resetEmblems();
      await store.getEmblems();
    } else {
      store.resetEmblems();
      await store.getFilteredEmblems(filterCriteria.platforms, filterCriteria.nameOrShareId);
    }
  },
  { debounce: 500, maxWait: 1000, deep: true }
);

onMounted(async () => {
  await store.getEmblems();
  await store.getAllTags();
});

const onScrollDebounced = useDebounceFn(async (e) => {
  watchEffect(async () => {
    const clientHeight = e.target.clientHeight;
    const scrollHeight = e.target.scrollHeight;
    const scrollTop = e.target.scrollTop;
    if (scrollTop + clientHeight >= scrollHeight && !store.hasInFlightRequest) {
      store.currentPageNumber++;
      await store.getEmblems();
    }
  });
}, 1000);
</script>
<template>
  <section>
    <div>
      <Card>
        <template #content>
          <div class="flex">
            <div style="width: 93%" class="flex-grow-1 mr-5">
              <InputText
                v-model="filterCriteria.nameOrShareId"
                class="mr-5"
                placeholder="Search Name, ShareId"
                style="width: 100%"
              ></InputText>
            </div>
            <div class="flex-grow-1">
              <Button
                @click="showAddEmblemComponent()"
                icon="pi pi-plus"
                aria-label="Add"
                severity="primary"
                label="Add"
              >
              </Button>
            </div>
          </div>
          <div class="flex justify-content-evenly mt-5">
            <div class="flex align-items-center mr-1" v-for="item in platforms" :key="item.value">
              <Checkbox
                @click="onCheckboxClicked(item.value)"
                :name="item.key"
                :binary="true"
                :input-id="item.key"
                v-bind:model-value="filterCriteria.platforms.indexOf(item.value) > -1"
              ></Checkbox>
              <label :for="item.key" class="ml-2">
                {{ item.key }}
              </label>
            </div>
            <div class="flex align-items-center">
              <span class="p-float-label">
                <MultiSelect
                  style="width: 15rem"
                  id="ms-tags"
                  v-model="filterCriteria.tags"
                  :options="tags"
                />
                <label for="ms-tags">Tags</label>
              </span>
            </div>
          </div>
        </template>
      </Card>
    </div>
    <div @scroll="onScrollDebounced($event)" class="scrollable">
      <div>
        <div class="mt-2 grid">
          <div
            v-for="emblem in store.emblems"
            :key="emblem.id"
            class="p-2 md:col-6 lg:col-3 sm:col-1"
          >
            <EmblemCard :key="emblem.id" :emblem="emblem"></EmblemCard>
          </div>
        </div>
      </div>
      <div v-if="store.isLoading">
        <div class="loading-container flex align-items-center justify-content-center">
          <ProgressSpinner></ProgressSpinner>
        </div>
      </div>
    </div>
    <Dialog
      :draggable="false"
      v-model:visible="isAddEmblemShown"
      modal
      header="Add Emblem"
      :style="{ width: '50vw' }"
    >
      <AddEmblemForm></AddEmblemForm>
    </Dialog>
  </section>
</template>

<style scoped>
.loading-container {
  height: 100vh;
}

.scrollable {
  max-height: 1080px;
  width: 100%;
  overflow-y: scroll;
  overflow-x: hidden;
}
</style>
