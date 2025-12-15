using UnityEngine;

public class VictorySceneSetup : MonoBehaviour
{
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 1f; // asegurarse que el juego no siga en pausa
    }
}