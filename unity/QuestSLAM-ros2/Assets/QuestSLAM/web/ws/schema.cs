using System;
using UnityEngine;

namespace QuestSLAM.web.dataschema
{
    [Serializable]
    public class Vec3
    {
        public float x;
        public float y;
        public float z;
    }

    [Serializable]
    public class Quat
    {
        public float x;
        public float y;
        public float z;
        public float w;
    }

    [Serializable]
    public class Pose
    {
        public Vec3 pos;
        public Quat rot;
    }

    [Serializable]
    public struct Packet<T> where T : struct
    {
        public string msgType;
        public T data;
        
        public Packet(string type, T packetData)
        {
            msgType = type;
            data = packetData;
        }
    }

    [Serializable]
    public class IncommingPacket
    {
        public string msgType;
        public string data;
        
        public T GetData<T>() where T : struct
        {
            return JsonUtility.FromJson<T>(data);
        }
    }
    

    [Serializable]
    public struct TelemetryPacket
    {
        public bool connectionStatus;
        public float batteryPercentage;
        public int headsetID;
        public string rosConnectionIP;
        public float rosTime;
        public float cpu;
        public float mem;
        public float temp;
        public bool isTracking;
        public float trackingspeed; 
        public float fps;
        public Pose pose;
    }

    [Serializable]
    public struct LogPacket
    {
        public string logType;
        public string data;
    }

    [Serializable]
    public class Config
    {
        public int headsetID;
        public string rosConnectionIP;
        public int trackingspeed;
        public bool toggleCamera;
        public bool AutoStart;
        public bool AprilTagTracking;
    }
}