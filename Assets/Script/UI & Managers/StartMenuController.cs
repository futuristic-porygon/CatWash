using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    [Header("Sonido de Botón")]
    public AudioClip botonSFX;

    private AudioManager audioManager;

    void Awake()
    {
        Time.timeScale = 1f;
    }

    void Start()
    {
        // Busca el AudioManager en la escena actual
        audioManager = FindAnyObjectByType<AudioManager>();
    }

    private void PlayButtonSound()
    {
        if (audioManager != null && botonSFX != null)
            audioManager.PlaySFX(botonSFX);
    }

    public void OnStartClick()
    {
        PlayButtonSound();
        SceneManager.LoadScene("Main");       
    }

    public void OnClickExit()
    {
        PlayButtonSound();
        Application.Quit();
    }

    public void OnClickCredits()
    {
        PlayButtonSound();
        SceneManager.LoadScene("Credits");
    }

    public void OnClickTutorial()
    {
        PlayButtonSound();
        SceneManager.LoadScene("Tutorial");
    }

    public void OnClickMainManu()
    {
        PlayButtonSound();
        SceneManager.LoadScene("Main Menu");
    }
}
