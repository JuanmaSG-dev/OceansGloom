using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    public DialogueSystem dialogueSystem;
    public DecisionSystem decisionSystem;
    public GameObject dialogueBubble;
    private bool isPlayerInZone = false; // Flag para verificar si el jugador está en la zona
    public Vector3 bubbleOffset = new(0, 1f, 0);

    public string npcID;
    private string[] fishingDialogue;

    private void Start()
    {
        ToggleDialogueBubble(false);
        if (string.IsNullOrEmpty(npcID))
        {
            npcID = gameObject.name; // Usa el nombre del objeto como ID si no se asigna uno
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = true; // El jugador está en la zona
            ToggleDialogueBubble(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = false; // El jugador salió de la zona
            ToggleDialogueBubble(false);
        }
    }

    public void ToggleDialogueBubble(bool show)
    {
        dialogueBubble.SetActive(show);
    }

    void Update()
    {
        // Sólo iniciar el diálogo si el jugador está en la zona
        if (isPlayerInZone && Input.GetKeyDown(KeyCode.E))
        {
            switch (npcID)
            {
                case "0":
                    fishingDialogue = new string[] {
                        "Buenos días viajero.",
                        "Hacía tiempo que no veía un rostro nuevo.",
                        "La vida aquí no está tan mal.",
                        "Aunque te advierto que si sigues eso cambiará.",
                        "Nunca vi a nadie volver, da un poco de miedo."
                    };
                    break;

                case "1":
                    fishingDialogue = new string[] { "¡Ahoy!", "El mar es peligroso, pero gratificante." };
                    break;
                default:
                    fishingDialogue = new string[] { "Hola, soy un NPC genérico." };
                    break;
            }

            dialogueSystem.StartDialogue(fishingDialogue, () => { });
        }
    }
}
