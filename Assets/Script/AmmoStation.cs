using UnityEngine;
using System.Collections;

public class AmmoStation : MonoBehaviour
{
    [Header("Configuración estación")]
    public float tiempoRecarga = 2f;
    public float cooldownRecarga = 3f;
    public bool recargaAutomatica = true;

    [Header("Tipo de arma que recarga esta estación")]
    public WeaponType tipoDeArma;

    private bool jugadorCerca = false;
    private bool enCooldown = false;

    [Header("UI Popup")]
    public GameObject popupUI;

    private Raycasteo armaActual;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        jugadorCerca = true;
        ActualizarArmaActiva();

        if (popupUI != null)
            popupUI.SetActive(armaActual != null && armaActual.tipoArma == tipoDeArma);

        if (armaActual == null || armaActual.tipoArma != tipoDeArma)
        {
            Debug.Log("Esta estación no recarga esta arma.");
            return;
        }

        if (recargaAutomatica && !enCooldown)
            StartCoroutine(RecargarMunicion());
        else if (!recargaAutomatica && !enCooldown)
            Debug.Log("Presiona 'E' para recargar.");
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        jugadorCerca = false;
        armaActual = null;

        StopAllCoroutines();
        enCooldown = false;

        if (popupUI != null)
            popupUI.SetActive(false);
    }

    private void Update()
    {
        if (!jugadorCerca) return;

        ActualizarArmaActiva();

        if (armaActual == null || armaActual.tipoArma != tipoDeArma) return;

        if (!recargaAutomatica && !enCooldown && Input.GetKeyDown(KeyCode.E))
            StartCoroutine(RecargarMunicion());
    }

    // ===========
    // ENCUENTRA EL ARMA ACTIVA DEL WEAPONSWITCHER
    // ===========
    private void ActualizarArmaActiva()
    {
        var player = GameObject.FindWithTag("Player");
        if (player == null) return;

        var switcher = player.GetComponentInChildren<WeaponSwitcher>();
        if (switcher == null) return;

        for (int i = 0; i < switcher.transform.childCount; i++)
        {
            Transform arma = switcher.transform.GetChild(i);

            if (arma.gameObject.activeSelf)
            {
                armaActual = arma.GetComponentInChildren<Raycasteo>();
                return;
            }
        }

        armaActual = null;
    }

    // ===========
    // COROUTINE SEGURA (SIN NULLREFERENCE)
    // ===========
    private IEnumerator RecargarMunicion()
    {
        if (armaActual == null || enCooldown) yield break;

        Raycasteo armaARecargar = armaActual; // copia estable
        enCooldown = true;

        Debug.Log("Recargando...");
        yield return new WaitForSeconds(tiempoRecarga);

        if (armaARecargar == null)
        {
            Debug.Log("Recarga cancelada: el arma cambió.");
            enCooldown = false;
            yield break;
        }

        armaARecargar.RecargarDesdeEstacion();
        Debug.Log("¡Recarga completa!");

        yield return new WaitForSeconds(cooldownRecarga);
        enCooldown = false;

        if (!recargaAutomatica)
            Debug.Log("Presiona 'E' para recargar de nuevo.");
    }
}
