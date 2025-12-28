<script setup lang="ts">
    import { isConnected, telemetry, connect, reconnect, WSmaxReconnectAttempts, WSreconnectAttempts, resetTelemetry } from './components/QuestSLAM/websocket.ts'
    import { fetchConfig, retryFetchConfig, fetchInfo, retryFetchInfo, hasAppInfo, hasConfig, APPreconnectAttempts, APPmaxReconnectAttempts, CONFIGreconnectAttempts, CONFIGmaxReconnectAttempts } from './components/QuestSLAM/settings.ts'
    import { ref, onMounted, watch } from 'vue'

    const isLoading = ref(true);

    onMounted(() => {
        resetTelemetry()
        connect()

        fetchConfig()
        fetchInfo()
    })

    function manualReconnect() {
        reconnect(true)
        retryFetchConfig(true)
        retryFetchInfo(true)
    }

    watch([isConnected, hasAppInfo, hasConfig], ([connected, appInfo, config]) => {
        if (connected && appInfo && config) {
          isLoading.value = false
        }
    })

</script>

<template>
    <template v-if="isLoading">
        <div class="h-screen">
            <LoadingPage  
                :WSattempts="WSreconnectAttempts" 
                :WSmaxattempts="WSmaxReconnectAttempts"
                :APPattempts="APPreconnectAttempts"
                :APPmaxattempts="APPmaxReconnectAttempts"
                :CONFIGattempts="CONFIGreconnectAttempts"
                :CONFIGmaxattempts="CONFIGmaxReconnectAttempts"
                :isLoading="isLoading"
                :isConnected="isConnected"
                :hasAppInfo="hasAppInfo" 
                :hasConfig="hasConfig"
                @manual-reconnect="manualReconnect"
            />
        </div>
    </template>
    <template v-else >
        <div class="flex-cols h-screen">
            <div class="h-16">
                <navbar :connectionStatus="telemetry.connectionStatus || false" :batteryPercentage="telemetry.batteryPercentage || 0"/>
            </div>
            <div class="h-[calc(100vh-64px-32px)]">
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
            </div>
        </div>
    </template>
</template>
