<script setup lang="ts">
import { PlatformType } from '@/enums/platform-type.enum';
import { toTypedSchema } from '@vee-validate/zod';
import { Form, useForm } from 'vee-validate';
import { any, nativeEnum, object, string } from 'zod';
import InputText from 'primevue/inputtext';
import RadioButton from 'primevue/radiobutton';
import Button from 'primevue/button';
import { computed, ref } from 'vue';
import { useEmblemStore } from '@/stores/emblems.store';
import type { Emblem } from '@/models/emblem.model';
import Tag from 'primevue/tag';
const MAX_FILE_SIZE = 250000;
const ACCEPTED_IMAGE_TYPES = ['image/jpeg', 'image/jpg', 'image/png'];
const fileData = ref<string | null>(null);
const file = ref<File | null>(null);
const { defineComponentBinds, handleSubmit, errors, resetField } = useForm({
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
      imageData: any()
        .refine(() => file.value?.size ?? 0 <= MAX_FILE_SIZE, `Max image size is 2.5MB.`)
        .refine(() => ACCEPTED_IMAGE_TYPES.includes(file.value?.type ?? '')),
      platform: nativeEnum(PlatformType, {
        required_error: 'Platform is required.'
      })
    })
  )
});

const name = defineComponentBinds('name');
const shareId = defineComponentBinds('shareId');
const imageData = defineComponentBinds('imageData');
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
  const emblem = values as Emblem;
  emblem.imageData = fileData.value!;
  await store.addEmblem(emblem);
});

function onFileChanged($event: Event) {
  const target = $event.target as HTMLInputElement;
  if (target && target.files) {
    file.value = target.files[0];

    const reader = new FileReader();
    reader.readAsDataURL(file.value);
    reader.onload = function () {
      fileData.value = reader.result?.toString()!;
    };
  }
}

function removeFile() {
  file.value = null;
  resetField('imageData');
}
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
          v-bind="imageData"
          placeholder=""
          type="file"
          @change="onFileChanged"
          :class="{ 'p-invalid': errors.imageData }"
        />
        <label for="name">Image</label>
      </span>
      <small id="image-help" class="p-error">
        {{ errors.imageData }}
      </small>
      <div v-if="file" class="field flex justify-content-start mt-2">
        <Tag
          @click="removeFile"
          :key="file?.name"
          :value="file?.name"
          icon="pi pi-times"
          severity="primary"
        >
        </Tag>
      </div>
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