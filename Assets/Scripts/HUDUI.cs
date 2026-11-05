using TMPro;
using UnityEngine;

public class HUDUI : MonoBehaviour
{
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI gemText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI distanceText;

    void Update()
    {
        GameManager gm = GameManager.Instance;
        if (gm == null) return;
        if (coinText != null) coinText.text = gm.Coins.ToString();
        if (gemText != null) gemText.text = gm.Gems.ToString();
        if (scoreText != null) scoreText.text = gm.Score.ToString();
        if (distanceText != null) distanceText.text = Mathf.FloorToInt(gm.Distance) + " m";
    }
}
