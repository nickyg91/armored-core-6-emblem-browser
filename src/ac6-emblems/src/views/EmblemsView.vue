<script setup lang="ts">
import EmblemCard from '@/components/EmblemCard.vue';
import { useEmblemStore } from '@/stores/emblems.store';
import InputText from 'primevue/inputtext';
import ProgressSpinner from 'primevue/progressspinner';
import { onMounted, reactive, ref } from 'vue';
import Button from 'primevue/button';
import Dialog from 'primevue/dialog';
import AddEmblemForm from '@/components/AddEmblemForm.vue';
import { PlatformType } from '@/enums/platform-type.enum';
import { toArrayOfEnumDescriptions } from '@/shared/enum-functions';
import Checkbox from 'primevue/checkbox';
import { watchDebounced } from '@vueuse/core';

const store = useEmblemStore();

const filterCriteria = reactive({
  platforms: new Array<PlatformType>(),
  nameOrShareId: ''
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
</script>
<template>
  <section>
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
    <div class="flex justify-content-evenly mt-3">
      <div class="flex" v-for="item in platforms" :key="item.value">
        <Checkbox
          @click="onCheckboxClicked(item.value)"
          :name="item.key"
          :binary="true"
          v-bind:model-value="filterCriteria.platforms.indexOf(item.value) > -1"
        ></Checkbox>
        <label :for="item.key" class="ml-2">{{ item.key }}</label>
      </div>
    </div>
    <template v-if="!store.isLoading">
      <div class="mt-2 grid">
        <div
          v-for="emblem in store.emblems"
          :key="emblem.id"
          class="p-2 md:col-6 lg:col-3 sm:col-1"
        >
          <EmblemCard :key="emblem.id" :emblem="emblem"></EmblemCard>
        </div>
      </div>
    </template>
    <template v-else>
      <div class="loading-container flex align-items-center justify-content-center">
        <ProgressSpinner></ProgressSpinner>
      </div>
    </template>
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
</style>
