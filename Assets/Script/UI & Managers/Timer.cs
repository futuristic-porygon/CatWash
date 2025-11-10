using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float remainingTime = 60f;

    private bool isPaused = false;

    void Update()
    {
        if (isPaused) return;

        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
        }
        else if (remainingTime <= 0)
        {
            remainingTime = 0;
            GameOver();
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
        Debug.Log($"⏱️ Timer {(estado ? "pausado" : "reanuda")}");
    }

    public void CambiarColor(Color c)
    {
        if (timerText != null)
            timerText.color = c;
    }

    public void GameOver()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
