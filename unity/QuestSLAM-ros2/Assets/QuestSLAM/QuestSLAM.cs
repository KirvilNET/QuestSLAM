using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UIElements;

using System.Net;
using System.Net.Sockets;
using System;


using RosSharp.RosBridgeClient;
using RosSharp;

using std_msgs = RosSharp.RosBridgeClient.MessageTypes.Std;
using geometry_msgs = RosSharp.RosBridgeClient.MessageTypes.Geometry;
using nav_msgs = RosSharp.RosBridgeClient.MessageTypes.Nav;
using rosapi = RosSharp.RosBridgeClient.MessageTypes.Rosapi;


using PassthroughCameraSamples;
using UnityEngine.UI;

using QuestSLAM.sim;
using QuestSLAM.web.server;
using QuestSLAM.web.dataschema;
using QuestSLAM.config;
using QuestSLAM.Utils;


namespace QuestSLAM.Manager
{
    public class QuestSLAMManager : MonoBehaviour
    {
        public Vector3 headset_position;
        public Vector3 headset_eulerAngles;
        public Quaternion headset_rotation;
        public Vector3 headset_angular;
        public Vector3 headset_linear;
        public int headsetID = 0;
        public bool sim = false;

        [SerializeField] private OVRCameraRig cameraRig;
        [SerializeField] private UIDocument ui;
        private Label dashboardText;
        private Label IPText;

        private string myAddr;

        private nav_msgs.Odometry odom;
        private std_msgs.Float32 battery;

        RosSocket socket;
        RosConnector connector;

        private Utils.System sys;
        private SITL sitl;
        private webserver server;
        private ConfigManager config;

        private TelemetryPacket Tpacket;

        private float updateInterval = 1f / 30f;
        private float timeSinceLastUpdate = 0f;

        public void UpdateIPAddressText()
        {
            IPHostEntry hostEntry = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in hostEntry.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    myAddr = ip.ToString();
                    VisualElement root = ui.rootVisualElement;

                    VisualElement TextContainer = root.Q<VisualElement>("UI").Q<VisualElement>("Container").Q<VisualElement>("TextContainer");

                    dashboardText = TextContainer.Q<Label>("Dashboard");
                    IPText = TextContainer.Q<Label>("IP");

                    if (myAddr == "127.0.0.1")
                    {
                        IPText.text = "No Adapter Found";
                        dashboardText.text = "Dashboard being hosted on http://localhost:9234";
                    }
                    else
                    {
                        IPText.text = $"IP: {myAddr}";
                        dashboardText.text = $"Dashboard being hosted on http://{myAddr}:9234";
                    }
                }
                break;
            }
        }
        
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
               connectionStatus = sim ? true : connector.IsConnected.WaitOne(0),
               batteryPercentage = sim ? sitl.GetSimulatedBattery() : SystemInfo.batteryLevel * 100,
               headsetID = headsetID,
               rosConnectionIP = sim ? "SIMULATION MODE" :connector.RosBridgeServerUrl,
               rosTime = UnityEngine.Time.time,
               cpu = sim ? sitl.GetSimulatedCpu() : sys.GetCpuUsage(),
               mem = sim ? sitl.GetSimulatedMemory() : Profiler.GetTotalAllocatedMemoryLong(),
               temp = sim ? sitl.GetSimulatedTemp() : sys.GetCpuTempCelsius(),
               isTracking = sim ? true : sys.HasValidHeadPose(),
               trackingspeed = sys.TrackingSpeed(),
               fps =  sim ? sitl.GetSimulatedFps() : 1f / Time.deltaTime,
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
            QueuedLogger.Flush();
            
        }
            


        void MainUpdate()
        {
            #if UNITY_ANDROID && !UNITY_EDITOR
                socket = connector.RosSocket;
        
                headset_position = cameraRig.centerEyeAnchor.position;
                headset_rotation = cameraRig.centerEyeAnchor.rotation;
                headset_eulerAngles = cameraRig.centerEyeAnchor.eulerAngles;

                genOdomMsgs();
                genROSTelemetry();
    
            #endif

            #if UNITY_EDITOR 
                sim = true;

                headset_position = sitl.GetSimulatedPos();
                headset_eulerAngles = sitl.GetSimulatedEulerAngles();
                headset_rotation = sitl.GetSimulatedRot(headset_eulerAngles);
                
            #endif
            
        }

        private async void Awake()
        {
            UpdateIPAddressText();
           
            getCommmandArgs();

            sitl = GetComponent<SITL>();
            sys = new Utils.System();
            QueuedLogger logger = new QueuedLogger();
            config = new ConfigManager();

            #if UNITY_ANDROID && !UNITY_EDITOR
                connector = GetComponent<RosConnector>();
                connector.connect();
            #endif

            try {
                server = GetComponent<webserver>();

                server.StartServer(config);
            }
            catch (Exception e) 
            {
                QueuedLogger.Log($"Failed to start QuestSLAM WebUI Error: {e}");
            }

            config.Init();
            logger.Init();

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
