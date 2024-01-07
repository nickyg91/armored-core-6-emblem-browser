<script setup lang="ts">
const target = ref(null);
const targetIsVisible = useElementVisibility(target);
const props = defineProps<{ id: number; name: string }>();
const isLoading = ref(false);
const image = ref();
const url = ref<string | null>(null);

onUnmounted(() => {
  URL.revokeObjectURL(image.value);
});

watchEffect(async () => {
  if (targetIsVisible.value && !url.value) {
    await loadImage();
  }
});

async function loadImage() {
  try {
    isLoading.value = true;
    const blob = await $fetch(`/api/emblem/image/${props.id}`, {
      responseType: 'blob',
    });
    url.value = URL.createObjectURL(blob as Blob);
    image.value = url.value;
  } catch (error) {
  } finally {
    isLoading.value = false;
  }
}
</script>
<template>
  <div>
    <div
      v-if="isLoading"
      ref="target"
      class="img flex justify-center items-center"
    >
      <USkeleton />
    </div>
    <div
      v-else
      ref="target"
      class="img-container"
    >
      <img
        class="img"
        :src="image"
        :alt="'image for ' + name"
      />
    </div>
  </div>
</template>
<style scoped>
.img {
  width: 100%;
  height: 100%;
  border-top-left-radius: 6px;
  border-top-right-radius: 6px;
  border-bottom-left-radius: 6px;
  border-bottom-right-radius: 6px;
  animation: fadeIn 3s;
}

.img-container {
  width: 250px;
  height: 172px;
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
