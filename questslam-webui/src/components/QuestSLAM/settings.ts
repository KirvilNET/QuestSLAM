import { reactive, ref } from 'vue'
import type { Config, AppInfo } from './schema.ts'

export let APPreconnectAttempts = ref(0);
export let APPmaxReconnectAttempts = 5;

export let CONFIGreconnectAttempts = ref(0);
export let CONFIGmaxReconnectAttempts = 5;

let reconnectDelay = 2000;

//General
export const headsetID = ref('');
export const rosConnectionIP = ref('');
export const trackingspeed = ref(0);
export const AutoStart = ref(false);

//Camera
export const toggleCamera = ref(false);
export const AprilTagTracking = ref(false);
export const AprilTagFamily = ref('');

export let uriConfig = "http://" + window.location.hostname +":9234/api/config";
export let uriInfo = "http://" + window.location.hostname + ":9234/api/info";


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

        const post = await fetch(uriConfig, {method: 'POST',  headers: {'Content-Type': 'application/json'}, body: data});

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
      const response = await fetch(uriConfig)

      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`)
      }

      var newConfig = await response.json() as Config
      console.log('Config loaded:', newConfig)

      Object.assign(currentConfig, newConfig)
      hasConfig.value = true
    } catch (err) {
      console.error('Failed to load config:', err)
      retryFetchConfig()
      hasConfig.value = false
    }
}

export function retryFetchConfig(manual?: boolean) {
  if (!manual || undefined) {
    if (CONFIGreconnectAttempts.value < CONFIGmaxReconnectAttempts) {
      CONFIGreconnectAttempts.value++;
      console.log(`Getting Config... (${CONFIGreconnectAttempts.value}/${CONFIGmaxReconnectAttempts})`);

      setTimeout(() => {
        fetchConfig();
      }, reconnectDelay);

    } else {
      console.error('Max config get attempts reached');
    }
  } else {
    fetchConfig()
  }
}

export async function fetchInfo() {
    hasAppInfo.value = false
    try {
      const response = await fetch(uriInfo)

      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`)
      }

      var info = await response.json() as AppInfo
      console.log('Info loaded:', info)

      Object.assign(Info, info)
      hasAppInfo.value = true
    } catch (err) {
      console.error('Failed to load app info:', err)
      retryFetchInfo()
      hasAppInfo.value = false
    }
}

export function retryFetchInfo(manual?: boolean) {
  if (!manual || undefined) {
    if (APPreconnectAttempts.value < APPmaxReconnectAttempts) {
      APPreconnectAttempts.value++;
      console.log(`Getting app info... (${APPreconnectAttempts.value}/${APPmaxReconnectAttempts})`);

      setTimeout(() => {
        fetchInfo();
      }, reconnectDelay);

    } else {
      console.error('Max app info get attempts reached');
    }
  } else {
    fetchInfo()
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