using UnityEngine;

using System;
using System.IO;

using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

using QuestSLAM.Utils;

namespace QuestSLAM.config
{
    public class ConfigManager
    {
        /// <summary>The schema for the config file.</summary>
        [Serializable]
        public class Config
        {
            /// <summary>The ID of the headset. Only matters if using QuestSLAM in multi headset mode</summary>
            public int headsetID { get; set; }

            /// <summary>The ip the the ros connector will attempt to connect to. this must be a valid ROS2 instance and actively running ros2bridge.</summary>
            public string rosConnectionIP { get; set; }

            /// <summary>The tracking speed of the OVR instance</summary>
            public int trackingspeed { get; set; }

            /// <summary>Whether sim mode is enabled</summary>
            public bool sim { get; set; }

            /// <summary>Toggle for the passthrough camera</summary>
            public bool toggleCamera { get; set; }

            /// <summary>Whether QuestSLAM should start up on boot</summary>
            public bool AutoStart { get; set; }

            /// <summary>Toggle for tracking AprilTags. Does not work if toggleCamera is not true</summary>
            public bool AprilTagTracking { get; set; }

            /// <summary>April Tag Family</summary>
            public string AprilTagFamily { get; set; }
        }

        private static ConfigManager instance;
        public static ConfigManager Instance => instance ??= new ConfigManager();

        private string configPath;

        private Config config;
        
        /// <summary>Initilize the config path based on its platform</summary>
        public void Init()
        {
            #if UNITY_ANDROID && !UNITY_EDITOR
                configPath = Path.Combine(Application.persistentDataPath, "config.yaml");
            #else
                configPath = Path.Combine(Application.streamingAssetsPath, "config.yaml");
            #endif
        }

        /// <summary>Load the config from the config file</summary>
        public void Load()
        {
            try
            {
                if (!File.Exists(configPath))
                {   
                    QueuedLogger.Log($"Config file not found at {configPath}. Creating default config.", QueuedLogger.Levels.WARNING);
                    InitConfig();
                    return;
                }

                string raw = File.ReadAllText(configPath);
                var deserializer = new DeserializerBuilder()
                    .WithNamingConvention(CamelCaseNamingConvention.Instance)
                    .Build();
                config = deserializer.Deserialize<Config>(raw);
                QueuedLogger.Log($"Config loaded from {configPath} \n data: {raw}");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to load config: {e.Message}");
                InitConfig();
            }
        }

        /// <summary>Save the config to the file</summary>
        public void Save(Config newConfig)
        {
            try
            {
                var serializer = new SerializerBuilder()
                    .WithNamingConvention(CamelCaseNamingConvention.Instance)
                    .Build();
                string yaml = serializer.Serialize(newConfig);
                File.WriteAllText(configPath, yaml);
                QueuedLogger.Log("Successfully saved the new config");
            }
            catch (System.Exception e)
            {
                QueuedLogger.LogError($"Failed to save config: {e.Message}");
            }
        }

        /// <summary>Initilize the config file</summary>
        public void InitConfig()
        {
            config = new Config
            {
                headsetID = 0,
                rosConnectionIP = "127.0.0.1",
                trackingspeed = 120,
                sim = false,
                toggleCamera = false,
                AutoStart = false,
                AprilTagTracking = false,
                AprilTagFamily = "36h11"
            };

            Save(config);
        }
        
        /// <summary>Get a the config as the config class</summary>
        public Config GetConfig()
        {
            Load();
            return config;
        }

        /// <summary>Get a specific config value</summary>
        public int getHeadsetID()
        {
            if (config == null) Load();
            return config.headsetID;
        }

        /// <inheritdoc/>
        public string getRosConnectionIP()
        {
            if (config == null) Load();
            return config.rosConnectionIP;
        }

        /// <inheritdoc/>
        public int getTrackingSpeed()
        {
            if (config == null) Load();
            return config.trackingspeed;
        }

        /// <inheritdoc/>
        public bool getSim()
        {
            if (config == null) Load();
            return config.sim;
        }

        /// <inheritdoc/>
        public bool getToggleCamera()
        {
            if (config == null) Load();
            return config.toggleCamera;
        }

        /// <inheritdoc/>
        public bool getAutoStart()
        {
            if (config == null) Load();
            return config.AutoStart;
        }

        /// <inheritdoc/>
        public bool getAprilTagTracking()
        {
            if (config == null) Load();
            return config.AprilTagTracking;
        }
    }
}