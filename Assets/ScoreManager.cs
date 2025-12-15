using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    int score = 0;
    List<int> topScores = new List<int>();

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        topScores.Clear();
        for (int i = 1; i <= 5; i++)
            topScores.Add(PlayerPrefs.GetInt("Top" + i, 0));
    }

    public void AddPoint()
    {
        score++;
    }

    public int GetScore() => score;
    public int GetCurrentScore() => score;
    public int GetHighScore() => topScores[0];
    public List<int> GetTop5() => new List<int>(topScores);

    public void SaveScore()
    {
        // Agregar score nuevo
        topScores.Add(score);

        // Quitar repetidos
        topScores = topScores.Distinct().ToList();

        // Ordenar de mayor a menor
        topScores.Sort((a, b) => b.CompareTo(a));

        // 🔥 IMPORTANTE: rellenar para evitar crash
        while (topScores.Count < 5)
            topScores.Add(0);

        // Mantener solo 5
        if (topScores.Count > 5)
            topScores = topScores.GetRange(0, 5);

        // Guardar
        for (int i = 1; i <= 5; i++)
            PlayerPrefs.SetInt("Top" + i, topScores[i - 1]);

        PlayerPrefs.Save();
    }

    public void ResetScore()
    {
        score = 0;
    }
}
