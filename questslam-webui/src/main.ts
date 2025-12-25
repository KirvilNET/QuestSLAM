import { createApp } from 'vue';

import router from './router/router.ts'

import './style.css';

import App from './App.vue';

//Loading
import LoadingPage from './pages/Loading.vue'

//Dashboard
import navbar from './components/navbar.vue';
import telemetry from './components/dashboard/telemetry.vue';
import inforow from './components/dashboard/InfoRow.vue';
import quickactions from './components/dashboard/quickactions.vue';
import ui from './components/dashboard/3d/ui.vue'
import render from './components/dashboard/3d/render.vue'
import viz from './components/dashboard/3d/viz.vue'

//Settings
import general from './components/settings/general.vue'
import camera from './components/settings/camera.vue'
import plugins from './components/settings/plugins.vue'
import about from './components/settings/about.vue'

const app = createApp(App);

//components
app.component('inforow', inforow);

//Loading Page
app.component('LoadingPage', LoadingPage)

//Dashboard
app.component('navbar', navbar);
app.component('telemetry', telemetry);
app.component('quickactions', quickactions);
app.component('ui', ui);
app.component('render', render);
app.component('viz', viz);

//Settings
app.component('Settings-General', general)
app.component('Settings-Camera', camera)
app.component('Settings-Plugins', plugins)
app.component('Settings-About', about)


app.use(router)
app.mount('#app');
