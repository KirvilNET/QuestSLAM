using UnityEngine;

using System;
using System.IO;

using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

using QuestSLAM.Utils;

namespace QuestSLAM.config
{
    public class Config
    {
        public int headsetID { get; set; }
        public string rosConnectionIP { get; set; }
        public int trackingspeed { get; set; }
        public bool toggleCamera { get; set; }
        public bool AutoStart { get; set; }
        public bool AprilTagTracking { get; set; }
    }

    public class ConfigManager
    {
        private static ConfigManager instance;
        public static ConfigManager Instance => instance ??= new ConfigManager();

        private string configPath;

        private Config config;
        
        public void Init()
        {
            #if UNITY_ANDROID && !UNITY_EDITOR
                configPath = Path.Combine(Application.persistentDataPath, "config.yaml");
            #else
                configPath = Path.Combine(Application.streamingAssetsPath, "config.yaml");
            #endif
        }

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
                QueuedLogger.Log($"Config loaded from {configPath} \n data: {raw}", QueuedLogger.Levels.INFO);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to load config: {e.Message}");
                InitConfig();
            }
        }

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

        public void InitConfig()
        {
            config = new Config
            {
                headsetID = 0,
                rosConnectionIP = "127.0.0.1",
                trackingspeed = 120,
                toggleCamera = false,
                AutoStart = false,
                AprilTagTracking = false
            };

            Save(config);
        }
        
        public Config GetConfig()
        {
            Load();
            return config;
        }
    }
}