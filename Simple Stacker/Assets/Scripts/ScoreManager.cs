using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text highScoreText;


    private int score = 0;
    private int highScore = 0;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        highScore = PlayerPrefs.GetInt("highScore", 0);
        scoreText.text = "Score: " + score.ToString();
        highScoreText.text = "Highscore: " + highScore.ToString();
        scoreText.color = Color.cyan;
    }

    public void AddPoint()
    {
        score++;
        scoreText.text = "Score: " + score.ToString();
        scoreText.color = Color.HSVToRGB((score / 50f) % 1f, 1f, 1f);
        if (highScore > score)
            PlayerPrefs.SetInt("highScore", score);
    }

    public void ResetScoreText()
    {
        score = 0;
        scoreText.text = "Score: " + score.ToString();
        scoreText.color = Color.cyan;
    }
}
