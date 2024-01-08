<script setup lang="ts">
import { any, array, nativeEnum, object, string, z } from 'zod';
import { computed, ref } from 'vue';
import type { Form, FormSubmitEvent } from '@nuxt/ui/dist/runtime/types/form';
import { toRadioOptionsEnumDescriptions } from '@/shared/enum-functions';
import { PlatformType } from '~/types/platform-type.enum';
import type { Emblem } from '~/types/emblems/emblem.model';

const MAX_FILE_SIZE = 250000;
const ACCEPTED_IMAGE_TYPES = ['image/jpeg', 'image/jpg', 'image/png'];
const fileData = ref<string | null>(null);
const file = ref<File | null>(null);
const emit = defineEmits<{ (e: 'addComplete'): void }>();

const schema = object({
  name: string().min(1, 'Name is required.').max(64, 'Name cannot be more than 64 characters in length.').default(''),
  shareId: string()
    .min(1, 'Share ID is required.')
    .max(256, 'Share ID cannot be more than 256 characters in length.')
    .default(''),
  imageData: any()
    .refine(() => file.value?.size ?? MAX_FILE_SIZE >= 0, 'Max image size is 2.5MB.')
    .refine(() => ACCEPTED_IMAGE_TYPES.includes(file.value?.type ?? '')),
  platform: nativeEnum(PlatformType, {
    required_error: 'Platform is required.',
  }),
  tags: array(z.string()).nonempty('At least one tag is required.').max(10, 'Cannot exceed more than 10 tags.'),
});

const store = useEmblemsStore();
const platformMap = computed(() => {
  return toRadioOptionsEnumDescriptions(PlatformType);
});

const form = ref<Form<Emblem> | null>(null);

const filteredSuggestions = ref<string[]>([...store.tags]);

type EmblemSchema = z.output<typeof schema>;
// eslint-disable-next-line require-await
async function onSubmit(values: FormSubmitEvent<EmblemSchema>) {
  debugger;
  const emblem = values.data;
  console.log(emblem);
  emblem.imageData = fileData.value!;
  // emblem.imageExtension = file.value!.type;
  // await store.addEmblem(emblem);
  emit('addComplete');
}
function onFileChanged($event: Event) {
  const target = $event.target as HTMLInputElement;
  if (target && target.files) {
    file.value = target.files[0];
    const reader = new FileReader();
    reader.readAsDataURL(file.value);
    reader.onload = function () {
      fileData.value = reader?.result?.toString() ?? '';
    };
  }
}

const state = reactive({
  name: '',
  shareId: '',
  imageData: '',
  platform: '',
  tags: [],
});

function removeFile() {
  file.value = null;
  state.imageData = '';
}
</script>
<template>
  <UForm
    ref="form"
    :state="state"
    :schema="schema"
    class="space-y-4"
    @submit="onSubmit"
  >
    <UFormGroup
      label="Name"
      name="name"
    >
      <UInput
        v-model="state.name"
        placeholder="Name"
      />
    </UFormGroup>
    <UFormGroup
      label="Share ID"
      name="shareId"
    >
      <UInput
        v-model="state.shareId"
        placeholder="Share ID"
      />
    </UFormGroup>
    <UFormGroup
      label="Tags"
      name="tags"
    >
      <USelectMenu
        v-model="state.tags"
        :options="filteredSuggestions"
        creatable
        searchable
        multiple
      >
      </USelectMenu>
    </UFormGroup>
    <UFormGroup
      label="Image"
      name="imageData"
    >
      <UInput
        v-model="state.imageData"
        placeholder="Upload an Image"
        type="file"
        @change="onFileChanged"
      />
      <UButton
        v-if="file"
        icon="i-heroicons-x-mark"
        :label="file?.name"
        @click="removeFile"
      >
      </UButton>
    </UFormGroup>
    <UFormGroup>
      <URadioGroup
        v-model="state.platform"
        :options="platformMap"
        name="platform"
      >
      </URadioGroup>
    </UFormGroup>
    <div class="footer mt-2">
      <div class="flex">
        <UButton
          type="submit"
          icon="i-heroicons-check"
          label="Submit"
        ></UButton>
      </div>
    </div>
  </UForm>
</template>
<style scoped lang="css">
.p-invalid-section {
  border: 1px solid var(--red-300);
  border-radius: 5px;
  padding: 0.75em;
}
</style>
