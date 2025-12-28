<script setup lang="ts">
  import '@preline/dropdown'
  import '@floating-ui/dom'
  import Splitter from 'primevue/splitter';
  import SplitterPanel from 'primevue/splitterpanel';

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
      pos: {x: number; y: number; z: number},
      rot: {x: number; y: number; z: number; w: number}
    }

  }>()

</script>

<template>
  <div class="h-full grid grid-cols-1 gap-2 lg:grid-cols-3 lg:gap-4 mt-5 ml-5 mr-5">
    <div class="h-full rounded-2xl bg-[#282828]">
      <Splitter class="h-px bg-white" style="height: 100%" layout="vertical">
        <SplitterPanel :size="75" :minSize="50"> 
          <telemetry
            :connectionStatus = "props.connectionStatus"
            :batteryPercentage = "props.batteryPercentage"
            :headsetID = "props.headsetID"
            :rosConnectionIP = "props.rosConnectionIP"
            :rosTime = "props.rosTime"
            :cpu = "props.cpu"
            :mem = "props.mem"
            :temp = "props.temp"
            :isTracking = "props.isTracking"
            :trackingspeed = "props.trackingspeed"
            :fps = "props.fps"
            :pose = "props.pose"
          /> 
        </SplitterPanel>
        <SplitterPanel :size="25" :minSize="3"> 
          <quickactions /> 
        </SplitterPanel>
      </Splitter>
    </div>
    <div ref="3dviewer" class="h-full rounded-2xl bg-[#282828] lg:col-span-2">
      <viz :pose="props.pose"/>
    </div>
  </div>
</template>