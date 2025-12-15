using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreboardUI : MonoBehaviour
{
    [Header("Referencias UI")]
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI rankingText;

    void Start()
    {
        // Inicia una coroutine para asegurarse de que ScoreManager ya tenga el puntaje final
        StartCoroutine(RefreshUI());
    }

    private IEnumerator RefreshUI()
    {
        // Espera un frame para que ScoreManager haya guardado el score final
        yield return null;

        if (ScoreManager.Instance == null) yield break;

        // Guardar el score final por si no se ha hecho todavía
        ScoreManager.Instance.SaveScore();

        // Obtener top 5
        var top = ScoreManager.Instance.GetTop5();

        // Actualizar textos
        finalScoreText.text = "YOUR SCORE: " + ScoreManager.Instance.GetCurrentScore();

        rankingText.text =
            
            "1st " + "............" + top[0] + "\n" +
            "2nd " + "............" + top[1] + "\n" +
            "3rd " + "............" + top[2] + "\n" +
            "4th " + "............" + top[3] + "\n" +
            "5th " + "............" + top[4];
    }

    // 🔹 Alternativa: método público para refrescar desde otro script
    public void RefreshScores()
    {
        if (ScoreManager.Instance == null) return;

        ScoreManager.Instance.SaveScore();
        var top = ScoreManager.Instance.GetTop5();

        finalScoreText.text = "YOUR SCORE: " + ScoreManager.Instance.GetCurrentScore();
        rankingText.text =
            
            "1st " + "............" + top[0] + "\n" +
            "2nd " + "............" + top[1] + "\n" +
            "3rd " + "............" + top[2] + "\n" +
            "4th " + "............" + top[3] + "\n" +
            "5th " + "............" + top[4];
    }
}
