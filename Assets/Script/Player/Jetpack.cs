using UnityEngine;

public class Jetpack : MonoBehaviour
{
    [Header("Configuración del Jetpack")]
    public float jetpackForce = 5f;      // Fuerza de empuje vertical
    public float heightLimit = 11.5f;    // Altura máxima permitida

    [Header("Combustible")]
    public float fuel = 100f;
    public float fuelDecreaseRate = 20f;
    public float fuelRefillRate = 10f;
    public float fuelRefillDelay = 2f;

    [Header("UI")]
    public FuelDisplay fuelDisplay;      // Referencia a la barra de combustible

    private PlayerMove playerMove;
    private bool isHoldingSpace = false;
    private float fuelRefillTimer = 0f;

    void Awake()
    {
        playerMove = GetComponent<PlayerMove>();
        if (playerMove == null)
            Debug.LogError("⚠️ PlayerMove no encontrado en el mismo GameObject.");

        if (fuelDisplay == null)
            Debug.LogError("⚠️ FuelDisplay no asignado en el Inspector.");
    }

    void Start()
    {
        // Actualizar barra inicial
        if (fuelDisplay != null)
            fuelDisplay.SetFuelLevel(fuel, 100f);
    }

    void Update()
    {
        // Detectar input del jetpack
        if (Input.GetKeyDown(KeyCode.Space))
            isHoldingSpace = true;
        if (Input.GetKeyUp(KeyCode.Space))
            isHoldingSpace = false;

        // Recarga de combustible cuando no se usa
        if (!isHoldingSpace && fuel < 100f)
        {
            fuelRefillTimer += Time.deltaTime;
            if (fuelRefillTimer >= fuelRefillDelay)
            {
                fuel += fuelRefillRate * Time.deltaTime;
                fuel = Mathf.Clamp(fuel, 0f, 100f);

                if (fuelDisplay != null)
                    fuelDisplay.SetFuelLevel(fuel, 100f);
            }
        }
        else
        {
            fuelRefillTimer = 0f;
        }
    }

    void FixedUpdate()
    {
        if (isHoldingSpace && fuel > 0f)
        {
            // Aplicar empuje solo si no se supera la altura máxima
            if (transform.position.y < heightLimit)
            {
                playerMove.veloY.y = jetpackForce;

                fuel -= fuelDecreaseRate * Time.fixedDeltaTime;
                fuel = Mathf.Clamp(fuel, 0f, 100f);

                if (fuelDisplay != null)
                    fuelDisplay.SetFuelLevel(fuel, 100f);
            }
            else
            {
                playerMove.veloY.y = 0f;
            }
        }
    }
}