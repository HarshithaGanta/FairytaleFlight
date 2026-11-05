using UnityEngine;

// Randomly places trees and flowers on both sides of a path segment.
public class SegmentDecorator : MonoBehaviour
{
    public GameObject[] trees;
    public GameObject[] flowers;
    public float segmentLength = 40f;
    public int treesPerSide = 5;
    public int flowersPerSide = 14;

    void Start()
    {
        for (int side = -1; side <= 1; side += 2)
        {
            Scatter(trees, treesPerSide, side, 8f, 18f, 0.8f, 1.5f);
            Scatter(flowers, flowersPerSide, side, 5.3f, 8f, 0.8f, 1.4f);
        }
    }

    void Scatter(GameObject[] prefabs, int count, int side, float minX, float maxX, float minScale, float maxScale)
    {
        if (prefabs == null || prefabs.Length == 0) return;
        for (int i = 0; i < count; i++)
        {
            GameObject prefab = prefabs[Random.Range(0, prefabs.Length)];
            if (prefab == null) continue;
            GameObject g = Instantiate(prefab, transform);
            g.transform.localPosition = new Vector3(side * Random.Range(minX, maxX), -0.05f, Random.Range(0f, segmentLength));
            g.transform.localRotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
            g.transform.localScale = Vector3.one * Random.Range(minScale, maxScale);
        }
    }
}
