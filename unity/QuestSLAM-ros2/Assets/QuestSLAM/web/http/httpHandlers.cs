using UnityEngine;

using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebSockets;
using EmbedIO.Actions;

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;

using QuestSLAM.config;
using QuestSLAM.Utils;

namespace QuestSLAM.web.Handlers
{
    public class APIHandler : WebModuleBase
    {
        public override bool IsFinalHandler => true;
        private ConfigManager config;
        public APIHandler(ConfigManager configContext) : base("/api")
        {
            config = configContext;
        }

        protected override async Task OnRequestAsync(IHttpContext context)
        {
            try
            {
                if (context.RequestedPath == "/config")
                {
                   ConfigHandler(context);
                }
                else if (context.RequestedPath == "/info")
                {
                    InfoHandler(context);
                }
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                await context.SendStringAsync(new { error = ex.Message }.ToString(), "application/json", Encoding.UTF8);
            }
        }

        private async void ConfigHandler(IHttpContext context)
        {
            if (context.Request.HttpVerb == HttpVerbs.Get)
            {
                try
                {
                    var current = config.GetConfig();
                    var currentJson = new dataschema.Config
                    {
                        headsetID = current.headsetID,
                        rosConnectionIP = current.rosConnectionIP,
                        trackingspeed = current.trackingspeed,
                        toggleCamera = current.toggleCamera,
                        AutoStart = current.AutoStart,
                        AprilTagTracking = current.AprilTagTracking
                    };

                    var data = JsonUtility.ToJson(currentJson);

                    context.Response.StatusCode = 200;
                    await context.SendStringAsync(data, "application/json", Encoding.UTF8);
                    QueuedLogger.Log("✓ Config retrieved");
                }
                catch (Exception ex)
                {
                    QueuedLogger.LogError($"Failed to get config: {ex.Message}");
                    context.Response.StatusCode = 500;
                    await context.SendStringAsync(new { error = ex.Message }.ToString(), "application/json", Encoding.UTF8);
                }
            }
            else if (context.Request.HttpVerb == HttpVerbs.Post)
            {
                try
                {
                    string body = await context.GetRequestBodyAsStringAsync();
                    var data = JsonUtility.FromJson<dataschema.Config>(body);

                    var newConfig = new Config
                    {
                        headsetID = data.headsetID,
                        rosConnectionIP = data.rosConnectionIP,
                        trackingspeed = data.trackingspeed,
                        toggleCamera = data.toggleCamera,
                        AutoStart = data.AutoStart,
                        AprilTagTracking = data.AprilTagTracking
                    };

                    config.Save(newConfig);
                }
                catch (Exception ex)
                {
                    QueuedLogger.LogError($"Failed to update config: {ex.Message}");
                    context.Response.StatusCode = 400;
                    await context.SendStringAsync(new { error = ex.Message }.ToString(), "application/json", Encoding.UTF8);
                }
            }
            else
            {
                context.Response.StatusCode = 405;
                await context.SendStringAsync(new { error = "Method not allowed" }.ToString(), "application/json", Encoding.UTF8);
            }
        }

        private async void InfoHandler(IHttpContext context)
        {
            if (context.Request.HttpVerb == HttpVerbs.Get)
            {
                try
                {
                    var AppInfo = new dataschema.AppInfo
                    {
                        AppVersion = Application.version,
                        AppName = Application.productName,
                        BuildDate = File.GetLastWriteTime(Application.dataPath).ToString("yyyy-MM-dd HH:mm:ss"),
                        HorisionOSVersion = SystemInfo.operatingSystem,
                        UnityVersion = Application.unityVersion,
                        DeviceModel = SystemInfo.deviceModel,
                    };

                    var data = JsonUtility.ToJson(AppInfo);

                    context.Response.StatusCode = 200;
                    await context.SendStringAsync(data, "application/json", Encoding.UTF8);
                    QueuedLogger.Log("✓ AppInfo retrieved");
                } 
                catch (Exception ex)
                {
                    QueuedLogger.LogError($"Failed to get app info: {ex.Message}");
                    context.Response.StatusCode = 500;
                    await context.SendStringAsync(new { error = ex.Message }.ToString(), "application/json", Encoding.UTF8);
                }
            } 
            else
            {
                context.Response.StatusCode = 405;
                await context.SendStringAsync(new { error = "Method not allowed" }.ToString(), "application/json", Encoding.UTF8);
            }
        }
    }
}