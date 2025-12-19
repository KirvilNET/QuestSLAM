<script setup>
    import { ref, reactive, onMounted } from 'vue'

    const pos = reactive({
        pos: {x: 0.0, y: 0.0, z: 0.0},
        rot: {x: 0.0, y: 0.0, z: 0.0, w: 0.0}
    })
</script>

<template>
    <!-- Overlay info -->
    <div class="absolute top-2 left-2 text-xs bg-[#121212] rounded px-2 py-1">
        <div class="grid grid-cols-2 gap-2">
           <div>
                <p class="text-white">Posistion</p>
                <div>
                    <div class="flex gap-1">
                        <p class="text-[#ff000d]">X:</p>
                        <p class="text-white">{{ pos.pos.x }}</p>
                    </div>
                    <div class="flex gap-1">
                        <p class="text-[#09ff00]">Y:</p>
                        <p class="text-white">{{ pos.pos.y }}</p>
                    </div>
                    <div class="flex gap-1">
                        <p class="text-[#000dff]">Z:</p>
                        <p class="text-white">{{ pos.pos.z }}</p>
                    </div>
                </div>
           </div> 
           <div>
                <p class="text-white">Rotation</p>
                <div class="flex gap-1">
                    <p class="text-[#ff000d]">X:</p>
                    <p class="text-white">{{ pos.rot.x }}</p>
                </div>
                <div class="flex gap-1">
                    <p class="text-[#09ff00]">Y:</p>
                    <p class="text-white">{{ pos.rot.y }}</p>
                </div>
                <div class="flex gap-1">
                    <p class="text-[#000dff]">Z:</p>
                    <p class="text-white">{{ pos.rot.z }}</p>
                </div>
                    
           </div>
        </div>
    </div>
</template>