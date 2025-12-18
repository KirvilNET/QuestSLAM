<script setup>

</script>

<template>
  <nav class="bg-panel rounded-2xl px-4 py-3 flex items-center justify-between shadow">
    <div class="flex items-center gap-3">
    <div class="h-8 w-8 rounded-lg bg-accent text-black flex items-center justify-center font-bold">Q</div>
    <div>
    <p class="font-semibold leading-none">QuestSLAM</p>
    <p class="text-xs text-slate-400">Device Dashboard</p>
    </div>
    </div>
    <div class="flex items-center gap-2">
    <button class="text-sm px-3 py-1 rounded-lg bg-panel2 hover:bg-slate-700 transition">Dashboard</button>
    <button class="text-sm px-3 py-1 rounded-lg bg-panel2 hover:bg-slate-700 transition">Settings</button>
    <button class="text-sm px-3 py-1 rounded-lg bg-panel2 hover:bg-slate-700 transition">Logs</button>
    <span class="ml-2 px-3 py-1 rounded-full text-xs bg-panel2 border border-slate-700">
    {{ status }}
    </span>
    </div>
  </nav>
</template>

<style scoped>

</style>