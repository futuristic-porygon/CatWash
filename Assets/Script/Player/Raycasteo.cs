using UnityEngine;
using UnityEngine.UI; // 👈 necesario para usar Slider
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

    [Header("UI")]
    [SerializeField] private Slider ammoBar; // 👈 referencia a la barra de munición

    private void Start()
    {
        shootPoint = transform.parent;
        currentAmmo = maxAmmo;

        if (ammoBar != null)
        {
            ammoBar.maxValue = maxAmmo;
            ammoBar.value = currentAmmo;
        }
    }

    void OnEnable()
    {
        sinMunicion = false;
        if (ammoBar != null) ammoBar.value = currentAmmo;
    }

    void Update()
    {
        if (sinMunicion) return;

        if (Input.GetMouseButtonDown(0))
        {
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
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (currentAmmo > 0)
            {
                Transform clone = Instantiate(Bala, shooter.position, shooter.rotation);
                clone.GetComponent<Rigidbody>().AddForce(transform.forward * bolaVelo);

                if (hit.collider != null && hit.collider.CompareTag("Enemigo"))
                {
                    hit.collider.GetComponent<Gato>()?.Limpiar(poderLimpieza * Time.deltaTime * 20);
                }

                currentAmmo--;
                ActualizarBarra(); // 👈 actualiza la barra

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

    // 🔄 Método para actualizar la barra de munición
    private void ActualizarBarra()
    {
        if (ammoBar != null)
            ammoBar.value = currentAmmo;
    }

    // 🔋 Método llamado por la estación de recarga
    public void RecargarDesdeEstacion()
    {
        currentAmmo = maxAmmo;
        sinMunicion = false;
        Debug.Log("Munición restaurada desde estación de recarga.");
        ActualizarBarra(); // 👈 actualiza también al recargar
    }
}