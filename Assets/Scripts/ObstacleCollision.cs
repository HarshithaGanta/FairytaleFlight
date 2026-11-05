using UnityEngine;

// Put on every obstacle prefab. The obstacle's collider must have "Is Trigger" ticked.
public class ObstacleCollision : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (GameManager.Instance != null) GameManager.Instance.GameOver();
    }
}
