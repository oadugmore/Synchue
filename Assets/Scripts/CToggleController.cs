using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class CToggleController : MonoBehaviour
{
    [SerializeField][Tooltip("Whether to use toggle or hold mode.")]
    private bool holdToActivate;
    [SerializeField][Tooltip("The color of cycle object to control.")]
    private InteractColor color;
    [SerializeField][Tooltip("Ignore controller input outside of this range.")]
    private float leftControlBound = 15f;
    [SerializeField][Tooltip("Ignore controller input outside of this range.")]
    private float rightControlBound = 15f;
    [SerializeField][Tooltip("The current toggle status of the controller.")]
    private bool on = false;

    private List<ICToggleObject> toggleObjects;
    private Transform playerTransform;
    private bool lastButtonStatus = false;
    private bool previousStatus = false;

    private void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        toggleObjects = new List<ICToggleObject>(GetComponentsInChildren<ICToggleObject>());
        UpdateObjectStates(false);
    }

    private void FixedUpdate()
    {
        // Check if status should respond to controller
        if ((playerTransform.position.x + leftControlBound) > transform.position.x && (playerTransform.position.x - rightControlBound) < transform.position.x)
        {
            var input = Controller.GetButton(color);
            if (holdToActivate)
            {
                on = input;
            }
            else if (lastButtonStatus != input)
            {
                // if this is a new button press, toggle status
                on = input ? !on : on;
                lastButtonStatus = input;
            }
            //cyclePosition += (Time.fixedDeltaTime * input) / cycleTime;
            //cyclePosition = Mathf.Repeat(cyclePosition, 1);
        }

        // Check if status changed. It could be changed by controller or through the editor
        if (on != previousStatus)
        {
            UpdateObjectStates(on);
            previousStatus = on;
        }
    }

    private void UpdateObjectStates(bool on)
    {
        foreach (var o in toggleObjects)
        {
            o.Toggle(on);
        }
    }
}
