using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class EndingLoader : MonoBehaviour
{

    public DialogueSystem dialogueSystem;
    public GameObject fishZone;
    public GameObject panel;
    public GameObject Creditos;
    public bool isFinished = false;
    private string[] fishingDialogue;
    private bool isDialogueRunning = false;
    int currentLanguage;
    // Start is called before the first frame update
    void Start()
    {
        panel.gameObject.SetActive(false);
        currentLanguage = PlayerPrefs.GetInt("Language", 0); // 0 = Español, 1 = Inglés
    }

    // Update is called once per frame
    void Update()
    {
        if (isFinished) return;
        Finish();

    }

    public async void Finish() {
        await WaitUntilFishingCompleted();
        isFinished = true;
        panel.gameObject.SetActive(true);
        isDialogueRunning = true;
        if (currentLanguage == 0) // Español
            {
                fishingDialogue = new string[]
                {
                    "Lograste salvar mi cuerpo, pero no todo termina aquí.",
                    "El Vacío salió de mi alma, pero ahora busca otra alma para ocupar.",
                    "Por desgracia para tí, eres el único aquí.",
                    "Gracias por salvarme, pero ahora tú serás juzgado por el Vacío.",
                };
            }
            else // Inglés
            {
                fishingDialogue = new string[]
                {
                    "You managed to save my body, but not everything is over.",
                    "The Void left my soul, but now it's looking for another soul to occupy.",
                    "Sadly for you, you're the only one here.",
                    "Thanks for saving me, but now you'll be judged by the Void.",
                };
            }
            // Toca juzgar los 2 finales, FINAL BUENO (OBTUVO TODO) Y FINAL MALO (FALLÓ ALGUNA MISIÓN)
            if (HUDManager.Instance.brotherQuestComplete && HUDManager.Instance.TheftQuestComplete && HUDManager.Instance.isKey1Used && HUDManager.Instance.isKey2Used && HUDManager.Instance.isKey3Used) {
                if (currentLanguage == 0) // Español
                {
                    fishingDialogue = new string[]
                    {
                        "Ya veo...",
                        "Conseguiste todas las misiones que puse en tu camino.",
                        "Conseguiste los 3 cofres y ayudaste al hermano perdido.",
                        "Te fiaste del ladrón y te dio la llave a la ciudad.",
                        "¡Felicidades!",
                        "Salvaré tu alma, ahora, con amnesia, despertarás en tu casa del faro.",
                        "Tu misión será vigilar a la gente que entre al Oeste.",
                        "FINAL BUENO."
                    };
                }
                else // Inglés
                {
                    fishingDialogue = new string[]
                    {
                        "I see...",
                        "You managed to complete all the missions I put on your path.",
                        "You got the 3 chests and helped the lost brother.",
                        "You trusted the thief and he gave you the key to the city.",
                        "Congratulations!",
                        "I will save your soul, now, with amnesia, you'll wake up in your lighthouse.",
                        "Your mission will be to watch the people who enter the West.",
                        "GOOD ENDING."
                    };
                }
            } else {
                if (currentLanguage == 0) // Español
                {
                    fishingDialogue = new string[]
                    {
                        "Ya veo...",
                        "No lograste completar todas las misiones que puse en tu camino.",
                        "Además de los cofres, había que ayudar al hermano perdido y al ladrón.",
                        "Apúntatelo para la próxima vez.",
                        "Lo lamento, ahora, tu serás el próximo Vigilante del Vacío.",
                        "Tu cuerpo cayó al mar, pero tu alma sigue viva.",
                        "Debes llevar a alguien digno para salvar tu cuerpo y seguir el ciclo.",
                        "FINAL MALO."
                    };
                }
                else // Inglés
                {
                    fishingDialogue = new string[]
                    {
                        "I see...",
                        "You didn't complete all the missions I put on your path.",
                        "Not only the chests, but you had to help the lost brother and the thief.",
                        "Don't forget the next time.",
                        "I'm sorry, now you'll be the next Void Watcher.",
                        "Your body fell into the sea, but your soul is still alive.",
                        "You must find someone worthy to save your body and continue the cycle.",
                        "BAD ENDING."
                    };
                }
            }
            await dialogueSystem.StartDialogue(fishingDialogue, () => { isDialogueRunning = false; });
            Debug.Log("Final...");
            if (!isDialogueRunning) {
                Creditos.gameObject.SetActive(true);
            }
    }

    private async Task WaitUntilFishingCompleted()
    {
        while (fishZone != null) // Si la zona de pesca desaparece, significa que la pesca se completó
        {
            await Task.Yield();
        }
    }
}
