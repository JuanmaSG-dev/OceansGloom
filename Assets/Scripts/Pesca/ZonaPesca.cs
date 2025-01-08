using UnityEngine;

public class ZonaPesca : MonoBehaviour
{
    private ShipController shipController;  // Referencia al ShipController del barco
    public DialogueSystem dialogueSystem;
    public DecisionSystem decisionSystem;

    public FishingMinigame fishingMinigame;
    public Bounds Bounds => GetComponent<CircleCollider2D>().bounds;

    public delegate void FishingCompleted(GameObject zone);
    public event FishingCompleted onFishingCompleted;  // Evento que se activa cuando se pesca en la zona
    int currentLanguage;
    private string[] fishingDialogue;

    private void Start()
    {
        currentLanguage = PlayerPrefs.GetInt("Language", 0); // 0 = Español, 1 = Inglés
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Si el objeto que entra es el barco
        if (other.CompareTag("Player"))
        {
            // Obtenemos el componente ShipController del barco
            shipController = other.GetComponent<ShipController>();

            // Activamos la burbuja de dialogo si encontramos el ShipController
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

    async void Update()
    {
        // Si el jugador esta en la zona de pesca y presiona E, ejecuta la accion de pesca
        if (shipController != null && Input.GetKeyDown(KeyCode.E) && !fishingMinigame.isFishingActive)
        {
            if (currentLanguage == 0) // Español
                {
                    fishingDialogue = new string[] { "Encontráste una zona de pesca.", "¿Quieres pescar aquí?" };
                }
                else // Inglés
                {
                    fishingDialogue = new string[] { "You found a fishing zone.", "Do you want to fish here?" };
                }
            
            await dialogueSystem.StartDialogue(fishingDialogue, () =>
            {
                // Una vez terminado el dialogo, mostrar las opciones de decision
                if (currentLanguage == 0) // Español
                {
                    string[] options = { "Pescar", "Irse" };
                    decisionSystem.StartDecision(options, OnDecisionMade);
                }
                else // Inglés
                {
                    string[] options = { "Fish", "Leave" };
                    decisionSystem.StartDecision(options, OnDecisionMade);
                }
                
            });
        }
    }

    private void OnDecisionMade(int choice)
    {
        if (choice == 0)
        {
            Debug.Log("You chose to Fish!");
            StartFishingMinigame();
            // Logica para iniciar la pesca
        }
        else if (choice == 1)
        {
            Debug.Log("You chose to Leave.");
            // Logica para salir o cerrar el dialogo
        }
    }

    private void StartFishingMinigame()
    {
        shipController.SetControlEnabled(false);
        fishingMinigame.gameObject.SetActive(true);
        fishingMinigame.StartFishing(this);
    }

    private void FinishFishing()
    {
        onFishingCompleted?.Invoke(gameObject);
    }

    public void DeactivateZone()
    {
        Destroy(gameObject);
    }

}
