<script setup lang="ts">
import { PlatformType } from '@/enums/platform-type.enum';
import { toTypedSchema } from '@vee-validate/zod';
import { Form, useForm } from 'vee-validate';
import { nativeEnum, object, string } from 'zod';
import InputText from 'primevue/inputtext';
import RadioButton from 'primevue/radiobutton';
import Button from 'primevue/button';
import { computed } from 'vue';
import { useEmblemStore } from '@/stores/emblems.store';
import type { Emblem } from '@/models/emblem.model';

const { defineComponentBinds, handleSubmit, errors } = useForm({
  validationSchema: toTypedSchema(
    object({
      name: string()
        .nonempty('Name is required.')
        .max(64, 'Name cannot be more than 64 characters in length.')
        .default(''),
      shareId: string()
        .nonempty('Share ID is required.')
        .max(256, 'Share ID cannot be more than 256 characters in length.')
        .default(''),
      imageUrl: string().url('Value must be a valid URL.').optional(),
      platform: nativeEnum(PlatformType, {
        required_error: 'Platform is required.'
      })
    })
  )
});

const name = defineComponentBinds('name');
const shareId = defineComponentBinds('shareId');
const imageUrl = defineComponentBinds('imageUrl');
const platformValue = defineComponentBinds('platform');
const store = useEmblemStore();

const platformValues = Object.keys(PlatformType).filter((key) => isNaN(Number(key)));
const platformMap = computed(() => {
  const map = new Map<string, number>();
  Object.keys(PlatformType)
    .filter((key) => isNaN(Number(key)))
    .map((key) => {
      map.set(key, Number(PlatformType[key as keyof typeof PlatformType]));
    });
  return map;
});

const onSubmit = handleSubmit(async (values) => {
  await store.addEmblem(values as Emblem);
});
</script>
<template>
  <Form>
    <div class="field mt-5 mb-5">
      <span class="p-float-label">
        <InputText style="width: 100%" v-bind="name" :class="{ 'p-invalid': errors.name }" />
        <label for="name">Name</label>
      </span>
      <small id="name-help" class="p-error">
        {{ errors.name }}
      </small>
    </div>
    <div class="field mb-5">
      <span class="p-float-label">
        <InputText style="width: 100%" v-bind="shareId" :class="{ 'p-invalid': errors.shareId }" />
        <label for="name">Share ID</label>
      </span>
      <small id="shareId-help" class="p-error">
        {{ errors.shareId }}
      </small>
    </div>
    <div class="field mb-5">
      <span class="p-float-label">
        <InputText
          style="width: 100%"
          v-bind="imageUrl"
          :class="{ 'p-invalid': errors.imageUrl }"
        />
        <label for="name">Image URL</label>
      </span>
      <small id="imageUrl-help" class="p-error">
        {{ errors.imageUrl }}
      </small>
    </div>
    <div class="field flex justify-content-evenly">
      <template v-for="platform in platformValues" :key="platform">
        <div>
          <RadioButton v-bind="platformValue" name="platform" :value="platformMap.get(platform)">
          </RadioButton>
          <label :for="platform" class="ml-2">{{ platform }}</label>
        </div>
      </template>
    </div>
    <div class="m2-5">
      <small id="platform-help" class="p-error">
        {{ errors.platform }}
      </small>
    </div>

    <div class="footer mt-2">
      <div class="flex justify-content-end">
        <Button @click="onSubmit" :loading="store.isAddLoading" label="Submit"></Button>
      </div>
    </div>
  </Form>
</template>
