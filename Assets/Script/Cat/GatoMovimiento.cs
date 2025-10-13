using UnityEngine;
using UnityEngine.AI;

public class GatoMovimiento : MonoBehaviour
{
    private NavMeshAgent agente;
    private float tiempoEspera = 3f;
    private float contador;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        contador = tiempoEspera;
        IrANuevaPosicion();
    }

    void Update()
    {
        contador -= Time.deltaTime;

        // Si ya llegó al destino o pasó el tiempo de espera, buscar nueva posición
        if (!agente.pathPending && agente.remainingDistance <= agente.stoppingDistance)
        {
            if (contador <= 0f)
            {
                IrANuevaPosicion();
                contador = tiempoEspera;
            }
        }
    }

    void IrANuevaPosicion()
    {
        // Buscar un punto aleatorio en el NavMesh
        Vector3 randomDir = Random.insideUnitSphere * 10f; // radio de movimiento
        randomDir += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDir, out hit, 10f, NavMesh.AllAreas))
        {
            agente.SetDestination(hit.position);
        }
    }
}