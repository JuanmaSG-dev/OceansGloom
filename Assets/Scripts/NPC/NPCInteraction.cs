using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    private bool isPlayerInRange = false;
    public GameObject dialogueBubble;
    public GameObject canvas;

    private void Start()
    {
        // El bocadillo de texto no aparece visible por defecto
        dialogueBubble.SetActive(false);
        canvas.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            dialogueBubble.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            dialogueBubble.SetActive(false);
        }
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    private void Interact()
    {
        canvas.SetActive(true);
        if (Input.GetKeyDown(KeyCode.Q))
        {
            canvas.SetActive(false);
        }
    }

}

