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
                    buildName += "PlatformerAndroid.apk";
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
            options = BuildOptions.None,
            locationPathName = buildName
        };
        return options;
    }

    /// <summary>
    /// Called by Jenkins. Increments the Android bundle version code and performs build for ARM64.
    /// </summary>
    [MenuItem("CI/Build New Android Version")]
    public static int BuildNewAndroidVersion()
    {
        //IncrementAndroidBundleVersionCode();
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
        var buildResult = PerformAndroidBuild();
        return buildResult;
    }

    /// <summary>
    /// Called by Jenkins. Increments the Android bundle version code and performs a build for all architectures.
    /// </summary>
    [MenuItem("CI/Build New Android Version All Architectures")]
    public static int BuildNewAndroidVersionFull()
    {
        //IncrementAndroidBundleVersionCode();
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.All;
        var buildResult = PerformAndroidBuild();
        return buildResult;
    }

    private static int PerformAndroidBuild()
    {
        PlayerSettings.Android.keystorePass = Secrets.GetAndroidPassword();
        PlayerSettings.Android.keyaliasPass = Secrets.GetAndroidPassword();
        //PlayerSettings.Android.keystoreName = Path.GetFullPath(PlayerSettings.Android.keystoreName);
        var buildOptions = CreatePlayerOptions(BuildTarget.Android);
        var buildReport = BuildPipeline.BuildPlayer(buildOptions);
        return (buildReport.summary.result == BuildResult.Failed) ? 1 : 0;
    }

    [MenuItem("CI/Increment Android Bundle Version Code")]
    public static int IncrementAndroidBundleVersionCode()
    {
        PlayerSettings.Android.bundleVersionCode++;
        return 0;
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
