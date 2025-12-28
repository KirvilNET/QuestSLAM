<script setup>
  import { ref, onMounted, watch } from 'vue'
  import { fetchConfig, currentConfig, Info } from '../components/QuestSLAM/settings.ts';

  const activeTab = ref('general')

  const tabs = [
    { id: 'general', label: 'General' },
    { id: 'camera', label: 'Cameras' },
    { id: 'plugins', label: 'Plugins' },
    { id: 'about', label: 'About' },
  ]

</script>

<template>
  <div class="flex justify-center items-center h-screen w-full">
    <div class="h-90% w-200 rounded-2xl bg-[#282828]">
      <template v-if="loading">
        <div class="flex flex-col justify-center items-center">
          <h1 class="text-white text-2xl mb-4 ">Loading</h1>
          <div class="flex gap-2">
            <span class="size-3 animate-bounce rounded-full bg-[#9c66ff]"></span>
            <span class="size-3 animate-bounce rounded-full bg-[#9c66ff] [animation-delay:0.2s]"></span>
            <span class="size-3 animate-bounce rounded-full bg-[#9c66ff] [animation-delay:0.4s]"></span>
          </div>
        </div>
      </template>
      <template v-else>
        <div class="-mb-px border-b border-[#8c52ff] flex justify-between">
          <div role="tablist" class="flex gap-1">
            <button v-for="tab in tabs" :key="tab.id" @click="activeTab = tab.id" :class="{ active: activeTab === tab.id }" class="border-b-2 border-transparent hover:border-[#ab79ff] px-4 py-2 text-sm font-medium text-[#8c52ff] transition-colors hover:text-[#ab79ff]">
              {{ tab.label }}
            </button>
          </div>
          <div class="flex mr-4">
            <button @click="fetchConfig">
              <svg class="fill-white" xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 512 512"><path d="M436.7 74.7L448 85.4 448 32c0-17.7 14.3-32 32-32s32 14.3 32 32l0 128c0 17.7-14.3 32-32 32l-128 0c-17.7 0-32-14.3-32-32s14.3-32 32-32l47.9 0-7.6-7.2c-.2-.2-.4-.4-.6-.6-75-75-196.5-75-271.5 0s-75 196.5 0 271.5 196.5 75 271.5 0c8.2-8.2 15.5-16.9 21.9-26.1 10.1-14.5 30.1-18 44.6-7.9s18 30.1 7.9 44.6c-8.5 12.2-18.2 23.8-29.1 34.7-100 100-262.1 100-362 0S-25 175 75 75c99.9-99.9 261.7-100 361.7-.3z"/></svg>
            </button>
          </div>
        </div>
        <div role="tabpanel" class="mt-4 h-100%">
          <div v-if="activeTab === 'general'">
            <!-- General settings -->
            <Settings-General :currentConfig="currentConfig"/>
          </div>
          <div v-if="activeTab === 'camera'">
            <!-- Camera settings -->
            <Settings-Camera :currentConfig="currentConfig"/>
          </div>
          <div v-if="activeTab === 'plugins'">
            <!-- Plugins -->
            <Settings-Plugins :currentConfig="currentConfig"/>
          </div>
          <div v-if="activeTab === 'about'">
            <!-- About -->
            <Settings-About 
              :AppInfo="Info"
            />
          </div>
          <div v-if="activeTab === 'developer'">
            <!-- About -->
            <Settings-Developer :currentConfig="currentConfig"/>
          </div>
        </div>
      </template>
      
    </div>
  </div>
</template>
