using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class CCycleController : MonoBehaviour
{
    [SerializeField]
    private float cycleTime;
    [SerializeField]
    private InteractColor color;
    [SerializeField]
    private float leftControlBound = 15f;
    [SerializeField]
    private float rightControlBound = 30f;
    [SerializeField]
    [Range(0f, 1f)]
    private float cyclePosition = 0f;

    private List<ICCycleObject> transformObjects;
    private Transform cameraTransform;
    private float previousCyclePos = 0f;


    // Use this for initialization
    private void Start()
    {
        cameraTransform = Camera.main.transform;
        transformObjects = new List<ICCycleObject>(GetComponentsInChildren<ICCycleObject>());
        Assert.AreNotEqual(cycleTime, 0f);
        UpdateObjectPositions();
    }

    private void FixedUpdate()
    {
        var input = Controller.GetAxis(color);
        cyclePosition += (Time.fixedDeltaTime * input) / cycleTime;
        if (cyclePosition != previousCyclePos && (cameraTransform.position.x + leftControlBound) > transform.position.x && (cameraTransform.position.x - rightControlBound) < transform.position.x)
        {
            previousCyclePos = cyclePosition;
            cyclePosition = Mathf.Repeat(cyclePosition, 1);
            UpdateObjectPositions();
        }
    }

    private void UpdateObjectPositions()
    {
        foreach (var o in transformObjects)
        {
            o.UpdateCyclePosition(cyclePosition);
        }
    }

}
