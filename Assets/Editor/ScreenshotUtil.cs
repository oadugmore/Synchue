using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ScreenshotUtil : MonoBehaviour
{
    public class TakeScreenshot
{
 
    [MenuItem("Tools/Take Screenshot")]
    static public void OnTakeScreenshot()
    {
        // Application.CaptureScreenshot(EditorUtility.SaveFilePanel("Save Screenshot As", "", "", "png"));
        ScreenCapture.CaptureScreenshot(EditorUtility.SaveFilePanel("Save Screenshot As", "", "", "png"));
    }
}

}
