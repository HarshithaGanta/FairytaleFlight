using UnityEngine;

// Gentle idle animation for the princess on the main menu.
public class IdleBob : MonoBehaviour
{
    public float bobHeight = 0.05f;
    public float bobSpeed = 2f;
    public float swayDegrees = 12f;
    Vector3 startPos;
    float startYaw;

    void Start()
    {
        startPos = transform.position;
        startYaw = transform.eulerAngles.y;
    }

    void Update()
    {
        transform.position = startPos + Vector3.up * (Mathf.Sin(Time.unscaledTime * bobSpeed) * bobHeight);
        transform.rotation = Quaternion.Euler(0f, startYaw + Mathf.Sin(Time.unscaledTime * 0.8f) * swayDegrees, 0f);
    }
}
