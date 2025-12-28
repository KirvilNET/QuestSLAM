import { ref } from 'vue'
import type { LogEntry } from '../QuestSLAM/schema.ts'

export const logs = ref<LogEntry[]>([])

export const NumberofErrorLogs = ref(0);
export const NumberofWarningLogs = ref(0);
export const NumberofInfoLogs = ref(0);
export const NumberofWebLogs = ref(0);

export const searchQuery = ref('')

export function newLogEntry(message: string, level: LogEntry['level'], timestamp: string, stackTrace?: string) {
    var entry: LogEntry = {
        message, 
        stackTrace, 
        level, 
        timestamp, 
    };

    switch (level) {
        case 'info':
            NumberofInfoLogs.value++
            break;
        case 'error':
            NumberofErrorLogs.value++
            break;
        case 'warning':
            NumberofWarningLogs.value++
            break;
        case 'web':
            NumberofWebLogs.value++
            break;
    }

    logs.value.push(entry)
}

export function clearLogs() {
  logs.value = []

  NumberofErrorLogs.value = 0
  NumberofWarningLogs.value = 0
  NumberofInfoLogs.value = 0
}

export function getTime(hour12: boolean = false) {
    const recivedtimestamp = new Date().toLocaleTimeString('en-US', {
        hour12: hour12,
        hour: '2-digit',
        minute: '2-digit',
        second: '2-digit',
    });

    return recivedtimestamp;
}

export function downloadLogs() {
  const text = logs.value
    .map(log => `[${log.timestamp}] ${log.level.toUpperCase()}: ${log.message}`)
    .join('\n')
  
  const blob = new Blob([text], { type: 'text/plain' })
  const url = URL.createObjectURL(blob)
  const link = document.createElement('a')
  link.href = url
  link.download = `QuestSLAM-logs-${new Date().toISOString().slice(0, 19)}.txt`
  link.click()
  URL.revokeObjectURL(url)
}
