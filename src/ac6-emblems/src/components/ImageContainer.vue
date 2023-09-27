<script setup lang="ts">
import axios from 'axios';
import { onUnmounted, ref, watchEffect } from 'vue';
import ProgressSpinner from 'primevue/progressspinner';
import { useElementVisibility } from '@vueuse/core';
const target = ref(null);
const targetIsVisible = useElementVisibility(target);
const props = defineProps<{ id: number; name: string }>();
const isLoading = ref(false);
const image = ref();
const url = ref<string | null>(null);
async function loadImage() {
  try {
    isLoading.value = true;
    const blob = (
      await axios.get(`/api/emblem/image/${props.id}`, {
        responseType: 'blob'
      })
    ).data;
    url.value = URL.createObjectURL(blob);
    image.value = url.value;
  } catch (error) {
    //nothing right now
  } finally {
    isLoading.value = false;
  }
}

onUnmounted(() => {
  URL.revokeObjectURL(image.value);
});

watchEffect(async () => {
  if (targetIsVisible.value && !url.value) {
    await loadImage();
  }
});
</script>
<template>
  <div
    ref="target"
    class="img flex align-items-center justify-content-center spinner"
    v-if="isLoading"
  >
    <ProgressSpinner></ProgressSpinner>
  </div>
  <div ref="target" class="img-container" v-else>
    <img class="img" :src="image" :alt="'image for ' + name" />
  </div>
</template>

<style scoped>
.img {
  width: 100%;
  height: 100%;
  border-top-left-radius: 6px;
  border-top-right-radius: 6px;
  animation: fadeIn 3s;
}

.img-container {
  width: 100%;
  height: 100%;
}

.spinner {
  animation: fadeOut;
}

@keyframes fadeIn {
  0% {
    opacity: 0;
  }
  100% {
    opacity: 1;
  }
}

@keyframes fadeOut {
  0% {
    opacity: 1;
  }
  100% {
    opacity: 0;
  }
}
</style>
