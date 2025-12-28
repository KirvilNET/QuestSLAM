<script setup lang="ts">
    import HSCollapse from '@preline/collapse'
    import { ref, onMounted, computed } from 'vue'
    import type { LogEntry } from '../QuestSLAM/schema.ts'

    const styleText = (level: LogEntry['level']) => {
        const colors: Record<LogEntry['level'], string> = {
            info: 'text-[#5c9eed]',
            warning: 'text-[#ffa200]',
            error: 'text-[#d70022]',
            web: 'text-[#5c9eed]'
        }
        return colors[level]
    }

    const styleMainContainer = (level: LogEntry['level']) => {
      const colors: Record<LogEntry['level'], string> = {
        info: 'bg-[#121212]',
        warning: 'bg-[#715100] border border-[#ffa200]',
        error: 'bg-[#570002] border border-[#ff0000]',
        web: 'bg-[#121212]'
      }
      return colors[level]
    }

    const props = defineProps<{
        entry: LogEntry;
        showTimeRecived: boolean;
        id: string;
    }>();

    onMounted(() => {
        HSCollapse.autoInit();
    })

</script>

<template>
    <div :class="['px-3 py-2 rounded flex gap-2 w-full h-100% transition-colors', styleMainContainer(entry.level)]">
        <div :class="['flex gap-2 justify-between w-48']">
            <div class="flex shrink-0">
                <span class="text-white shrink-0 w-20">{{ entry.timestamp }}</span>
            </div>
            <span :class="['shrink-0 w-16 font-semibold', styleText(entry.level)]">{{ entry.level.toUpperCase() }}</span>
        </div>
        <div class="w-full h-full">
            <div class="flex gap-2 w-full">
                <button v-if="entry.stackTrace != null" type="button" class="hs-collapse-toggle w-4 items-center text-sm font-medium text-white " id="hs-basic-collapse" aria-expanded="false" v-bind:aria-controls="'log'+id" v-bind:data-hs-collapse="'#'+'log'+id">
                    <svg class="rotate-270 hs-collapse-open:rotate-90 size-4" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                      <path d="m6 9 6 6 6-6"></path>
                    </svg>
                </button>
                <span class="flex-1 text-white">{{ entry.message }}</span>   
            </div> 
        </div>
        <div :id="'log'+id" class="hs-collapse hidden w-full overflow-auto transition-[height] duration-300" aria-labelledby="hs-basic-collapse">
            <div class="p-2">
                <p class="text-white">{{ entry.stackTrace }}</p>
            </div>
        </div>
    </div>
</template>