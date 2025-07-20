using UnityEngine;
using TMPro;
using System.Net;
using System.Net.Sockets;

using RosSharp.RosBridgeClient;
using RosSharp;
using std_msgs = RosSharp.RosBridgeClient.MessageTypes.Std;
using geometry_msgs = RosSharp.RosBridgeClient.MessageTypes.Geometry;
using nav_msgs = RosSharp.RosBridgeClient.MessageTypes.Nav;





public class QuestSLAM : MonoBehaviour
{
    public Vector3 headset_position;
    public Vector3 headset_eulerAngles;
    public Quaternion headset_rotation;
    public Vector3 headset_angular;
    public Vector3 headset_linear;
    [SerializeField]
    private OVRCameraRig cameraRig;
    [SerializeField]
    private TMP_Text headset_ip;
    [SerializeField]
    private TMP_Text ros_ip;
    [SerializeField]
    private TMP_InputField IPinput;
    [SerializeField]
    private UnityEngine.UI.Button setButton;

    //private bool IsTracking;
    private string myAddr;
    private string ip;

    private nav_msgs.Odometry odom;
    private std_msgs.Float32 battery;
    
    RosSocket socket;
    RosConnector connector;

    //private TouchScreenKeyboard overlayKeyboard;
    //string Odompub_id;

    public void UpdateIPAddressText()
    {
        IPHostEntry hostEntry = Dns.GetHostEntry(Dns.GetHostName());
        foreach (IPAddress ip in hostEntry.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                myAddr = ip.ToString();
                TextMeshProUGUI ipText = headset_ip as TextMeshProUGUI;
                if (myAddr == "127.0.0.1")
                {
                    headset_ip.text = "No Adapter Found";
                }
                else
                {
                    headset_ip.text = "ip: " + myAddr;
                }
            }
            break;
        }
    }

    public void SetNewBridgeIP()
    {
        
        PlayerPrefs.SetString("ROSBRIDGEIP", IPinput.text);
        
        ip = IPinput.text;
        
        if (ip != PlayerPrefs.GetString("ROSBRIDGEIP"))
        {
            PlayerPrefs.Save();
            Debug.Log("Creating New ipsave");
            connector = GetComponent<RosConnector>();
            connector.connect();
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

       socket.Publish(socket.Advertise<nav_msgs.Odometry>("odom"), odom);
    }


    void genTelemetry()
    {
        battery = new std_msgs.Float32
        {
            data = SystemInfo.batteryLevel

        };

        //string batteryPub = ;

        socket.Publish(socket.Advertise<std_msgs.Float32>("battery_level"), battery);
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

           

            Debug.Log("Received command-line argument: " + commandLine);
            PlayerPrefs.SetString("ROSBRIDGEIP", commandLine);
            PlayerPrefs.Save();
            

            
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to read command-line argument: " + e.Message);
        }
        #endif

       
    }



    void Start()
    {

        ros_ip.text = "ROS bridge: " + PlayerPrefs.GetString("ROSBRIDGEIP");

        UpdateIPAddressText();

        getCommmandArgs();

        connector = GetComponent<RosConnector>();
        connector.connect();


        
        setButton.onClick.AddListener(SetNewBridgeIP);

        
        
    }
    void Update()
    {
        socket = connector.RosSocket;

        headset_position = cameraRig.centerEyeAnchor.position;
        headset_rotation = cameraRig.centerEyeAnchor.rotation;
        headset_eulerAngles = cameraRig.centerEyeAnchor.eulerAngles;


        genOdomMsgs();
        genTelemetry();

        //string Odompub_id = socket.Advertise<nav_msgs.Odometry>("odom");
        //string batteryPub = socket.Advertise<std_msgs.Float32>("battery_level");

        //socket.Publish(batteryPub, battery);
        //socket.Publish(socket.Advertise<nav_msgs.Odometry>("odom"), odom);

    }

}
