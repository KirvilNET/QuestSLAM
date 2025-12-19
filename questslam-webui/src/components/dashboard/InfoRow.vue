<script setup lang="ts">
    defineProps<{
      label: string
      unit: string
      value: string | number
    }>()
</script>

<template>
    <div class="flex justify-between items-center p-1">
        <span class="text-xs text-slate-300">{{ label }}:</span>
        <span class="text-m text-white">{{ value }}{{ unit }}</span>
    </div>
</template>