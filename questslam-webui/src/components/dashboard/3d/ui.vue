<script setup lang="ts">
    const props = defineProps<{
        pose: {
            pos: {x: number, y: number, z: number},
            rot: {x: number, y: number, z: number, w: number}
        }
        dimention: boolean
    }>()

    const emit = defineEmits<{
        (e: 'toggle-labels'): void
        (e: 'reset-view'): void
        (e: 'dimention-change'): void
    }>()

</script>

<template>
    <div class="absolute top-4 left-4 text-xs bg-[#121212] rounded px-2 py-1">
        <div class="grid grid-cols-2 gap-2">
           <div>
                <p class="text-white">Posistion</p>
                <div>
                    <div class="flex gap-1">
                        <p class="text-[#ff000d]">X:</p>
                        <p class="text-white">{{ Math.round(props.pose.pos.x * 100) / 10}}</p>
                    </div>
                    <div class="flex gap-1">
                        <p class="text-[#09ff00]">Y:</p>
                        <p class="text-white">{{ Math.round(props.pose.pos.y * 100) / 10 }}</p>
                    </div>
                    <div class="flex gap-1">
                        <p class="text-[#000dff]">Z:</p>
                        <p class="text-white">{{ Math.round(props.pose.pos.z * 100) / 10 }}</p>
                    </div>
                </div>
           </div> 
           <div>
                <p class="text-white">Rotation</p>
                <div class="flex gap-1">
                    <p class="text-[#ff000d]">X:</p>
                    <p class="text-white">{{ Math.round(props.pose.rot.x * 100) / 10 }}</p>
                </div>
                <div class="flex gap-1">
                    <p class="text-[#09ff00]">Y:</p>
                    <p class="text-white">{{ Math.round(props.pose.rot.y * 100) / 10 }}</p>
                </div>
                <div class="flex gap-1">
                    <p class="text-[#000dff]">Z:</p>
                    <p class="text-white">{{ Math.round(props.pose.rot.z * 100) / 10 }}</p>
                </div>
                <div class="flex gap-1">
                    <p class="text-white">W:</p>
                    <p class="text-white">{{ Math.round(props.pose.rot.w * 100) / 10 }}</p>
                </div>   
           </div>
        </div>
    </div>
    <div class="grid grid-cols-1 absolute top-30 left-4 gap-2 flex">
        <button class="rounded-sm  p-2 text-white transition-colors bg-[#121212] hover:bg-[#282828] hover:text-gray-900 focus:ring-2 focus:ring-blue-500 focus:ring-offset-2 focus:ring-offset-white focus:outline-none disabled:pointer-events-auto disabled:opacity-50" @click="emit('toggle-labels')">
            <svg  class="fill-white" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 640 640" width="24" height="24"><path d="M96.5 160L96.5 309.5C96.5 326.5 103.2 342.8 115.2 354.8L307.2 546.8C332.2 571.8 372.7 571.8 397.7 546.8L547.2 397.3C572.2 372.3 572.2 331.8 547.2 306.8L355.2 114.8C343.2 102.7 327 96 310 96L160.5 96C125.2 96 96.5 124.7 96.5 160zM208.5 176C226.2 176 240.5 190.3 240.5 208C240.5 225.7 226.2 240 208.5 240C190.8 240 176.5 225.7 176.5 208C176.5 190.3 190.8 176 208.5 176z"/></svg>
        </button>
        <button class="rounded-sm  p-2 text-white transition-colors bg-[#121212] hover:bg-[#282828] hover:text-gray-900 focus:ring-2 focus:ring-blue-500 focus:ring-offset-2 focus:ring-offset-white focus:outline-none disabled:pointer-events-auto disabled:opacity-50" @click="emit('reset-view')">
            <svg class="fill-white" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 640 640" width="24" height="24"><path d="M320 48C337.7 48 352 62.3 352 80L352 98.3C450.1 112.3 527.7 189.9 541.7 288L560 288C577.7 288 592 302.3 592 320C592 337.7 577.7 352 560 352L541.7 352C527.7 450.1 450.1 527.7 352 541.7L352 560C352 577.7 337.7 592 320 592C302.3 592 288 577.7 288 560L288 541.7C189.9 527.7 112.3 450.1 98.3 352L80 352C62.3 352 48 337.7 48 320C48 302.3 62.3 288 80 288L98.3 288C112.3 189.9 189.9 112.3 288 98.3L288 80C288 62.3 302.3 48 320 48zM160 320C160 408.4 231.6 480 320 480C408.4 480 480 408.4 480 320C480 231.6 408.4 160 320 160C231.6 160 160 231.6 160 320zM320 224C373 224 416 267 416 320C416 373 373 416 320 416C267 416 224 373 224 320C224 267 267 224 320 224z"/></svg>
        </button>
        <button class="hidden rounded-sm  p-2 text-white transition-colors bg-[#121212] hover:bg-[#282828] hover:text-[#8c52ff] focus:ring-2 focus:ring-blue-500 focus:ring-offset-2 focus:ring-offset-white focus:outline-none disabled:pointer-events-auto disabled:opacity-50" @click="emit('dimention-change')">
           {{ dimention ? '3D' : '2D' }} 
        </button>
    </div>
</template>