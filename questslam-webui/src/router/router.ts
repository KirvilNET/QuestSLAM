import { createRouter, createWebHistory } from 'vue-router'

import Dashboard from '../pages/Dashboard.vue';
import Settings from '../pages/Settings.vue';
import Logs from '../pages/Logs.vue';
import CameraView from '../pages/cameraview.vue'

const routes = [
  { path: '/', component: Dashboard },
  { path: '/settings', component: Settings },
  { path: '/logs', component: Logs },
  { path: '/camera', component: CameraView },
]

const router = createRouter({
  history: createWebHistory(),
  routes,
})

export default router