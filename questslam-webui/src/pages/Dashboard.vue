<script setup>
  import { ref, onMounted } from 'vue'
  import '@preline/dropdown'
  import '@floating-ui/dom'
  import Splitter from 'primevue/splitter';
  import SplitterPanel from 'primevue/splitterpanel';
</script>

<template>
  <div class="grid grid-cols-1 gap-2 lg:grid-cols-3 lg:gap-4 mt-5 ml-5 mr-5">
    <div class="h-100% rounded-2xl bg-[#282828]">
      <Splitter class="h-px bg-white" style="height: 100%" layout="vertical">
        <SplitterPanel :size="75" :minSize="50"> <telemetry /> </SplitterPanel>
        <SplitterPanel :size="25" :minSize="3"> <quickactions /> </SplitterPanel>
      </Splitter>
    </div>
    <div ref="3dviewer" class="h-100% rounded-2xl bg-[#282828] lg:col-span-2">
      <viz />
    </div>
  </div>
</template>