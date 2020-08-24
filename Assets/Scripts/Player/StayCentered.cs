using UnityEngine;

public class StayCentered : MonoBehaviour
{
    public float effort = 3;

    private new Rigidbody rigidbody;
    private float centerZ;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        centerZ = rigidbody.position.z;
    }

    private void FixedUpdate()
    {
        var zPos = Mathf.MoveTowards(rigidbody.position.z, centerZ, effort * Time.deltaTime);
        rigidbody.MovePosition(new Vector3(rigidbody.position.x, rigidbody.position.y, zPos));
    }
}
