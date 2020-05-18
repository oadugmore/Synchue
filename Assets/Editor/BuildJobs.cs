using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class BuildJobs : ScriptableObject
{

    private static BuildPlayerOptions CreatePlayerOptions(BuildTarget target)
    {
        // Set app path
        string buildName = "Builds/";
        switch (target)
        {
            case BuildTarget.Android:
                {
                    buildName += "PlatformerAndroid.aab";
                    break;
                }
            case BuildTarget.iOS:
                {
                    buildName += "PlatformeriOS";
                    break;
                }
            default:
                {
                    buildName += "Platformer";
                    break;
                }
        }

        // Create Build Player Options
        var options = new BuildPlayerOptions
        {
            scenes = FindEnabledEditorScenes(),
            target = target,
            options = target == BuildTarget.iOS ?
                BuildOptions.AcceptExternalModificationsToPlayer | BuildOptions.CompressWithLz4HC :
                BuildOptions.CompressWithLz4HC,
            locationPathName = buildName
        };
        return options;
    }

    /// <summary>
    /// Called by Jenkins. Performs a build for ARM64.
    /// </summary>
    [MenuItem("CI/Build New Android Version ARM64")]
    public static int BuildNewAndroidVersion()
    {
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
        var buildResult = PerformAndroidBuild();
        return buildResult;
    }

    /// <summary>
    /// Called by Jenkins. Performs a build for all architectures.
    /// </summary>
    [MenuItem("CI/Build New Android Version All Architectures")]
    public static int BuildNewAndroidVersionFull()
    {
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.All;
        var buildResult = PerformAndroidBuild();
        return buildResult;
    }

    private static int PerformAndroidBuild()
    {
        PlayerSettings.Android.keystorePass = Environment.GetEnvironmentVariable("ANDROID_KEY_PASSWORD");
        PlayerSettings.Android.keyaliasPass = Environment.GetEnvironmentVariable("ANDROID_KEY_PASSWORD");
        EditorUserBuildSettings.buildAppBundle = true;
        EditorUserBuildSettings.androidCreateSymbolsZip = false;
        var buildOptions = CreatePlayerOptions(BuildTarget.Android);
        var buildReport = BuildPipeline.BuildPlayer(buildOptions);
        return (buildReport.summary.result == BuildResult.Failed) ? 1 : 0;
    }

    /// <summary>
    /// Called by Jenkins. Performs a build for iOS.
    /// </summary>
    [MenuItem("CI/Build New iOS Version")]
    public static int BuildNewiOSVersion()
    {
        var buildOptions = CreatePlayerOptions(BuildTarget.iOS);
        var buildReport = BuildPipeline.BuildPlayer(buildOptions);
        return (buildReport.summary.result == BuildResult.Failed) ? 1 : 0;
    }

    private static string[] FindEnabledEditorScenes()
    {
        List<string> EditorScenes = new List<string>();
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (scene.enabled)
            {
                EditorScenes.Add(scene.path);
            }
        }
        return EditorScenes.ToArray();
    }

}
