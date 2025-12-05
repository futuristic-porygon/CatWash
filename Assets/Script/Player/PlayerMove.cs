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

    [Header("Ground check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float radio;
    [SerializeField] private LayerMask whatIsGround;

    private bool isGrounded;

    AudioManager audioManager;

    public static bool dialogue = false;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        charCtrl = GetComponent<CharacterController>();
        if (groundCheck == null) groundCheck = transform.GetChild(2);
        velocidadActual = velocidadNormal;
    }

    private void Update()
    {
        if (!dialogue)
        {
            // Cambio de velocidad
            velocidadActual = Input.GetKey(KeyCode.LeftShift) ? velocidadRapida : velocidadNormal;

            // Movimiento horizontal
            movx = Input.GetAxis("Horizontal") * velocidadActual;
            movz = Input.GetAxis("Vertical") * velocidadActual;
            Vector3 movimiento = transform.right * movx + transform.forward * movz;
            charCtrl.Move(movimiento * Time.deltaTime);

            // Ground check
            isGrounded = Physics.CheckSphere(groundCheck.position, radio, whatIsGround);
            if (isGrounded && veloY.y < 0)
                veloY.y = 0f;

            // Salto normal
            if (isGrounded && Input.GetKeyDown(KeyCode.Space))
                veloY.y = Mathf.Sqrt(fuerzaSalto * -2f * gravity);
        }
    }

    private void FixedUpdate()
    {
        // Aplicar gravedad
        veloY.y += gravity * Time.fixedDeltaTime;

        // Aplicar movimiento vertical
        charCtrl.Move(veloY * Time.fixedDeltaTime);
    }
}
