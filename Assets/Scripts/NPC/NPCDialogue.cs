using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class NPCDialogue : MonoBehaviour
{
    public DialogueSystem dialogueSystem;
    public DecisionSystem decisionSystem;
    public GameObject dialogueBubble;
    private bool isPlayerInZone = false; // Flag para verificar si el jugador está en la zona
    public Vector3 bubbleOffset = new(0, 1f, 0);

    public ChestInteraction chestInteraction;
    public string npcID;
    private string[] fishingDialogue;
    private bool warningSaid = false;
    private bool bottlemessage = false;
    private bool leavingMessage = false;
    private bool voidmessage = false;
    private Rigidbody2D playerRb;
    private bool isDialogueRunning = false;

    private void Start()
    {
        ToggleDialogueBubble(false);
        if (string.IsNullOrEmpty(npcID))
        {
            npcID = gameObject.name; // Usa el nombre del objeto como ID si no se asigna uno
        }
        GameObject player = GameObject.Find("Barco");
        if (player != null)
        {
            playerRb = player.GetComponent<Rigidbody2D>();
            if (playerRb == null)
            {
                Debug.LogError("El GameObject 'Barco' no tiene un Rigidbody2D.");
            }
        }
        else
        {
            Debug.LogError("No se encontró el GameObject 'Barco'.");
        }
    }

    async void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = true; // El jugador está en la zona
            ToggleDialogueBubble(true);
            if (isPlayerInZone && npcID == "5" && !warningSaid) {
                warningSaid = true;
                fishingDialogue = new string[] {
                    "Alto ahí, identifícate.",
                    "Falsa alarma, solo eres un barco pesquero.",
                    "Perdona, intuyo que eres nuevo aquí, estamos alerta por culpa de un ladrón.",
                    "Confiaré en ti y te dejaré pasar, pero no hagas nada raro o me veré obligado a matarte.",
                    "No hagas que me arrepienta."
                }; 
                await dialogueSystem.StartDialogue(fishingDialogue, () => { });
            }
            if (isPlayerInZone && npcID == "8" && !leavingMessage) {
                leavingMessage = true;
                fishingDialogue = new string[] {
                    "Estás aquí por algún motivo.",
                    "Hay algo que te impide escapar.",
                };
                await dialogueSystem.StartDialogue(fishingDialogue, () => { leavingMessage = false;});
                playerRb.transform.position = new Vector2(playerRb.transform.position.x - 0.5f, playerRb.transform.position.y);
            }
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

    void Update() {
        if (isPlayerInZone && Input.GetKeyDown(KeyCode.E) && !isDialogueRunning)
        {
            RunDialogueLogic();
        }
    }
    private async void RunDialogueLogic()
    {
        isDialogueRunning = true;
        switch (npcID)
        {
            case "0": // Faro
                fishingDialogue = new string[] {
                    "Recuerda lo que te dije.",
                    "Sal de aquí mientras tengas la opción.",
                    "El Oeste no perdona a nadie, si continúas, no volverás a ser el mismo.",
                    "Pero a quién quiero engañar, nadie le hará caso a un anciano como yo.",
                    "Al menos no digas que no te lo avisé.",
                    "Si tan solo pudiese haber detenido a esa persona, tendría la consciencia limpia.",
                    "Márchate, tu presencia me recuerda a él."
                };
                break;
            case "1": // Isla central
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
            case "2": // Pescador
                fishingDialogue = new string[] {
                    "Oh, una cara nueva.",
                    "Y no eres un pirata, ¿me equivoco?",
                    "Últimamente aquí vienen muchos malhechores buscando una fortuna.",
                    "Como pescador que soy, este sitio es una maravilla.",
                    "Nunca entendí porqué lo prohibieron, es igual de peligroso que cualquier océano.",
                    "¿Y tú qué haces aquí? ¿También eres un pescador?",
                    "Te daré un consejo, hay una probabilidad de conseguir un pescado con un color distinto.",
                    "¡Tener uno es una muestra de tu perseverancia!"
                };
                break;
            case "3": // Cazador de tiburones
                fishingDialogue = new string[] {
                    "Eh tú, el del barco pesquero.",
                    "¿Te has encontrado ya algún tiburón?",
                    "Todos estos barcos hundidos, ¿qué te parece?",
                    "Estoy seguro que esos tiburones son los responsables.",
                    "Siempre los estoy observando, cuando veas el agua tornarse grisácea, eso significa que hay uno merodeando.",
                    "Es raro, porque no parecen perseguir a nadie, pero no dudarán en destruir tu barco si te metes en su camino.",
                    "Estoy tratando de recuperar una llave en esa zona.",
                    "¡Pero mi barco está dañado, que rabia!"
                };
                break;
            case "4": // Traidor
                if (chestInteraction.CheckIfOpen(0)) {
                        fishingDialogue = new string[] {
                        "Buen trabajo, eres bueno en esto.",
                        "Lo prometido es deuda, si necesitas algo en la próxima zona, ahí estaré.",
                        "Aunque no soy fanático de adentrarme más en este océano.",
                        "Sé detectar cuando algo anda mal, y percibo que si avanzo mucho, no gane nada, ¿entiendes?",
                        "Pues eso, buena suerte.",
                    };
                } else {
                    fishingDialogue = new string[] {
                        "Escóndete, no dejes que te vean...",
                        "No eres de la ciudad, ¿no?",
                        "Odio la ciudad, está ocupada por una banda de piratas que no me gusta.",
                        "Aunque parezcan hospitalarios, tienen el control de la zona.",
                        "Además, ¿quién está tan loco de construir una ciudad en este sitio?",
                        "Te diré un secreto, les robé una llave a esos malhechores.",
                        "Abre un cofre allá a lo lejos, si me entregas lo que se halla en su interior, podré ayudarte a seguir adelante.",
                        "Nadie es capáz de cruzar la siguiente zona por su cuenta, y yo tengo experiencia, créeme."
                    };
                }
                break;

            case "5": // Ciudad 1
                fishingDialogue = new string[] {
                    "Puedes pasar.",
                };
                break;
            case "6": // Ciudad 2
                if (HUDManager.Instance.hasBottle && bottlemessage) {
                    fishingDialogue = new string[] {
                        "Fascinante, si que eres un buen pescador.",
                        "Veamos que dice su mensaje...",
                        "'Estoy atrapado en una zona llena de huesos, caí al mar y una corriente me llevó lejo-'",
                        "El mensaje se corta de forma abrupta, una zona de huesos...",
                        "Debe estar pasando esta zona, ir allí es demasiado peligroso para nosotros.",
                        "Sé que estoy pidiendo mucho para ser una desconocida, pero mi hermano está en problemas.",
                        "Si lo encuentras, intenta salvarlo.",
                        "Lo prometido es deuda, aquí tienes la llave para el cofre de la ciudad."
                    };
                } else {
                    fishingDialogue = new string[] {
                        "Eh tu, pesquero.",
                        "Sé que este océano no es el lugar ideal para hacer amigos.",
                        "Pero necesito ayuda, y no suele haber mucha gente aquí.",
                        "Mi hermano se fue al mar hace poco y desapareció...",
                        "Me temo lo peor, ya que este sitio no perdona a nadie.",
                        "Podrías intentar recuperar algo, mi hermano solía lanzar botellas al mar en caso de estar en peligro.",
                        "No es la primera vez que pasa, pero esta vez no estoy yo para buscarlo, mi barco se rompió.",
                        "Intenta pescar la botella, si me la entregas, te daré una llave para el cofre de la ciudad."
                    };
                    bottlemessage = true;
                }
                
                break;
            case "7": // Grieta
                fishingDialogue = new string[] {
                    "Acércate, mortal...",
                    "Debes avanzar...",
                    "Hasta el final del Oeste...",
                    "Te estaré esperando...",
                };
                break;
            case "9": // Barco void
                if (!voidmessage) {
                    voidmessage = true;
                    fishingDialogue = new string[] {
                        "Otra pobre alma perdida en mis aguas...",
                        "Deseas avanzar, puedo sentirlo...",
                        "Antes de dejarte pasar, necesito que demuestres que eres capaz de sobrevivir...",
                        "Requiero que me traigas 10 peces...",
                        "Solo así te dejaré seguir adelante...",
                        "No te olvides de completar esta zona, una vez que pases no podrás volver...",
                        "Vuelve a hablar conmigo cuando estés listo...",
                    };
                } else if (HUDManager.Instance.fishCount < 10) {
                    fishingDialogue = new string[] {
                        "Estás aquí de nuevo, ¿traes lo que te pedí?",
                        "Veo que no, te falta algo...",
                        "No puedo dejar pasar a cualquier fanático, esto no es un juego...",
                        "Vuelve cuando tengas los 10 peces...",
                        "Solo así te dejaré seguir adelante...",
                    };
                } else {
                    fishingDialogue = new string[] {
                        "Estás aquí de nuevo, ¿traes lo que te pedí?",
                        "Buen trabajo, parece que te tomas enserio esto...",
                        "Aún así, debes saber que esta zona no es nada comparada con lo que está por venir.",
                        "No me hago responsable de tu muerte...",
                        "Nos volveremos a ver más adelante...",
                    };
                }
                break;
            default:
                fishingDialogue = new string[] { "Hola, soy un NPC genérico." };
                break;
        }
        await dialogueSystem.StartDialogue(fishingDialogue, () => {isDialogueRunning = false;});
        if (npcID == "4" && !HUDManager.Instance.IsKeyUsed(0) && !isDialogueRunning) {
            HUDManager.Instance.CollectKey(0);
        }
        if (npcID == "6" && !HUDManager.Instance.HasKey(1) && HUDManager.Instance.hasBottle && !isDialogueRunning) {
            HUDManager.Instance.CollectKey(1);
        }
        if (npcID == "9" && HUDManager.Instance.fishCount >= 10 && voidmessage && !isDialogueRunning) {
            StartCoroutine(TransitionToNextLevel());
        }
        
    }

    private IEnumerator TransitionToNextLevel()
    {
        yield return new WaitForSeconds(2f); // Tiempo para la animación
        //SceneManager.LoadScene("BoneyWaters"); // Cargar la siguiente escena
        Debug.Log("Transition to BoneyWaters");
    }
}
