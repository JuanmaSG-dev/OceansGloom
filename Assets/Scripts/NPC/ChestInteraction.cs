using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestInteraction : MonoBehaviour
{
    public DialogueSystem dialogueSystem;
    public DecisionSystem decisionSystem;
    public GameObject dialogueBubble;
    private bool isPlayerInZone = false; // Flag para verificar si el jugador está en la zona
    public Vector3 bubbleOffset = new(0, 1f, 0);

    public int keyID;
    private string[] fishingDialogue;

    private void Start()
    {
        ToggleDialogueBubble(false);
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
            isPlayerInZone = false; // El jugador saliá de la zona
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
            switch (keyID)
            {
                case 0:
                    if (HUDManager.Instance != null && HUDManager.Instance.GetKeyState(keyID)) // Verifica si la llave está activa
                    {
                        fishingDialogue = new string[] {
                        "¡Cofre obtenido!",
                    };
                        // Lógica de recompensa
                        Destroy(gameObject); // Elimina el cofre después de abrirlo
                        break;
                    }
                    else
                    {
                        fishingDialogue = new string[] {
                        "¡Te falta una llave!",
                    };
                    break;
                    }
                    
                case 1:
                    if (HUDManager.Instance != null && HUDManager.Instance.GetKeyState(keyID)) // Verifica si la llave está activa
                    {
                        fishingDialogue = new string[] {
                        "¡Cofre obtenido!",
                    };
                        
                        // Lógica de recompensa
                        Destroy(gameObject); // Elimina el cofre después de abrirlo
                        break;
                    }
                    else
                    {
                        fishingDialogue = new string[] {
                        "¡Te falta una llave!",
                    };
                    break;
                    }
                case 2:
                    if (HUDManager.Instance != null && HUDManager.Instance.GetKeyState(keyID)) // Verifica si la llave está activa
                    {
                        fishingDialogue = new string[] {
                        "¡Cofre obtenido!",
                    };
                        
                        // Lógica de recompensa
                        Destroy(gameObject); // Elimina el cofre después de abrirlo
                        break;
                    }
                    else
                    {
                        fishingDialogue = new string[] {
                        "¡Te falta una llave!",
                    };
                    break;
                    }
                default:
                    fishingDialogue = new string[] { "Hola, soy un NPC genérico." };
                    break;
            }

            dialogueSystem.StartDialogue(fishingDialogue, () => { });
        }
    }
}
