using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private CharacterController charCtrl;

    private float movx;
    private float movz;

    private Vector3 veloY;

    [SerializeField]
    private float fuerzaSalto;

    [SerializeField]
    private float gravity = -9.8f;

    [SerializeField]
    private float velocidad;

    private bool isGrounded;
    private Transform groundCheck;

    [SerializeField]
    private float radio;
    [SerializeField]
    private LayerMask whatIsGround;

    private void Awake()
    {
        charCtrl = GetComponent<CharacterController>();
        groundCheck = transform.GetChild(2);
    }

    private void Update()
    {
        movx = Input.GetAxis("Horizontal") * velocidad * Time.deltaTime;
        movz = Input.GetAxis("Vertical") * velocidad * Time.deltaTime;

        //Para que se mueva en SU izquierda y derecha, no en la global
        Vector3 movimiento = transform.right * movx + transform.forward * movz;
        charCtrl.Move(movimiento);


        isGrounded = Physics.CheckSphere(groundCheck.position, radio, whatIsGround);

        if (isGrounded && veloY.y < 0)
        {
            veloY.y = 0;
        }

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            veloY.y = Mathf.Sqrt(fuerzaSalto * gravity * -2);
        }

        //Ejerciendole la gravedad
        veloY.y += gravity * Time.deltaTime;

        charCtrl.Move(veloY * Time.deltaTime);


    }
}
