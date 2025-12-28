<script setup lang="ts">
    import '@preline/collapse'

    const props = defineProps<{
        WSattempts: number,
        WSmaxattempts: number,
        APPattempts: number,
        APPmaxattempts: number,
        CONFIGattempts: number,
        CONFIGmaxattempts: number
        isLoading: boolean,
        isConnected: boolean,
        hasAppInfo: boolean,
        hasConfig: boolean
    }>()

    const emit = defineEmits<{
        (e: 'manual-reconnect'): void
    }>()

</script>

<template>
    <div class="flex justify-center items-center h-screen ">
        <div class=" bg-[#282828] p-8 w-80 h-100% rounded-2xl border border-[#8c52ff]">
            <div class="flex flex-col justify-center items-center">
                <h1 class="text-white text-2xl mb-4 ">Connecting</h1>
                <div class="flex gap-2 mb-4">
                    <span class="size-3 animate-bounce rounded-full bg-[#9c66ff] "></span>
                    <span class="size-3 animate-bounce rounded-full bg-[#9c66ff] [animation-delay:0.2s] "></span>
                    <span class="size-3 animate-bounce rounded-full bg-[#9c66ff] [animation-delay:0.4s] "></span>
                </div>

                <button type="button" class="hs-collapse-toggle inline-flex items-center gap-2 text-sm font-medium text-[#9c66ff] " id="hs-basic-collapse" aria-expanded="false" aria-controls="hs-basic-collapse-heading" data-hs-collapse="#hs-basic-collapse-heading">
                    Connection Info
                    <svg class="hs-collapse-open:rotate-180 size-4" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                      <path d="m6 9 6 6 6-6"></path>
                    </svg>
                </button>
            </div>
            <div class="flex-cols w-100% h-100%">
                <div id="hs-basic-collapse-heading" class="hs-collapse hidden w-full overflow-hidden transition-[height] duration-300" aria-labelledby="hs-basic-collapse">
                  <div class="mt-1 rounded-lg py-2 px-4 ">
                        <div class="flex justify-between items-center">
                            <p class="text-white text-xs"> WebSocket: </p>
                            <div class="flex items-center gap-2">
                                <p class="text-white text-xs">({{ WSattempts }}/{{ WSmaxattempts }})</p>
                                <svg v-if="isConnected" class="fill-[#00ffaa]" xmlns="http://www.w3.org/2000/svg" width="12" height="12" viewBox="0 0 448 512"><path d="M434.8 70.1c14.3 10.4 17.5 30.4 7.1 44.7l-256 352c-5.5 7.6-14 12.3-23.4 13.1s-18.5-2.7-25.1-9.3l-128-128c-12.5-12.5-12.5-32.8 0-45.3s32.8-12.5 45.3 0l101.5 101.5 234-321.7c10.4-14.3 30.4-17.5 44.7-7.1z"/></svg>
                                <svg v-else class="fill-[#ff0000]" xmlns="http://www.w3.org/2000/svg" width="12" height="12" viewBox="0 0 384 512"><path d="M376.6 84.5c11.3-13.6 9.5-33.8-4.1-45.1s-33.8-9.5-45.1 4.1L192 206 56.6 43.5C45.3 29.9 25.1 28.1 11.5 39.4S-3.9 70.9 7.4 84.5L150.3 256 7.4 427.5c-11.3 13.6-9.5 33.8 4.1 45.1s33.8 9.5 45.1-4.1L192 306 327.4 468.5c11.3 13.6 31.5 15.4 45.1 4.1s15.4-31.5 4.1-45.1L233.7 256 376.6 84.5z"/></svg>
                            </div> 
                        </div>
                        <div class="flex justify-between items-center">
                            <p class="text-white text-xs"> App Info: </p>
                            <div class="flex items-center gap-2">
                                <p class="text-white text-xs">({{ APPattempts }}/{{ APPmaxattempts }})</p>
                                <svg v-if="hasAppInfo" class="fill-[#00ffaa]" xmlns="http://www.w3.org/2000/svg" width="12" height="12" viewBox="0 0 448 512"><path d="M434.8 70.1c14.3 10.4 17.5 30.4 7.1 44.7l-256 352c-5.5 7.6-14 12.3-23.4 13.1s-18.5-2.7-25.1-9.3l-128-128c-12.5-12.5-12.5-32.8 0-45.3s32.8-12.5 45.3 0l101.5 101.5 234-321.7c10.4-14.3 30.4-17.5 44.7-7.1z"/></svg>
                                <svg v-else class="fill-[#ff0000]" xmlns="http://www.w3.org/2000/svg" width="12" height="12" viewBox="0 0 384 512"><path d="M376.6 84.5c11.3-13.6 9.5-33.8-4.1-45.1s-33.8-9.5-45.1 4.1L192 206 56.6 43.5C45.3 29.9 25.1 28.1 11.5 39.4S-3.9 70.9 7.4 84.5L150.3 256 7.4 427.5c-11.3 13.6-9.5 33.8 4.1 45.1s33.8 9.5 45.1-4.1L192 306 327.4 468.5c11.3 13.6 31.5 15.4 45.1 4.1s15.4-31.5 4.1-45.1L233.7 256 376.6 84.5z"/></svg>
                            </div>
                        </div>
                        <div class="flex justify-between items-center">
                            <p class="text-white text-xs"> Config: </p>
                            <div class="flex items-center gap-2">
                                <p class="text-white text-xs">({{ CONFIGattempts }}/{{ CONFIGmaxattempts }})</p>
                                <svg v-if="hasConfig" class="fill-[#00ffaa]" xmlns="http://www.w3.org/2000/svg" width="12" height="12" viewBox="0 0 448 512"><path d="M434.8 70.1c14.3 10.4 17.5 30.4 7.1 44.7l-256 352c-5.5 7.6-14 12.3-23.4 13.1s-18.5-2.7-25.1-9.3l-128-128c-12.5-12.5-12.5-32.8 0-45.3s32.8-12.5 45.3 0l101.5 101.5 234-321.7c10.4-14.3 30.4-17.5 44.7-7.1z"/></svg>
                                <svg v-else class="fill-[#ff0000]" xmlns="http://www.w3.org/2000/svg" width="12" height="12" viewBox="0 0 384 512"><path d="M376.6 84.5c11.3-13.6 9.5-33.8-4.1-45.1s-33.8-9.5-45.1 4.1L192 206 56.6 43.5C45.3 29.9 25.1 28.1 11.5 39.4S-3.9 70.9 7.4 84.5L150.3 256 7.4 427.5c-11.3 13.6-9.5 33.8 4.1 45.1s33.8 9.5 45.1-4.1L192 306 327.4 468.5c11.3 13.6 31.5 15.4 45.1 4.1s15.4-31.5 4.1-45.1L233.7 256 376.6 84.5z"/></svg>
                            </div>
                        </div>
                        <div class="flex justify-center items-center w-100%">
                            <button class="text-[#8c52ff] hover:text-[#9c66ff]" @click="emit('manual-reconnect')">Reconnect</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>