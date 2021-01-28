using UnityEngine;

public class FollowTrackingCamera : MonoBehaviour
{
    // Camera target to look at.
    public Transform target;

    // Restrict movement
    public bool lockVerticalMovement;
    public bool lockHorizontalMovement;

    // Camera limits.
    public float min = 10f;
    public float max = 60;

    // Rotation.
    public float rotateSpeed = 1f;

    // Options.
    public bool doRotate;
    public bool doZoom;

    // The movement amount when zooming.
    public float zoomStep = 30f;
    public float zoomSpeed = 5f;
    private float heightWanted;
    private float distanceWanted;

    // Result vectors.
    private Vector3 zoomResult;
    private Quaternion rotationResult;
    private Vector3 targetAdjustedPosition;

    // Get these from initial transform position instead
    private float xPos;
    private float height;
    private float distance;

    private void Start()
    {
        // Initialise default zoom vals.
        xPos = transform.position.x;
        heightWanted = height = transform.position.y;
        distanceWanted = distance = -transform.position.z;

        // Setup our default camera.  We set the zoom result to be our default position.
        zoomResult = new Vector3(xPos, height, -distance);
    }

    private void LateUpdate()
    {
        // Check target.
        if (!target)
        {
            Debug.LogError("This camera has no target, you need to assign a target in the inspector.");
            return;
        }

        if (doZoom)
        {
            // Record our mouse input.  If we zoom add this to our height and distance.
            float mouseInput = Input.GetAxis("Mouse ScrollWheel");
            heightWanted -= zoomStep * mouseInput;
            distanceWanted -= zoomStep * mouseInput;

            // Make sure they meet our min/max values.
            heightWanted = Mathf.Clamp(heightWanted, min, max);
            distanceWanted = Mathf.Clamp(distanceWanted, min, max);

            height = Mathf.Lerp(height, heightWanted, Time.deltaTime * zoomSpeed);
            distance = Mathf.Lerp(distance, distanceWanted, Time.deltaTime * zoomSpeed);

            // Post our result.
            zoomResult = new Vector3(0f, height, -distance);
        }

        if (doRotate)
        {
            // Work out the current and wanted rots.
            float currentRotationAngle = transform.eulerAngles.y;
            float wantedRotationAngle = target.eulerAngles.y;

            // Smooth the rotation.
            currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotateSpeed * Time.deltaTime);

            // Convert the angle into a rotation.
            rotationResult = Quaternion.Euler(0f, currentRotationAngle, 0f);
        }

        // Set the camera position reference.
        targetAdjustedPosition = rotationResult * zoomResult;
        transform.position = new Vector3(
            lockHorizontalMovement ? transform.position.x : targetAdjustedPosition.x + target.position.x,
            lockVerticalMovement ? transform.position.y : targetAdjustedPosition.y + target.position.y,
            targetAdjustedPosition.z + target.position.z);

        // Face the desired position.
        //transform.LookAt(target);
    }
}
