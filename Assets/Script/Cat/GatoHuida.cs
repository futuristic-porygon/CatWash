using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class GatoHuida : MonoBehaviour
{
    private NavMeshAgent agente;
    private Transform jugador;

    public Animator anim;

    [Header("Parámetros de huida")]
    public float rangoHuida = 8f;
    public float distanciaSegura = 14f;
    public float distanciaHuida = 6f;
    public float velocidadNormal = 2f;
    public float velocidadHuida = 5f;
    public float rotacionSuavizada = 8f;
    public float distanciaBusquedaNavMesh = 4f;

    private bool estaHuyendo;

    private void Awake()
    {
        agente = GetComponent<NavMeshAgent>();
        if (anim == null)
            anim = GetComponent<Animator>();
    }

    private void Start()
    {
        GameObject jugadorObj = GameObject.FindGameObjectWithTag("Player");
        if (jugadorObj != null)
            jugador = jugadorObj.transform;
        else
            Debug.LogWarning($"[{name}] No se encontró un objeto con tag 'Player'.");

        agente.speed = velocidadNormal;
        agente.avoidancePriority = Random.Range(0, 100);
        IntentarIniciarAgente();
    }

    private void Update()
    {
        if (!agente.isOnNavMesh)
        {
            IntentarIniciarAgente();
            ActualizarAnimacion();
            return;
        }

        if (jugador == null)
        {
            ActualizarAnimacion();
            return;
        }

        float distanciaJugador = Vector3.Distance(transform.position, jugador.position);
        if (distanciaJugador <= rangoHuida)
        {
            estaHuyendo = true;
            agente.speed = velocidadHuida;
        }
        else if (estaHuyendo && distanciaJugador > distanciaSegura)
        {
            estaHuyendo = false;
            agente.speed = velocidadNormal;
            agente.ResetPath();
        }

        if (estaHuyendo)
            HuirDelJugador();

        if (agente.velocity.sqrMagnitude > 0.1f)
        {
            Quaternion rotacionDeseada = Quaternion.LookRotation(agente.velocity.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacionDeseada, Time.deltaTime * rotacionSuavizada);
        }

        ActualizarAnimacion();
    }

    private void HuirDelJugador()
    {
        Vector3 direccionHuida = transform.position - jugador.position;
        direccionHuida.y = 0f;
        if (direccionHuida.sqrMagnitude < 0.01f)
            direccionHuida = transform.forward;
        direccionHuida.Normalize();

        for (int intento = 0; intento < 8; intento++)
        {
            float angulo = intento == 0 ? 0f : (intento % 2 == 1 ? 1f : -1f) * ((intento + 1) / 2) * 30f;
            Vector3 direccion = Quaternion.Euler(0f, angulo, 0f) * direccionHuida;
            Vector3 destinoPropuesto = transform.position + direccion * distanciaHuida;

            if (!NavMesh.SamplePosition(destinoPropuesto, out NavMeshHit hit,
                    distanciaBusquedaNavMesh, agente.areaMask))
                continue;

            NavMeshPath ruta = new NavMeshPath();
            if (!agente.CalculatePath(hit.position, ruta) || ruta.status != NavMeshPathStatus.PathComplete)
                continue;

            agente.SetDestination(hit.position);
            return;
        }
    }

    private void IntentarIniciarAgente()
    {
        if (agente != null && !agente.isOnNavMesh &&
            NavMesh.SamplePosition(transform.position, out NavMeshHit hit,
                distanciaBusquedaNavMesh, agente.areaMask))
        {
            agente.Warp(hit.position);
        }
    }

    private void ActualizarAnimacion()
    {
        if (anim != null)
        {
            bool estaCaminando = agente != null && agente.isOnNavMesh && agente.velocity.sqrMagnitude > 0.01f;
            anim.SetBool("isWalking", estaCaminando);
        }
    }
}

