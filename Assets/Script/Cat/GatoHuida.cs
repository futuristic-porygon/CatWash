using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class GatoHuida : MonoBehaviour
{
    private NavMeshAgent agente;
    private Transform jugador;

    public Animator anim; // ← igual que los otros gatos

    [Header("Parámetros de huida")]
    public float rangoHuida = 8f;
    public float distanciaSegura = 14f;
    public float distanciaHuida = 6f;
    public float velocidadNormal = 2f;
    public float velocidadHuida = 5f;
    public float rotacionSuavizada = 8f;

    private bool estaHuyendo = false;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();

        // Obtener Animator si no está asignado
        if (anim == null)
            anim = GetComponent<Animator>();

        GameObject jugadorObj = GameObject.FindGameObjectWithTag("Player");
        if (jugadorObj != null)
            jugador = jugadorObj.transform;
        else
            Debug.LogWarning($"[{name}] No se encontró un objeto con tag 'Player'.");

        agente.speed = velocidadNormal;
    }

    void Update()
    {
        if (jugador == null) return;

        float distanciaJugador = Vector3.Distance(transform.position, jugador.position);

        // Iniciar huida
        if (distanciaJugador <= rangoHuida)
        {
            estaHuyendo = true;
            agente.speed = velocidadHuida;
        }

        // Detener huida cuando está lejos
        if (estaHuyendo && distanciaJugador > distanciaSegura)
        {
            estaHuyendo = false;
            agente.speed = velocidadNormal;
            agente.ResetPath();
        }

        // Movimiento mientras huye
        if (estaHuyendo)
        {
            HuirDelJugador();
        }

        // Rotación suave hacia dirección de movimiento
        if (agente.velocity.sqrMagnitude > 0.1f)
        {
            Quaternion rotacionDeseada = Quaternion.LookRotation(agente.velocity.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacionDeseada, Time.deltaTime * rotacionSuavizada);
        }

        // Actualizar animación
        ActualizarAnimacion();
    }

    void HuirDelJugador()
    {
        Vector3 direccionHuida = (transform.position - jugador.position).normalized;
        Vector3 destinoPropuesto = transform.position + direccionHuida * distanciaHuida;

        Debug.DrawLine(transform.position, destinoPropuesto, Color.red);

        if (NavMesh.SamplePosition(destinoPropuesto, out NavMeshHit hit, 3f, NavMesh.AllAreas))
        {
            agente.SetDestination(hit.position);
        }
        else
        {
            Vector3 alternativo = Quaternion.Euler(0, 45, 0) * direccionHuida;
            Vector3 destinoAlterno = transform.position + alternativo * distanciaHuida;

            if (NavMesh.SamplePosition(destinoAlterno, out NavMeshHit hit2, 3f, NavMesh.AllAreas))
            {
                agente.SetDestination(hit2.position);
            }
        }
    }

    void ActualizarAnimacion()
    {
        float velocidad = agente.velocity.magnitude;

        bool estaCaminando = velocidad > 0.1f;

        // Usa el mismo parámetro que los otros scripts
        anim.SetBool("isWalking", estaCaminando);
    }
}
