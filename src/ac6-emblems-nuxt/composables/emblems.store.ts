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
    data: emblemResults,
    pending,
    error,
    execute,
  } = useFetch<IEmblemSearchResult>(`/api/emblem/search/${currentPageNumber.value}/${totalPerPage.value}`);

  const {
    // data: emblemTags,
    // pending: pendingTags,
    // error: tagsError,
    execute: executeGetTags,
  } = useFetch<string[]>('/api/emblem/tags');

  const fetchEmblems = async () => {
    const result = await execute();
    if (result) {
      result.emblems.forEach((element) => {
        emblems.value.push(element);
      });
      totalEmblems.value = result.totalEmblems;
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
    emblemResults,
    pending,
    error,
    fetchEmblems,
    fetchTags,
    resetEmblems,
  };
});
