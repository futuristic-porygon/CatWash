using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private CharacterController charCtrl;

    private float movx;
    private float movz;

    public Vector3 veloY;

    [Header("Movimiento")]
    [SerializeField] private float velocidadNormal = 5f;
    [SerializeField] private float velocidadRapida = 10f;
    private float velocidadActual;

    [Header("Salto y gravedad")]
    [SerializeField] private float fuerzaSalto;
    [SerializeField] private float gravity = -9.8f;

    [Header("Jetpack")]
    [SerializeField] private float jetpackForce = 8f;
    [SerializeField] private float fuel = 100f;
    [SerializeField] private float fuelDecreaseRate = 20f;
    [SerializeField] private float fuelRefillRate = 10f;
    [SerializeField] private float fuelRefillDelay = 2f;
    [SerializeField] private float heightLimit = 11.5f;

    [Header("Ground check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float radio;
    [SerializeField] private LayerMask whatIsGround;

    private bool isGrounded;
    private bool isHoldingSpace = false;
    private float fuelRefillTimer = 0f;

    private void Awake()
    {
        charCtrl = GetComponent<CharacterController>();
        if (groundCheck == null) groundCheck = transform.GetChild(2);
        velocidadActual = velocidadNormal;
    }

    private void Update()
    {
        // Cambio de velocidad
        velocidadActual = Input.GetKey(KeyCode.LeftShift) ? velocidadRapida : velocidadNormal;

        // Movimiento horizontal
        movx = Input.GetAxis("Horizontal") * velocidadActual * Time.deltaTime;
        movz = Input.GetAxis("Vertical") * velocidadActual * Time.deltaTime;
        Vector3 movimiento = transform.right * movx + transform.forward * movz;
        charCtrl.Move(movimiento);

        // Ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, radio, whatIsGround);
        if (isGrounded && veloY.y < 0) veloY.y = 0f;

        // Salto normal
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
            veloY.y = Mathf.Sqrt(fuerzaSalto * -2f * gravity);

        // Detectar jetpack
        if (Input.GetKeyDown(KeyCode.Space)) isHoldingSpace = true;
        if (Input.GetKeyUp(KeyCode.Space)) isHoldingSpace = false;

        // Recarga de combustible
        if (!isHoldingSpace && fuel < 100f)
        {
            fuelRefillTimer += Time.deltaTime;
            if (fuelRefillTimer >= fuelRefillDelay)
            {
                fuel += fuelRefillRate * Time.deltaTime;
                fuel = Mathf.Clamp(fuel, 0f, 100f);
            }
        }
        else
        {
            fuelRefillTimer = 0f;
        }
    }

    private void FixedUpdate()
    {
        // Jetpack
        if (isHoldingSpace && fuel > 0f && transform.position.y < heightLimit)
        {
            veloY.y = jetpackForce;
            fuel -= fuelDecreaseRate * Time.fixedDeltaTime;
            fuel = Mathf.Clamp(fuel, 0f, 100f);
        }

        // Aplicar gravedad
        veloY.y += gravity * Time.fixedDeltaTime;

        // Mover al jugador verticalmente
        charCtrl.Move(veloY * Time.fixedDeltaTime);

        // Limitar altura máxima
        if (transform.position.y > heightLimit)
        {
            Vector3 pos = transform.position;
            pos.y = heightLimit;
            transform.position = pos;
            if (veloY.y > 0f) veloY.y = 0f;
        }
    }
}