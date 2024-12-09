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
        // S�lo iniciar el di�logo si el jugador está en la zona
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
                        "Nunca vi a nadie volver, da un poco de miedo.",
                        "Además, hay unas especies de grietas que cada vez aparecen más.",
                        "Llámame loco, pero la última vez que me acerqué a una, escuché una voz..."
                    };
                    break;
                case "1":
                    fishingDialogue = new string[] {
                        "Buenos días viajero.",
                        "Hacía tiempo que no veía un rostro nuevo.",
                        "La vida aquí no está tan mal.",
                        "Aunque te advierto que si sigues eso cambiará.",
                        "Nunca vi a nadie volver, da un poco de miedo.",
                        "Además, hay unas especies de grietas que cada vez aparecen más.",
                        "Llámame loco, pero la última vez que me acerqué a una, escuché una voz..."
                    };
                    break;
                case "2":
                    fishingDialogue = new string[] {
                        "Oh, una cara nueva.",
                        "Y no eres un pirata, ¿me equivoco?",
                        "Últimamente aquí vienen muchos malhechores buscando una fortuna.",
                        "Como pescador que soy, este sitio es una maravilla.",
                        "Nunca entendí porqué lo prohibieron, es igual de peligroso que cualquier océano.",
                        "¿Y tú qué haces aquí? ¿También eres un pescador?",
                        "Te daré un consejo, hay una probabilidad de conseguir un pescado con un color distinto.",
                        "Tener uno es una muestra de tu perseverancia!"
                    };
                    break;
                default:
                    fishingDialogue = new string[] { "Hola, soy un NPC genérico." };
                    break;
            }

            dialogueSystem.StartDialogue(fishingDialogue, () => { });
        }
    }
}
