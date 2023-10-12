<script setup lang="ts">
import { PlatformType } from '@/enums/platform-type.enum';
import { toTypedSchema } from '@vee-validate/zod';
import { Form, useForm } from 'vee-validate';
import { any, array, nativeEnum, object, string, z } from 'zod';
import InputText from 'primevue/inputtext';
import RadioButton from 'primevue/radiobutton';
import Button from 'primevue/button';
import { computed, ref } from 'vue';
import { useEmblemStore } from '@/stores/emblems.store';
import type { Emblem } from '@/models/emblem.model';
import Tag from 'primevue/tag';
import { toMapOfEnumDescriptions } from '@/shared/enum-functions';
import AutoComplete, { type AutoCompleteCompleteEvent } from 'primevue/autocomplete';

const MAX_FILE_SIZE = 250000;
const ACCEPTED_IMAGE_TYPES = ['image/jpeg', 'image/jpg', 'image/png'];
const fileData = ref<string | null>(null);
const file = ref<File | null>(null);
const emit = defineEmits<{ (e: 'addComplete'): void }>();
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
      }),
      tags: array(z.string())
        .nonempty('At least one tag is required.')
        .max(10, 'Cannot exceed more than 10 tags.')
    })
  )
});
const name = defineComponentBinds('name');
const shareId = defineComponentBinds('shareId');
const imageData = defineComponentBinds('imageData');
const platformValue = defineComponentBinds('platform');
const tags = defineComponentBinds('tags');
const store = useEmblemStore();
const platformValues = Object.keys(PlatformType).filter((key) => isNaN(Number(key)));
const platformMap = computed(() => {
  return toMapOfEnumDescriptions(PlatformType);
});

const filteredSuggestions = ref<string[]>([...store.tags]);

const onSubmit = handleSubmit(async (values) => {
  // for now.
  const emblem = values as unknown as Emblem;
  emblem.imageData = fileData.value!;
  emblem.imageExtension = file.value!.type;
  await store.addEmblem(emblem);
  emit('addComplete');
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
function tagQuery(event: AutoCompleteCompleteEvent) {
  const query = event.query;
  if (query.length === 0) {
    filteredSuggestions.value = [...store.tags];
  }
  const filtered: string[] = [query];

  store.tags.forEach((item) => {
    if (item.indexOf(query.toLowerCase()) > -1) {
      filtered.push(item);
    }
  });

  filteredSuggestions.value = filtered;
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
        <label for="shareId">Share ID</label>
      </span>
      <small id="shareId-help" class="p-error">
        {{ errors.shareId }}
      </small>
    </div>
    <div class="field mb-5">
      <span class="p-float-label p-fluid">
        <span class="p-float-label">
          <AutoComplete
            v-bind="tags"
            multiple
            :suggestions="filteredSuggestions"
            @complete="tagQuery"
            :class="{ 'p-invalid': errors.tags }"
          />
          <label for="tags">Tags</label>
        </span>
      </span>
      <small id="tags-help" class="p-error">
        {{ errors.tags }}
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
        <label for="imageData">Image</label>
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
    <div
      :class="{ 'p-invalid-section': errors.platform }"
      class="field flex justify-content-evenly"
    >
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
<style scoped lang="css">
.p-invalid-section {
  border: 1px solid var(--red-300);
  border-radius: 5px;
  padding: 0.75em;
}
</style>
