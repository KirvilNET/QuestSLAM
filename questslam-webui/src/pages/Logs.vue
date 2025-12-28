<script setup>
  import { ref, onMounted, useTemplateRef, watch } from 'vue'
  import { logs, NumberofErrorLogs, NumberofInfoLogs, NumberofWarningLogs, searchQuery, getTime, newLogEntry, clearLogs, downloadLogs } from '../components/logging/logs.ts'

  let ErrorButtonValue = ref(true);
  let WarningButtonValue = ref(true);
  let InfoButtonValue = ref(true);
  let WebButtonValue = ref(true);

  
  let showTimeRecived = ref(true)
  const autoScroll = ref(true);
  const filterLevel = ref(['all'])

  function Errors() {
    ErrorButtonValue.value = !ErrorButtonValue.value
  }

  function Warning() {
    WarningButtonValue.value = !WarningButtonValue.value
  }

  function Info() {
    InfoButtonValue.value = !InfoButtonValue.value
  }

  function ToggleAutoScroll() {
    autoScroll.value = !autoScroll.value
  }

  watch([ErrorButtonValue, WarningButtonValue, InfoButtonValue, WebButtonValue], ([error, warning, info, web]) => {
    filterLevel.value = []

    if (error) filterLevel.value.push('error')
    if (warning) filterLevel.value.push('warning')
    if (info) filterLevel.value.push('info')
    if (web) filterLevel.value.push('web')
    if (error && warning && info && web) filterLevel.value.push('all')
  })

  console.log(logs.value)
</script>

<template>
   <div class="flex flex-col h-full">
     <div class="h-12 rounded-2xl bg-[#282828] flex items-center mt-5 ml-5 mr-5">
        <h1 class="text-white text-2xl p-4">Logs</h1>
        <div class="flex gap-2 items-center mr-10 w-full">
          <input type="text" v-model="searchQuery" placeholder="Search logs..."  class="w-full font-medium border-b border-[#ab79ff] focus:border-[#8c52ff] focus:bg-[#3f3f3f] hover:bg-[#3f3f3f] p-2 h-10 text-white outline-none ">

          <div class="mr-2 ml-2 h-8 w-px bg-[#8c52ff] self-center"></div>

          <div class="items-center inline-flex shrink-0">
            <button @click="Errors" :class="{'bg-[#ab79ff]': ErrorButtonValue, 'border-[#8c52ff]': ErrorButtonValue, 'hover:bg-[#ab79ff]': ErrorButtonValue}" class="border-b border-[#ab79ff] shrink-0 px-2 py-2 h-10 font-medium text-white transition-colors hover:bg-[#3f3f3f] outline-none">
              Errors <span v-if="NumberofErrorLogs > 0"> ({{ NumberofErrorLogs }})</span>
            </button>
          
            <button @click="Warning" :class="{'bg-[#ab79ff]': WarningButtonValue, 'border-[#8c52ff]': WarningButtonValue, 'hover:bg-[#ab79ff]': WarningButtonValue}" class="-ml-px border-b border-[#ab79ff] shrink-0 px-2 py-2 h-10 font-medium text-white transition-colors hover:bg-[#3f3f3f] outline-none">
              Warning <span v-if="NumberofWarningLogs > 0"> ({{ NumberofWarningLogs }})</span>
            </button>
          
            <button @click="Info" :class="{'bg-[#ab79ff]': InfoButtonValue, 'border-[#8c52ff]': InfoButtonValue, 'hover:bg-[#ab79ff]': InfoButtonValue}" class="-ml-px border-b border-[#ab79ff] shrink-0 px-2 py-2 h-10 font-medium text-white transition-colors hover:bg-[#3f3f3f] outline-none">
              Info <span v-if="NumberofInfoLogs > 0"> ({{ NumberofInfoLogs }})</span>
            </button>

            <button @click="ToggleAutoScroll" :class="{'bg-[#ab79ff]': autoScroll, 'border-[#8c52ff]': autoScroll, 'hover:bg-[#ab79ff]': autoScroll}" class="w-25 border-b border-[#ab79ff] shrink-0 px-2 py-2 h-10 font-medium text-white transition-colors hover:bg-[#3f3f3f] outline-none">
              Auto Scroll
            </button>
          </div>

          <div class="mr-2 ml-2 h-8 w-px bg-[#8c52ff] self-center"></div>

          <div class="inline-flex">
            <button @click="downloadLogs" class="-ml-px px-3 py-2 h-10 font-medium fill-white transition-colors hover:bg-[#3f3f3f] outline-none">
              <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 384 512"><path d="M0 64C0 28.7 28.7 0 64 0L213.5 0c17 0 33.3 6.7 45.3 18.7L365.3 125.3c12 12 18.7 28.3 18.7 45.3L384 448c0 35.3-28.7 64-64 64L64 512c-35.3 0-64-28.7-64-64L0 64zm208-5.5l0 93.5c0 13.3 10.7 24 24 24L325.5 176 208 58.5zM175 441c9.4 9.4 24.6 9.4 33.9 0l64-64c9.4-9.4 9.4-24.6 0-33.9s-24.6-9.4-33.9 0l-23 23 0-86.1c0-13.3-10.7-24-24-24s-24 10.7-24 24l0 86.1-23-23c-9.4-9.4-24.6-9.4-33.9 0s-9.4 24.6 0 33.9l64 64z"/></svg>
            </button>
          
            <button @click="clearLogs" class="-ml-px px-3 py-2 h-10 font-medium text-[#ff0000] transition-colors hover:bg-[#3f3f3f] outline-none">
              <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="size-5">
                <path stroke-linecap="round" stroke-linejoin="round" d="m14.74 9-.346 9m-4.788 0L9.26 9m9.968-3.21c.342.052.682.107 1.022.166m-1.022-.165L18.16 19.673a2.25 2.25 0 0 1-2.244 2.077H8.084a2.25 2.25 0 0 1-2.244-2.077L4.772 5.79m14.456 0a48.108 48.108 0 0 0-3.478-.397m-12 .562c.34-.059.68-.114 1.022-.165m0 0a48.11 48.11 0 0 1 3.478-.397m7.5 0v-.916c0-1.18-.91-2.164-2.09-2.201a51.964 51.964 0 0 0-3.32 0c-1.18.037-2.09 1.022-2.09 2.201v.916m7.5 0a48.667 48.667 0 0 0-7.5 0"></path>
              </svg>
            </button>
          </div>
        </div>
     </div>
     <div class="h-full rounded-2xl overflow-hidden bg-[#282828] mt-5 ml-5 mr-5">
        <logViewer 
          :logs="logs"
          :showTimeRecived="showTimeRecived"
          :autoScroll="autoScroll"
          :filterLevel="filterLevel"
        />
     </div>
   </div> 
</template>