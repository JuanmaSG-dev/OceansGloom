using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManagement : MonoBehaviour
{
    public GameObject player;
    public DialogueSystem dialogueSystem;
    public GameObject fishZone;
    private string[] fishingDialogue;

    public enum TutorialState
    {
        MovementInstructions,
        MovementActive,
        CollisionWarning,
        FishingZoneActive,
        TutorialComplete
    }

    public TutorialState currentState = TutorialState.MovementInstructions;

    private bool isCoroutineRunning = false; // Evita múltiples llamadas a la coroutina

    // Start is called before the first frame update
    void Start()
    {
        currentState = TutorialState.MovementInstructions;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case TutorialState.MovementInstructions:
                fishingDialogue = new string[] { "Para acelerar, pulsa W, y para girar A o D. Puedes frenar con S. Pulsa Espacio para avanzar los diálogos." };
                dialogueSystem.StartDialogue(fishingDialogue, () => { });
                currentState = TutorialState.MovementActive;
                break;

            case TutorialState.MovementActive:
                if (HasStartedMoving())
                {
                    StartCoroutine(WaitBeforeCollisionWarning());
                }
                break;

            case TutorialState.CollisionWarning:
                ShowFishingZone();
                fishingDialogue = new string[] { "Acércate a la zona de pesca y presiona E para pescar." };
                dialogueSystem.StartDialogue(fishingDialogue, () => { });
                currentState = TutorialState.FishingZoneActive;
                break;

            case TutorialState.FishingZoneActive:
                if (isFishingCompleted())
                {
                    fishingDialogue = new string[] { "¡Tutorial completado!" };
                    dialogueSystem.StartDialogue(fishingDialogue, () => { });
                    currentState = TutorialState.TutorialComplete;
                    StartCoroutine(EndTutorial());
                }
                break;
        }
    }

    private bool HasStartedMoving()
    {
        return Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;
    }

    private void ShowFishingZone()
    {
        fishZone.SetActive(true);
    }

    private bool isFishingCompleted()
    {
        return fishZone == null;
    }

    private IEnumerator EndTutorial()
    {   
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("TheWest");
    }


    // Coroutina para esperar 20 segundos
    private IEnumerator WaitBeforeCollisionWarning()
    {
        if (!isCoroutineRunning)
        {
            isCoroutineRunning = true;
            yield return new WaitForSeconds(10f);
            fishingDialogue = new string[] { "Cuidado con las paredes, si te chocas a mucha velocidad, mueres." };
            dialogueSystem.StartDialogue(fishingDialogue, () => { });
            yield return new WaitForSeconds(10f);
            currentState = TutorialState.CollisionWarning;
            isCoroutineRunning = false;
        }
    }
}
