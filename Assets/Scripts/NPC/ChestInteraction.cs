using System.Collections;
using UnityEngine;

public class ChestInteraction : MonoBehaviour
{
    public DialogueSystem dialogueSystem;
    public GameObject dialogueBubble;
    private bool isPlayerInZone = false;
    public Vector3 bubbleOffset = new Vector3(0, 1f, 0);

    public int keyID;
    private string[] fishingDialogue;
    public bool cofreOpen = false;
    int currentLanguage;

    private void Start()
    {
        currentLanguage = PlayerPrefs.GetInt("Language", 0); // 0 = Español, 1 = Inglés
        ToggleDialogueBubble(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = true;
            ToggleDialogueBubble(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = false;
            ToggleDialogueBubble(false);
        }
    }

    public void ToggleDialogueBubble(bool show)
    {
        dialogueBubble.SetActive(show);
    }

    async void Update()
    {
        if (isPlayerInZone && Input.GetKeyDown(KeyCode.E))
        {
            // Verifica si el jugador tiene la llave
            if (HUDManager.Instance.HasKey(keyID))
            {
                if (currentLanguage == 0) // Español
                {
                    fishingDialogue = new string[] { "¡Cofre obtenido!" };
                }
                else // Inglés
                {
                    fishingDialogue = new string[] { "Chest obtained!" };
                }

                // Marca el cofre como abierto y usa la llave
                cofreOpen = true;
                HUDManager.Instance.UseKey(keyID);

                Destroy(gameObject);  // Elimina el cofre después de abrirlo
            }
            else
            {
                if (currentLanguage == 0) // Español
                {
                    fishingDialogue = new string[] { "¡Te falta una llave!" };
                }
                else // Inglés
                {
                    fishingDialogue = new string[] { "You're missing a key!" };
                }
                
            }

            await dialogueSystem.StartDialogue(fishingDialogue, () => { });
        }
    }

    public bool CheckIfOpen(int keyID)
    {
        return cofreOpen;  // Devuelve si el cofre está abierto
    }
}
