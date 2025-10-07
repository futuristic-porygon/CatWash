using UnityEngine;

public class Raycasteo : MonoBehaviour
{
    [SerializeField]
    private int danio;
    public float poderLimpieza = 50f;

    [SerializeField]
    private int fuerza;

    private Transform shootPoint;

    private RaycastHit hit;

    [SerializeField]
    private Transform Bala;
    [SerializeField]
    private Transform shooter;
    [SerializeField]
    private float bolaVelo;
    //public AudioSource audioSource;


    private void Start()
    {
        shootPoint = transform.parent;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Physics.Raycast(shootPoint.position, transform.forward, out hit, 100);
            if (hit.transform != null)
            {
                Debug.Log(hit.transform.name);

                if (hit.transform.CompareTag("Enemigo"))
                {
                    hit.rigidbody.AddForce(-hit.normal * fuerza);
                    hit.rigidbody.AddForceAtPosition(shootPoint.forward * fuerza, hit.point);
                    //hit.transform.GetComponent<VidaEnemigo>().TomarDanio(danio);
                }
                if (hit.transform.CompareTag("Caja"))
                {
                    hit.rigidbody.AddForce(-hit.normal * fuerza);
                    hit.rigidbody.AddForceAtPosition(shootPoint.forward * fuerza, hit.point);
                }

            }


        }
        if (Input.GetMouseButtonDown(1))
        {
            Transform clone = Instantiate(Bala, shooter.position, shooter.rotation);
            clone.GetComponent<Rigidbody>().AddForce(transform.forward * bolaVelo);
            if (hit.collider.CompareTag("Enemigo"))
            {
                //hit.collider.GetComponent<Gato>().Limpiar(poderLimpieza * Time.deltaTime * 20);
            }
            //audioSource.Play();
        }
    }
}


