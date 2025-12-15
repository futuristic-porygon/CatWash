using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

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
    public Camera cam;
    public Volume globalVolume;
    public bool enCharco = false;
    public bool enBoost = false;

    private MotionBlur motionBlur;

    [Header("Salto y gravedad")]
    [SerializeField] private float fuerzaSalto;
    [SerializeField] private float gravity = -9.8f;

    [Header("Ground check")]
    public Transform groundCheck;
    [SerializeField] private float radio;
    [SerializeField] private LayerMask whatIsGround;

    private bool isGrounded;

    AudioManager audioManager;

    public static bool dialogue = false;

    // Para controlar sonido de sprint
    private bool estabaCorriendo = false;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        charCtrl = GetComponent<CharacterController>();
        if (groundCheck == null) groundCheck = transform.GetChild(2);
        velocidadActual = velocidadNormal;
        if (cam == null) cam = Camera.main;
    }

    private void Start()
    {
        globalVolume.profile.TryGet(out motionBlur);
    }

    private void Update()
    {
        if (dialogue) return; // Bloquea movimiento durante diálogos

        bool puedeCorrer = GameManager.Instance != null && GameManager.Instance.patinesDesbloqueados;
        bool estaCorriendo = puedeCorrer && Input.GetKey(KeyCode.LeftShift);

        // ------------------ Sonido al iniciar sprint ------------------
        if (estaCorriendo && !estabaCorriendo)
        {
            audioManager.PlaySFX(audioManager.Sprint);
        }
        estabaCorriendo = estaCorriendo;

        // -------------------------------------------------------
        // CALCULAR VELOCIDAD FINAL
        // -------------------------------------------------------
        velocidadActual = velocidadNormal;

        if (enCharco)
            velocidadActual *= 0.5f;

        if (enBoost)
            velocidadActual *= 2f;

        if (!enCharco && estaCorriendo)
            velocidadActual = velocidadRapida;

        // FOV cuando corres
        float targetFOV = estaCorriendo ? 85f : 60f;
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFOV, Time.deltaTime * 5f);

        // Blur del sprint
        float blurTarget = estaCorriendo ? 1f : 0f;
        motionBlur.intensity.value = Mathf.Lerp(motionBlur.intensity.value, blurTarget, Time.deltaTime * 2f);

        // -------------------------------------------------------
        // MOVIMIENTO
        // -------------------------------------------------------
        movx = Input.GetAxis("Horizontal") * velocidadActual;
        movz = Input.GetAxis("Vertical") * velocidadActual;

        Vector3 movimiento = transform.right * movx + transform.forward * movz;
        charCtrl.Move(movimiento * Time.deltaTime);

        // -------------------------------------------------------
        // SALTO & GRAVEDAD
        // -------------------------------------------------------
        isGrounded = Physics.CheckSphere(groundCheck.position, radio, whatIsGround);
        if (isGrounded && veloY.y < 0)
            veloY.y = 0f;

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
            veloY.y = Mathf.Sqrt(fuerzaSalto * -2f * gravity);
    }

    private void FixedUpdate()
    {
        veloY.y += gravity * Time.fixedDeltaTime;
        charCtrl.Move(veloY * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Charco"))
        {
            enCharco = true;
            audioManager.PlaySFX(audioManager.Charco);
        }

        if (other.CompareTag("Boost"))
        {
            enBoost = true;
            audioManager.PlaySFX(audioManager.Boost);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Charco"))
            enCharco = false;

        if (other.CompareTag("Boost"))
            enBoost = false;
    }
}
