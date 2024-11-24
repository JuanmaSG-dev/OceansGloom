using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    public DialogueSystem dialogueSystem;
    public DecisionSystem decisionSystem;
    public GameObject dialogueBubble;
    private Quaternion fixedRotationBubble;
    public Vector3 bubbleOffset = new(0, 1f, 0);

    private void Start()
    {
        ToggleDialogueBubble(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Si el objeto que entra es el barco
        if (other.CompareTag("Player"))
        {
                ToggleDialogueBubble(true);
        }
    }

    public void ToggleDialogueBubble(bool show)
    {
        dialogueBubble.SetActive(show);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ToggleDialogueBubble(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            string[] fishingDialogue = { "Buenos d�as viajero.", "Hac�a tiempo que no ve�a un rostro nuevo.", "La vida aqu� no est� tan mal.", "Aunque te advierto que si sigues eso cambiar�.", "Nunca vi a nadie volver, da un poco de miedo." };
            dialogueSystem.StartDialogue(fishingDialogue, () => { });
        }
    }
}
