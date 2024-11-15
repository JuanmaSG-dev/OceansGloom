using UnityEngine;

public class ZonaPesca : MonoBehaviour
{
    private ShipController shipController;  // Referencia al ShipController del barco
    public DialogueSystem dialogueSystem;
    public DecisionSystem decisionSystem;

    public FishingMinigame fishingMinigame;

    public delegate void FishingCompleted(GameObject zone);
    public event FishingCompleted onFishingCompleted;  // Evento que se activa cuando se pesca en la zona

    void OnTriggerEnter2D(Collider2D other)
    {
        // Si el objeto que entra es el barco
        if (other.CompareTag("Player"))
        {
            // Obtenemos el componente ShipController del barco
            shipController = other.GetComponent<ShipController>();

            // Activamos la burbuja de di�logo si encontramos el ShipController
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
        // Si el jugador est� en la zona de pesca y presiona E, ejecuta la acci�n de pesca
        if (shipController != null && Input.GetKeyDown(KeyCode.E) && !fishingMinigame.isFishingActive)
        {
            string[] fishingDialogue = { "Encontr�ste una zona de pesca.", "�Quieres pescar aqu�?" };
            dialogueSystem.StartDialogue(fishingDialogue, () =>
            {
                // Una vez terminado el di�logo, mostrar las opciones de decisi�n
                string[] options = { "Pescar", "Irse" };
                decisionSystem.StartDecision(options, OnDecisionMade);
            });
        }
    }

    private void OnDecisionMade(int choice)
    {
        if (choice == 0)
        {
            Debug.Log("You chose to Fish!");
            StartFishingMinigame();
            // L�gica para iniciar la pesca
        }
        else if (choice == 1)
        {
            Debug.Log("You chose to Leave.");
            // L�gica para salir o cerrar el di�logo
        }
    }

    private void StartFishingMinigame()
    {
        shipController.SetControlEnabled(false);
        fishingMinigame.gameObject.SetActive(true);
        fishingMinigame.StartFishing();
    }

    private void FinishFishing()
    {
        onFishingCompleted?.Invoke(gameObject);
    }
}
