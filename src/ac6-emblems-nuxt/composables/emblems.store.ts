import { defineStore } from 'pinia';
import type { IEmblemSearchResult } from '~/types/emblems/emblem-search-result.interface';
import type { Emblem } from '~/types/emblems/emblem.model';
import type { PlatformType } from '~/types/platform-type.enum';

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
    await executeGetTags();
    if (emblemTags) {
      emblemTags.value!.forEach((element) => {
        tags.value.push(element);
      });
    }
  };

  const resetEmblems = () => {
    emblems.value = [];
  };

  const fetchFilteredEmblems = async (platforms: PlatformType[], searchQuery: string, tags: string[]) => {
    const url = `/api/emblem/search/${currentPageNumber.value}/${totalPerPage.value}`;
    const queryString = new URLSearchParams();
    if (searchQuery) {
      queryString.append('name', searchQuery);
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
    const filteredResults = await $fetch<IEmblemSearchResult>(finalUrl, {
      method: 'GET',
    });
    if (totalEmblems.value === 0) {
      totalEmblems.value = filteredResults.totalEmblems;
    }
    emblems.value.push(...filteredResults.emblems);
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
    fetchFilteredEmblems,
  };
});
