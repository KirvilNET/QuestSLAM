<script setup lang="ts">
    import { ref } from 'vue'


    

    const props = defineProps<{
        appVersion: string
        buildDate: string
        HorizonOSVersion: string
    }>();
</script>

<template>
     <div class="p-4">
        <div class="p-2">
            <div class="w-100% flex justify-center items-center">
                <h1 class="text-white text-2xl">App Information</h1>
            </div>

            <div class="m-2 h-px bg-[#8c52ff]"></div>

            <p class="text-white text-xl">App Version: {{ appVersion }}</p>
            <p class="text-white text-xl">Build Data: {{ appVersion }}</p>
            <p class="text-white text-xl">Horision OS Version: {{ appVersion }}</p>

            <div class="w-100% flex justify-center items-center">

            </div>
        </div>
     </div>
</template>