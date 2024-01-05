<script setup lang="ts">
import type { Emblem } from '~/types/emblems/emblem.model';
import { PlatformType } from '~/types/platform-type.enum';
defineProps<{ emblem: Emblem }>();

function getPlatformName(platform: PlatformType): string {
  return PlatformType[platform];
}

function getTagColor(platform: PlatformType): string {
  switch (platform) {
    case PlatformType.PC:
      return 'purple';
    case PlatformType['Xbox One']:
    case PlatformType['Xbox Series X|S']:
      return 'green';
    case PlatformType.PS4:
    case PlatformType.PS5:
      return 'blue';
    default:
      return 'yelow';
  }
}
</script>
<template>
  <UCard :key="emblem.id" class="card-container">
    <template v-if="emblem.imageUrl" #header>
      <ImageContainer :id="emblem.id" :name="emblem.name"></ImageContainer>
    </template>
    <h1>{{ emblem.name }}</h1>
    <h2>{{ emblem.shareId }}</h2>
    <h3 v-if="emblem.tags.length > 0">Tags</h3>
    <div class="flex flex-wrap">
      <div v-for="tag in emblem.tags" :key="tag" class="m-2">
        <UBadge :ui="{ color: 'gray' }">{{ tag }}</UBadge>
      </div>
      <div>
        <UBadge variant="soft" :color="getTagColor(emblem.platform)" :ui="{ rounded: 'rounded-full' }">
          {{ getPlatformName(emblem.platform) }}
        </UBadge>
      </div>
    </div>
  </UCard>
</template>
<style scoped>
.card-container {
  height: 100%;
}
</style>
