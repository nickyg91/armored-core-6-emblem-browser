<script setup lang="ts">
import { PlatformType } from '@/enums/platform-type.enum';
import type { Emblem } from '@/models/emblem.model';
import Card from 'primevue/card';
import Tag from 'primevue/tag';
import ImageContainer from './ImageContainer.vue';
defineProps<{ emblem: Emblem }>();

function getTagColor(platform: PlatformType): string {
  switch (platform) {
    case PlatformType.PC:
      return 'warning';
    case PlatformType['Xbox One']:
    case PlatformType['Xbox Series X|S']:
      return 'success';
    case PlatformType.PS4:
    case PlatformType.PS5:
      return 'primary';
    default:
      return 'info';
  }
}

function getPlatformName(platform: PlatformType): string {
  return PlatformType[platform];
}
</script>

<template>
  <Card class="card-container">
    <template v-if="emblem.imageUrl" #header>
      <ImageContainer :id="emblem.id" :name="emblem.name"></ImageContainer>
    </template>
    <template #title> {{ emblem.name }} </template>
    <template #subtitle>
      <Tag :severity="getTagColor(emblem.platform)" :value="getPlatformName(emblem.platform)">
      </Tag>
    </template>
    <template #content>
      <h2>{{ emblem.shareId }}</h2>
    </template>
  </Card>
</template>

<style scoped>
.card-container {
  height: 100%;
}
</style>