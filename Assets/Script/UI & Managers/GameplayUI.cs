using UnityEngine;
using TMPro;

public class GameplayUI : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;

    void Start()
    {
        // Mostrar highscore solo al cargar
        if (ScoreManager.Instance != null)
            highScoreText.text = "HIGHSCORE: " + ScoreManager.Instance.GetHighScore();
    }

    void Update()
    {
        if (ScoreManager.Instance != null)
            scoreText.text = "SCORE: " + ScoreManager.Instance.GetScore();
    }
}