using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0f, 4.2f, -7f);
    public Vector3 lookAtOffset = new Vector3(0f, 1.2f, 6f);
    public float fieldOfView = 60f;

    [Header("Smoothing")]
    [Range(0f, 1f)] public float laneFollowAmount = 0.6f;
    public float xSmoothSpeed = 6f;
    [Range(0f, 1f)] public float yFollowAmount = 0.5f;
    public float ySmoothSpeed = 5f;

    void Start()
    {
        Camera cam = GetComponent<Camera>();
        if (cam != null) cam.fieldOfView = fieldOfView;
        if (target != null)
        {
            transform.position = DesiredPosition();
            transform.rotation = Quaternion.LookRotation(LookPoint(transform.position) - transform.position);
        }
    }

    Vector3 DesiredPosition()
    {
        return new Vector3(target.position.x * laneFollowAmount + offset.x,
                           target.position.y * yFollowAmount + offset.y,
                           target.position.z + offset.z);
    }

    Vector3 LookPoint(Vector3 camPos)
    {
        return new Vector3(camPos.x, target.position.y * yFollowAmount, target.position.z) + lookAtOffset;
    }

    void LateUpdate()
    {
        if (target == null) return;
        Vector3 desired = DesiredPosition();
        Vector3 p = transform.position;
        p.x = Mathf.Lerp(p.x, desired.x, 1f - Mathf.Exp(-xSmoothSpeed * Time.deltaTime));
        p.y = Mathf.Lerp(p.y, desired.y, 1f - Mathf.Exp(-ySmoothSpeed * Time.deltaTime));
        p.z = desired.z;                       // exact Z = no shaking while running
        transform.position = p;
        transform.rotation = Quaternion.LookRotation(LookPoint(p) - p);
    }
}
