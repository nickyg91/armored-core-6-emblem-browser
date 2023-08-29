<script setup lang="ts">
import { RouterView } from 'vue-router';
import AddEmblemForm from '@/components/AddEmblemForm.vue';
import Dialog from 'primevue/dialog';
import Button from 'primevue/button';
import Toast from 'primevue/toast';
import { ref } from 'vue';
const isAddEmblemShown = ref(false);
function showAddEmblemComponent() {
  isAddEmblemShown.value = true;
}
</script>

<template>
  <section>
    <div class="p-5">
      <Suspense>
        <RouterView />
      </Suspense>
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
    <div class="absolute bottom-0 right-0 p-2">
      <Button
        @click="showAddEmblemComponent()"
        icon="pi pi-plus"
        rounded
        aria-label="Add"
        severity="primary"
      >
      </Button>
    </div>
  </section>
  <Toast />
</template>
<style>
.nav {
  height: 1.5em;
}
</style>
