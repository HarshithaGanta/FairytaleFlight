using UnityEngine;

public class CoinCollection : MonoBehaviour
{
    public float rotateSpeed = 180f;
    public GameObject collectEffect;

    void Update()
    {
        transform.Rotate(0f, rotateSpeed * Time.deltaTime, 0f, Space.World);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (GameManager.Instance != null) GameManager.Instance.AddCoin();
        AudioManager.Coin();
        if (collectEffect != null) Instantiate(collectEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
