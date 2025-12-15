using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class GatoAtraido : MonoBehaviour
{
    private NavMeshAgent agente;
    private Transform jugador;

    public Animator anim; // ← ahora es público igual que en el otro gato

    [Header("Parámetros de atracción")]
    public float rangoDeteccion = 10f;
    public float distanciaParada = 2f;
    public float velocidadNormal = 3f;
    public float rotacionSuavizada = 8f;

    private bool siguiendoJugador = false;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();

        // Obtener Animator si no fue asignado en el Inspector
        if (anim == null)
            anim = GetComponent<Animator>();

        // Buscar jugador
        GameObject jugadorObj = GameObject.FindGameObjectWithTag("Player");
        if (jugadorObj != null)
            jugador = jugadorObj.transform;
        else
            Debug.LogWarning($"[{name}] No se encontró un Player. El gato no podrá acercarse.");

        agente.speed = velocidadNormal;
        agente.stoppingDistance = distanciaParada;
    }

    void Update()
    {
        if (jugador == null) return;

        float distancia = Vector3.Distance(transform.position, jugador.position);

        if (distancia <= rangoDeteccion && distancia > distanciaParada)
        {
            siguiendoJugador = true;
            agente.isStopped = false;
            agente.SetDestination(jugador.position);
        }
        else
        {
            siguiendoJugador = false;
            agente.isStopped = true;
        }

        // Rotación suave hacia el jugador
        if (siguiendoJugador)
        {
            Vector3 dir = (jugador.position - transform.position).normalized;
            dir.y = 0;

            if (dir.sqrMagnitude > 0.01f)
            {
                Quaternion rot = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * rotacionSuavizada);
            }
        }

        ActualizarAnimacion();
    }

    void ActualizarAnimacion()
    {
        float velocidad = new Vector3(agente.velocity.x, 0, agente.velocity.z).magnitude;
        bool estaCaminando = velocidad > 0.1f;

        anim.SetBool("isWalking", estaCaminando);
    }
}
