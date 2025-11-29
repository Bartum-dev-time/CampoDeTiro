using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Stats")]
    public int currentScore = 0;
    public int targetsDestroyed = 0;
    public float gameTime = 0f;
    public bool gameActive = true;

    [Header("Game Settings")]
    public float gameDuration = 60f; // Tiempo límite en segundos
    public bool hasTimeLimit = false;

    [Header("UI Reference")]
    public UIManager uiManager;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        if (uiManager != null)
            uiManager.UpdateScore(currentScore);
    }

    void Update()
    {
        if (gameActive)
        {
            gameTime += Time.deltaTime;

            if (hasTimeLimit && gameTime >= gameDuration)
            {
                EndGame();
            }

            if (uiManager != null && hasTimeLimit)
            {
                float timeRemaining = gameDuration - gameTime;
                uiManager.UpdateTimer(timeRemaining);
            }
        }

        // Reiniciar juego
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }

        // Salir
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void AddScore(int points)
    {
        currentScore += points;
        if (uiManager != null)
            uiManager.UpdateScore(currentScore);
    }

    public void TargetDestroyed()
    {
        targetsDestroyed++;
        if (uiManager != null)
            uiManager.UpdateTargetsDestroyed(targetsDestroyed);
    }

    void EndGame()
    {
        gameActive = false;
        if (uiManager != null)
            uiManager.ShowGameOver(currentScore, targetsDestroyed);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        gameActive = false;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        gameActive = true;
    }
}