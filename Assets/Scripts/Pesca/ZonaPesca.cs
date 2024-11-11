using UnityEngine;

public class ZonaPesca : MonoBehaviour
{
    private ShipController shipController;  // Referencia al ShipController del barco
    public DialogueSystem dialogueSystem;
    public DecisionSystem decisionSystem;

    void OnTriggerEnter2D(Collider2D other)
    {
        // Si el objeto que entra es el barco
        if (other.CompareTag("Player"))
        {
            // Obtenemos el componente ShipController del barco
            shipController = other.GetComponent<ShipController>();

            // Activamos la burbuja de diálogo si encontramos el ShipController
            if (shipController != null)
            {
                shipController.ToggleDialogueBubble(true);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Cuando el barco sale de la zona de pesca, apagamos la burbuja
        if (other.CompareTag("Player") && shipController != null)
        {
            shipController.ToggleDialogueBubble(false);
            shipController = null;  // Limpiamos la referencia
        }
    }

    void Update()
    {
        // Si el jugador está en la zona de pesca y presiona E, ejecuta la acción de pesca
        if (shipController != null && Input.GetKeyDown(KeyCode.E))
        {
            string[] fishingDialogue = { "You found a fishing spot!", "Press E to start fishing." };
            dialogueSystem.StartDialogue(fishingDialogue, () =>
            {
                // Una vez terminado el diálogo, mostrar las opciones de decisión
                string[] options = { "Fish", "Leave" };
                decisionSystem.StartDecision(options, OnDecisionMade);
            });
        }
    }

    private void OnDecisionMade(int choice)
    {
        if (choice == 0)
        {
            Debug.Log("You chose to Fish!");
            // Lógica para iniciar la pesca
        }
        else if (choice == 1)
        {
            Debug.Log("You chose to Leave.");
            // Lógica para salir o cerrar el diálogo
        }
    }
}
