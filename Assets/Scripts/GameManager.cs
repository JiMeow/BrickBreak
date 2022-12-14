using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField]
    Text scoreText;
    [SerializeField]
    Text livesText;
    [SerializeField]
    Text startGameText;
    [SerializeField]
    Text gameOverText;
    private void Awake()
    {
        instance = this;
        Time.timeScale = 0;
        gameOverText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Die then restart game
            if (livesText.text == "LIVE: 0")
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            //Start game
            else
            {
                Time.timeScale = 1;
                startGameText.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// It takes the current score, adds the new score to it, and then updates the score text
    /// </summary>
    /// <param name="score">The score to add to the current score.</param>
    public void AddScore(int score)
    {
        int nowScore = int.Parse(scoreText.text.Substring(6));
        scoreText.text = "SCORE: " + (nowScore + score);
    }

    /// <summary>
    /// It takes the current number of lives, subtracts one from it, and then updates the text on the
    /// screen
    /// </summary>
    public void RemoveLives()
    {
        int nowLive = int.Parse(livesText.text.Substring(5));
        int newLive = nowLive - 1;
        if (newLive == 0)
        {
            GameOver();
        }
        livesText.text = "LIVE: " + (newLive);
    }

    /// <summary>
    /// This function stops the game from running and shows the game over text
    /// </summary>
    void GameOver()
    {
        Time.timeScale = 0;
        gameOverText.gameObject.SetActive(true);
    }
}
