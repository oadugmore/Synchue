using System;
using UnityEngine;

public class Controller : MonoBehaviour
{

    private static Controller current = null;

    public ColorButton blueButton;
    public ColorButton orangeButton;
    public float increment = 0.1f;
    public bool forceTouchInput = false;

    private bool keyboardInput = false;
    private float blueAxis = 0f;
    private float orangeAxis = 0f;
    private const float whiteAxis = 1f; // constant input


    // Use this for initialization
    private void Start()
    {
        if (current == null)
        {
            current = this;
        }
        else
        {
            Debug.LogError("Only one instance of Controller is allowed.");
        }

        #if UNITY_EDITOR
        if (!forceTouchInput)
        {
            keyboardInput = true;
            Debug.Log("Running in the Unity Editor. Enabling keyboard input.");
        }
        else
        {
            Debug.Log("Running in the Unity Editor with touch input enabled.");
        }
        #endif
    }

    // put this logic in FixedUpdate to synchronize with physics
    private void FixedUpdate()
    {
        if (keyboardInput)
        {
            GetKeyboardInput();
        }
        else
        {
            GetTouchInput();
        }

        blueAxis = Mathf.Clamp(blueAxis, 0f, 1f);
        orangeAxis = Mathf.Clamp(orangeAxis, 0f, 1f);
    }

    private void GetKeyboardInput()
    {
        if (Input.GetKey(KeyCode.J))
        {
            blueAxis += increment;
        }
        else
        {
            blueAxis -= increment;
        }

        if (Input.GetKey(KeyCode.F))
        {
            orangeAxis += increment;
        }
        else
        {
            orangeAxis -= increment;
        }
    }

    private void GetTouchInput()
    {
        if (current.blueButton.IsPressed())
        {
            blueAxis += increment;
        }
        else
        {
            blueAxis -= increment;
        }

        if (current.orangeButton.IsPressed())
        {
            orangeAxis += increment;
        }
        else
        {
            orangeAxis -= increment;
        }
    }


    public static float GetAxis(InteractColor color)
    {
        switch (color)
        {
            case InteractColor.Blue:
                return current.blueAxis;
            case InteractColor.Orange:
                return current.orangeAxis;
            case InteractColor.White:
                return whiteAxis;
            default:
                throw new NotImplementedException("Update controller to support " + color + ".");
        }
    }

}

public enum InteractColor
{
    Blue, Orange, White
}
