<script setup lang="ts">
    import { ref, computed, watch } from 'vue'
    import type { LogEntry } from '../QuestSLAM/schema.ts'
    import { logs, searchQuery } from './logs.ts'

    const logContainer = ref<HTMLDivElement>();

    const props = defineProps<{
        logs: LogEntry[]
        showTimeRecived: boolean;
        autoScroll: boolean;
        filterLevel: string[]
    }>();

    const filteredLogs = computed(() => {
      return logs.value.filter(log => {
        const levelMatch = props.filterLevel.includes('all') || props.filterLevel.includes(log.level)
        const searchMatch = log.message.toLowerCase().includes(searchQuery.value.toLowerCase())
        return levelMatch && searchMatch
      })
    })

    watch(
        () => props.logs,
        (x) => {
          if (props.autoScroll) {
            if (logContainer != null) logContainer.value!.scrollTop = logContainer.value!.scrollHeight
          }
        }
    )

</script>

<template>
    <div class="h-full rounded-2xl flex-1 overflow-y-auto p-4 font-mono text-sm [&::-webkit-scrollbar]:w-2">
        <template v-if="logs.length == 0">
            <div class="flex justify-center items-center h-full">
                <p class="text-xl text-white">No logs yet...</p>
            </div>
        </template>
        <template v-else>
            <div ref="logContainer" class="flex flex-col gap-2">
                <template v-for="(entry, id) in filteredLogs">
                    <log :entry="entry" :showTimeRecived="showTimeRecived" :id="id.toString()"/>
                </template>         
            </div>
        </template>
    </div>
</template>