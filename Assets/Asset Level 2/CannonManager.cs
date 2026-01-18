using UnityEngine;
using TMPro;

public class CannonManager : MonoBehaviour
{
    [Header("Game Settings")]
    public float gameDuration = 60f;
    public float difficultyIncreaseRate = 0.01f;
    public float CurrentDifficulty { get; private set; } = 0f;

    private float gameTimer;
    private bool timeIsUp = false;

    [Header("UI References")]
    public TextMeshProUGUI timerText;

    void Start()
    {
        gameTimer = gameDuration;
        CurrentDifficulty = 0f;
        timeIsUp = false;
    }

    void Update()
    {
        if (HealthSystem.globalGameEnded || timeIsUp) return;

        if (gameTimer > 0)
        {
            gameTimer -= Time.deltaTime;
            CurrentDifficulty += difficultyIncreaseRate * Time.deltaTime;
            CurrentDifficulty = Mathf.Clamp01(CurrentDifficulty);
        }
        else
        { 
            timeIsUp = true;
            TriggerWin();
        }

        UpdateTimerUI();
    }

    void UpdateTimerUI()
    {
        if (timerText != null)
        {
            float timeToDisplay = Mathf.Max(0, gameTimer);
            int minutes = Mathf.FloorToInt(timeToDisplay / 60);
            int seconds = Mathf.FloorToInt(timeToDisplay % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    void TriggerWin()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.GetComponent<HealthSystemLevel2>().WinLevel2();
        }
    }
}