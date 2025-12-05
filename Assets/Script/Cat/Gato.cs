using UnityEngine;
using UnityEngine.UIElements;

public class Gato : MonoBehaviour
{
    public float suciedad = 60f;
    public Material limpioMat;
    private Renderer rend;
    private bool limpio = false;

    [SerializeField] private float tiempoAntesDeDestruir = 3f;
    AudioManager audioManager;

    public GameObject particulasImpacto;

    Animator animator;
    Rigidbody rb;
    UnityEngine.AI.NavMeshAgent agent;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
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

            // DETENER MOVIMIENTO
            DetenerMovimiento();

            if (particulasImpacto != null)
            {
                Instantiate(particulasImpacto, transform.position, transform.rotation);
            }

            if (GameManager.Instance != null)
                GameManager.Instance.GatoLimpio();

            audioManager.PlaySFX(audioManager.clean);
            ScoreManager.Instance.AddPoint();

            Destroy(gameObject, tiempoAntesDeDestruir);
        }
    }

    private void DetenerMovimiento()
    {
        /* 1) SI EL GATO TIENE ANIMATOR
        if (animator != null)
        {
            animator.enabled = false;
        }*/

        // 3) SI USA NAVMESH
        if (agent != null)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
        }
    }
}
