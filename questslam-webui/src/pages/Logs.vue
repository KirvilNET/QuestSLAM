<script setup>
  import { ref, onMounted } from 'vue'
  import '@preline/dropdown'
  import '@floating-ui/dom'
  import Splitter from 'primevue/splitter';
  import SplitterPanel from 'primevue/splitterpanel';
</script>

<template>
   <div class="grid grid-rows-2">
     <div class="h-24 rounded-2xl bg-[#282828] flex mt-5 ml-5 mr-5">
        <h1 class="text-white text-2xl p-4">Logs</h1>
        <input id="search" type="text" v-model="searchQuery" placeholder="Search logs..." class="bg-[#121212] rounded-2xl p-2 h-8">
     </div>
     <div class="h-full rounded-2xl bg-[#282828] mt-5 ml-5 mr-5">
        
     </div>
   </div> 
</template>