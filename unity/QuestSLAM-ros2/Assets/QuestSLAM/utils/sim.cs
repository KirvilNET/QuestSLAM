using UnityEngine;
using System;
using System.Numerics;

namespace QuestSLAM.sim
{
    public class SITL : MonoBehaviour
    {
        private float simulationTime = 0f;

        public float GetSimulatedBattery()
        {
            return Mathf.Max(0, 100 - (simulationTime * 5f));
        }

        public float GetSimulatedCpu()
        {
            return 40 + Mathf.PerlinNoise(simulationTime * 0.5f, 0) * 40;
        }

        public float GetSimulatedMemory()
        {
            return (60 + Mathf.PerlinNoise(simulationTime * 0.3f, 100) * 20) * 1024 * 1024; // Convert to bytes
        }

        public float GetSimulatedTemp()
        {
            return 35 + (simulationTime * 2) + Mathf.PerlinNoise(simulationTime * 0.4f, 200) * 5;
        }

        public float GetSimulatedTrackingSpeed()
        {
            return 20 + Mathf.PerlinNoise(simulationTime * 0.6f, 300) * 10;
        }

        public float GetSimulatedFps()
        {
            return 90 - (Mathf.Sin(simulationTime * 0.5f) * 15);
        }

        public UnityEngine.Vector3 GetSimulatedPos()
        {
            float angle = simulationTime * 45;

            UnityEngine.Vector3 pos = new UnityEngine.Vector3(
                Mathf.Cos(angle * Mathf.Deg2Rad) * 2,
                1.5f + Mathf.Sin(simulationTime * 1.5f) * 0.3f,
                Mathf.Sin(angle * Mathf.Deg2Rad) * 2
            );

            return pos;
        }

        public UnityEngine.Vector3 GetSimulatedEulerAngles()
        {
            UnityEngine.Vector3 eulerAngles = new UnityEngine.Vector3(
                Mathf.Sin(simulationTime * 0.3f) * 30,      
                simulationTime * 45,                         
                Mathf.Cos(simulationTime * 0.5f) * 20       
            );

            return eulerAngles;
        }

        public UnityEngine.Quaternion GetSimulatedRot(UnityEngine.Vector3 euler)
        {
            UnityEngine.Quaternion q = UnityEngine.Quaternion.Euler(euler);

            return q;
        }
    }
}