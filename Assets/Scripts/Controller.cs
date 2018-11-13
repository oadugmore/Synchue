using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{

    private static Controller current = null;

    public ColorButton blueButton;
    public ColorButton orangeButton;
    public float increment = 0.1f;

    private float blueAxis = 0f;
    private float orangeAxis = 0f;


    // Use this for initialization
    void Start()
    {
        if (current == null)
            current = this;
        else
        {
            Debug.LogError("Only one instance of Controller is allowed.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // put this logic in FixedUpdate to synchronize with physics
    private void FixedUpdate() 
    {
        if (current.blueButton.IsPressed())
            blueAxis += increment;
        else
            blueAxis -= increment;

        if (current.orangeButton.IsPressed())
            orangeAxis += increment;
        else
            orangeAxis -= increment;
            
        blueAxis = Mathf.Clamp(blueAxis, 0f, 1f);
        orangeAxis = Mathf.Clamp(orangeAxis, 0f, 1f);
    }

    // private void SetButtonDown(int buttonId)
    // {
    //     if (buttonId == 1)
    //         blueButtonPressed = true;
    //     else if (buttonId == 2)
    //         orangeButtonPressed = true;
    // }

    // private void SetButtonUp(int buttonId)
    // {
    //     if (buttonId == 1)
    //         blueButtonPressed = false;
    //     else if (buttonId == 2)
    //         orangeButtonPressed = false;
    // }

    // public static bool GetBlueButtonDown()
    // {
    //     return current.blueButton.IsPressed();
    // }

    // public static bool GetOrangeButtonDown()
    // {
    //     return current.orangeButton.IsPressed();
    // }

    public static float GetBlueAxis()
    {
        return current.blueAxis;
    }

    public static float GetOrangeAxis()
    {
        return current.orangeAxis;
    }


}
