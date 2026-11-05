using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { Playing, Paused, GameOver }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("References")]
    public Transform player;

    [Header("Speed / Difficulty (configurable)")]
    public float startSpeed = 12f;
    public float maxSpeed = 22f;
    public float speedIncreasePerSecond = 0.2f;

    [Header("Scoring")]
    public int coinScore = 10;
    public int gemScore = 50;
    public float scorePerMeter = 1f;

    public GameState State { get; private set; }
    public int Coins { get; private set; }
    public int Gems { get; private set; }
    public float Distance { get; private set; }
    public int Score { get; private set; }
    public int BestScore { get; private set; }
    public float CurrentSpeed { get; private set; }

    // 0 = easiest, 1 = hardest
    public float Difficulty01 { get { return Mathf.InverseLerp(startSpeed, maxSpeed, CurrentSpeed); } }

    public event Action OnGameOver;

    float startZ;
    int bonusScore;

    void Awake()
    {
        Instance = this;
        Time.timeScale = 1f;
    }

    void Start()
    {
        StartGame();
    }

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    public void StartGame()
    {
        State = GameState.Playing;
        Coins = 0; Gems = 0; bonusScore = 0; Score = 0; Distance = 0;
        CurrentSpeed = startSpeed;
        startZ = player != null ? player.position.z : 0f;
        BestScore = PlayerPrefs.GetInt("BestScore", 0);
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (State != GameState.Playing) return;
        CurrentSpeed = Mathf.Min(maxSpeed, CurrentSpeed + speedIncreasePerSecond * Time.deltaTime);
        if (player != null) Distance = Mathf.Max(0f, player.position.z - startZ);
        Score = Mathf.FloorToInt(Distance * scorePerMeter) + bonusScore;
    }

    public void AddCoin()
    {
        if (State != GameState.Playing) return;
        Coins++;
        bonusScore += coinScore;
    }

    public void AddGem()
    {
        if (State != GameState.Playing) return;
        Gems++;
        bonusScore += gemScore;
    }

    public void GameOver()
    {
        if (State != GameState.Playing) return;
        State = GameState.GameOver;
        if (Score > BestScore)
        {
            BestScore = Score;
            PlayerPrefs.SetInt("BestScore", BestScore);
            PlayerPrefs.Save();
        }
        AudioManager.Hit();
        AudioManager.GameOverSound();
        AudioManager.Running(false);
        if (OnGameOver != null) OnGameOver();
    }

    public void Pause()
    {
        if (State != GameState.Playing) return;
        State = GameState.Paused;
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        if (State != GameState.Paused) return;
        State = GameState.Playing;
        Time.timeScale = 1f;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
