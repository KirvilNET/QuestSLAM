import { reactive, ref } from 'vue'
import type { Config, AppInfo } from './schema.ts'


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

const defaultInfo: AppInfo = {
  AppVersion: '',
  AppName: '',
  BuildDate: '',
  HorisionOSVersion: '',
  UnityVersion: '',
  DeviceModel: '',
}


export const currentConfig = reactive<Config>({ ...defaultConfig })
export const Info = reactive<AppInfo>({ ...defaultInfo})

export const hasConfig = ref(false);
export const hasAppInfo = ref(false);

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

export async function fetchConfig() {
    hasConfig.value = false
    try {
      const response = await fetch('http://localhost:9234/api/config')

      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`)
      }

      var newConfig = await response.json() as Config
      console.log('Config loaded:', newConfig)

      Object.assign(currentConfig, newConfig)

    } catch (err) {
      console.error('Failed to load config:', err)
    } finally {
      hasConfig.value = true
    }
}

export async function fetchInfo() {
    hasAppInfo.value = false
    try {
      const response = await fetch('http://localhost:9234/api/info')

      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`)
      }

      var info = await response.json() as Config
      console.log('Config loaded:', info)

      Object.assign(Info, info)

    } catch (err) {
      console.error('Failed to load config:', err)
    } finally {
      hasAppInfo.value = true
    }
}

export async function resetToDefault() {
  Object.assign(currentConfig, defaultConfig)
}

export function ExportConfig() {
  
  const jsonString = JSON.stringify(currentConfig, null, 2)
  const blob = new Blob([jsonString], { type: 'application/json' })
  const url = URL.createObjectURL(blob)
  const link = document.createElement('a')

  link.href = url
  link.download = 'QuestSLAM_config.json'
  link.click()
  URL.revokeObjectURL(url)
}

export async function ImportConfig(event: Event) {
  const target = event.target as HTMLInputElement
  const file = target.files?.[0]
  
  if (!file) return
  
  try {
    const text = await file.text()
    const config: Config = JSON.parse(text)
    
    Object.assign(currentConfig, config)
    console.log('Imported config:', config)
    
  } catch (error) {
    console.error('Failed to import config:', error)
    alert('Invalid JSON file')
  }
} 