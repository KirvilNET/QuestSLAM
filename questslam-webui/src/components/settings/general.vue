<script setup lang="ts">
    import { ref } from 'vue'
    import { save, resetToDefault, ExportConfig, ImportConfig } from '../QuestSLAM/settings';
    import type { Config } from '../QuestSLAM/schema.ts'

    const props = defineProps<{
        currentConfig: Config;
        serverConfig: Config;
    }>();

</script>

<template>
    <div class="p-4">
        <div class="p-2">
            <h1 class="text-white text-2xl">Dashboard Options</h1>
            <span class="mt-2 mb-2 h-2 bg-white w-full"></span>
            <div class="mt-2">
                <div class="flex flex-col gap-2">
                    <label for="headsetID" class="text-white">Headset ID</label>
                    <input v-model="props.currentConfig.headsetID" :placeholder="props.serverConfig.headsetID.toString()" type="text" id="headsetID" class="border-b border-[#8c52ff] text-white">

                    <label for="rosConnectionIP" class="text-white">ROS Connection IP</label>
                    <input v-model="props.currentConfig.rosConnectionIP" :placeholder="props.serverConfig.rosConnectionIP" type="text" id="rosConnectionIP" class="border-b border-[#8c52ff] text-white">

                    <div class="relative mb-6">
                        <label for="trackingSpeed" class="block mb-2.5 text-white">Tracking Speed: {{ props.currentConfig.trackingspeed }}hz</label>
                        <input v-model="props.currentConfig.trackingspeed" id="trackingSpeed" type="range" value="120" min="60" max="120" class="w-full h-4 bg-neutral-quaternary rounded-full cursor-pointer">
                        <span class="text-sm text-body absolute start-0 -bottom-6 text-white">60hz</span>
                        <span class="text-sm text-body absolute end-0 -bottom-6 text-white">120hz</span>
                    </div>
                    <div class="flex gap-2">
                        <input v-model="props.currentConfig.AutoStart" type="checkbox" id="autoStart" class="">
                        <label for="autoStart" class="text-white">Auto Start</label>
                    </div>
                </div>
            </div>
            <div class="flex gap-2 mt-4">
                <button class="px-4 py-2 text-white bg-[#8c52ff] hover:bg-[#ab79ff] rounded-lg font-semibold transition-colors" @click="save">Save</button>
                <button class="px-4 py-2 text-white bg-[#ff0000] hover:bg-[#ff6666] rounded-lg font-semibold transition-colors" @click="resetToDefault">Reset to Default</button>
            </div>
        </div>

        <div class="m-2 h-px bg-[#8c52ff]"></div>

        <div class="flex-col p-2">
            <h1 class="text-white text-2xl mb-4">Import/Export</h1>
            <div class="flex gap-2">
                <button class="px-4 py-2 text-white bg-[#8c52ff] hover:bg-[#ab79ff] rounded-lg font-semibold transition-colors" @click="ExportConfig">Export Config</button>
                <button class="px-4 py-2 text-white bg-[#8c52ff] hover:bg-[#ab79ff] rounded-lg font-semibold transition-colors" @click="ImportConfig">Import Config From File</button>
            </div>
            
        </div>
    </div>
</template>