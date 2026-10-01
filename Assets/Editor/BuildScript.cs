using UnityEditor.Build.Reporting;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor;
using System.Linq;
using UnityEngine;

public static class BuildScript
{
    public static void BuildWindows()
    {
        string[] scenes = GetScenes();

        var options = new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = "Build/Windows/ZooWorld.exe",
            target = BuildTarget.StandaloneWindows64,
            options = BuildOptions.None,
        };

        BuildReport report = BuildPipeline.BuildPlayer(options);
        BuildSummary summary = report.summary;
        
        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log($"Build succeeded: {summary.totalSize} bytes");
            EditorApplication.Exit(0);
        }
        else
        {
            Debug.LogError($"Build failed: {summary.result}");
            EditorApplication.Exit(1);
        }
    }

    public static void BuildAndroid()
    {
        string[] scenes = GetScenes();

        var options = new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = "Build/Android/ZooWorld.apk",
            target = BuildTarget.Android,
            options = BuildOptions.None,
        };

        BuildReport report = BuildPipeline.BuildPlayer(options);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log($"Build succeeded: {summary.totalSize} bytes");
            EditorApplication.Exit(0);
        }
        else
        {
            Debug.LogError($"Build failed: {summary.result}");
            EditorApplication.Exit(1);
        }
    }

    private static string[] GetScenes()
    {
        return EditorBuildSettings.scenes
            .Where(scene => scene.enabled)
            .Select(scene => scene.path)
            .ToArray();
    }

    public static void BuildAddressables()
    {
        AddressableAssetSettings.BuildPlayerContent(out var result);

        if (!string.IsNullOrEmpty(result.Error))
        {
            Debug.LogError($"Addressables build failed: {result.Error}");
            UnityEditor.EditorApplication.Exit(1);
        }

        Debug.Log("Addressables build succeeded");
    }
}
