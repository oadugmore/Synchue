using System;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{

    private static Controller current = null;

    public ColorButton blueButton;
    public ColorButton orangeButton;
    public float buttonSpeed = 7f;
    public bool useTouchInputInEditor = false;

    private bool keyboardInput = false;
    private float blueAxis = 0f;
    private bool blueOn = false;
    private float orangeAxis = 0f;
    private bool orangeOn = false;
    private const float whiteAxis = 1f; // constant input
    private const bool whiteOn = true;

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
        if (!useTouchInputInEditor)
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

    private void Update()
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
        var change = buttonSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.J))
        {
            blueAxis += change;
            blueOn = true;
        }
        else
        {
            blueAxis -= change;
            blueOn = false;
        }

        if (Input.GetKey(KeyCode.F))
        {
            orangeAxis += change;
            orangeOn = true;
        }
        else
        {
            orangeAxis -= change;
            orangeOn = false;
        }
    }

    private void GetTouchInput()
    {
        var change = buttonSpeed * Time.deltaTime;
        if (current.blueButton.IsPressed())
        {
            blueAxis += change;
            blueOn = true;
        }
        else
        {
            blueAxis -= change;
            blueOn = false;
        }

        if (current.orangeButton.IsPressed())
        {
            orangeAxis += change;
            orangeOn = true;
        }
        else
        {
            orangeAxis -= change;
            orangeOn = false;
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

    public static bool GetButton(InteractColor color)
    {
        switch (color)
        {
            case InteractColor.Blue:
                return current.blueOn;
            case InteractColor.Orange:
                return current.orangeOn;
            case InteractColor.White:
                return whiteOn;
            default:
                throw new NotImplementedException("Update controller to support " + color + ".");
        }
    }

}

public enum InteractColor
{
    Blue, Orange, White
}
