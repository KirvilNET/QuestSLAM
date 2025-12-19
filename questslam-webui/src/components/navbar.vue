<script setup>
  import { ref, onMounted } from 'vue'

  const status = ref(false)
  const statustext = ref("Waiting")
  const battery = ref(73)

  onMounted(() => {
    if (status.value === true) {
      document.getElementById("connection").classList.add('bg-[#66ffcc]')
      document.getElementById("connection").classList.add('text-[#000000]')
      statustext.value = "Connected"
    } else {
      document.getElementById("connection").classList.add('bg-[#ff0000]')
      document.getElementById("connection").classList.add('text-[#ffffff]')
      statustext.value = "Disconnected"
    }
  })
</script>

<template>
  <nav class="border-b border-[#8c52ff] bg-[#282828] px-4 py-1 flex items-center justify-between shadow gap-1">
    <div class="flex items-center gap-3 ml-2">
      <div>
        <p class="text-[#8c52ff] font-semibold leading-none">QuestSLAM</p>
        <p class="text-[#ffffff] font-semibold text-xs mt-1 leading-none">Dashboard</p>
      </div>
    </div>
    <div class="hs-dropdown relative">
      <button type="button" class="hs-dropdown-toggle md:hidden right-0 relative size-9 flex justify-center items-center font-medium text-sm rounded-lg border border-gray-200 text-gray-800 hover:bg-gray-100 focus:outline-hidden focus:bg-gray-100 disabled:opacity-50 disabled:pointer-events-none dark:text-white dark:border-neutral-700 dark:hover:bg-neutral-700 dark:focus:bg-neutral-700" id="hs-header-base-collapse"  aria-expanded="false" aria-controls="hs-header-base" aria-label="Toggle navigation"  data-hs-collapse="#hs-header-base" >
		    <svg class="hs-collapse-open:hidden size-4" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><line x1="3" x2="21" y1="6" y2="6"/><line x1="3" x2="21" y1="12" y2="12"/><line x1="3" x2="21" y1="18" y2="18"/></svg>
		    <svg class="hs-collapse-open:block shrink-0 hidden size-4" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M18 6 6 18"/><path d="m6 6 12 12"/></svg>
		    <span class="sr-only">Toggle navigation</span>
		  </button>

      <div id="navbar-content" class="hs-dropdown-menu transition-[opacity,margin] duration hs-dropdown-open:opacity-100 w-full hidden z-10 min-w-60 bg-[#282828] sm:block" aria-labelledby="hs-header-base-collapse" >
		  	<div class="overflow-hidden overflow-y-auto max-h-[75vh] [&::-webkit-scrollbar]:w-2 [&::-webkit-scrollbar-thumb]:rounded-full [&::-webkit-scrollbar-track]:bg-gray-100 [&::-webkit-scrollbar-thumb]:bg-gray-300 dark:[&::-webkit-scrollbar-track]:bg-neutral-700 dark:[&::-webkit-scrollbar-thumb]:bg-neutral-500">
		  		<div class="py-2 md:py-0 flex flex-col md:flex-row md:items-center gap-0.5 md:gap-1">
		  			<div class="flex flex-col md:flex-row md:justify-end md:items-center gap-0.5 md:gap-1">
              <router-link class="text-[#ffffff] text-sm px-3 py-1 rounded-lg transition" to="/">Dashboard</router-link>
              <router-link class="text-[#ffffff] text-sm px-3 py-1 rounded-lg transition" to="/camera">Cameras</router-link>
              <router-link class="text-[#ffffff] text-sm px-3 py-1 rounded-lg transition" to="/logs">Logs</router-link>
              <router-link class="text-[#ffffff] text-sm px-3 py-1 rounded-lg transition" to="/settings">Settings</router-link>
              <span id="connection" class=" ml-2 px-3 py-1 rounded-full text-xs bg-panel2 text-center">{{ statustext }}</span>
              <div class="bg-panel2 rounded-xl p-3 md:w-32 text-center">
                <div class="relative h-6 w-full bg-[#575757] rounded-full overflow-hidden">
                  <div class="absolute top-0 left-0 h-6 bg-[#00ffaa] transition-all" :style="{ width: battery + '%' }"></div>
                  <div class="absolute inset-0 flex items-center justify-center">
                    <p class="text-[#ffffff] text-sm font-semibold mb-0.5 drop-shadow-sm">{{ battery }}%</p>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
      
    </div>
    
  </nav>
</template>

