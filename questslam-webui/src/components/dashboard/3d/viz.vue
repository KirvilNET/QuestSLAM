<script setup lang="ts">
    import { ref } from 'vue'

    const props = defineProps<{
        pose: {
            pos: {x: number, y: number, z: number},
            rot: {x: number, y: number, z: number, w: number}
        }
    }>()
    
    const showLabels = ref(true)
    const resetView = ref(false)
    const dimention = ref(true)

    function toggleLabels() {
        showLabels.value = !showLabels.value
    }

    function onResetView() {
        resetView.value = !resetView.value
    }

    function dimentionChange() {
        dimention.value = !dimention.value
    }

</script>

<template>
    <div class="rounded-2xl p-3 w-full h-full relative">
        <ui 
            :pose="pose" 
            :dimention="dimention" 
            @toggle-labels="toggleLabels" 
            @reset-view="onResetView"
            @dimention-change="dimentionChange"
        />
        <render 
            :resetView="resetView" 
            :showLabels="showLabels"
            :dimention="dimention" 
            :pose="pose" 
        />
    </div>
</template>