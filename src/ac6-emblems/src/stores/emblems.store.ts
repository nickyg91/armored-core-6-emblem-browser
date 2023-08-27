import type { IEmblemSearchResult } from '@/models/emblem-search-result.interface';
import type { Emblem } from '@/models/emblem.model';
import axios from 'axios';
import { defineStore } from 'pinia';
import { reactive, ref } from 'vue';

export const useEmblemStore = defineStore('emblemStore', () => {
  const emblems = reactive<Array<Emblem>>([]);
  const currentPageNumber = ref(1);
  const totalPerPage = ref(25);
  const totalEmblems = ref(0);

  async function getEmblems(): Promise<void> {
    const results = await axios.get<IEmblemSearchResult>(
      `api/emblem/search/${currentPageNumber.value}/${totalPerPage.value}`
    );
    if (totalEmblems.value === 0) {
      totalEmblems.value = results.data.totalEmblems;
    }
    emblems.push(...results.data.emblems);
  }

  return {
    emblems,
    currentPageNumber,
    getEmblems
  };
});
