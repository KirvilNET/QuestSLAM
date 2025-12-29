<script setup lang="ts">
    //import { ref } from 'vue'
    import type { AppInfo } from '../QuestSLAM/schema';
    
    const props = defineProps<{
        AppInfo: AppInfo
    }>();
</script>

<template>
     <div class="p-4">
        <div class="p-2">
            <div class="w-100% flex justify-center items-center">
                <h1 class="text-white text-2xl">App Information</h1>
            </div>

            <div class="m-2 h-px bg-[#8c52ff]"></div>

            <div class="flex justify-between">
                <p class="text-white text-xl">App Version: </p>
                <p class="text-white text-xl">{{ props.AppInfo.AppVersion }}</p>
            </div>
            <div class="flex justify-between">
                <p class="text-white text-xl">Build Date: </p>
                <p class="text-white text-xl">{{ props.AppInfo.BuildDate }}</p>
            </div>
            <div class="flex justify-between">
                <p class="text-white text-xl">Horision OS Version: </p>
                <p class="text-white text-xl">{{ props.AppInfo.HorisionOSVersion }}</p>
            </div>
            <div class="flex justify-between">
                <p class="text-white text-xl">Unity Version: </p>
                <p class="text-white text-xl">{{ props.AppInfo.UnityVersion }}</p>
            </div>
            <div class="flex justify-between">
                <p class="text-white text-xl">Device Model: </p>
                <p class="text-white text-xl">{{ props.AppInfo.DeviceModel }}</p>
            </div>      

            <div class="w-100% flex justify-center items-center">

            </div>
        </div>
     </div>
</template>