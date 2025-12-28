import { reactive, ref } from 'vue'
import type { Telemetry } from './schema';
import { newLogEntry } from '../logging/logs.ts'

let ws: WebSocket | null = null

export let uri = "ws://" + window.location.hostname + ":9234/ws";

export let WSreconnectAttempts = ref(0);
export let WSmaxReconnectAttempts = 5;
let reconnectDelay = 2000;
export const isConnected = ref(false);

const defaultTelemetry: Telemetry ={
  connectionStatus: false,
  batteryPercentage: 0,
  headsetID: 0,
  rosConnectionIP: '127.0.0.1',
  rosTime: 0,
  cpu: 0,
  mem: 0,
  temp: 0,
  isTracking: false,
  trackingspeed: 0,
  fps: 0,
  pose: {
    pos: {x: 0, y: 0, z: 0},
    rot: {x: 0, y: 0, z: 0, w: 0}
  }
}

export let telemetry = reactive<Telemetry>({ ...defaultTelemetry});

export function resetTelemetry() {
  Object.assign(telemetry, defaultTelemetry)
}

export function connect() {
    try {
        ws = new WebSocket(uri);

        ws.onopen = () => {
            isConnected.value = true
            console.log('Connected to Unity');
        }

        ws.onmessage = (event) => {
          let JsonData = JSON.parse(event.data);
          if (!JsonData.msgType) {
            console.warn('Message missing msgType:', JsonData);
            return;
          }
        
          switch (JsonData.msgType) {
            case "telemetry":
              telemetry.connectionStatus = JsonData.data.connectionStatus;
              telemetry.batteryPercentage = JsonData.data.batteryPercentage;
              telemetry.headsetID = JsonData.data.headsetID;
              telemetry.rosConnectionIP = JsonData.data.rosConnectionIP;
              telemetry.rosTime = Math.round(JsonData.data.rosTime * 100) / 100;
              telemetry.cpu = Math.round(JsonData.data.cpu * 1) / 1;
              telemetry.mem = JsonData.data.mem;
              telemetry.temp = Math.round(JsonData.data.temp * 10) / 10;
              telemetry.isTracking = JsonData.data.isTracking;
              telemetry.trackingspeed = JsonData.data.trackingspeed;
              telemetry.fps = JsonData.data.fps;
              telemetry.pose.pos.x = JsonData.data.pose.pos.x;
              telemetry.pose.pos.y = JsonData.data.pose.pos.y;
              telemetry.pose.pos.z = JsonData.data.pose.pos.z;
              telemetry.pose.rot.x = JsonData.data.pose.rot.x;
              telemetry.pose.rot.y = JsonData.data.pose.rot.y;
              telemetry.pose.rot.z = JsonData.data.pose.rot.z;
              telemetry.pose.rot.w = JsonData.data.pose.rot.w;

              break;
            
            case "log":
              HandleLog(JsonData.data)
              break;
            
            default:
              console.warn("Unknown message type:", JsonData.msgType, JsonData);
              break;
          }
        }

        ws.onerror = (err) => {
            isConnected.value = false;
            resetTelemetry();
            reconnect();
            console.error('WS error', err)
        }
    } catch(err) {
        console.error('Failed to create WebSocket:', err);
        isConnected.value = false;
        resetTelemetry();
        reconnect();
    }
}

function HandleLog(data: any) {

  switch (data.level) {
    case 0:
      //console.log("New INFO Log Entry", data)
      newLogEntry(data.message, 'info', data.timestamp)
    break;

    case 1:
      //console.log("New WARNING Log Entry", data)
      newLogEntry(data.message, 'warning', data.timestamp)
    break;

    case 2:
      //console.log("New ERROR Log Entry", data)
      newLogEntry(data.message, 'error', data.timestamp, data.stackTrace)
    break;

    default:
      console.warn("Unknown message type:", data);
    break;
  }
}

export function reconnect(manual?: boolean) {
  if (!manual) {
    if (WSreconnectAttempts.value < WSmaxReconnectAttempts) {
      WSreconnectAttempts.value++;
      console.log(`Reconnecting... (${WSreconnectAttempts.value}/${WSmaxReconnectAttempts})`);

      setTimeout(() => {
        connect();
      }, reconnectDelay);

    } else {
      console.error('Max websocket reconnection attempts reached');
    }
  } else {
    connect();
  }
}
