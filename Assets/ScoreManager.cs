using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;

    int score = 0;
    int highscore = 0;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        highscore = PlayerPrefs.GetInt("highscore", 0);
        scoreText.text=score.ToString() + "POINTS";
        highScoreText.text = "HIGHSCORE: " + highscore.ToString();
    }

    public void AddPoint()
    {
        score += 1;
        scoreText.text = score.ToString() + "POINTS";
        if(highscore < score)
            PlayerPrefs.SetInt("highscore", score);
    }
}
