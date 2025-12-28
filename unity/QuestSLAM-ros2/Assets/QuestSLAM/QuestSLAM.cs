using UnityEngine;
using UnityEngine.Profiling;


using System;
using System.IO;



using RosSharp.RosBridgeClient;
using RosSharp;

using std_msgs = RosSharp.RosBridgeClient.MessageTypes.Std;
using geometry_msgs = RosSharp.RosBridgeClient.MessageTypes.Geometry;
using nav_msgs = RosSharp.RosBridgeClient.MessageTypes.Nav;

using QuestSLAM.sim;
using QuestSLAM.web.server;
using QuestSLAM.web.dataschema;
using QuestSLAM.config;
using QuestSLAM.Utils;
using QuestSLAM.UI;
using QuestSLAM.ros;
using System.Threading.Tasks;


namespace QuestSLAM.Manager
{
    public class QuestSLAMManager : MonoBehaviour
    {
        #region Fields

        [SerializeField] private OVRCameraRig cameraRig;

        #endregion

        #region Headset Transforms

        public Vector3 headset_position;
        public Vector3 headset_eulerAngles;
        public Quaternion headset_rotation;
        public Vector3 headset_angular;
        public Vector3 headset_linear;

        #endregion


        private nav_msgs.Odometry odom;
        private std_msgs.Float32 battery;

        RosSocket socket;
        ROSConnector connector;

        private Utils.System sys;
        private SITL sitl;
        private webserver server;
        private ConfigManager config;
        private UIManager ui;
        private Utils.AppInfo info;

        private TelemetryPacket Tpacket;
        
        void genOdomMsgs()
        {
            odom = new nav_msgs.Odometry
            {
                header = new std_msgs.Header
                {
                    frame_id = "odom",
                    stamp = new RosSharp.RosBridgeClient.MessageTypes.BuiltinInterfaces.Time
                    {
                        nanosec = (uint)UnityEngine.Time.time * 1000,
                        sec = (int)UnityEngine.Time.time
                    }
                },
                child_frame_id = "base_link",
                pose = new geometry_msgs.PoseWithCovariance
                {
                    pose = new geometry_msgs.Pose
                    {
                        position = new geometry_msgs.Point
                        {
                            x = headset_position.Unity2Ros().x,
                            y = headset_position.Unity2Ros().y,
                            z = headset_position.Unity2Ros().z
                        },
                        orientation = new geometry_msgs.Quaternion
                        {
                            x = headset_rotation.Unity2Ros().x,
                            y = headset_rotation.Unity2Ros().y,
                            z = headset_rotation.Unity2Ros().z,
                            w = headset_rotation.Unity2Ros().w
                        }
                    },
                    covariance = new double[36]
                },
                twist = new geometry_msgs.TwistWithCovariance
                {
                    twist = new geometry_msgs.Twist
                    {
                        linear = new geometry_msgs.Vector3
                        {
                            x = headset_linear.Unity2Ros().x,
                            y = headset_linear.Unity2Ros().y,
                            z = headset_linear.Unity2Ros().z,
                        },
                        angular = new geometry_msgs.Vector3
                        {
                            x = headset_angular.Unity2Ros().x,
                            y = headset_angular.Unity2Ros().y,
                            z = headset_angular.Unity2Ros().z,
                        }

                    },
                    covariance = new double[36]
                }

            };

            socket.Publish(socket.Advertise<nav_msgs.Odometry>("QuestSLAM/odom"), odom);
        }


        void genROSTelemetry()
        {
            battery = new std_msgs.Float32
            {
                data = SystemInfo.batteryLevel * 100

            };

            socket.Publish(socket.Advertise<std_msgs.Float32>("QuestSLAM/battery_level"), battery);
        }

        void getCommmandArgs()
        {
            #if UNITY_ANDROID && !UNITY_EDITOR
                try
                {
                    AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                    AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                    AndroidJavaObject intent = currentActivity.Call<AndroidJavaObject>("getIntent");
                    string commandLine = intent.Call<string>("getStringExtra", "ip");

                    QueuedLogger.Log("Received command-line argument: " + commandLine);
                    PlayerPrefs.SetString("ROSBRIDGEIP", commandLine);
                    PlayerPrefs.Save();
                }
                catch (System.Exception e)
                {
                    QueuedLogger.LogError("Failed to read command-line argument: " + e.Message);
                }
            #endif
        }

        // 30hz update
        void SlowUpdate()
        {
            Tpacket = new TelemetryPacket
            {
               connectionStatus = config.getSim() ? true : connector.IsConnected.WaitOne(0),
               batteryPercentage = config.getSim() ? sitl.GetSimulatedBattery() : SystemInfo.batteryLevel * 100,
               rosConnectionIP = config.getSim() ? "SIMULATION MODE" :connector.RosBridgeServerUrl,
               rosTime = UnityEngine.Time.time,
               cpu = config.getSim() ? sitl.GetSimulatedCpu() : sys.GetCpuUsage(),
               mem = config.getSim() ? sitl.GetSimulatedMemory() : Profiler.GetTotalAllocatedMemoryLong(),
               temp = config.getSim() ? sitl.GetSimulatedTemp() : sys.GetCpuTempCelsius(),
               isTracking = config.getSim() ? true : sys.HasValidHeadPose(),
               trackingspeed = sys.TrackingSpeed(),
               fps =  config.getSim() ? sitl.GetSimulatedFps() : 1f / Time.deltaTime,
               pose = new web.dataschema.Pose
               {
                   pos = new web.dataschema.Vec3
                   {
                       x = headset_position.Unity2Ros().x,
                       y = headset_position.Unity2Ros().y,
                       z = headset_position.Unity2Ros().z
                   },
                   rot = new web.dataschema.Quat
                   {
                       x = headset_rotation.Unity2Ros().x,
                       y = headset_rotation.Unity2Ros().y,
                       z = headset_rotation.Unity2Ros().z,
                       w = headset_rotation.Unity2Ros().w
                   }
               }
            };

            server.SendTelemetry(Tpacket);
            QueuedLogger.Flush(server);
            
        }

        void MainUpdate()
        {
            socket = connector.RosSocket;
    
            headset_position = cameraRig.centerEyeAnchor.position;
            headset_rotation = cameraRig.centerEyeAnchor.rotation;
            headset_eulerAngles = cameraRig.centerEyeAnchor.eulerAngles;

            genOdomMsgs();
            genROSTelemetry();
            
        }

        private async void Awake()
        {
           
            getCommmandArgs();

            sitl = GetComponent<SITL>();
            ui = GetComponent<UIManager>();

            sys = new Utils.System();
            QueuedLogger logger = new QueuedLogger();
            config = new ConfigManager();
            info = new Utils.AppInfo();

            config.Init();
            logger.Init();

            ui.Init(info.getAppInfo().AppVersion);

            connector = GetComponent<ROSConnector>();
            connector.connect(config.getRosConnectionIP(), "9090");

            try {
                server = GetComponent<webserver>();
                var AppContext = info.getAppInfo();

                await server.StartServer(config, AppContext);
            }
            catch (Exception e) 
            {
                QueuedLogger.Log($"Failed to start QuestSLAM WebUI Error: {e}");
            }

            InvokeRepeating(nameof(SlowUpdate), 0, 1f / 3);
            InvokeRepeating(nameof(MainUpdate), 0, 1f / 120);

        }

        void OnApplicationQuit()
        {
            socket.Unadvertise("QuestSLAM/odom");
            socket.Unadvertise("QuestSLAM/battery_level");  
        }
    }
}
