<script setup lang="ts">
import { ref, onMounted, onUnmounted, watch } from 'vue'
import * as THREE from 'three'
import { OrbitControls } from 'three/examples/jsm/controls/OrbitControls.js';

import { makeAxis } from './helper.ts'

const props = defineProps<{
  pose: {
    pos: {x: number, y: number, z: number},
    rot: {x: number, y: number, z: number, w: number}
  }
  showLabels: boolean
  resetView: boolean
  dimention: boolean
}>()

const threejs = ref<HTMLDivElement | null>(null)

let originLabel: THREE.Sprite | null = null
let headsetLabel: THREE.Sprite | null = null

let headset: THREE.Group | null = null

let initialCameraPosition: THREE.Vector3
let initialControlsTarget: THREE.Vector3

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

    initialCameraPosition = camera.position.clone()
    initialControlsTarget = controls.target.clone()

    let origin = makeAxis('origin', 'rgba(0,0,0,0.7)', '#ffffff')
    headset = makeAxis('headset', 'rgba(0,0,0,0.7)', '#ffffff')

    originLabel = origin.getObjectByName('label') as THREE.Sprite
    headsetLabel = headset.getObjectByName('label') as THREE.Sprite
    
    const gridHelper = new THREE.GridHelper(100, 101, 0xff0000, 0xffffff);

    const World = new THREE.Group()
    World.add(gridHelper)

    scene.add(World)

    scene.add(origin)
    scene.add(headset)

    animate();

    // Handle container resize
    const resizeObserver = new ResizeObserver(() => {
        if (!threejs.value) return
        const width = threejs.value.clientWidth;
        const height = threejs.value.clientHeight;
        renderer.setSize(width, height);
        camera.aspect = width / height;
        camera.updateProjectionMatrix();
    })

    resizeObserver.observe(container);

    onUnmounted(() => {
        renderer.dispose();
        cancelAnimationFrame(frameId)
        resizeObserver.disconnect();
    })
})


watch(
  () => props.showLabels,
  (visible) => {
    if (originLabel) originLabel.visible = visible;
    if (headsetLabel) headsetLabel.visible = visible;
  },
  { immediate: true } 
)

watch(
  () => props.resetView,
  (x) => {
    if (x) resetCamera();
  },
  { immediate: true } 
)

function resetCamera() {
  camera.position.copy(initialCameraPosition)   // reset position
  controls.target.copy(initialControlsTarget)  
  controls.update()
}

function animate() {
  frameId = requestAnimationFrame(animate)
  controls.update()
  updateHeadsetPos(props.pose);
  renderer.render(scene, camera)
}

function updateHeadsetPos(pose: {pos: {x: number, y: number, z: number}, rot: {x: number, y: number, z: number, w: number}}) {
  headset?.position.set(pose.pos.x, pose.pos.y, pose.pos.z)
  headset?.rotation.set(pose.rot.x, pose.rot.y, pose.rot.z)
}
</script>

<template>
  <div ref="threejs" class="w-full h-full"></div>
</template>