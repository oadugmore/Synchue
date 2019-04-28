using System.Collections.Generic;
using UnityEngine;

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
    private float previousCyclePosition = 0f;


    // Use this for initialization
    private void Start()
    {
        cameraTransform = Camera.main.transform;
        transformObjects = new List<ICCycleObject>(GetComponentsInChildren<ICCycleObject>());
        UpdateObjectPositions();
    }

    private void FixedUpdate()
    {
        if ((cameraTransform.position.x + leftControlBound) > transform.position.x && (cameraTransform.position.x - rightControlBound) < transform.position.x)
        {
            float input = Controller.GetAxis(color);
            cyclePosition += (Time.fixedDeltaTime * input) / cycleTime;
            while (cyclePosition > 1)
            {
                cyclePosition--;
            }

            if (previousCyclePosition != cyclePosition)
            {
                previousCyclePosition = cyclePosition;
                UpdateObjectPositions();
            }
        }
    }

    private void UpdateObjectPositions()
    {
        foreach (ICCycleObject o in transformObjects)
        {
            o.UpdateCyclePosition(cyclePosition);
        }
    }

}
