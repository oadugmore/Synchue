using UnityEngine;

public class AlterGravity : MonoBehaviour
{
    public Vector3 gravity;
    private Vector3 originalGravity;

    private void OnEnable()
    {
        originalGravity = Physics.gravity;
        Physics.gravity = gravity;
    }

    private void OnDisable()
    {
        Physics.gravity = originalGravity;
    }
}
