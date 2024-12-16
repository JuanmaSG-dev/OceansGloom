using UnityEngine;
using TMPro;
using System;
using System.Threading.Tasks;  // Necesario para trabajar con Task

public class DialogueSystem : MonoBehaviour
{
    // Panel de Diálogo
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    private string[] dialogueLines;
    private int currentLine = 0;

    public ShipController shipController;

    private Action onFinishCallback;  // Para el callback de terminar el diálogo

    private void Start()
    {
        dialoguePanel.SetActive(false); // Se asegura de que el panel esté oculto al principio.
    }

    // Inicia el diálogo y retorna un Task para esperar a que termine
    public async Task StartDialogue(string[] lines, Action onFinish = null)
    {
        dialogueLines = lines;
        currentLine = 0;
        dialoguePanel.SetActive(true);
        ShowLine();

        shipController.SetControlEnabled(false); // Desactiva los controles del barco
        shipController.currentSpeed = 0; // Detiene el movimiento del barco

        onFinishCallback = onFinish;  // Guarda el callback para cuando se termine el diálogo

        // Espera hasta que el jugador termine de ver el diálogo
        await WaitForDialogueToFinish();
    }

    // Este método será llamado en cada frame
    private void Update()
    {
        // Si el panel de diálogo está activo y el jugador presiona "Espacio", avanza
        if (dialoguePanel.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            NextLine();  // Avanza a la siguiente línea de diálogo
        }
    }

    // Muestra la línea de diálogo actual
    private void ShowLine()
    {
        dialogueText.text = dialogueLines[currentLine];
    }

    // Avanza a la siguiente línea o cierra el diálogo si ha terminado
    private void NextLine()
    {
        currentLine++;
        if (currentLine < dialogueLines.Length)
        {
            ShowLine();  // Muestra la siguiente línea
        }
        else
        {
            EndDialogue();  // Si no hay más líneas, termina el diálogo
        }
    }

    // Termina el diálogo
    private void EndDialogue()
    {
        shipController.SetControlEnabled(true); // Reactiva los controles del barco
        dialoguePanel.SetActive(false);  // Desactiva el panel de diálogo

        // Llama al callback, si lo proporcionaron
        onFinishCallback?.Invoke();
    }

    // Método para esperar a que el diálogo termine
    private async Task WaitForDialogueToFinish()
    {
        // Espera hasta que el diálogo haya terminado
        while (currentLine < dialogueLines.Length)
        {
            await Task.Yield();  // Espera hasta el siguiente frame
        }

        // Al finalizar, invoca el callback si se proporcionó uno
        onFinishCallback?.Invoke();
    }
}
