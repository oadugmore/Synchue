using System;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    public ColorButton blueButton;
    public ColorButton orangeButton;
    public float buttonSpeed = 7f;
    public bool useTouchInputInEditor;

    private static Controller current;
    private bool keyboardInput;
    private float blueAxis;
    private bool blueOn;
    private float orangeAxis;
    private bool orangeOn;
    private const float whiteAxis = 1f; // constant input
    private const bool whiteOn = true;
    private WorldColorButton[] purpleButtons;
    private float purpleAxis;
    private bool purpleOn;

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

        purpleButtons = FindObjectsOfType<WorldColorButton>();

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
        GetInput();
    }

    private void GetInput()
    {
        var change = buttonSpeed * Time.deltaTime;
        if (!keyboardInput && blueButton.isPressed || keyboardInput && Input.GetKey(KeyCode.J))
        {
            blueAxis += change;
            blueOn = true;
        }
        else
        {
            blueAxis -= change;
            blueOn = false;
        }

        if (!keyboardInput && orangeButton.isPressed || keyboardInput && Input.GetKey(KeyCode.F))
        {
            orangeAxis += change;
            orangeOn = true;
        }
        else
        {
            orangeAxis -= change;
            orangeOn = false;
        }

        if (AnyWorldButtonPressed())
        {
            purpleAxis += change;
            purpleOn = true;
        }
        else
        {
            purpleAxis -= change;
            purpleOn = false;
        }

        blueAxis = Mathf.Clamp(blueAxis, 0f, 1f);
        orangeAxis = Mathf.Clamp(orangeAxis, 0f, 1f);
        purpleAxis = Mathf.Clamp(purpleAxis, 0f, 1f);
    }

    private bool AnyWorldButtonPressed()
    {
        foreach (var button in purpleButtons)
        {
            if (button.isPressed)
            {
                return true;
            }
        }
        return false;
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
            case InteractColor.Purple:
                return current.purpleAxis;
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
            case InteractColor.Purple:
                return current.purpleOn;
            default:
                throw new NotImplementedException("Update controller to support " + color + ".");
        }
    }

}

public enum InteractColor
{
    Blue, Orange, White, Purple
}
