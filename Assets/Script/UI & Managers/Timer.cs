using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float remainingTime = 60f;

    private bool isPaused = false;
    private bool soundPlayed = false; // 🔥 evita duplicado del sonido

    private Color colorOriginal;

    void Start()
    {
        if (timerText != null)
            colorOriginal = timerText.color;
    }

    void Update()
    {
        if (isPaused) return;

        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
        }
        else if (remainingTime <= 0)
        {
            remainingTime = 0f;

            if (!soundPlayed)
            {
                soundPlayed = true;
                GameOver();
            }

            if (timerText != null)
                timerText.color = Color.red;
        }

        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);

        if (timerText != null)
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void Pausar(bool estado)
    {
        isPaused = estado;

        Debug.Log($"⏱️ Timer {(estado ? "pausado" : "reiniciado")}");

        if (estado)
        {
            // Popup notificando pausa
            GameManager.Instance?.MostrarPopupTimer();
        }

        if (!estado)
        {
            // Avisar al Spawner
            Object.FindAnyObjectByType<Spawner>()?.AumentarLimiteSimultaneo();
        }
    }

    public void CambiarColor(Color c)
    {
        if (timerText != null)
            timerText.color = c;
    }

    private void GameOver()
    {
        // 🎵 Detener música
        AudioManager.Instance?.StopMusic();

        // 🔊 Reproducir sonido de fin
        AudioManager.Instance?.PlaySFX(AudioManager.Instance.Fin);

        // ⏳ Esperar antes de cambiar de escena
        StartCoroutine(GameOverDelay());
    }

    private System.Collections.IEnumerator GameOverDelay()
    {
        yield return new WaitForSeconds(3f);

        Debug.Log("Score final: " + ScoreManager.Instance.GetScore());

        ScoreManager.Instance?.SaveScore();

        SceneManager.LoadScene("Victoria");
    }
    public void ReiniciarColor()
    {
        if (timerText != null)
            timerText.color = colorOriginal;
    }
}
