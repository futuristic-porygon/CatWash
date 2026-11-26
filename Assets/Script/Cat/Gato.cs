using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class Gato : MonoBehaviour
{
    // NavMeshAgent controls movement, so physical contacts between cats can
    // fight that movement and leave both agents/animations apparently stuck.
    // Keep colliders active for the player and world, but ignore cat pairs.
    private static readonly HashSet<Gato> gatosActivos = new HashSet<Gato>();
    private Collider[] collidersGato;

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

    // 👉 NUEVO: indicar si el gato pertenece al tutorial
    public bool esTutorial = false;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        collidersGato = GetComponentsInChildren<Collider>(true);
    }

    private void OnEnable()
    {
        if (collidersGato == null)
            collidersGato = GetComponentsInChildren<Collider>(true);

        foreach (Gato otroGato in gatosActivos)
        {
            if (otroGato != null && otroGato != this)
                IgnorarColisionCon(otroGato, true);
        }

        gatosActivos.Add(this);
    }

    private void OnDisable()
    {
        gatosActivos.Remove(this);
        foreach (Gato otroGato in gatosActivos)
        {
            if (otroGato != null)
                IgnorarColisionCon(otroGato, false);
        }
    }

    private void IgnorarColisionCon(Gato otroGato, bool ignorar)
    {
        if (collidersGato == null || otroGato.collidersGato == null)
            return;

        foreach (Collider propio in collidersGato)
        {
            if (propio == null)
                continue;

            foreach (Collider ajeno in otroGato.collidersGato)
            {
                if (ajeno != null)
                    Physics.IgnoreCollision(propio, ajeno, ignorar);
            }
        }
    }

    void Start()
    {
        rend = GetComponentInChildren<Renderer>();
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

            DetenerMovimiento();

            if (particulasImpacto != null)
                Instantiate(particulasImpacto, transform.position, transform.rotation);

            audioManager.PlaySFX(audioManager.clean);

            // 👉 SOLO si NO es tutorial cuenta como gato limpiado
            if (!esTutorial)
            {
                if (GameManager.Instance != null)
                    GameManager.Instance.GatoLimpio();

                ScoreManager.Instance.AddPoint();
            }

            Destroy(gameObject, tiempoAntesDeDestruir);
        }
    }

    private void DetenerMovimiento()
    {
        /*
        // Si quieres desactivar animación cuando se limpia
        if (animator != null)
            animator.enabled = false;
        */

        if (agent != null)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
        }
    }
}

