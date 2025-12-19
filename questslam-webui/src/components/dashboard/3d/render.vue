<script setup lang="ts">
import { ref, onMounted, onUnmounted } from 'vue'
import * as THREE from 'three'
import { OrbitControls } from 'three/examples/jsm/controls/OrbitControls.js';

const threejs = ref<HTMLDivElement | null>(null)

let renderer: THREE.WebGLRenderer
let camera: THREE.PerspectiveCamera
let scene: THREE.Scene
let controls: OrbitControls
let frameId: number

onMounted(() => {
  if (!threejs.value) return

  const container = threejs.value
  
  scene = new THREE.Scene()
  scene.background = new THREE.Color(0x3f3f3f)
  camera = new THREE.PerspectiveCamera(75, container.clientWidth / container.clientHeight, 0.1, 1000)

  renderer = new THREE.WebGLRenderer({ antialias: true })
  renderer.setSize(container.clientWidth, container.clientHeight)
  container.appendChild(renderer.domElement)

  controls = new OrbitControls(camera, renderer.domElement);

  controls.enableDamping = true; 
  controls.dampingFactor = 0.05;
  controls.minDistance = 1;      
  controls.maxDistance = 30;     
  controls.maxPolarAngle = Math.PI / 2; 

  camera.position.set(5, 5, 5)
  camera.lookAt(0, 0, 0)


  
  const gridHelper = new THREE.GridHelper(100, 101, 0xff0000, 0xffffff);
  const headsetPos = new THREE.AxesHelper(1)

  scene.add(headsetPos)
  scene.add(gridHelper);



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
  controls.update();
  renderer.render(scene, camera)
}
</script>

<template>
  <div ref="threejs" class="w-full h-full"></div>
</template>