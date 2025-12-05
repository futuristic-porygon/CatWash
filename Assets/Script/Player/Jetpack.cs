using UnityEngine;

public class Jetpack : MonoBehaviour
{
    [Header("Configuración del Jetpack")]
    public float jetpackForce = 5f;
    public float heightLimit = 11.5f;

    [Header("Combustible")]
    public float fuel = 100f;
    public float fuelDecreaseRate = 20f;
    public float fuelRefillRate = 10f;
    public float fuelRefillDelay = 2f;

    [Header("UI")]
    public FuelDisplay fuelDisplay;

    private PlayerMove playerMove;
    private bool isHoldingSpace = false;
    private float fuelRefillTimer = 0f;

    public bool jetpackUnlocked = false;

    void Awake()
    {
        playerMove = GetComponent<PlayerMove>();

        if (playerMove == null)
            Debug.LogError("PlayerMove no encontrado");
    }

    void Update()
    {
        // Input jetpack
        if (Input.GetKeyDown(KeyCode.Space))
            isHoldingSpace = true;
        if (Input.GetKeyUp(KeyCode.Space))
            isHoldingSpace = false;

        // Recargar combustible
        if (!isHoldingSpace && fuel < 100f)
        {
            fuelRefillTimer += Time.deltaTime;

            if (fuelRefillTimer >= fuelRefillDelay)
            {
                fuel += fuelRefillRate * Time.deltaTime;
                fuel = Mathf.Clamp(fuel, 0, 100);

                fuelDisplay?.SetFuelLevel(fuel, 100f);
            }
        }
        else
        {
            fuelRefillTimer = 0f;
        }
    }

    private void FixedUpdate()
    {
        if (!jetpackUnlocked) return; // 🛑 No usar jetpack hasta estar desbloqueado

        if (isHoldingSpace && fuel > 0f)
        {
            if (transform.position.y < heightLimit)
            {
                playerMove.veloY.y = jetpackForce;

                fuel -= fuelDecreaseRate * Time.fixedDeltaTime;
                fuel = Mathf.Clamp(fuel, 0, 100);

                fuelDisplay?.SetFuelLevel(fuel, 100f);
            }
            else
            {
                playerMove.veloY.y = 0f;

                fuel -= fuelDecreaseRate * Time.fixedDeltaTime;
                fuel = Mathf.Clamp(fuel, 0, 100);

                fuelDisplay?.SetFuelLevel(fuel, 100f);
            }
        }
    }
    public void SetJetpackState(bool state)
    {
        jetpackUnlocked = state;
    }
}
