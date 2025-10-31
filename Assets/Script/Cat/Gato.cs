using UnityEngine;

public class Gato : MonoBehaviour
{
    public float suciedad = 60f;
    public Material limpioMat;
    private Renderer rend;
    private bool limpio = false;

    [SerializeField] private float tiempoAntesDeDestruir = 3f;

    public void Awake()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RegisterGato();
        }
    }

    void Start()
    {
        rend = GetComponent<Renderer>();

        transform.rotation = Quaternion.Euler(0f, 180f, 0f);
    }

    public void Limpiar(float cantidad)
    {
        if (limpio) return;

        suciedad -= cantidad;

        if (suciedad <= 0)
        {
            limpio = true;
            rend.material = limpioMat;

            if (GameManager.Instance != null)
                GameManager.Instance.GatoLimpio();

            Debug.Log("¡Gato limpio!");

            //Destroy(gameObject, tiempoAntesDeDestruir);
        }
    }
}