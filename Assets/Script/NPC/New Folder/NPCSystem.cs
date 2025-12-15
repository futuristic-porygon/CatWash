using UnityEngine;
using TMPro;

public class NPCSystem : MonoBehaviour
{
    public GameObject canvas;
    public TextMeshProUGUI dialogueText;

    public GameObject pressEText;

    public DialogueData dialogue;   // ← ScriptableObject con el diálogo

    private bool player_detection = false;
    private bool isDialogueRunning = false;

    private int index = 0;

    void Update()
    {
        if (player_detection && Input.GetKeyDown(KeyCode.E) && !isDialogueRunning)
        {
            StartDialogue();
        }
        else if (isDialogueRunning && Input.GetKeyDown(KeyCode.E))
        {
            NextLine();
        }
    }

    void StartDialogue()
    {
        if (dialogue == null || dialogue.lines.Count == 0)
        {
            Debug.LogWarning($"El NPC {name} no tiene diálogo asignado.");
            return;
        }

        pressEText.SetActive(false);

        index = 0;
        isDialogueRunning = true;
        PlayerMove.dialogue = true;

        canvas.SetActive(true);
        dialogueText.text = dialogue.lines[index];
    }

    void NextLine()
    {
        index++;

        if (index >= dialogue.lines.Count)
        {
            EndDialogue();
            return;
        }

        dialogueText.text = dialogue.lines[index];
    }

    void EndDialogue()
    {
        dialogueText.text = "";        // ← LIMPIA EL TEXTO
        canvas.SetActive(false);       // Oculta la caja de diálogo
        isDialogueRunning = false;
        PlayerMove.dialogue = false;

        if (player_detection)
            pressEText.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
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
            pressEText.SetActive(false);
        }
    }
}
