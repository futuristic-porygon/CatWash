using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class GatoAtraido : MonoBehaviour
{
    private NavMeshAgent agente;
    private Transform jugador;

    [Header("Parámetros de atracción")]
    public float rangoDeteccion = 10f;   // Distancia máxima a la que detecta al jugador
    public float distanciaParada = 2f;   // Qué tan cerca se acerca antes de detenerse
    public float velocidadNormal = 3f;   // Velocidad al moverse hacia el jugador
    public float rotacionSuavizada = 8f; // Qué tan rápido gira hacia el jugador

    private bool siguiendoJugador = false;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();

        // Buscar al jugador automáticamente por tag
        GameObject jugadorObj = GameObject.FindGameObjectWithTag("Player");
        if (jugadorObj != null)
        {
            jugador = jugadorObj.transform;
        }
        else
        {
            Debug.LogWarning($"[{name}] No se encontró un objeto con tag 'Player'. El gato no podrá acercarse.");
        }

        agente.speed = velocidadNormal;
        agente.stoppingDistance = distanciaParada;
    }

    void Update()
    {
        if (jugador == null) return;

        float distancia = Vector3.Distance(transform.position, jugador.position);

        if (distancia <= rangoDeteccion && distancia > distanciaParada)
        {
            // Se acerca al jugador
            siguiendoJugador = true;
            agente.isStopped = false;
            agente.SetDestination(jugador.position);
        }
        else if (distancia <= distanciaParada)
        {
            // Se detiene cerca del jugador
            siguiendoJugador = false;
            agente.isStopped = true;
        }
        else
        {
            // Jugador fuera de rango
            siguiendoJugador = false;
            agente.isStopped = true;
        }

        // Rotación suave hacia el jugador si lo está siguiendo
        if (siguiendoJugador)
        {
            Vector3 direccion = (jugador.position - transform.position).normalized;
            direccion.y = 0f;

            if (direccion.sqrMagnitude > 0.001f)
            {
                Quaternion rotDeseada = Quaternion.LookRotation(direccion);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotDeseada, Time.deltaTime * rotacionSuavizada);
            }
        }
    }
}