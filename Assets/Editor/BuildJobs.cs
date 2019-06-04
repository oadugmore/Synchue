using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class BuildJobs
{

    private static BuildPlayerOptions CreatePlayerOptions(BuildTarget target)
    {
        string buildName = "Builds/";
        switch (target)
        {
            case BuildTarget.Android:
                buildName += "PlatformerAndroid.apk";
                break;
            case BuildTarget.iOS:
                buildName += "PlatformeriOS";
                break;
            default:
                buildName += "Platformer";
                break;
        }
        var options = new BuildPlayerOptions
        {
            scenes = FindEnabledEditorScenes(),
            target = target,
            options = BuildOptions.None,
            locationPathName = buildName
        };
        return options;
    }


    [MenuItem("Jenkins/Build Android")]
    static int PerformAndroidBuild()
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
