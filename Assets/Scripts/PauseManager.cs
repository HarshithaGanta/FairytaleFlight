using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel;
    public Button pauseButton;
    public Button resumeButton;
    public Button restartButton;
    public Button mainMenuButton;

    void Start()
    {
        if (pausePanel != null) pausePanel.SetActive(false);
        if (pauseButton != null) pauseButton.onClick.AddListener(Pause);
        if (resumeButton != null) resumeButton.onClick.AddListener(Resume);
        if (restartButton != null) restartButton.onClick.AddListener(() => GameManager.Instance.Restart());
        if (mainMenuButton != null) mainMenuButton.onClick.AddListener(() => GameManager.Instance.GoToMainMenu());
    }

    void Update()
    {
        if (!InputHelper.EscapePressed() || GameManager.Instance == null) return;
        if (GameManager.Instance.State == GameState.Playing) Pause();
        else if (GameManager.Instance.State == GameState.Paused) Resume();
    }

    public void Pause()
    {
        if (GameManager.Instance.State != GameState.Playing) return;
        GameManager.Instance.Pause();
        AudioManager.Running(false);
        if (pausePanel != null) pausePanel.SetActive(true);
    }

    public void Resume()
    {
        if (GameManager.Instance.State != GameState.Paused) return;
        GameManager.Instance.Resume();
        if (pausePanel != null) pausePanel.SetActive(false);
    }
}
