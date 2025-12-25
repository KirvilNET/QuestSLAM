<script setup lang="ts">
    import { toggleCamera, AprilTagTracking, save, resetToDefault } from '../QuestSLAM/settings.ts'
    import type { Config } from '../QuestSLAM/schema.ts'

    

    const props = defineProps<{
        currentConfig: Config;
    }>();
</script>

<template>
    <div class="p-4">
        <div class="p-2">
            <h1 class="text-white text-2xl">Camera Options</h1>
            
            <div class="mt-2">
                <div class="flex flex-col gap-2">
                    <div class="flex gap-2">
                        <input v-model="props.currentConfig.toggleCamera" type="checkbox" id="toggleCamera" class="">
                        <label for="toggleCamera" class="text-white">Enable Cameras</label>
                    </div>

                    <div class="flex justify-between">
                        <label for="tagFamily" class="block mb-2.5 text-s font-medium text-white">Camera Eye</label>
                        <select id="tagFamily" class="inline-block px-3 py-2.5 bg-[#3f3f3f] text-sm rounded-xl focus:ring-brand focus:border-brand shadow-xs placeholder:text-body text-white">
                          <option selected value="right">Right</option>
                          <option value="left">Left</option>
                        </select>
                    </div>

                    <div class="m-2 h-px bg-[#8c52ff]"></div>

                    <h2 class="text-white text-xl">AprilTags</h2>
                    <div class="flex gap-2">
                        <input v-model="AprilTagTracking" type="checkbox" id="AprilTagTracking" class="">
                        <label for="AprilTagTracking" class="text-white">AprilTag Tracking</label>
                    </div>
                    <div class="flex justify-between">
                        <label for="tagFamily" class="block mb-2.5 text-s font-medium text-white">Tag Family</label>
                        <select id="tagFamily" class="inline-block px-3 py-2.5 bg-[#3f3f3f] text-sm rounded-xl focus:ring-brand focus:border-brand shadow-xs placeholder:text-body text-white">
                          <option selected value="36h11">36h11</option>
                          <option value="16h5">16h5</option>
                          <option value="25h9">25h9</option>
                          <option value="Circle21h7">Circle21h7</option>
                          <option value="Circle49h12">Circle49h12</option>
                          <option value="Custom52h12">Custom52h12</option>
                          <option value="Standard41h12">Standard41h12</option>
                          <option value="Standard52h13">Standard52h13</option>
                        </select>
                    </div>

                    <div class="m-2 h-px bg-[#8c52ff]"></div>
                </div>
            </div>
            <div class="flex gap-2 mt-4">
                <button class="px-4 py-2 text-white bg-[#8c52ff] hover:bg-[#ab79ff] rounded-lg font-semibold transition-colors" @click="save">Save</button>
                <button class="px-4 py-2 text-white bg-[#ff0000] hover:bg-[#ff6666] rounded-lg font-semibold transition-colors" @click="resetToDefault">Reset to Default</button>
            </div>
        </div>
    </div>
</template>