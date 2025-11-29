using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("HUD Elements")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI targetsText;
    public TextMeshProUGUI timerText;
    public Image crosshair;

    [Header("Game Over Panel")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI finalTargetsText;

    void Start()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    public void UpdateScore(int score)
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    public void UpdateTargetsDestroyed(int targets)
    {
        if (targetsText != null)
            targetsText.text = "Targets: " + targets;
    }

    public void UpdateTimer(float timeRemaining)
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(timeRemaining / 60);
            int seconds = Mathf.FloorToInt(timeRemaining % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    public void ShowGameOver(int finalScore, int finalTargets)
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);

            if (finalScoreText != null)
                finalScoreText.text = "Final Score: " + finalScore;

            if (finalTargetsText != null)
                finalTargetsText.text = "Targets Destroyed: " + finalTargets;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void HideCrosshair()
    {
        if (crosshair != null)
            crosshair.enabled = false;
    }

    public void ShowCrosshair()
    {
        if (crosshair != null)
            crosshair.enabled = true;
    }
}