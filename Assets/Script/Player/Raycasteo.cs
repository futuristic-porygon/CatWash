using UnityEngine;
using System.Collections;

public class Raycasteo : MonoBehaviour
{
    [SerializeField] private int danio;
    public float poderLimpieza = 50f;
    [SerializeField] private int fuerza;

    private Transform shootPoint;
    private RaycastHit hit;

    [SerializeField] private Transform Bala;
    [SerializeField] private Transform shooter;
    [SerializeField] private float bolaVelo;

    public int maxAmmo = 10;
    private int currentAmmo;
    private bool sinMunicion = false;

    private void Start()
    {
        shootPoint = transform.parent;
        currentAmmo = maxAmmo;
    }

    void OnEnable()
    {
        sinMunicion = false;
    }

    void Update()
    {
        // Si no hay munición, no puede disparar
        if (sinMunicion) return;

        if (Input.GetMouseButtonDown(0))
        {
            // Disparo de raycast
            Physics.Raycast(shootPoint.position, transform.forward, out hit, 100);
            if (hit.transform != null)
            {
                Debug.Log(hit.transform.name);

                if (hit.transform.CompareTag("Enemigo"))
                {
                    Rigidbody rb = hit.rigidbody;
                    if (rb != null)
                    {
                        rb.AddForce(-hit.normal * fuerza);
                        rb.AddForceAtPosition(shootPoint.forward * fuerza, hit.point);
                    }
                    //hit.transform.GetComponent<VidaEnemigo>().TomarDanio(danio);
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            // Disparo de proyectil
            if (currentAmmo > 0)
            {
                Transform clone = Instantiate(Bala, shooter.position, shooter.rotation);
                clone.GetComponent<Rigidbody>().AddForce(transform.forward * bolaVelo);

                // Limpieza de enemigos
                if (hit.collider != null && hit.collider.CompareTag("Enemigo"))
                {
                    hit.collider.GetComponent<Gato>()?.Limpiar(poderLimpieza * Time.deltaTime * 20);
                }

                currentAmmo--;
                if (currentAmmo <= 0)
                {
                    sinMunicion = true;
                    Debug.Log("Sin munición. Dirígete a una estación de recarga.");
                }
            }
            else
            {
                Debug.Log("No puedes disparar. Necesitas recargar en una estación.");
            }
        }
    }

    // Método llamado por la estación de recarga
    public void RecargarDesdeEstacion()
    {
        currentAmmo = maxAmmo;
        sinMunicion = false;
        Debug.Log("Munición restaurada desde estación de recarga.");
    }
}