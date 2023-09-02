<script setup lang="ts">
import axios from 'axios';
import { onMounted, ref } from 'vue';
import ProgressSpinner from 'primevue/progressspinner';
const props = defineProps<{ id: number; name: string }>();
const isLoading = ref(false);
const image = ref();
async function loadImage() {
  try {
    isLoading.value = true;
    var bytes = (
      await axios.get(`/api/emblem/image/${props.id}`, {
        responseType: 'blob'
      })
    ).data;
    const url = URL.createObjectURL(bytes);
    image.value = url;
  } catch (error) {
    //nothing right now
  } finally {
    isLoading.value = false;
  }
}
onMounted(async () => {
  await loadImage();
});
</script>
<template>
  <div class="img flex align-items-center justify-content-center spinner" v-if="isLoading">
    <ProgressSpinner></ProgressSpinner>
  </div>
  <div v-else>
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
