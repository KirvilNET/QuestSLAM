using UnityEditor;
using System;
using System.Linq;

public class QuestSLAMBuild
{
    public static void Build()
    {
        string[] args = Environment.GetCommandLineArgs();

        string buildType = GetArgValue(args, "-build-type");
        string name = GetArgValue(args, "-name");

        if (!string.IsNullOrEmpty(buildType))
        {
            switch (buildType)
            {
                case "release":
                    PlayerSettings.Android.minifyRelease = true;
                    break;
                case "dev":
                    PlayerSettings.Android.minifyDebug = true;
                    break;
            }
        }

        if (!string.IsNullOrEmpty(name))
        {
            PlayerSettings.productName = name;
        }


        Console.WriteLine($"Build Type: {buildType}");
    }

    private static bool HasArg(string[] args, string argName)
    {
        return args.Contains(argName);
    }

    // Helper method to get the value of a key-value pair argument
    private static string GetArgValue(string[] args, string argName)
    {
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == argName && i + 1 < args.Length)
            {
                // Return the next argument as the value, unless it starts with a hyphen (next flag)
                if (!args[i+1].StartsWith("-"))
                {
                    return args[i+1];
                }
            }
        }
        return null; // Return null if not found or no value provided
    }
}