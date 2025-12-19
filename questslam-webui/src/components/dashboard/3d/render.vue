<script setup lang="ts">
import { ref, onMounted, onUnmounted } from 'vue'
import * as THREE from 'three'

const threejs = ref<HTMLDivElement | null>(null)
let renderer: THREE.WebGLRenderer
let camera: THREE.PerspectiveCamera
let scene: THREE.Scene
let frameId: number

onMounted(() => {
  if (!threejs.value) return

  const container = threejs.value
  
  scene = new THREE.Scene()
  scene.background = new THREE.Color(0x3f3f3f)
  camera = new THREE.PerspectiveCamera(90, container.clientWidth / container.clientHeight, 0.1, 1000)

  camera.position.set(2, 2, 2)
  camera.lookAt(0, 0, 0)

  

  renderer = new THREE.WebGLRenderer({ antialias: true })
  renderer.setSize(container.clientWidth, container.clientHeight)
  container.appendChild(renderer.domElement)

  

  const headsetPos = new THREE.AxesHelper(1)
  scene.add(headsetPos)

  animate()

  // Handle container resize
  const resizeObserver = new ResizeObserver(() => {
    if (!threejs.value) return
    const width = threejs.value.clientWidth
    const height = threejs.value.clientHeight
    renderer.setSize(width, height)
    camera.aspect = width / height
    camera.updateProjectionMatrix()
  })

  resizeObserver.observe(container)

  onUnmounted(() => {
    renderer.dispose()
    resizeObserver.disconnect()
  })
})

function animate() {
  frameId = requestAnimationFrame(animate)
  renderer.render(scene, camera)
}
</script>

<template>
  <div ref="threejs" class="w-full h-full"></div>
</template>