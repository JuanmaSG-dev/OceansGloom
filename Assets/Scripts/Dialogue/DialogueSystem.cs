using UnityEngine;
using TMPro;
using System;

public class DialogueSystem : MonoBehaviour
{
    // Panel de Di�logo
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    private string[] dialogueLines;
    private int currentLine = 0;

    public ShipController shipController;

    private Action onFinishCallback;  // Para el callback de terminar el di�logo

    private void Start()
    {
        dialoguePanel.SetActive(false); // Se asegura de que el panel est� oculto al principio.
    }

    // Inicia el di�logo
    public void StartDialogue(string[] lines, Action onFinish = null)
    {
        dialogueLines = lines;
        currentLine = 0;
        dialoguePanel.SetActive(true);
        ShowLine();

        shipController.SetControlEnabled(false); // Desactiva los controles del barco
        shipController.currentSpeed = 0; // Detiene el movimiento del barco

        onFinishCallback = onFinish;  // Guarda el callback para cuando se termine el di�logo
    }

    // Este m�todo ser� llamado en cada frame
    private void Update()
    {
        // Si el panel de di�logo est� activo y el jugador presiona "Espacio", avanza
        if (dialoguePanel.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            NextLine();  // Avanza a la siguiente l�nea de di�logo
        }
    }

    // Muestra la l�nea de di�logo actual
    private void ShowLine()
    {
        dialogueText.text = dialogueLines[currentLine];
    }

    // Avanza a la siguiente l�nea o cierra el di�logo si ha terminado
    private void NextLine()
    {
        currentLine++;
        if (currentLine < dialogueLines.Length)
        {
            ShowLine();  // Muestra la siguiente l�nea
        }
        else
        {
            EndDialogue();  // Si no hay m�s l�neas, termina el di�logo
        }
    }

    // Termina el di�logo
    private void EndDialogue()
    {
        shipController.SetControlEnabled(true); // Reactiva los controles del barco
        dialoguePanel.SetActive(false);  // Desactiva el panel de di�logo

        // Llama al callback, si lo proporcionaron
        onFinishCallback?.Invoke();
    }
}
