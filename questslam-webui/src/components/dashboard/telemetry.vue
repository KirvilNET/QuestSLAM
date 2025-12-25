<script setup lang="ts">

    const props = defineProps<{
        connectionStatus: boolean,
        batteryPercentage: number,
        headsetID: number,
        rosConnectionIP: string,
        rosTime: number,
        cpu: number,
        mem: number,
        temp: number,
        isTracking: boolean,
        trackingspeed: number,
        fps: number,
        pose: {
            pos: {x: number; y: number; z: number}
            rot: {x: number; y: number; z: number}
        }
    }>()

</script>

<template>
    <div class="p-5 bg-[#282828] rounded-t-2xl overflow-hidden" :style="{ width: '100%', maxWidth: '100%' }">
        <h1 class="text-white text-2xl">Telemetry</h1>

        <span class="flex items-center mt-2 mb-2">
            <span class="h-px flex-1 bg-linear-to-r from-transparent to-gray-300 "></span>
            <span class="shrink-0 px-4 text-gray-900 dark:text-white">ROS</span>
            <span class="h-px flex-1 bg-linear-to-l from-transparent to-gray-300 "></span>
        </span>

        <div class="p-3 border border-[#8c52ff] rounded-lg">
            <inforow label="ROS IP" unit="" :value="props.rosConnectionIP"/>
            <inforow label="ROS Time" unit="" :value="props.rosTime"/>
            <inforow label="Headset ID" unit="" :value="props.headsetID"/>
        </div>

        <span class="flex items-center mt-2 mb-2">
            <span class="h-px flex-1 bg-linear-to-r from-transparent to-gray-300 "></span>
            <span class="shrink-0 px-4 text-gray-900 dark:text-white">System</span>
            <span class="h-px flex-1 bg-linear-to-l from-transparent to-gray-300 "></span>
        </span>

        <div class="p-3 border border-[#8c52ff] rounded-lg">
            <inforow unit="%" label="CPU" :value="props.cpu"/>
            <inforow unit="/8192" label="Memory" :value="props.mem"/>
            <inforow unit="°C" label="CPU Temprature" :value="props.temp"/>
        </div>

        <span class="flex items-center mt-2 mb-2">
            <span class="h-px flex-1 bg-linear-to-r from-transparent to-gray-300 "></span>
            <span class="shrink-0 px-4 text-gray-900 dark:text-white">QuestSLAM</span>
            <span class="h-px flex-1 bg-linear-to-l from-transparent to-gray-300 "></span>
        </span>

        <div class="p-3 border border-[#8c52ff] rounded-lg">
            <inforow unit="fps" label="FPS" :value="props.fps"/>
            <inforow unit="" label="Is Tracking" :value="props.isTracking"/>
            <inforow unit="hz" label="Tracking Speed" :value="props.trackingspeed"/>
        </div>
        

    </div>
</template>