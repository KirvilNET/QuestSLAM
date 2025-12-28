using UnityEngine;

using EmbedIO;

using System;
using System.Threading.Tasks;
using System.Text;

using QuestSLAM.config;
using QuestSLAM.Utils;

namespace QuestSLAM.web.Handlers
{
    public class APIHandler : WebModuleBase
    {
        public override bool IsFinalHandler => true;
        private ConfigManager config;
        private AppInfoSchema app;

        public APIHandler(ConfigManager configContext, AppInfoSchema appContext) : base("/api")
        {
            config = configContext;
            app = appContext;
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
                else if (context.RequestedPath == "/resetPose")
                {
                    ResetPoseHandler(context);
                }
                else if (context.RequestedPath == "/restart")
                {
                    RestartAppHandler(context);
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
                    var data = JsonUtility.FromJson<ConfigManager.Config>(body);

                    /*
                    var newConfig = new ConfigManager.Config
                    {
                        headsetID = data.headsetID,
                        rosConnectionIP = data.rosConnectionIP,
                        trackingspeed = data.trackingspeed,
                        toggleCamera = data.toggleCamera,
                        AutoStart = data.AutoStart,
                        AprilTagTracking = data.AprilTagTracking,
                        AprilTagFamily = data.AprilTagFamily
                    };
                    */

                    config.Save(data);
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
                    
                    var newInfo = new dataschema.AppInfo
                    {
                        AppVersion = app.AppVersion,
                        AppName = app.AppName,
                        BuildDate = app.BuildDate,
                        HorisionOSVersion = app.HorisionOSVersion,
                        UnityVersion = app.UnityVersion,
                        DeviceModel = app.DeviceModel
                    };

                    var data = JsonUtility.ToJson(newInfo);

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

        private async void ResetPoseHandler(IHttpContext context)
        {
            if (context.Request.HttpVerb == HttpVerbs.Get)
            {
                
            }
            else
            {
                context.Response.StatusCode = 405;
                await context.SendStringAsync(new { error = "Method not allowed" }.ToString(), "application/json", Encoding.UTF8);
            }
        }

        private async void RestartAppHandler(IHttpContext context)
        {
            if (context.Request.HttpVerb == HttpVerbs.Get)
            {
                #if UNITY_ANDROID && !UNITY_EDITOR
                    AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer")
                        .GetStatic<AndroidJavaObject>("currentActivity");

                    AndroidJavaObject intent = activity.Call<AndroidJavaObject>("getPackageManager")
                        .Call<AndroidJavaObject>("getLaunchIntentForPackage", 
                            activity.Call<string>("getPackageName"));

                    activity.Call("startActivity", intent);

                    AndroidJavaClass jc = new AndroidJavaClass("android.os.Process");
                    Invoke(jc.CallStatic("killProcess", jc.CallStatic<int>("myPid")), 1f);
                #endif
            }
            else
            {
                context.Response.StatusCode = 405;
                await context.SendStringAsync(new { error = "Method not allowed" }.ToString(), "application/json", Encoding.UTF8);
            }
        }

        private async void ReconnectDataTransport(IHttpContext context)
        {
            if (context.Request.HttpVerb == HttpVerbs.Get)
            {
                
            }
            else
            {
                context.Response.StatusCode = 405;
                await context.SendStringAsync(new { error = "Method not allowed" }.ToString(), "application/json", Encoding.UTF8);
            }
        }
    }
}