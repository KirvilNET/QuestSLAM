using System;
using System.IO;
using System.Linq;

using UnityEngine;
using UnityEngine.XR;


namespace QuestSLAM.Utils
{
    class System
    { 
        // CPU usaage
        private long prevIdle;
        private long prevTotal;
        private bool initialized;

        // Tracking Speed
        private Vector3 lastPos;
        private Quaternion lastRot;
        private float lastTime;
        private int poseUpdates;
        private const float WindowSeconds = 1.0f;

        public bool HasValidHeadPose()
        {
            InputDevice head = InputDevices.GetDeviceAtXRNode(XRNode.Head);
        
            if (!head.isValid)
                return false;
        
            bool tracked;
            Vector3 pos;
            Quaternion rot;
        
            if (!head.TryGetFeatureValue(CommonUsages.isTracked, out tracked))
                return false;
        
            if (!head.TryGetFeatureValue(CommonUsages.devicePosition, out pos))
                return false;
        
            if (!head.TryGetFeatureValue(CommonUsages.deviceRotation, out rot))
                return false;
        
            return tracked;
        }

        public float GetCpuUsage()
        {
            try
            {
                string line = File.ReadLines("/proc/stat").First();
                string[] parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                // cpu  user nice system idle iowait irq softirq steal guest guest_nice
                long user = long.Parse(parts[1]);
                long nice = long.Parse(parts[2]);
                long system = long.Parse(parts[3]);
                long idle = long.Parse(parts[4]);
                long iowait = parts.Length > 5 ? long.Parse(parts[5]) : 0;
                long irq = parts.Length > 6 ? long.Parse(parts[6]) : 0;
                long softirq = parts.Length > 7 ? long.Parse(parts[7]) : 0;
                long steal = parts.Length > 8 ? long.Parse(parts[8]) : 0;

                long idleTime = idle + iowait;
                long totalTime = user + nice + system + idle + iowait + irq + softirq + steal;

                if (!initialized)
                {
                    prevIdle = idleTime;
                    prevTotal = totalTime;
                    initialized = true;
                    return 0f;
                }

                long deltaIdle = idleTime - prevIdle;
                long deltaTotal = totalTime - prevTotal;

                prevIdle = idleTime;
                prevTotal = totalTime;

                if (deltaTotal == 0)
                    return 0f;

                float cpuUsage = (1f - (float)deltaIdle / deltaTotal) * 100f;
                return Mathf.Clamp(cpuUsage, 0f, 100f);
            }
            catch
            {
                return -1f;
            }
        }

        public float GetCpuTempCelsius()
        {
            try
            {
                var zones = Directory.GetDirectories("/sys/class/thermal/", "thermal_zone*");

                foreach (var zone in zones)
                {
                    string typePath = Path.Combine(zone, "type");
                    string tempPath = Path.Combine(zone, "temp");

                    if (!File.Exists(typePath) || !File.Exists(tempPath))
                        continue;

                    string type = File.ReadAllText(typePath).Trim().ToLower();

                    // Look for CPU-related zones
                    if (type.Contains("cpu"))
                    {
                        string tempStr = File.ReadAllText(tempPath).Trim();
                        if (float.TryParse(tempStr, out float milliC))
                        {
                            return milliC / 1000f;
                        }
                    }
                }
            }
            catch { }

            return -1f;
        }

        public float TrackingSpeed()
        {
            InputDevice head = InputDevices.GetDeviceAtXRNode(XRNode.Head);
            if (!head.isValid)
                return 0f;

            Vector3 pos;
            Quaternion rot;

            if (!head.TryGetFeatureValue(CommonUsages.devicePosition, out pos) ||
                !head.TryGetFeatureValue(CommonUsages.deviceRotation, out rot))
                return 0f;

            // First sample
            if (lastTime == 0f)
            {
                lastPos = pos;
                lastRot = rot;
                lastTime = Time.time;
                return 0f;
            }

            // Detect pose change (epsilon avoids noise)
            bool changed =
                Vector3.Distance(pos, lastPos) > 0.0001f ||
                Quaternion.Angle(rot, lastRot) > 0.01f;

            if (changed)
            {
                poseUpdates++;
                lastPos = pos;
                lastRot = rot;
            }

            float elapsed = Time.time - lastTime;

            if (elapsed >= WindowSeconds)
            {
                float hz = poseUpdates / elapsed;
                poseUpdates = 0;
                lastTime = Time.time;
                return hz;
            }

            return -1f; // not ready yet
        }
    }

    [Serializable]
    public class AppInfoSchema
    {
        /// <summary>The Version of the App</summary>
        public string AppVersion { get; set; }

        /// <summary>The name of the App</summary>
        public string AppName { get; set; }

        /// <summary>The Build Date</summary>
        public string BuildDate { get; set; }

        /// <summary>The HorisionOS Version</summary>
        public string HorisionOSVersion { get; set; }

        /// <summary>The Unity Version</summary>
        public string UnityVersion { get; set; }

        /// <summary>The Device Model</summary>
        public string DeviceModel { get; set; }
    }

    public class AppInfo
    {
        public AppInfoSchema getAppInfo()
        {
            try
            {
                return new AppInfoSchema
                {
                    AppVersion = Application.version,
                    AppName = Application.productName,
                    BuildDate = File.GetLastWriteTime(Application.dataPath).ToString("yyyy-MM-dd HH:mm:ss"),
                    HorisionOSVersion = SystemInfo.operatingSystem,
                    UnityVersion = Application.unityVersion,
                    DeviceModel = SystemInfo.deviceModel,
                };
            }
            catch (Exception ex)
            {
                QueuedLogger.LogError($"Failed to get app info: {ex.Message}");
                return null;
            }
        }
    }
}