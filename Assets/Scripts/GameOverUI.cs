using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    public GameObject panel;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI gemsText;
    public TextMeshProUGUI distanceText;
    public TextMeshProUGUI bestText;
    public Button retryButton;
    public Button mainMenuButton;

    void Start()
    {
        if (panel != null) panel.SetActive(false);
        if (retryButton != null) retryButton.onClick.AddListener(OnRetry);
        if (mainMenuButton != null) mainMenuButton.onClick.AddListener(OnMainMenu);
        if (GameManager.Instance != null) GameManager.Instance.OnGameOver += Show;
    }

    void OnDestroy()
    {
        if (GameManager.Instance != null) GameManager.Instance.OnGameOver -= Show;
    }

    void Show()
    {
        GameManager gm = GameManager.Instance;
        if (panel != null) panel.SetActive(true);
        if (scoreText != null) scoreText.text = "Score: " + gm.Score;
        if (coinsText != null) coinsText.text = "Coins: " + gm.Coins;
        if (gemsText != null) gemsText.text = "Gems: " + gm.Gems;
        if (distanceText != null) distanceText.text = "Distance: " + Mathf.FloorToInt(gm.Distance) + " m";
        if (bestText != null) bestText.text = "Best Score: " + gm.BestScore;
    }

    void OnRetry() { GameManager.Instance.Restart(); }
    void OnMainMenu() { GameManager.Instance.GoToMainMenu(); }
}
