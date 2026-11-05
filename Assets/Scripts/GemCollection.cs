using UnityEngine;

public class GemCollection : MonoBehaviour
{
    public float rotateSpeed = 120f;
    public float floatHeight = 0.2f;
    public float floatSpeed = 2.5f;
    public GameObject collectEffect;

    float baseY;
    float phase;

    void Start()
    {
        baseY = transform.localPosition.y;
        phase = Random.value * 6.28f;
    }

    void Update()
    {
        Vector3 p = transform.localPosition;
        p.y = baseY + Mathf.Sin(Time.time * floatSpeed + phase) * floatHeight;
        transform.localPosition = p;
        transform.Rotate(0f, rotateSpeed * Time.deltaTime, 0f, Space.World);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (GameManager.Instance != null) GameManager.Instance.AddGem();
        AudioManager.Gem();
        if (collectEffect != null) Instantiate(collectEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
