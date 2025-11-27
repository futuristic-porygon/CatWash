using UnityEngine;

public class GatoTutorial : MonoBehaviour
{
    public float suciedadMax = 60f;
    public float suciedad = 60f;

    public Material sucioMat;
    public Material limpioMat;

    public float tiempoSucioDeNuevo = 3f;

    private Renderer rend;
    private bool limpio = false;

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        rend = GetComponentInChildren<Renderer>(); // 🔥 CORREGIDO
        transform.rotation = Quaternion.Euler(0f, 180f, 0f);
    }

    public void Limpiar(float cantidad)
    {
        if (limpio) return;

        suciedad -= cantidad;

        if (suciedad <= 0)
        {
            limpio = true;
            rend.sharedMaterial = limpioMat; // 🔥 garantiza WIP correcto

            audioManager.PlaySFX(audioManager.clean);

            // Empieza a ensuciarse después de X segundos
            Invoke(nameof(EnsuciarDeNuevo), tiempoSucioDeNuevo);
        }
    }

    void EnsuciarDeNuevo()
    {
        suciedad = suciedadMax;
        limpio = false;

        rend.sharedMaterial = sucioMat; // 🔥 vuelve al material sucio
    }
}
