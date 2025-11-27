using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class NPCSystem : MonoBehaviour
{
    public GameObject canvas;               // Canvas del diálogo
    public TextMeshProUGUI dialogueText;

    public GameObject pressEText;           // ← NUEVO texto "Press E to talk"

    bool player_detection = false;
    bool isDialogueRunning = false;

    List<string> lines = new List<string>();
    int index = 0;

    void Update()
    {
        // Iniciar diálogo
        if (player_detection && Input.GetKeyDown(KeyCode.E) && !isDialogueRunning)
        {
            StartDialogue();
        }
        // Avanzar diálogo
        else if (isDialogueRunning && Input.GetKeyDown(KeyCode.E))
        {
            Next();
        }
    }

    void StartDialogue()
    {
        pressEText.SetActive(false); // Ocultar "Press E"

        lines.Clear();
        lines.Add("Heyo!");
        lines.Add("Why don't you start cleaning that cat over there?");
        lines.Add("There's a refill machine behind me. They're red to find them easily.");
        lines.Add("You can always leave by opening the pause menu using ESC.");

        index = 0;
        isDialogueRunning = true;
        PlayerMove.dialogue = true;

        canvas.SetActive(true);
        dialogueText.text = lines[index];
    }

    void Next()
    {
        index++;

        if (index >= lines.Count)
        {
            EndDialogue();
            return;
        }

        dialogueText.text = lines[index];
    }

    void EndDialogue()
    {
        canvas.SetActive(false);
        isDialogueRunning = false;
        PlayerMove.dialogue = false;

        // Mostrar "Press E" si sigues cerca
        if (player_detection)
            pressEText.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Jugador detectado");
            player_detection = true;
            if (!isDialogueRunning)
                pressEText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player_detection = false;
            pressEText.SetActive(false); // Ocultar texto
        }
    }
}
