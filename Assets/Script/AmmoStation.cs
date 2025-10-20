using UnityEngine;
using System.Collections;

public class AmmoStation : MonoBehaviour
{
    [Header("Configuración de estación de recarga")]
    public float tiempoRecarga = 2f;
    public bool recargaAutomatica = true;

    private bool jugadorCerca = false;
    private Raycasteo raycasteoJugador;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;
            raycasteoJugador = other.GetComponentInChildren<Raycasteo>();

            if (recargaAutomatica)
                StartCoroutine(RecargarMunicion());
            else
                Debug.Log("Presiona 'E' para recargar.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
            raycasteoJugador = null;
        }
    }

    private void Update()
    {
        if (!recargaAutomatica && jugadorCerca && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(RecargarMunicion());
        }
    }

    private IEnumerator RecargarMunicion()
    {
        if (raycasteoJugador == null) yield break;

        Debug.Log("Recargando en estación...");
        yield return new WaitForSeconds(tiempoRecarga);

        raycasteoJugador.RecargarDesdeEstacion();
    }
}