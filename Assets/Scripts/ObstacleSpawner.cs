using System.Collections.Generic;
using UnityEngine;

// Fills each new path segment with obstacles, coins and gems.
// FAIRNESS RULE: every row keeps a "safe lane" that has no obstacle that needs dodging,
// and the safe lane moves at most one lane per row. So a route ALWAYS exists.
public class ObstacleSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject coinPrefab;
    public GameObject gemPrefab;
    public GameObject[] lowObstacles;    // can be jumped over (rock, log, crate)
    public GameObject[] tallObstacles;   // must be dodged (tree, stone block)

    [Header("Layout")]
    public float laneDistance = 2.5f;
    public float segmentLength = 40f;
    public float rowSpacing = 8f;
    public float firstRowOffset = 4f;

    [Header("Difficulty: value at easiest -> value at hardest")]
    public float obstacleChanceStart = 0.35f;
    public float obstacleChanceEnd = 0.85f;
    public float twoBlockedChanceStart = 0f;
    public float twoBlockedChanceEnd = 0.5f;
    public float jumpRowChanceStart = 0.1f;
    public float jumpRowChanceEnd = 0.3f;

    [Header("Collectibles")]
    [Range(0f, 1f)] public float gemChance = 0.25f;
    public float coinHeight = 1f;
    public float gemHeight = 1.2f;

    int safeLane = 1;
    bool restNextRow;

    public void Populate(Transform segment)
    {
        GameManager gm = GameManager.Instance;
        if (gm != null && gm.State == GameState.GameOver) return;
        float d = gm != null ? gm.Difficulty01 : 0f;

        float obstacleChance = Mathf.Lerp(obstacleChanceStart, obstacleChanceEnd, d);
        float twoBlocked = Mathf.Lerp(twoBlockedChanceStart, twoBlockedChanceEnd, d);
        float jumpChance = Mathf.Lerp(jumpRowChanceStart, jumpRowChanceEnd, d);

        int rows = Mathf.FloorToInt((segmentLength - firstRowOffset) / rowSpacing) + 1;
        for (int i = 0; i < rows; i++)
        {
            float z = firstRowOffset + i * rowSpacing;

            bool rest = restNextRow;
            restNextRow = false;
            if (!rest) safeLane = Mathf.Clamp(safeLane + Random.Range(-1, 2), 0, 2);

            bool obstacleRow = !rest && Random.value < obstacleChance;
            if (!obstacleRow)
            {
                SpawnFreePattern(segment, z);
            }
            else
            {
                bool jumpRow = lowObstacles != null && lowObstacles.Length > 0 && Random.value < jumpChance;
                bool blockBoth = Random.value < twoBlocked;

                List<int> others = new List<int>();
                for (int l = 0; l < 3; l++) if (l != safeLane) others.Add(l);
                if (Random.value < 0.5f) { int t = others[0]; others[0] = others[1]; others[1] = t; }
                if (!blockBoth) others.RemoveAt(1);

                foreach (int lane in others) SpawnObstacle(segment, lane, z, Random.value < 0.5f);

                if (jumpRow)
                {
                    SpawnObstacle(segment, safeLane, z, false);   // low obstacle in the safe lane: player must jump
                    SpawnCoinArc(segment, safeLane, z);
                    restNextRow = true;                           // next row stays empty and in the same lane
                }
                else if (Random.value < 0.7f)
                {
                    SpawnCoinLine(segment, safeLane, z);
                }
            }

            // gem sits exactly between two rows, so it never overlaps obstacles or coins
            if (Random.value < gemChance) SpawnGem(segment, Random.Range(0, 3), z + rowSpacing * 0.5f);
        }
    }

    void SpawnFreePattern(Transform seg, float z)
    {
        float r = Random.value;
        if (r < 0.2f) return;
        if (r < 0.55f) SpawnCoinLine(seg, Random.Range(0, 3), z);
        else if (r < 0.8f)
        {
            for (int lane = 0; lane < 3; lane++)
                for (int k = -1; k <= 1; k++) SpawnCoin(seg, lane, z + k * 1.5f, coinHeight);
        }
        else
        {
            int[] zig = { 0, 1, 2, 1, 0 };
            if (Random.value < 0.5f) zig = new int[] { 2, 1, 0, 1, 2 };
            for (int k = 0; k < zig.Length; k++) SpawnCoin(seg, zig[k], z + (k - 2) * 1.5f, coinHeight);
        }
    }

    void SpawnCoinLine(Transform seg, int lane, float z)
    {
        for (int k = -2; k <= 2; k++) SpawnCoin(seg, lane, z + k * 1.5f, coinHeight);
    }

    void SpawnCoinArc(Transform seg, int lane, float z)
    {
        for (int k = -2; k <= 2; k++)
        {
            float t = k / 2f;
            float h = coinHeight + (1f - t * t) * 0.9f;
            SpawnCoin(seg, lane, z + k * 1.4f, h);
        }
    }

    void SpawnCoin(Transform seg, int lane, float z, float y)
    {
        Place(coinPrefab, seg, lane, y, z);
    }

    void SpawnGem(Transform seg, int lane, float z)
    {
        Place(gemPrefab, seg, lane, gemHeight, z);
    }

    void SpawnObstacle(Transform seg, int lane, float z, bool tall)
    {
        GameObject[] pool = tall ? tallObstacles : lowObstacles;
        if (pool == null || pool.Length == 0) pool = tall ? lowObstacles : tallObstacles;
        if (pool == null || pool.Length == 0) return;
        Place(pool[Random.Range(0, pool.Length)], seg, lane, 0f, z);
    }

    void Place(GameObject prefab, Transform seg, int lane, float y, float z)
    {
        if (prefab == null) return;
        GameObject g = Instantiate(prefab, seg);
        g.transform.localPosition = new Vector3((lane - 1) * laneDistance, y, z);
    }
}
