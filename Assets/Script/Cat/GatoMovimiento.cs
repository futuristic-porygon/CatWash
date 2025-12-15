using UnityEngine;
using UnityEngine.AI;

public class GatoMovimiento : MonoBehaviour
{
    private NavMeshAgent agente;
    public Animator anim;

    private float tiempoEspera = 3f;
    private float contador;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();

        if (anim == null)
            anim = GetComponent<Animator>();

        agente.stoppingDistance = 0.2f; // Evita que “llegue” antes de tiempo
        contador = tiempoEspera;

        IrANuevaPosicion();
    }

    void Update()
    {
        contador -= Time.deltaTime;

        ActualizarAnimacion();

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
        Vector2 random2D = Random.insideUnitCircle * 10f;
        Vector3 destino = new Vector3(random2D.x, 0, random2D.y) + transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(destino, out hit, 2f, NavMesh.AllAreas))
        {
            agente.SetDestination(hit.position);
            Debug.Log("Nuevo destino: " + hit.position);
        }
    }

    void ActualizarAnimacion()
    {
        float velocidad = agente.velocity.magnitude;
        anim.SetBool("isWalking", velocidad > 0.1f);
    }
}
