<script setup lang="ts">
    const props = defineProps<{
        attempts: number,
        maxattempts: number
    }>()

</script>

<template>
    <div class="flex justify-center items-center h-screen ">
        <div class=" bg-[#282828] p-8 w-80 rounded-2xl border border-[#9c66ff]">
            <div class="flex flex-col justify-center items-center">
                <h1 class="text-white text-2xl mb-4 ">Connecting</h1>
                <div class="flex gap-2">
                    <span class="size-3 animate-bounce rounded-full bg-[#9c66ff] "></span>
                    <span class="size-3 animate-bounce rounded-full bg-[#9c66ff] [animation-delay:0.2s] "></span>
                    <span class="size-3 animate-bounce rounded-full bg-[#9c66ff] [animation-delay:0.4s] "></span>
                </div>
            </div>
            <div class="flex justify-center items-center mt-2">
                <p class="text-white text-xs">WebSocket connect attempts: {{ attempts }}/{{ maxattempts }}</p>
            </div>
        </div>
    </div>
</template>