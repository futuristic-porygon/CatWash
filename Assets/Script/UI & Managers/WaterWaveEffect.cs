using UnityEngine;
using UnityEngine.UI;

public class WaterWaveEffect : MonoBehaviour
{
    public float speed = 0.2f; // Velocidad de movimiento
    public float intensity = 0.02f; // Qué tanto se mueve la textura

    private Material mat;

    void Start()
    {
        Image img = GetComponent<Image>();

        // Clonamos el material para no modificar el original
        mat = Instantiate(img.material);
        img.material = mat;
    }

    void Update()
    {
        if (mat != null)
        {
            float offset = Mathf.Sin(Time.time * speed) * intensity;
            mat.mainTextureOffset = new Vector2(offset, 0f);
        }
    }
}