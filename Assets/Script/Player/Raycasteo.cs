using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Raycasteo : MonoBehaviour
{
    [SerializeField] private int danio;
    public float poderLimpieza = 50f;
    [SerializeField] private int fuerza;

    private Transform shootPoint;
    private RaycastHit hit;

    [Header("Bala")]
    [SerializeField] private Transform Bala;
    [SerializeField] private Transform shooter;
    [SerializeField] private float bolaVelo;

    [Header("Munición")]
    public int maxAmmo = 10;
    private int currentAmmo;
    private bool sinMunicion = false;
    public WeaponType tipoArma;

    [Header("UI")]
    [SerializeField] private Image ammoFill; // 👉 barra de agua interna

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void Start()
    {
        shootPoint = transform.parent;
        currentAmmo = maxAmmo;

        if (ammoFill != null)
            ammoFill.fillAmount = 1f; // lleno al iniciar
    }

    private void OnEnable()
    {
        sinMunicion = false;

        if (ammoFill != null)
            ammoFill.fillAmount = (float)currentAmmo / maxAmmo;
    }

    void Update()
    {
        if (sinMunicion) return;

        // Clic derecho → Raycast (empujar)
        if (Input.GetMouseButtonDown(1))
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

        // Clic izquierdo → Disparar
        if (Input.GetMouseButtonDown(0))
        {
            if (currentAmmo > 0)
            {
                Transform clone = Instantiate(Bala, shooter.position, shooter.rotation);
                clone.GetComponent<Rigidbody>().AddForce(transform.forward * bolaVelo);

                audioManager.PlaySFX(audioManager.weapon);

                currentAmmo--;
                ActualizarBarra();

                if (currentAmmo <= 0)
                {
                    sinMunicion = true;
                    //Debug.Log("Sin munición. Dirígete a una estación de recarga.");
                }
            }
            else
            {
                //Debug.Log("No puedes disparar. Necesitas recargar en una estación.");
            }
        }
    }

    
    private void ActualizarBarra()
    {
        if (ammoFill != null)
            ammoFill.fillAmount = Mathf.Clamp01((float)currentAmmo / maxAmmo);
    }

    
    public void RecargarDesdeEstacion()
    {
        currentAmmo = maxAmmo;
        sinMunicion = false;

        if (ammoFill != null)
            ammoFill.fillAmount = 1f;

        //Debug.Log("Munición restaurada desde estación de recarga.");
    }
}