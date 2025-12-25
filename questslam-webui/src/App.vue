<script setup lang="ts">
    import { isConnected, telemetry, connect, maxReconnectAttempts, reconnectAttempts, resetTelemetry } from './components/QuestSLAM/websocket.ts'
    import { ref, onMounted } from 'vue'

    onMounted(() => {
        resetTelemetry()
        connect()
    })
    
</script>

<template>
    <LoadingPage v-if="!isConnected" :attempts="reconnectAttempts" :maxattempts="maxReconnectAttempts"/>
    <template v-else>
        <navbar :connectionStatus="telemetry.connectionStatus || false" :batteryPercentage="telemetry.batteryPercentage || 0"/>
        <router-view 
            :connectionStatus = "telemetry.connectionStatus || false"
            :batteryPercentage = "telemetry.batteryPercentage || 0"
            :headsetID = "telemetry.headsetID || 0"
            :rosConnectionIP = "telemetry.rosConnectionIP || '127.0.0.1'"
            :rosTime = "telemetry.rosTime || 0"
            :cpu = "telemetry.cpu || 0"
            :mem = "telemetry.mem || 0"
            :temp = "telemetry.temp || 0"
            :isTracking = "telemetry.isTracking || false"
            :trackingspeed = "telemetry.trackingspeed || 0"
            :fps = "telemetry.fps || 0"
            :pose = "telemetry.pose || {
                pos: {x: 0, y: 0, z: 0},
                rot: {x: 0, y: 0, z: 0, w: 0}
            }"
        />
    </template>
</template>
