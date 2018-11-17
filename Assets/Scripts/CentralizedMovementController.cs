using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentralizedMovementController : MonoBehaviour
{
    [SerializeField]
    float cycleTime;
    [SerializeField]
    InteractColor color;
    [SerializeField]
    float controlDistance = 30f;
    [SerializeField]
    [Range(0f, 1f)]
    float cyclePosition = 0f;

    List<CentralizedMovementObject> movementObjects;
    Transform cameraTransform;
    float previousCyclePosition = 0f;


    // Use this for initialization
    void Start()
    {
        cameraTransform = Camera.main.transform;
        movementObjects = new List<CentralizedMovementObject>();
        GetComponentsInChildren<CentralizedMovementObject>(movementObjects);
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(cameraTransform.position, transform.position) < controlDistance)
        {
            float input = Controller.GetAxis(color);
            cyclePosition += (Time.fixedDeltaTime * input) / cycleTime;
            while (cyclePosition > 1)
                cyclePosition--;

            if (previousCyclePosition != cyclePosition)
            {
				previousCyclePosition = cyclePosition;
                foreach (CentralizedMovementObject o in movementObjects)
                {
                    o.UpdatePosition(cyclePosition);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
