<script setup>

</script>

<template>
    <div class="p-3 bg-[#282828] rounded-b-2xl border-t-4 border-[#8c52ff]">
        <span class="flex items-center mt-2 mb-2">
            <span class="h-px flex-1 bg-linear-to-r from-transparent to-gray-300 dark:to-gray-600"></span>
            <span class="shrink-0 px-4 text-gray-900 dark:text-white">Position</span>
            <span class="h-px flex-1 bg-linear-to-l from-transparent to-gray-300 dark:to-gray-600"></span>
        </span>
        <div class="gap-2 flex justify-evenly mt-2">
            <button class="px-4 py-2 text-white bg-[#8c52ff] hover:bg-[#ab79ff] rounded-lg font-semibold transition-colors">Reset Origin</button>
            <button class="px-4 py-2 text-white bg-[#8c52ff] hover:bg-[#ab79ff] rounded-lg font-semibold transition-colors">Recenter</button>
            <button class="px-4 py-2 text-white bg-[#8c52ff] hover:bg-[#ab79ff] rounded-lg font-semibold transition-colors">Freeze Position</button>
        </div>
        <!--
        <span class="flex items-center mt-2 mb-2">
            <span class="h-px flex-1 bg-linear-to-r from-transparent to-gray-300 dark:to-gray-600"></span>
            <span class="shrink-0 px-4 text-gray-900 dark:text-white">Camera</span>
            <span class="h-px flex-1 bg-linear-to-l from-transparent to-gray-300 dark:to-gray-600"></span>
        </span>
        
        <div class="gap-2 flex justify-evenly mt-2">
            <button class="px-4 py-2 text-white bg-[#8c52ff] hover:bg-[#ab79ff] rounded-lg font-semibold transition-colors">Calibrate Camera</button>
            <button class="px-4 py-2 text-white bg-[#8c52ff] hover:bg-[#ab79ff] rounded-lg font-semibold transition-colors">Stop Camera Stream</button>
            <button class="px-4 py-2 text-white bg-[#8c52ff] hover:bg-[#ab79ff] rounded-lg font-semibold transition-colors">Restart Camera Stream</button>
        </div>
        -->
    </div>
</template>