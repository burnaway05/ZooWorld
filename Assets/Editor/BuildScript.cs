using UnityEditor.Build.Reporting;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Build;
using UnityEditor;
using System.Linq;
using UnityEngine;
using UnityEditor.AddressableAssets;

public static class BuildScript
{
    public static void BuildWindowsAndResources()
    {
        AddressableAssetSettings.BuildPlayerContent(out var addressablesResult);

        if (!string.IsNullOrEmpty(addressablesResult.Error))
        {
            Debug.LogError($"Addressables build failed: {addressablesResult.Error}");

            EditorApplication.Exit(1);
            return;
        }

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
        var settings = AddressableAssetSettingsDefaultObject.Settings;

        Debug.Log($"Active Addressables profile ID: {settings.activeProfileId}");
        Debug.Log($"Active Addressables profile: " + $"{settings.profileSettings.GetProfileName(settings.activeProfileId)}");

        AddressableAssetSettings.BuildPlayerContent(out var result);

        if (!string.IsNullOrEmpty(result.Error))
        {
            Debug.LogError($"Addressables build failed: {result.Error}");
            UnityEditor.EditorApplication.Exit(1);
        }

        Debug.Log("Addressables build succeeded");

        EditorApplication.Exit(0);
    }

    public static void BuildAddressablesUpdate()
    {
        string statePath = System.IO.Path.Combine(System.Environment.CurrentDirectory, "ContentState", "addressables_content_state.bin");

        Debug.Log($"[Addressables] Content update using: {statePath}");

        if (!System.IO.File.Exists(statePath))
        {
            Debug.LogError($"State file not found: {statePath}");
            EditorApplication.Exit(1);
            return;
        }

        var result = ContentUpdateScript.BuildContentUpdate(AddressableAssetSettingsDefaultObject.Settings, statePath);

        if (!string.IsNullOrEmpty(result.Error))
        {
            Debug.LogError($"[Addressables] Content update failed: {result.Error}");
            EditorApplication.Exit(1);
            return;
        }

        Debug.Log("[Addressables] Content update succeeded");
        EditorApplication.Exit(0);
    }
}
