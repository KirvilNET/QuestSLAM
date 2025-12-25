import { reactive, ref } from 'vue'
import type { Config } from './schema.ts'


//General
export const headsetID = ref('');
export const rosConnectionIP = ref('');
export const trackingspeed = ref(0);
export const AutoStart = ref(false);

//Camera
export const toggleCamera = ref(false);
export const AprilTagTracking = ref(false);
export const AprilTagFamily = ref('');


const defaultConfig: Config = {
    headsetID: 0,
    rosConnectionIP: '0.0.0.0',
    trackingspeed: 120,
    toggleCamera: false,
    AutoStart: false,
    AprilTagTracking: false,
    AprilTagFamily: '36h11',
}

export const serverConfig = reactive<Config>({ ...defaultConfig })
export const currentConfig = reactive<Config>({ ...defaultConfig })
const loading = ref(false);

export async function save() {
   try {

        var data = JSON.stringify(currentConfig);
        console.log(data)

        const post = await fetch("http://localhost:9234/api/config", {method: 'POST',  headers: {'Content-Type': 'application/json'}, body: data});

        if (!post.ok) {
            throw new Error(`HTTP error! status: ${post.status}`);
        }


        console.log('Success:', post);
        return post;

    } catch (error) {
        console.error('Error:', error);
    }
       
}

export const fetchConfig = async () => {
    loading.value = true
    try {
      const response = await fetch('http://localhost:9234/api/config')

      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`)
      }

      var newConfig = await response.json() as Config
      console.log('Config loaded:', newConfig)

      Object.assign(serverConfig, newConfig)

    } catch (err) {
      console.error('Failed to load config:', err)
    } finally {
      loading.value = false
    }
}

export async function resetToDefault() {
  
}
export function ExportConfig() {

}
export function ImportConfig() {

}