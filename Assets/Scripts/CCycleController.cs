using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[AddComponentMenu(Constants.centralizedControlMenuName + "/Cycle Controller")]
public class CCycleController : MonoBehaviour
{
    [SerializeField][Tooltip("The time in seconds it takes to complete one cycle.")]
    private float cycleTime = 1f;
    [SerializeField][Tooltip("The color of cycle object to control.")]
    private InteractColor color;
    [SerializeField][Tooltip("Ignore controller input outside of this range.")]
    private float leftControlBound = 15f;
    [SerializeField][Tooltip("Ignore controller input outside of this range.")]
    private float rightControlBound = 15f;
    [SerializeField] [Range(0f, 1f)][Tooltip("The current percent in the cycle that this controller is at.")]
    private float cyclePosition = 0f;

    private List<ICCycleObject> cycleObjects;
    private Transform playerTransform;
    private float previousCyclePos = 0f;

    private void Start()
    {
        if (cycleTime == 0f)
        {
            Debug.LogError("Cycle Time should be greater than zero.");
            return;
        }
        playerTransform = GameObject.FindWithTag("Player").transform;
        cycleObjects = new List<ICCycleObject>(GetComponentsInChildren<ICCycleObject>());
        UpdateObjectPositions();
    }

    private void FixedUpdate()
    {
        // Check if cyclePosition should respond to controller
        if ((playerTransform.position.x + leftControlBound) > transform.position.x && (playerTransform.position.x - rightControlBound) < transform.position.x)
        {
            var input = Controller.GetAxis(color);
            cyclePosition += (Time.deltaTime * input) / cycleTime;
            cyclePosition = Mathf.Repeat(cyclePosition, 1);
        }

        // Check if cyclePosition changed. It could be changed by controller or through the editor
        if (cyclePosition != previousCyclePos)
        {
            UpdateObjectPositions();
            previousCyclePos = cyclePosition;
        }
    }

    private void UpdateObjectPositions()
    {
        foreach (var o in cycleObjects)
        {
            o.UpdateCyclePosition(cyclePosition);
        }
    }
}
