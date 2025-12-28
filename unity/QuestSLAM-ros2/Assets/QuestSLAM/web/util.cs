using UnityEngine;

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;

using QuestSLAM.Utils;

namespace QuestSLAM.web.util
{
    public class FileManager
    {
        private string path;
        public string GetStaticFilesPath()
        {
            #if UNITY_ANDROID && !UNITY_EDITOR
                path = Path.Combine(Application.persistentDataPath, "web");
                QueuedLogger.Log($"Static file path: path");
                return path;
            #else
                path = Path.Combine(Application.streamingAssetsPath, "web");
                QueuedLogger.Log($"Static file path: path");
                return path;
            #endif
        }

        public void DoStaticFilesExist(string path)
        {
            if (Directory.Exists(path))
                return;

            Directory.CreateDirectory(path);

            string fallbackSourcePath = Path.Combine(
                Application.streamingAssetsPath,
                "web",
                "fallback.html"
            );

            string fallbackTargetPath = Path.Combine(path, "index.html");

            try
            {
                if (File.Exists(fallbackSourcePath))
                {
                    File.Copy(fallbackSourcePath, fallbackTargetPath);
                    QueuedLogger.Log($"Copied fallback HTML from {fallbackSourcePath}");
                }
            }
            catch (Exception ex)
            {
                QueuedLogger.LogError($"Failed to copy fallback HTML: {ex.Message}");
            }
        }

        public async Task ExtractAndroidFileAsync(string sourceRelative, string targetAbsolute)
        {
            string sourcePath = Path.Combine(Application.streamingAssetsPath, sourceRelative);

            using (var www = UnityEngine.Networking.UnityWebRequest.Get(sourcePath))
            {
                var operation = www.SendWebRequest();
                while (!operation.isDone)
                    await Task.Yield();

                if (www.result == UnityEngine.Networking.UnityWebRequest.Result.Success)
                {
                    File.WriteAllBytes(targetAbsolute, www.downloadHandler.data);
                    QueuedLogger.Log($"Extracted: {sourceRelative}");
                }
                else
                {
                    QueuedLogger.LogWarning($"Failed to extract {sourceRelative}: {www.error}");
                }
            }
        }

        public async Task ExtractAndroidUIFilesAsync(string targetPath)
        {
            if (Directory.Exists(targetPath))
            {
                QueuedLogger.Log("Clearing old UI files...");
                Directory.Delete(targetPath, true);
            }

            QueuedLogger.Log("Extracting UI files from APK...");

            Directory.CreateDirectory(targetPath);
            string assetsDir = Path.Combine(targetPath, "assets");
            Directory.CreateDirectory(assetsDir);

            await ExtractAndroidFileAsync("web/index.html", Path.Combine(targetPath, "index.html"));
            await ExtractAndroidFileAsync("web/assets/main.css", Path.Combine(assetsDir, "main.css"));
            await ExtractAndroidFileAsync("web/assets/main.js", Path.Combine(assetsDir, "main.js"));
            await ExtractAndroidFileAsync("web/VR.svg", Path.Combine(targetPath, "VR.svg"));

            //await ExtractAndroidFileAsync("web/logo-dark.svg", Path.Combine(targetPath, "logo-dark.svg"));

            QueuedLogger.Log("UI extraction complete");
        }

    }
}