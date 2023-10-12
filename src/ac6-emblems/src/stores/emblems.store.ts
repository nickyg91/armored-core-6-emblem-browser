import type { IEmblemSearchResult } from '@/models/emblem-search-result.interface';
import type { Emblem } from '@/models/emblem.model';
import axios from 'axios';
import { defineStore } from 'pinia';
import { ref } from 'vue';
import { useToast } from 'primevue/usetoast';
import type { PlatformType } from '@/enums/platform-type.enum';

export const useEmblemStore = defineStore('emblemStore', () => {
  const emblems = ref<Array<Emblem>>([]);
  const currentPageNumber = ref(1);
  const totalPerPage = ref(25);
  const totalEmblems = ref(0);
  const toast = useToast();
  const isLoading = ref(false);
  const isAddLoading = ref(false);
  const tags = ref<Array<string>>([]);
  const hasInFlightRequest = ref(false);

  axios.interceptors.request.use((config) => {
    hasInFlightRequest.value = true;
    return config;
  });

  axios.interceptors.response.use((config) => {
    hasInFlightRequest.value = false;
    return config;
  });

  async function getEmblems(): Promise<void> {
    try {
      if (totalEmblems.value === emblems.value.length && emblems.value.length !== 0) {
        return;
      }
      isLoading.value = true;
      const results = await axios.get<IEmblemSearchResult>(
        `api/emblem/search/${currentPageNumber.value}/${totalPerPage.value}`
      );
      totalEmblems.value = results.data.totalEmblems;

      results.data.emblems.forEach((emblem) => {
        emblems.value.push(emblem);
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
      isLoading.value = false;
    }
  }

  async function getFilteredEmblems(
    platforms: PlatformType[],
    nameOrShareId: string,
    tags: string[]
  ) {
    currentPageNumber.value = 1;
    emblems.value = [];
    try {
      isLoading.value = true;
      const url = `api/emblem/search/${currentPageNumber.value}/${totalPerPage.value}`;

      const queryString: URLSearchParams = new URLSearchParams();

      if (nameOrShareId) {
        queryString.append('name', nameOrShareId);
      }

      if (platforms.length > 0) {
        platforms.forEach((platform) => {
          queryString.append('platforms', platform.toString());
        });
      }

      if (tags.length > 0) {
        tags.forEach((tag) => {
          queryString.append('tags', tag);
        });
      }

      const finalUrl = url + '?' + queryString.toString();
      const results = await axios.get<IEmblemSearchResult>(finalUrl);
      if (totalEmblems.value === 0) {
        totalEmblems.value = results.data.totalEmblems;
      }
      emblems.value.push(...results.data.emblems);
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
      emblems.value.push(addedEmblem.data);
      toast.add({
        severity: 'success',
        detail: 'Emblem created successfully.',
        summary: 'Created',
        life: 2000
      });
      const containsNewTags = tags.value.filter((x) => emblem.tags.indexOf(x) < 0).length > 0;
      if (containsNewTags || tags.value.length === 0) {
        await getAllTags();
      }
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

  async function getAllTags(): Promise<void> {
    try {
      const allTags = await axios.get('api/emblem/tags');
      tags.value = allTags.data;
    } catch (error) {
      console.error(error);
      toast.add({
        severity: 'error',
        detail: 'Error getting emblems.',
        summary: 'Error',
        life: 2000
      });
    }
  }

  function resetEmblems() {
    emblems.value = [];
  }

  return {
    emblems,
    currentPageNumber,
    getEmblems,
    addEmblem,
    isLoading,
    isAddLoading,
    getFilteredEmblems,
    resetEmblems,
    getAllTags,
    hasInFlightRequest,
    tags
  };
});
