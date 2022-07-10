using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    public float scorePerSecond;
    public float scorePerZombie;

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

    public void ZombieKilled()
    {
        currentScore += scorePerZombie;
        soundManager.playSoundEffect(soundManager.zombieCagedSound);
        UpdateScoreText();
    }

    public void GameOver()
    {
        ZombieSpawner spawner = GameObject.FindObjectOfType<ZombieSpawner>();
        spawner.gameObject.SetActive(false);

        scorePerSecond = 0;
        scorePerZombie = 0;

        int highScore = PlayerPrefs.GetInt("Highscore");
        if (highScore < currentScore)
        {
            PlayerPrefs.SetInt("Highscore", (int) currentScore);
            highScore = (int) currentScore;
        }

        currentScoreText.text = "Current Score: " + (int) currentScore;
        highScoreText.text = "High Score: " + highScore;

        gameOverPanel.SetActive(true);

        soundManager.playMusic(soundManager.gameOverMusic);
    }
}
