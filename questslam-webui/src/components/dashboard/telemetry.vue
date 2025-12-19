<script setup>
    import { ref, reactive, onMounted } from 'vue'

    const telem = reactive({
        rosConnectionIP: "127.0.0.1",
        rosTime: 0.0,
        cpu: 0,
        gpu: 0,
        mem: 0,
        temp: 0.0,
        isTracking: false,
        trackingspeed: 120,
        fps: 72,
        pose: {x: 0, y: 0, z: 0},
        rot: {x: 0, y: 0, z: 0}
    })

</script>

<template>
    <div class="p-5 bg-[#282828] rounded-t-2xl overflow-hidden" :style="{ width: '100%', maxWidth: '100%' }">
        <h1 class="text-white text-2xl">Telemetry</h1>

        <span class="flex items-center mt-2 mb-2">
            <span class="h-px flex-1 bg-linear-to-r from-transparent to-gray-300 dark:to-gray-600"></span>
            <span class="shrink-0 px-4 text-gray-900 dark:text-white">ROS</span>
            <span class="h-px flex-1 bg-linear-to-l from-transparent to-gray-300 dark:to-gray-600"></span>
        </span>

        <div class="p-3 border border-[#8c52ff] rounded-lg">
            <inforow label="ROS IP" :value="telem.rosConnectionIP"/>
            <inforow label="ROS Time" :value="telem.rosTime"/>
        </div>

        <span class="flex items-center mt-2 mb-2">
            <span class="h-px flex-1 bg-linear-to-r from-transparent to-gray-300 dark:to-gray-600"></span>
            <span class="shrink-0 px-4 text-gray-900 dark:text-white">System</span>
            <span class="h-px flex-1 bg-linear-to-l from-transparent to-gray-300 dark:to-gray-600"></span>
        </span>

        <div class="p-3 border border-[#8c52ff] rounded-lg">
            <inforow unit="%" label="CPU" :value="telem.cpu"/>
            <inforow unit="%" label="GPU" :value="telem.gpu"/>
            <inforow unit="/8192" label="Memory" :value="telem.mem"/>
            <inforow unit="°C" label="CPU Temprature" :value="telem.temp"/>
        </div>

        <span class="flex items-center mt-2 mb-2">
            <span class="h-px flex-1 bg-linear-to-r from-transparent to-gray-300 dark:to-gray-600"></span>
            <span class="shrink-0 px-4 text-gray-900 dark:text-white">QuestSLAM</span>
            <span class="h-px flex-1 bg-linear-to-l from-transparent to-gray-300 dark:to-gray-600"></span>
        </span>

        <div class="p-3 border border-[#8c52ff] rounded-lg">
            <inforow unit="fps" label="FPS" :value="telem.fps"/>
            <inforow unit="" label="Is Tracking?" :value="telem.isTracking"/>
            <inforow unit="hz" label="Tracking Speed" :value="telem.trackingspeed"/>
            <inforow unit="°C" label="CPU Temprature" :value="telem.temp"/>
        </div>
        

    </div>
</template>