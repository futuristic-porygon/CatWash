using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("-------Audio Sources-------")]
    [SerializeField] public AudioSource musicSource;
    [SerializeField] public AudioSource SFXSource;
    [SerializeField] public AudioSource loopSource;   // ✅ NUEVO PARA JETPACK

    [Header("-------Audio Clips-------")]
    public AudioClip background;
    public AudioClip clean;
    public AudioClip weapon;
    public AudioClip weaponChage;
    public AudioClip rechargeRojo;
    public AudioClip rechargeVerde;
    public AudioClip rechargeAzul;
    public AudioClip Fin;
    public AudioClip Jetpack;
    public AudioClip Charco;
    public AudioClip Boost;
    public AudioClip Sprint;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (background != null)
        {
            musicSource.clip = background;
            musicSource.Play();
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
            SFXSource.PlayOneShot(clip);
    }

    public void StopMusic()
    {
        if (musicSource != null)
            musicSource.Stop();
    }
}
