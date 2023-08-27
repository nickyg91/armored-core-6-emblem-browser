<template>
  <div>{{ emblem.name }}</div>
  <div>{{ emblem.platform }}</div>
  <div>{{ emblem.shareId }}</div>
</template>

<script setup lang="ts">
import type { Emblem } from '@/models/emblem.model';
defineProps<{ emblem: Emblem }>();
</script>

<style scoped></style>
