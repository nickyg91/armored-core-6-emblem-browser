import { defineStore } from 'pinia';
import type { IEmblemSearchResult } from '~/types/emblems/emblem-search-result.interface';
import type { Emblem } from '~/types/emblems/emblem.model';

export const useEmblemsStore = defineStore('emblems', () => {
  const emblems = ref<Array<Emblem>>([]);
  const currentPageNumber = ref(1);
  const totalPerPage = ref(25);
  const totalEmblems = ref(0);
  const tags = ref<Array<string>>([]);
  const {
    data: emblemResultSet,
    pending,
    error,
    execute,
  } = useFetch<IEmblemSearchResult>(`/api/emblem/search/${currentPageNumber.value}/${totalPerPage.value}`, {
    immediate: false,
    retry: 0,
  });

  const {
    data: emblemTags,
    pending: pendingTags,
    error: tagsError,
    execute: executeGetTags,
  } = useFetch<string[]>('/api/emblem/tags', {
    immediate: false,
  });

  const createEmblem = async (emblem: Emblem) => {
    const createdEmblem = await $fetch<Emblem>('/api/emblem/create', {
      method: 'POST',
      body: emblem,
    });
    if (createdEmblem) {
      emblems.value.push(createdEmblem);
    }
  };

  const fetchEmblems = async () => {
    await execute();
    if (emblemResultSet.value) {
      emblemResultSet.value.emblems.forEach((element) => {
        emblems.value.push(element);
      });
      totalEmblems.value = emblemResultSet.value.totalEmblems;
    }
  };

  const fetchTags = async () => {
    const result = await executeGetTags();
    if (result) {
      result.forEach((element) => {
        tags.value.push(element);
      });
    }
  };

  const resetEmblems = () => {
    emblems.value = [];
  };

  return {
    emblems,
    currentPageNumber,
    totalPerPage,
    totalEmblems,
    tags,
    pending,
    error,
    emblemTags,
    pendingTags,
    tagsError,
    fetchEmblems,
    fetchTags,
    resetEmblems,
    createEmblem,
  };
});
