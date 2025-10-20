using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class GatoHuida : MonoBehaviour
{
    private NavMeshAgent agente;
    private Transform jugador;

    [Header("Parámetros de huida")]
    public float rangoHuida = 5f;        // Distancia mínima para empezar a huir
    public float distanciaSegura = 10f;  // Distancia a la que deja de huir
    public float distanciaHuida = 8f;    // Qué tan lejos intenta moverse
    public float velocidadNormal = 2f;   // Velocidad cuando está tranquilo
    public float velocidadHuida = 5f;    // Velocidad al huir
    public float rotacionSuavizada = 8f; // Qué tan rápido gira hacia la dirección de huida

    private bool estaHuyendo = false;

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
            Debug.LogWarning($"[{name}] No se encontró un objeto con tag 'Player'. El gato no podrá huir.");
        }

        agente.speed = velocidadNormal;
    }

    void Update()
    {
        if (jugador == null) return;

        float distanciaJugador = Vector3.Distance(transform.position, jugador.position);

        if (distanciaJugador <= rangoHuida)
        {
            if (!estaHuyendo)
            {
                estaHuyendo = true;
                agente.speed = velocidadHuida;
            }

            HuirDelJugador();
        }
        else if (estaHuyendo && distanciaJugador > distanciaSegura)
        {
            // Cuando ya está lejos, se detiene y baja su velocidad
            agente.ResetPath();
            agente.speed = velocidadNormal;
            estaHuyendo = false;
        }

        // Suaviza la rotación del gato hacia su dirección de movimiento
        if (agente.velocity.sqrMagnitude > 0.1f)
        {
            Quaternion rotacionDeseada = Quaternion.LookRotation(agente.velocity.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacionDeseada, Time.deltaTime * rotacionSuavizada);
        }
    }

    void HuirDelJugador()
    {
        // Dirección contraria al jugador
        Vector3 direccionHuida = (transform.position - jugador.position).normalized;
        Vector3 destinoHuida = transform.position + direccionHuida * distanciaHuida;

        if (NavMesh.SamplePosition(destinoHuida, out NavMeshHit hit, distanciaHuida, NavMesh.AllAreas))
        {
            agente.SetDestination(hit.position);
        }
    }
}