using System;
using System.Collections;
using System.Collections.Generic;
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
                    PlayerSettings.Android.keystorePass = Secrets.GetAndroidPassword();
                    PlayerSettings.Android.keyaliasPass = Secrets.GetAndroidPassword();
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
    /// Called by Jenkins. Increments the Android bundle version code and performs a build.
    /// </summary>
    [MenuItem("CI/Build New Android Version")]
    public static int BuildNewAndroidVersion()
    {
        IncrementAndroidBundleVersionCode();
        var buildResult = PerformAndroidBuild();
        return buildResult;
    }

    /// <summary>
    /// Called by Jenkins.
    /// </summary>
    [MenuItem("CI/Build Android")]
    public static int PerformAndroidBuild()
    {
        var options = CreatePlayerOptions(BuildTarget.Android);
        var report = BuildPipeline.BuildPlayer(options);
        var summary = report.summary;
        if (summary.result == BuildResult.Failed)
        {
            return 1;
        }
        return 0;
    }

    public static void IncrementAndroidBundleVersionCode()
    {
        PlayerSettings.Android.bundleVersionCode++;
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
