using System.Collections.Generic;
using UnityEngine;

public class EndlessPathGenerator : MonoBehaviour
{
    public GameObject segmentPrefab;
    public ObstacleSpawner spawner;
    public Transform player;

    public float segmentLength = 40f;
    public int segmentsAhead = 6;
    public float despawnBehind = 40f;
    public float firstSegmentZ = -20f;
    public int emptyStartSegments = 2;     // no obstacles at the very start

    readonly List<GameObject> segments = new List<GameObject>();
    float nextSpawnZ;
    int spawnedCount;

    void Start()
    {
        nextSpawnZ = firstSegmentZ;
        for (int i = 0; i < segmentsAhead; i++) SpawnSegment();
    }

    void Update()
    {
        if (player == null || segmentPrefab == null) return;
        GameManager gm = GameManager.Instance;
        if (gm != null && gm.State == GameState.GameOver) return;   // stop generating after Game Over

        while (player.position.z + segmentsAhead * segmentLength > nextSpawnZ) SpawnSegment();

        while (segments.Count > 0 && segments[0].transform.position.z + segmentLength < player.position.z - despawnBehind)
        {
            Destroy(segments[0]);
            segments.RemoveAt(0);
        }
    }

    void SpawnSegment()
    {
        GameObject seg = Instantiate(segmentPrefab, new Vector3(0f, 0f, nextSpawnZ), Quaternion.identity, transform);
        seg.name = "Segment_" + spawnedCount;
        if (spawner != null && spawnedCount >= emptyStartSegments) spawner.Populate(seg.transform);
        segments.Add(seg);
        nextSpawnZ += segmentLength;
        spawnedCount++;
    }
}
