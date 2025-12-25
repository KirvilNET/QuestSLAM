using UnityEngine;

using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebSockets;
using EmbedIO.Actions;
using EmbedIO.Net;

using System;
using System.Collections.Generic;
using System.Text;

using QuestSLAM.web;
using QuestSLAM.web.Handlers;
using QuestSLAM.config;
using QuestSLAM.Utils;


namespace QuestSLAM.web.server
{
    public class webserver : MonoBehaviour
    {
        private WebServer server;

        private Websocket wsModule;
        private APIHandler API;

        private util.FileManager fs = new util.FileManager();

        public bool isRunning = false;
        public string url = "http://localhost:9234";


        public void StartServer(ConfigManager configcontext) 
        {
            if (isRunning)
            {
                QueuedLogger.LogWarning("Server is already running");
                return;
            }

            string staticPath = fs.GetStaticFilesPath();

            try 
            {
                #if UNITY_ANDROID && !UNITY_EDITOR
                    url = $"http://0.0.0.0:9234}"; // Bind to all interfaces on Quest
                #else
                    url = "http://localhost:9234"; // Local testing
                #endif
                
                wsModule = new Websocket();
                API = new APIHandler(configcontext);

                server = new WebServer(o => o.WithUrlPrefix(url).WithMode(HttpListenerMode.EmbedIO))
                    .WithCors()
                    .WithModule(wsModule)
                    .WithModule(API)
                    .WithStaticFolder("/", staticPath, true);
                    
                    

                server.Start();
                isRunning = true;

                QueuedLogger.Log($"EmbedIO Server Started on http://localhost:9234");
                QueuedLogger.Log($"WebSocket: ws://localhost:9234/ws");
            }
            catch (Exception e)
            {
                QueuedLogger.LogError($"Failed to start WebSocket server: {e.Message}\n{e.StackTrace}");
                isRunning = false;
            }
        }

        #region Senders

        public void SendTelemetry(dataschema.TelemetryPacket telemetry)
        {
            if (wsModule != null && isRunning)
            {
                var packet = new dataschema.Packet<dataschema.TelemetryPacket>("telemetry", telemetry);
                string json = JsonUtility.ToJson(packet);
                wsModule.BroadcastMessage(json);
            }
        }

        public void SendLog(dataschema.LogPacket log)
        {
            if (wsModule != null && isRunning)
            {
                var packet = new dataschema.Packet<dataschema.LogPacket>("log", log);
                string json = JsonUtility.ToJson(packet);
                wsModule.BroadcastMessage(json);
            }
        }

        #endregion
    }
}