using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject gatoPrefab;
    public int maxGatos = 20;
    public float spawnInterval = 2f;

    private List<GameObject> listaGatos = new List<GameObject>();
    private int contador = 0;

    void Start()
    {
        StartCoroutine(SpawnGatos());
    }

    private IEnumerator SpawnGatos()
    {
        while (contador < maxGatos)
        {
            int xPos = Random.Range(-16, 13);
            int zPos = Random.Range(-20, 1);

            GameObject gato = Instantiate(gatoPrefab, new Vector3(xPos, 0, zPos), Quaternion.identity);

            listaGatos.Add(gato);

           
            StartCoroutine(RemoverGatoAlDestruir(gato));

            contador++;
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private IEnumerator RemoverGatoAlDestruir(GameObject gato)
    {
       
        while (gato != null)
        {
            yield return null;
        }

        
        listaGatos.Remove(gato);
    }
}