using UnityEngine;

public class GatoFlotante : MonoBehaviour
{
    public Animator anim;

    [Header("Movimiento general")]
    public float velocidadMovimiento = 1.5f;     // velocidad horizontal
    public float radioMovimiento = 4f;           // rango desde el punto original

    [Header("Flotación vertical")]
    public float amplitudFlotacion = 0.5f;       // qué tanto sube/baja
    public float velocidadFlotacion = 2f;        // ritmo (ondas)

    [Header("Rotación suave")]
    public float suavizadoRotacion = 4f;

    private Vector3 centro;          // punto de referencia inicial
    private float tiempo;

    private Vector3 objetivo;        // siguiente punto al que va el gato

    void Start()
    {
        centro = transform.position;
        tiempo = Random.Range(0f, 5f);

        if (anim == null)
            anim = GetComponent<Animator>();

        NuevoDestino();
    }

    void Update()
    {
        tiempo += Time.deltaTime * velocidadFlotacion;

        // —— FLUCTUACIÓN VERTICAL (ritmo) ——
        float offsetY = Mathf.Sin(tiempo) * amplitudFlotacion;

        // —— MOVIMIENTO HACIA EL OBJETIVO ——
        Vector3 posActual = transform.position;
        Vector3 destinoConFlotacion = new Vector3(objetivo.x, centro.y + offsetY, objetivo.z);

        transform.position = Vector3.MoveTowards(
            posActual,
            destinoConFlotacion,
            velocidadMovimiento * Time.deltaTime
        );

        // —— ROTACIÓN SUAVE HACIA LA DIRECCIÓN ——
        Vector3 direccion = (destinoConFlotacion - posActual).normalized;
        if (direccion.magnitude > 0.1f)
        {
            Quaternion rot = Quaternion.LookRotation(direccion);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * suavizadoRotacion);
        }

        // Si llegó cerca → elegimos un nuevo destino flotante
        if (Vector3.Distance(posActual, destinoConFlotacion) < 0.2f)
        {
            NuevoDestino();
        }

        // —— Animación ——
        anim.SetBool("isWalking", true); // siempre moviéndose
    }

    void NuevoDestino()
    {
        // El objetivo siempre estará dentro de un rango alrededor del punto central
        Vector2 circulo = Random.insideUnitCircle * radioMovimiento;

        objetivo = new Vector3(
            centro.x + circulo.x,
            centro.y,    // Y se controla solo por la onda flotante
            centro.z + circulo.y
        );
    }
}
