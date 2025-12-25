<script setup lang="ts">
    import '@preline/collapse'

    const props = defineProps<{
        attempts: number,
        maxattempts: number,
        isConnected: boolean,
        hasAppInfo: boolean,
        hasConfig: boolean
    }>()

</script>

<template>
    <div class="flex justify-center items-center h-screen ">
        <div class=" bg-[#282828] p-8 w-80 h-100% rounded-2xl border border-[#8c52ff]">
            <div class="flex flex-col justify-center items-center">
                <h1 class="text-white text-2xl mb-4 ">Connecting</h1>
                <div class="flex gap-2">
                    <span class="size-3 animate-bounce rounded-full bg-[#9c66ff] "></span>
                    <span class="size-3 animate-bounce rounded-full bg-[#9c66ff] [animation-delay:0.2s] "></span>
                    <span class="size-3 animate-bounce rounded-full bg-[#9c66ff] [animation-delay:0.4s] "></span>
                </div>
            </div>
            <div class="flex-cols mt-2 w-100% h-100%">
                <div class="flex justify-center items-center ">
                    <template v-if="attempts > 0">
                        <p class="text-white text-xs">WebSocket connect attempts: {{ attempts }}/{{ maxattempts }}</p>
                    </template>
                </div>

                <button type="button" class="hs-collapse-toggle inline-flex items-center gap-x-2 text-sm font-medium text-[#9c66ff] " id="hs-basic-collapse" aria-expanded="false" aria-controls="hs-basic-collapse-heading" data-hs-collapse="#hs-basic-collapse-heading">
                    More Info
                    <svg class="hs-collapse-open:rotate-180 size-4" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                      <path d="m6 9 6 6 6-6"></path>
                    </svg>
                </button>
                <div id="hs-basic-collapse-heading" class="hs-collapse hidden w-full overflow-hidden transition-[height] duration-300" aria-labelledby="hs-basic-collapse">
                  <div class="mt-5 bg-white rounded-lg py-3 px-4 dark:bg-neutral-800">
                        <div>
                            <p class="text-white text-xs"> App Info: </p>
                            <svg v-if="!hasAppInfo" class=" inline-flex" xmlns="http://www.w3.org/2000/svg" width="12" height="12" viewBox="0 0 448 512"><path d="M434.8 70.1c14.3 10.4 17.5 30.4 7.1 44.7l-256 352c-5.5 7.6-14 12.3-23.4 13.1s-18.5-2.7-25.1-9.3l-128-128c-12.5-12.5-12.5-32.8 0-45.3s32.8-12.5 45.3 0l101.5 101.5 234-321.7c10.4-14.3 30.4-17.5 44.7-7.1z"/></svg>
                            <svg v-else xmlns="http://www.w3.org/2000/svg" width="12" height="12" viewBox="0 0 384 512"><path d="M376.6 84.5c11.3-13.6 9.5-33.8-4.1-45.1s-33.8-9.5-45.1 4.1L192 206 56.6 43.5C45.3 29.9 25.1 28.1 11.5 39.4S-3.9 70.9 7.4 84.5L150.3 256 7.4 427.5c-11.3 13.6-9.5 33.8 4.1 45.1s33.8 9.5 45.1-4.1L192 306 327.4 468.5c11.3 13.6 31.5 15.4 45.1 4.1s15.4-31.5 4.1-45.1L233.7 256 376.6 84.5z"/></svg>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>