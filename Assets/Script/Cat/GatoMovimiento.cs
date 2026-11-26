using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class GatoMovimiento : MonoBehaviour
{
    private NavMeshAgent agente;
    public Animator anim;

    [SerializeField] private float tiempoEspera = 3f;
    [SerializeField] private float radioMovimiento = 10f;
    [SerializeField] private float distanciaBusquedaNavMesh = 4f;
    private float contador;

    private void Awake()
    {
        agente = GetComponent<NavMeshAgent>();
        if (anim == null)
            anim = GetComponent<Animator>();
    }

    private void Start()
    {
        agente.stoppingDistance = 0.2f;
        agente.avoidancePriority = Random.Range(0, 100);
        contador = tiempoEspera;
        IntentarIniciarAgente();
    }

    private void Update()
    {
        if (!agente.isOnNavMesh)
        {
            IntentarIniciarAgente();
            ActualizarAnimacion(false);
            return;
        }

        contador -= Time.deltaTime;
        ActualizarAnimacion(agente.velocity.magnitude > 0.1f);

        if (agente.pathPending || contador > 0f)
            return;

        bool llegoAlDestino = agente.hasPath && agente.remainingDistance <= agente.stoppingDistance;
        bool rutaFallida = agente.pathStatus == NavMeshPathStatus.PathInvalid ||
                           (agente.pathStatus == NavMeshPathStatus.PathPartial &&
                            agente.remainingDistance <= agente.stoppingDistance);

        if (llegoAlDestino || rutaFallida || !agente.hasPath)
        {
            IrANuevaPosicion();
            contador = tiempoEspera;
        }
    }

    private void IntentarIniciarAgente()
    {
        if (agente == null || agente.isOnNavMesh)
            return;

        if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit,
                distanciaBusquedaNavMesh, agente.areaMask))
        {
            agente.Warp(hit.position);
            IrANuevaPosicion();
        }
    }

    private void IrANuevaPosicion()
    {
        if (agente == null || !agente.isOnNavMesh)
            return;

        for (int intento = 0; intento < 8; intento++)
        {
            Vector2 desplazamiento = Random.insideUnitCircle * radioMovimiento;
            Vector3 destinoPropuesto = transform.position + new Vector3(desplazamiento.x, 0f, desplazamiento.y);

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

    private void ActualizarAnimacion(bool caminando)
    {
        if (anim != null)
            anim.SetBool("isWalking", caminando);
    }
}

