using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Prefab del gato")]
    public GameObject gatoPrefab;

    [Header("Spawn simultáneo")]
    public int maxSimultaneos = 10;       // Límite actual
    public int aumentoAlReanudar = 5;    // Aumenta +5 cuando Timer se reanuda

    [Header("Intervalo de spawn")]
    public float intervaloSpawn = 2f;     // Spawnea 1 cada cierto tiempo

    [Header("Rango de Spawn (relativo al Spawner)")]
    public Vector2 rangoX = new Vector2(-16, 13);
    public Vector2 rangoZ = new Vector2(-20, 1);

    [Header("Validación de Terreno")]
    public LayerMask groundMask;
    public float alturaRaycast = 10f;
    public float distanciaRaycast = 50f;
    [Range(0f, 60f)]
    public float maxAnguloInclinacion = 20f;

    private List<GameObject> gatosVivos = new List<GameObject>();
    private bool puedeSpawnear = true;

    void Start()
    {
        StartCoroutine(ControlSpawn());
    }

    private IEnumerator ControlSpawn()
    {
        while (true)
        {
            // Si hay menos gatos del límite → spawn uno
            if (puedeSpawnear && gatosVivos.Count < maxSimultaneos)
            {
                SpawnearSiHaySuelo();
                yield return new WaitForSeconds(intervaloSpawn);
            }
            else
            {
                yield return null;
            }
        }
    }

    private void SpawnearSiHaySuelo()
    {
        float xPos = Random.Range(rangoX.x, rangoX.y);
        float zPos = Random.Range(rangoZ.x, rangoZ.y);

        Vector3 basePos = transform.position + new Vector3(xPos, 0, zPos);

        if (Physics.Raycast(basePos + Vector3.up * alturaRaycast, Vector3.down, out RaycastHit hit, distanciaRaycast, groundMask))
        {
            if (Vector3.Angle(hit.normal, Vector3.up) <= maxAnguloInclinacion)
            {
                GameObject gato = Instantiate(gatoPrefab, hit.point, Quaternion.identity);
                gatosVivos.Add(gato);

                // 🔹 Remover automáticamente cuando desaparezca
                StartCoroutine(RemoverCuandoMuera(gato));
            }
        }
    }

    private IEnumerator RemoverCuandoMuera(GameObject gato)
    {
        while (gato != null)
            yield return null;

        gatosVivos.Remove(gato);
    }

    // 🔹 Aumentar límite cuando Timer reanuda
    public void AumentarLimiteSimultaneo()
    {
        maxSimultaneos += aumentoAlReanudar;
        Debug.Log($"Nuevo límite de gatos simultáneos: {maxSimultaneos}");
    }

    // 🔹 Gizmo (igual que antes)
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.25f);

        float width = rangoX.y - rangoX.x;
        float depth = rangoZ.y - rangoZ.x;

        Vector3 center = transform.position + new Vector3(
            (rangoX.x + rangoX.y) / 2f,
            0,
            (rangoZ.x + rangoZ.y) / 2f
        );

        Vector3 size = new Vector3(width, 0, depth);

        Gizmos.DrawCube(center, size);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, size);
    }
}
