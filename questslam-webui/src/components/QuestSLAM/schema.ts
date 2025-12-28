export interface Config {
    headsetID: number
    rosConnectionIP: string
    trackingspeed: number
    toggleCamera: boolean
    AutoStart: boolean
    AprilTagTracking: boolean
    AprilTagFamily: string
}

export interface AppInfo {
    AppVersion: string
    AppName: string
    BuildDate: string
    HorisionOSVersion: string
    UnityVersion: string
    DeviceModel: string
}

export type Telemetry  = {
    connectionStatus: boolean;
    batteryPercentage: number;
    headsetID: number;
    rosConnectionIP: string;
    rosTime: number;
    cpu: number;
    mem: number;
    temp: number;
    isTracking: boolean;
    trackingspeed: number;
    fps: number;
    pose: {
        pos: {x: number; y: number; z: number};
        rot: {x: number; y: number; z: number; w: number};
    };
};

export interface LogEntry {
  message: string
  stackTrace?: string
  level: 'info' | 'warning' | 'error' | 'web'
  timestamp: string
}