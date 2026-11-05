using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    public Button startButton;
    public Button instructionsButton;
    public Button exitButton;
    public Button closeInstructionsButton;
    public GameObject instructionsPanel;
    public string gameSceneName = "Game";

    void Start()
    {
        Time.timeScale = 1f;
        if (instructionsPanel != null) instructionsPanel.SetActive(false);
        if (startButton != null) startButton.onClick.AddListener(StartGame);
        if (instructionsButton != null) instructionsButton.onClick.AddListener(() => instructionsPanel.SetActive(true));
        if (closeInstructionsButton != null) closeInstructionsButton.onClick.AddListener(() => instructionsPanel.SetActive(false));
        if (exitButton != null) exitButton.onClick.AddListener(ExitGame);
    }

    void StartGame() { SceneManager.LoadScene(gameSceneName); }

    void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
