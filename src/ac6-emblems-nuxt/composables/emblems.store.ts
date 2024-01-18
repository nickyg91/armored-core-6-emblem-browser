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
    const emblemResultSet = await $fetch<IEmblemSearchResult>(
      `/api/emblem/search/${currentPageNumber.value}/${totalPerPage.value}`,
      {
        method: 'GET',
      },
    );
    if (emblemResultSet) {
      emblemResultSet.emblems.forEach((element) => {
        emblems.value.push(element);
      });
      totalEmblems.value = emblemResultSet.totalEmblems;
    }
  };

  const fetchTags = async () => {
    const data = await $fetch<string[]>('/api/emblem/tags', {
      method: 'GET',
    });
    if (data) {
      data.forEach((element) => {
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
    fetchEmblems,
    fetchTags,
    resetEmblems,
    createEmblem,
    fetchFilteredEmblems,
  };
});
