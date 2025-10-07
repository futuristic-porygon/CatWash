using UnityEngine;

public class Bala : MonoBehaviour
{

    public float poderLimpieza = 10f;
    public float tiempoVida = 5f;

    void Start()
    {

        Destroy(gameObject, tiempoVida);
    }

    void OnCollisionEnter(Collision collision)
    {

        /*Gato gato = collision.gameObject.GetComponent<Gato>();
        if (gato != null)
        {
            gato.Limpiar(poderLimpieza);
        }*/


    }
}