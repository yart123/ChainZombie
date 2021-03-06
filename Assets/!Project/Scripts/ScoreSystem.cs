using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    [Header("Scores")]
    public int scorePerSecond;
    public int scorePerCaged;
    public int scorePerHit;

    [Header("Links")]
    public TMP_Text hpText;
    public TMP_Text gameScoreText;
    public TMP_Text currentScoreText;
    public TMP_Text highScoreText;
    public GameObject gameOverPanel;
    public GameObject mobileUI;

    private float currentScore;
    private SoundManager soundManager;
    private void Start()
    {
        soundManager = GameObject.FindObjectOfType<SoundManager>();

        if (Application.platform != RuntimePlatform.Android)
        {
            mobileUI.SetActive(false);
        }
    }

    void Update()
    {
        currentScore += scorePerSecond * Time.deltaTime;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        gameScoreText.text = "Score: " + (int) currentScore;
    }

    public void ZombieHit()
    {
        currentScore += scorePerHit;
        UpdateScoreText();
    }
    public void ZombieCaged()
    {
        currentScore += scorePerCaged;
        UpdateScoreText();
    }

    public void GameOver()
    {
        // Stop spawning zombies
        ZombieSpawner spawner = GameObject.FindObjectOfType<ZombieSpawner>();
        spawner.gameObject.SetActive(false);

        // Stop earning score
        scorePerSecond = 0;

        // Save highscore
        int highScore = PlayerPrefs.GetInt("Highscore");
        if (highScore < currentScore)
        {
            PlayerPrefs.SetInt("Highscore", (int) currentScore);
            highScore = (int) currentScore;
        }

        // Show Game Over panel
        currentScoreText.text = "Current Score: " + (int) currentScore;
        highScoreText.text = "High Score: " + highScore;
        gameOverPanel.SetActive(true);

        soundManager.playMusic(soundManager.gameOverMusic);
    }
}
