using UnityEngine;

using EmbedIO;

using System;

using QuestSLAM.web.Handlers;
using QuestSLAM.config;
using QuestSLAM.Utils;
using QuestSLAM.web.dataschema;

using System.Threading.Tasks;

namespace QuestSLAM.web.server
{
    public class webserver : MonoBehaviour
    {
        private WebServer server;
        private Websocket wsModule;
        private APIHandler API;

        private util.FileManager fs;

        public bool isRunning = false;
        public string url;

        public async Task StartServer(ConfigManager configcontext, AppInfoSchema appcontext) 
        {   
            fs = new util.FileManager();

            if (isRunning)
            {
                QueuedLogger.LogWarning("Server is already running");
                return;
            }

            string staticPath = fs.GetStaticFilesPath();
  
            #if UNITY_ANDROID && !UNITY_EDITOR
                await fs.ExtractAndroidUIFilesAsync(staticPath);
            #else
                fs.DoStaticFilesExist(staticPath);
                await Task.CompletedTask;
            #endif

            try 
            {
                #if UNITY_ANDROID && !UNITY_EDITOR
                    url = $"http://*:9234"; // Bind to all interfaces on Quest
                #else
                    url = "http://localhost:9234"; // Local testing
                #endif
                
                wsModule = new Websocket();
                API = new APIHandler(configcontext, appcontext);

                server = new WebServer(o => o.WithUrlPrefix(url).WithMode(HttpListenerMode.EmbedIO))
                    .WithCors()
                    .WithModule(wsModule)
                    .WithModule(API)
                    .WithStaticFolder("/", staticPath, true);                    

                server.Start();
                isRunning = true;

                QueuedLogger.Log($"EmbedIO Server Started on {url}");
                QueuedLogger.Log($"WebSocket: ws://localhost:9234/ws");
            }
            catch (Exception e)
            {
                QueuedLogger.LogError($"Failed to start WebSocket server: {e.Message}\n{e.StackTrace}");
                isRunning = false;
            }
        }

        #region Senders

        public void SendTelemetry(TelemetryPacket telemetry)
        {
            if (wsModule != null && isRunning)
            {
                var packet = new Packet<TelemetryPacket>("telemetry", telemetry);
                string json = JsonUtility.ToJson(packet);
                wsModule.BroadcastMessage(json);
            }
        }

        public void SendLog(QueuedLogger.LogEntry entry, bool hasException = false)
        {
            if (wsModule == null || !isRunning) return;

            LogPacket log;
            string time = DateTimeOffset.FromUnixTimeMilliseconds(entry.Timestamp).LocalDateTime.ToLongTimeString();

            switch (entry.Level)
            {
                case QueuedLogger.Levels.INFO:
                    log = new LogPacket(entry.Message, QueuedLogger.Levels.INFO, time);
                    break;
                case QueuedLogger.Levels.WARNING:
                    log = new LogPacket(entry.Message, QueuedLogger.Levels.WARNING, time);
                    break;
                case QueuedLogger.Levels.ERROR:
                    if (hasException) 
                        log = new LogPacket(entry.Message, QueuedLogger.Levels.ERROR, time, entry.Exception);
                    else 
                        log = new LogPacket(entry.Message, QueuedLogger.Levels.ERROR, time);
                    break;
                default:
                    return;
            }

            var packet = new Packet<LogPacket>("log", log);
            string json = JsonUtility.ToJson(packet);
            wsModule.BroadcastMessage(json);
                
        }
        #endregion

        #region Unity Lifecycle 

        void OnApplicationQuit()
        {
            server.Dispose();
        }

        #endregion
    }
}