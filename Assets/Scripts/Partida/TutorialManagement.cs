using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManagement : MonoBehaviour
{
    public GameObject player;
    public DialogueSystem dialogueSystem;
    public GameObject fishZone;
    private string[] fishingDialogue;

    private enum TutorialStep
    {
        None,
        MovementInstructions,
        CollisionWarning,
        FishingZoneActive,
        TutorialComplete
    }

    private TutorialStep currentStep = TutorialStep.None;
    private bool isStepRunning = false;
    int currentLanguage;

    void Start()
    {
        currentStep = TutorialStep.MovementInstructions; // Inicia con las instrucciones de movimiento
        currentLanguage = PlayerPrefs.GetInt("Language", 0); // 0 = Español, 1 = Inglés
    }

    void Update()
    {
        RunTutorial();
    }

    private async void RunTutorial()
    {
        if (isStepRunning) return; // Evita que se ejecute más de una vez el mismo paso

        switch (currentStep)
        {
            case TutorialStep.MovementInstructions:
                isStepRunning = true;

                if (currentLanguage == 0) // Español
                {
                    fishingDialogue = new string[]
                    {
                        "Para acelerar, pulsa W, y para girar A o D. Puedes frenar con S. Pulsa Espacio para avanzar los diálogos.",
                        "Cuidado con las paredes, si te chocas a mucha velocidad, mueres."
                    };
                }
                else // Inglés
                {
                    fishingDialogue = new string[]
                    {
                        "To accelerate, press W, and to turn A or D. You can slow down with S. Press Space to advance dialogues.",
                        "Careful with the walls, if you crash at high speed, you die."
                    };
                }
                

                Debug.Log("Mostrando instrucciones de movimiento...");
                await dialogueSystem.StartDialogue(fishingDialogue);
                currentStep = TutorialStep.CollisionWarning;
                isStepRunning = false;
                break;

            case TutorialStep.CollisionWarning:
                isStepRunning = true;
                if (currentLanguage == 0) // Español
                {
                    fishingDialogue = new string[] { "Acércate a la zona de pesca y presiona E para pescar." };
                }
                else // Inglés
                {
                    fishingDialogue = new string[] { "Get close to the fishing zone and press E to fish." };
                }
                ShowFishingZone();
                Debug.Log("Mostrando advertencia de colisión...");
                await dialogueSystem.StartDialogue(fishingDialogue);
                currentStep = TutorialStep.FishingZoneActive;
                isStepRunning = false;
                break;

            case TutorialStep.FishingZoneActive:
                isStepRunning = true;

                Debug.Log("Esperando a que el jugador complete la pesca...");
                await WaitUntilFishingCompleted();
                if (currentLanguage == 0) // Español
                {
                    fishingDialogue = new string[] { "¡Tutorial completado!" };
                }
                else // Inglés
                {
                    fishingDialogue = new string[] { "Tutorial completed!" };
                }
                
                Debug.Log("Mostrando diálogo de finalización del tutorial...");
                await dialogueSystem.StartDialogue(fishingDialogue);

                currentStep = TutorialStep.TutorialComplete;
                isStepRunning = false;
                break;

            case TutorialStep.TutorialComplete:
                isStepRunning = true;

                Debug.Log("Finalizando tutorial...");
                await EndTutorial();

                isStepRunning = false;
                break;

            default:
                break;
        }
    }

    private void ShowFishingZone()
    {
        if (fishZone != null)
        {
            Debug.Log("Activando la zona de pesca...");
            fishZone.SetActive(true);
        }
        else
        {
            Debug.LogError("FishZone no está asignada o no existe.");
        }
    }

    private async Task WaitUntilFishingCompleted()
    {
        while (fishZone != null) // Si la zona de pesca desaparece, significa que la pesca se completó
        {
            await Task.Yield();
        }
    }

    private async Task EndTutorial()
    {
        Debug.Log("Esperando 5 segundos antes de cargar el siguiente nivel...");
        await Task.Delay(5000);
        SceneManager.LoadScene("TheWest");
    }
}
