using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentralizedTransformationController : MonoBehaviour
{
    [SerializeField]
    float cycleTime;
    [SerializeField]
    InteractColor color;
    [SerializeField]
    float leftControlBound = 15f;
    [SerializeField]
    float rightControlBound = 30f;
    [SerializeField]
    [Range(0f, 1f)]
    float cyclePosition = 0f;

    List<CentralizedTransformationObject> transformObjects;
    Transform cameraTransform;
    float previousCyclePosition = 0f;


    // Use this for initialization
    void Start()
    {
        cameraTransform = Camera.main.transform;
        transformObjects = new List<CentralizedTransformationObject>(GetComponentsInChildren<CentralizedTransformationObject>());
    }

    private void FixedUpdate()
    {
        if ((cameraTransform.position.x + leftControlBound) > transform.position.x && (cameraTransform.position.x - rightControlBound) < transform.position.x)
        {
            float input = Controller.GetAxis(color);
            cyclePosition += (Time.fixedDeltaTime * input) / cycleTime;
            while (cyclePosition > 1)
                cyclePosition--;

            if (previousCyclePosition != cyclePosition)
            {
				previousCyclePosition = cyclePosition;
                foreach (CentralizedTransformationObject o in transformObjects)
                {
                    o.UpdateCyclePosition(cyclePosition);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
