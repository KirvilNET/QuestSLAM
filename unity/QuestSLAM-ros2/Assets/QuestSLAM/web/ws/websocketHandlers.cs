using UnityEngine;

using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebSockets;

using System.Text;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System;

using QuestSLAM.web.dataschema;
using QuestSLAM.config;
using QuestSLAM.Utils;

namespace QuestSLAM.web.Handlers
{   
    public class Websocket : WebSocketModule
    {
        private Dictionary<string, bool> connectedClients = new Dictionary<string, bool>();

        public Websocket() : base("/ws", true) { }

        protected override async System.Threading.Tasks.Task OnClientConnectedAsync(IWebSocketContext context)
        {
            QueuedLogger.Log($"WebSocket client connected: {context.Id}");

            await System.Threading.Tasks.Task.CompletedTask;
        }

        protected override async System.Threading.Tasks.Task OnClientDisconnectedAsync(IWebSocketContext context)
        {
            QueuedLogger.Log($"WebSocket client disconnected: {context.Id}");
            await System.Threading.Tasks.Task.CompletedTask;
        }

        protected override async System.Threading.Tasks.Task OnMessageReceivedAsync(IWebSocketContext context, byte[] buffer, IWebSocketReceiveResult result)
        {
            string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
            QueuedLogger.Log($"WebSocket message from {context.Id}: {message}");
            await System.Threading.Tasks.Task.CompletedTask;
        }

        public async void BroadcastMessage(string message)
        {
            await BroadcastAsync(message);
        }
    }
}