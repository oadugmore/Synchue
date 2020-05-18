using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerSpeedDisplay : MonoBehaviour
{

    public Text speedText;

    private float previousBlueAxis = 0f;
    private float timeLeavingZero = 0f;
    private bool reachedZero = false;

    void Update()
    {
        var blueAxis = Controller.GetAxis(InteractColor.Blue);
        if (reachedZero && previousBlueAxis < blueAxis && blueAxis == 1f)
        {
            reachedZero = false;
            speedText.text = (Time.time - timeLeavingZero).ToString();
        }
        else if (previousBlueAxis == 0f && blueAxis > 0f)
        {
            timeLeavingZero = Time.time;
            reachedZero = true;
        }
        previousBlueAxis = blueAxis;
    }
}
