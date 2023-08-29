import type { IEmblemSearchResult } from '@/models/emblem-search-result.interface';
import type { Emblem } from '@/models/emblem.model';
import axios from 'axios';
import { defineStore } from 'pinia';
import { reactive, ref } from 'vue';
import { useToast } from 'primevue/usetoast';

export const useEmblemStore = defineStore('emblemStore', () => {
  const emblems = reactive<Array<Emblem>>([]);
  const currentPageNumber = ref(1);
  const totalPerPage = ref(25);
  const totalEmblems = ref(0);
  const toast = useToast();
  const isLoading = ref(false);
  const isAddLoading = ref(false);

  async function getEmblems(): Promise<void> {
    try {
      isLoading.value = true;
      const results = await axios.get<IEmblemSearchResult>(
        `api/emblem/search/${currentPageNumber.value}/${totalPerPage.value}`
      );
      if (totalEmblems.value === 0) {
        totalEmblems.value = results.data.totalEmblems;
      }
      emblems.push(...results.data.emblems);
    } catch (error) {
      console.error(error);
      toast.add({
        severity: 'error',
        detail: 'Error getting emblems.',
        summary: 'Error',
        life: 2000
      });
    } finally {
      isLoading.value = false;
    }
  }

  async function addEmblem(emblem: Emblem): Promise<void> {
    try {
      isAddLoading.value = true;
      const addedEmblem = await axios.post('api/emblem/create', emblem);
      emblems.push(addedEmblem.data);
      toast.add({
        severity: 'success',
        detail: 'Emblem created successfully.',
        summary: 'Created',
        life: 2000
      });
    } catch (error) {
      console.error(error);
      toast.add({
        severity: 'error',
        detail: 'Error getting emblems.',
        summary: 'Error',
        life: 2000
      });
    } finally {
      isAddLoading.value = false;
    }
  }

  return {
    emblems,
    currentPageNumber,
    getEmblems,
    addEmblem,
    isLoading,
    isAddLoading
  };
});
