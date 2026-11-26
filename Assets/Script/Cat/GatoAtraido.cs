using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class GatoAtraido : MonoBehaviour
{
    private NavMeshAgent agente;
    private Transform jugador;

    public Animator anim;

    [Header("Parámetros de atracción")]
    public float rangoDeteccion = 10f;
    public float distanciaParada = 2f;
    public float velocidadNormal = 3f;
    public float rotacionSuavizada = 8f;
    public float distanciaBusquedaNavMesh = 4f;

    private bool siguiendoJugador;

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
            Debug.LogWarning($"[{name}] No se encontró un Player. El gato no podrá acercarse.");

        agente.speed = velocidadNormal;
        agente.stoppingDistance = distanciaParada;
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
            agente.isStopped = true;
            ActualizarAnimacion();
            return;
        }

        float distancia = Vector3.Distance(transform.position, jugador.position);
        siguiendoJugador = distancia <= rangoDeteccion && distancia > distanciaParada;

        if (siguiendoJugador && NavMesh.SamplePosition(jugador.position, out NavMeshHit hit,
                distanciaBusquedaNavMesh, agente.areaMask))
        {
            NavMeshPath ruta = new NavMeshPath();
            siguiendoJugador = agente.CalculatePath(hit.position, ruta) && ruta.status == NavMeshPathStatus.PathComplete;
            if (siguiendoJugador)
            {
                agente.isStopped = false;
                agente.SetDestination(hit.position);
            }
        }
        else
        {
            siguiendoJugador = false;
        }

        if (!siguiendoJugador)
            agente.isStopped = true;

        if (siguiendoJugador)
        {
            Vector3 dir = jugador.position - transform.position;
            dir.y = 0f;
            if (dir.sqrMagnitude > 0.01f)
            {
                Quaternion rot = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * rotacionSuavizada);
            }
        }

        ActualizarAnimacion();
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
            float velocidad = agente != null && agente.isOnNavMesh
                ? new Vector3(agente.velocity.x, 0f, agente.velocity.z).magnitude
                : 0f;
            anim.SetBool("isWalking", velocidad > 0.1f);
        }
    }
}

