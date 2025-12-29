using UnityEngine;

using System;
using System.Threading;

using RosSharp.RosBridgeClient.Protocols;
using RosSharp.RosBridgeClient;

using QuestSLAM.Utils;

namespace QuestSLAM.ros
{
    public enum RosVersion {ROS1 = 0, ROS2 = 1}
    public class ROSConnector: MonoBehaviour
    {
        public int SecondsTimeout { get; set; } = 10;
        public int MaxReconnectAttempts { get; set; } = 5;
        public int ReconnectAttempts { get; set; } = 0;

        public Protocol protocol;
        public RosSocket.SerializerEnum Serializer;
        public RosSocket RosSocket { get; private set; }
        public RosVersion selectedRosVersion = RosVersion.ROS2;

        public ManualResetEvent IsConnected { get; private set; }
        public string RosBridgeServerUrl;

        public void connect(string uri, string port, bool manual = false)
        {
            RosBridgeServerUrl = "ws://" + uri + ":" + port;
            Serializer = new RosSocket.SerializerEnum();

            IsConnected = new ManualResetEvent(false);

            if (ReconnectAttempts < MaxReconnectAttempts || manual)
            {
                new Thread(ConnectionThread).Start();
                QueuedLogger.Log($"Connecting to rosbridge ({ReconnectAttempts}/{MaxReconnectAttempts})");
            } 
            else
            {
                QueuedLogger.LogWarning($"Failed to connect to RosBridge at: {RosBridgeServerUrl}");
            }
            
        }

        public virtual void disconnect()
        {
            IsConnected.Reset();
            RosSocket.Close();
        }

        protected void ConnectionThread()
        {
            RosSocket = socket(protocol, RosBridgeServerUrl, OnConnected, OnClosed, Serializer);

                if (!IsConnected.WaitOne(SecondsTimeout * 1000))
                    QueuedLogger.LogWarning("Failed to connect to RosBridge at: " + RosBridgeServerUrl);
        }

        public static RosSocket socket(Protocol protocolType, string serverUrl, EventHandler onConnected = null, EventHandler onClosed = null, RosSocket.SerializerEnum serializer = RosSocket.SerializerEnum.Microsoft)
        {
            IProtocol protocol = ProtocolInitializer.GetProtocol(protocolType, serverUrl);
            protocol.OnConnected += onConnected;
            protocol.OnClosed += onClosed;

            return new RosSocket(protocol, serializer);
        }

        private void OnApplicationQuit() { disconnect(); }

        private void OnConnected(object sender, EventArgs e)
        {
            IsConnected.Set();
            QueuedLogger.Log("Connected to RosBridge: " + RosBridgeServerUrl);
        }

        private void OnClosed(object sender, EventArgs e)
        {
            IsConnected.Reset();
            QueuedLogger.Log("Disconnected from RosBridge: " + RosBridgeServerUrl);
        }
    }
}