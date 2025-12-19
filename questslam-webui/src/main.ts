import { createApp } from 'vue';

import router from './router/router.ts'

import './style.css';

import App from './App.vue';


//Dashboard
import navbar from './components/navbar.vue';
import telemetry from './components/dashboard/telemetry.vue';
import inforow from './components/dashboard/InfoRow.vue';
import quickactions from './components/dashboard/quickactions.vue';

//3d viewer
import coordinates from './components/dashboard/3d/coordinates.vue'
import render from './components/dashboard/3d/render.vue'
import viz from './components/dashboard/3d/viz.vue'

const app = createApp(App);

//components
app.component('inforow', inforow);

//main widgets
app.component('navbar', navbar);
app.component('telemetry', telemetry);
app.component('quickactions', quickactions);

//3d viewer
app.component('coordinates', coordinates);
app.component('render', render);
app.component('viz', viz);

app.use(router)
app.mount('#app');
