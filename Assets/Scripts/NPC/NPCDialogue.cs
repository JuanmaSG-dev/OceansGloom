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
    private bool voidmessage2 = false;
    private Rigidbody2D playerRb;
    private bool isDialogueRunning = false;
    int currentLanguage;

    private bool voidmessage3 = false;

    private void Start()
    {
        currentLanguage = PlayerPrefs.GetInt("Language", 0); // 0 = Español, 1 = Inglés
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
        if (npcID == "19" && HUDManager.Instance.isKey2Used)
        {
            this.gameObject.SetActive(true);
        } else if (npcID == "19" && !HUDManager.Instance.isKey2Used)
        {
            this.gameObject.SetActive(false);
        }
        if (npcID == "20" && HUDManager.Instance.isKey1Used)
        {
            this.gameObject.SetActive(true);
        } else if (npcID == "20" && !HUDManager.Instance.isKey1Used)
        {
            this.gameObject.SetActive(false);
        }
    }

    async void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = true; // El jugador está en la zona
            ToggleDialogueBubble(true);
            if (isPlayerInZone && npcID == "5" && !warningSaid)
            {
                warningSaid = true;
                if (currentLanguage == 0) // Español
                {
                    fishingDialogue = new string[] {
                        "Alto ahí, identifícate.",
                        "Falsa alarma, solo eres un barco pesquero.",
                        "Perdona, intuyo que eres nuevo aquí, estamos alerta por culpa de un ladrón.",
                        "Confiaré en ti y te dejaré pasar, pero no hagas nada raro o me veré obligado a matarte.",
                        "No hagas que me arrepienta."
                    };
                }
                else // Inglés
                {
                    fishingDialogue = new string[] {
                        "Stop there, identify yourself.",
                        "False alarm, you're just a fishing boat.",
                        "Excuse me, I sense you're new here, we're alerted by a thief.",
                        "I will trust you and let you pass, but don't do anything strange or I'll have to kill you.",
                        "Don't make me regret it."
                    };
                }

                await dialogueSystem.StartDialogue(fishingDialogue, () => { });
            }
            if (isPlayerInZone && npcID == "8" && !leavingMessage)
            {
                leavingMessage = true;
                if (currentLanguage == 0) // Español
                {
                    fishingDialogue = new string[] {
                        "Estás aquí por algún motivo.",
                        "Hay algo que te impide escapar.",
                    };
                }
                else // Inglés
                {
                    fishingDialogue = new string[] {
                        "You're here for some reason.",
                        "There's something that prevents you from leaving.",
                    };
                }

                await dialogueSystem.StartDialogue(fishingDialogue, () => { leavingMessage = false; });
                playerRb.transform.position = new Vector2(playerRb.transform.position.x - 0.5f, playerRb.transform.position.y);
            }
            if (isPlayerInZone && npcID == "21" && !voidmessage3)
            {
                voidmessage3 = true;
                if (currentLanguage == 0) // Español
                {
                    fishingDialogue = new string[] {
                        "Buen trabajo superando al Muro del Vacío",
                        "Hmm, ¿por qué me miras así?",
                        "Veo que sospechas de mi, ¿verdad?",
                        "Tienes tus motivos, pero yo tengo los míos.",
                        "Necesitaba a alguien para lograr este objetivo.",
                        "Yo fui quien te incitó a venir al Oeste.",
                        "Y yo te impedía huir.",
                        "El Muro del Vacío lo invoqué yo también.",
                        "Necesitaba saber que eras el elegido.",
                        "Y ahora estoy en lo cierto, eres al que estaba esperando.",
                        "Hazme un último favor, y pesca mi cuerpo.",
                        "Yo fallecí hace mucho tiempo, en estos mares.",
                        "Mi muerte provocó que el Vacío naciese.",
                        "Solo tú puedes acabar con él.",
                    };
                }
                else // Inglés
                {
                    fishingDialogue = new string[] {
                        "Good job defeating the Void Wall",
                        "Hmm, why are you looking at me like that?",
                        "I see you doubting me, right?",
                        "You have your reasons, but I have mine.",
                        "I needed someone to achieve my goal.",
                        "I was the one who incited you to come to the West.",
                        "And I stopped you from leaving.",
                        "The Wall of Void was invoked by me too.",
                        "I had to know that you were the chosen one.",
                        "And now I'm right, you're the one I was waiting for.",
                        "Do me a last favor and fish my body.",
                        "I died a long time ago, in these seas.",
                        "My death caused the Void to rise.",
                        "Only you can end it.",
                    };
                }

                await dialogueSystem.StartDialogue(fishingDialogue, () => { });
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

    void Update()
    {
        if (isPlayerInZone && Input.GetKeyDown(KeyCode.E) && !isDialogueRunning)
        {
            RunDialogueLogic();
        }
    }
    private async void RunDialogueLogic()
    {
        isDialogueRunning = true;
        if (currentLanguage == 0) // Español
        {
            switch (npcID)
            {
                case "0": // Faro
                    fishingDialogue = new string[] {
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
                    if (chestInteraction.CheckIfOpen(0))
                    {
                        fishingDialogue = new string[] {
                            "Buen trabajo, eres bueno en esto.",
                            "Lo prometido es deuda, si necesitas algo en la próxima zona, ahí estaré.",
                            "Aunque no soy fanático de adentrarme más en este océano.",
                            "Sé detectar cuando algo anda mal, y percibo que si avanzo mucho, no gane nada, ¿entiendes?",
                            "Pues eso, buena suerte.",
                        };
                    }
                    else
                    {
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
                    if (HUDManager.Instance.hasBottle && bottlemessage)
                    {
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
                    }
                    else
                    {
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
                    if (!voidmessage)
                    {
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
                    }
                    else if (HUDManager.Instance.fishCount < 10)
                    {
                        fishingDialogue = new string[] {
                            "Estás aquí de nuevo, ¿traes lo que te pedí?",
                            "Veo que no, te falta algo...",
                            "No puedo dejar pasar a cualquier fanático, esto no es un juego...",
                            "Vuelve cuando tengas los 10 peces...",
                            "Solo así te dejaré seguir adelante...",
                        };
                    }
                    else
                    {
                        fishingDialogue = new string[] {
                            "Estás aquí de nuevo, ¿traes lo que te pedí?",
                            "Buen trabajo, parece que te tomas enserio esto...",
                            "Aún así, debes saber que esta zona no es nada comparada con lo que está por venir.",
                            "No me hago responsable de tu muerte...",
                            "Nos volveremos a ver más adelante...",
                        };
                    }
                    break;
                case "10": // Traumado
                    fishingDialogue = new string[] {
                            "Aléjate de mi!",
                            "Todo aquí quiere matarme, cómo sé que tu no harás lo mismo!",
                            "Vine buscando una fortuna y solo encontré un ejercito de esqueletos...",
                            "No dudaron en dispararme, mi barco acabó destruido.",
                            "Pude construir esta choza, pero creo que no dudaré mucho aquí...",
                        };
                        break;
                    case "11": // Torre
                    fishingDialogue = new string[] {
                            "¡Eh tú!",
                            "Esta zona es territorio de la Armada Esqueleto.",
                            "Si entras en esta zona no dudaremos en atacarte.",
                            "Si eres tan necio de seguir, te acordarás de mis palabras.",
                            "¡¡¡Nadie es capaz de cruzar estos mares!!!",
                        };
                        break;
                    case "12": // Cazador
                    fishingDialogue = new string[] {
                            "Cuidado, escóndete.",
                            "Aquí hay una bestia terrible.",
                            "Nunca ví nada tan aterrador, se posó debajo de mi barco y de un mordisco acabó con él.",
                            "Logré escapar de milagro, pero mira esta zona, no hay barco en condiciones.",
                            "Ten mucho cuidado, si te acercas demasiado, será tu fin...",
                        };
                        break;
                    case "13": // Void
                    fishingDialogue = new string[] {
                            "¡BARCO DETECTADO, BARCO DETECTADO!",
                            "¡IDENTIFICANDO, IDENTIFICANDO!",
                            "!!!!!!!",
                            "IMPOSIBLE, IMPOSIBLE!",
                            "IMPOSIBLE, IMPOSIBLE!",
                        };
                        break;
                    case "14": // Esqueleto Army
                    fishingDialogue = new string[] {
                            "¡Yarrrr, quien osa invadirnos!",
                            "Soy el lider de la Armada Esqueleto.",
                            "Dudo que tu barco sea capaz de darme pelea.",
                            "Al fin y al cabo, somos una armada invencible.",
                            "¡Aunque muramos, siempre volvemos al combate!",
                            "Veo que lograste cruzar mis cañoneros, ¡pero eso no es nada!",
                            "Más te vale alejarte, no quiero negociar con un humano como tu."
                        };
                        break;
                    case "15": // Ultimo bastion
                    fishingDialogue = new string[] {
                            "Un humano, ¡es increible!",
                            "¿Cómo lograste llegar hasta aquí?",
                            "Nosotros somos el último bastión de este océano.",
                            "Fuimos humanos como tu, pero el vacío hace efecto en las personas.",
                            "Quedarse en este océano por mucho tiempo es dejar la humanidad atrás.",
                            "Entiendo que tu misión es continuar, pero he de avisarte.",
                            "Ese ser en el barco morado te está llevando a la muerte.",
                            "Nada bueno habrá si continúas, es lo único que puedo decirte..."
                        };
                        break;
                    case "16": // Locked door
                        if (!HUDManager.Instance.hasKey4) {
                            fishingDialogue = new string[] {
                                "Puerta cerrada.",
                                "Necesitas una llave."
                            };
                        }
                        else {
                            fishingDialogue = new string[] {
                                "Abriendo puerta.",
                            };
                        }
                        break;
                    case "17": // Corazón void
                        fishingDialogue = new string[] {
                            "Obtuviste el corazón del vacío.",
                        };
                        break;
                    case "18": // Barco void 2
                    if (!voidmessage2)
                    {
                        voidmessage2 = true;
                        fishingDialogue = new string[] {
                            "Nos volvemos a encontrar...",
                            "¿Qué te ha parecido esta zona?",
                            "Terrible, lo sé, yo no puedo dejar de contemplarla...",
                            "¿Estás preparado para seguir adelante?",
                            "Estás bastante cerca del final, al menos de la zona que yo conozco.",
                            "Necesito que hagas una última cosa para que te deje pasar.",
                            "El Corazón del Vacío, lo quiero, y no estoy en condiciones de ir a por él.",
                            "Si mal no recuerdo, está cruzando una zona cerrada, la llave debe estar en este area.",
                            "Buena suerte, vuelve a hablar conmigo cuando lo tengas.",
                        };
                    }
                    else if (!HUDManager.Instance.hasVoidHeart)
                    {
                        fishingDialogue = new string[] {
                            "Estás aquí de nuevo, ¿traes lo que te pedí?",
                            "Veo que no, te falta algo...",
                            "Realmente lo necesito para seguir adelante...",
                            "Vuelve cuando tengas el Corazón del Vacío...",
                            "Solo así te dejaré seguir adelante...",
                        };
                    }
                    else
                    {
                        fishingDialogue = new string[] {
                            "Estás aquí de nuevo, ¿traes lo que te pedí?",
                            "Incréible, y es un gran especímen...",
                            "Los corazónes del vacío se forman con el tiempo y el alma de los humanos.",
                            "Hacía tiempo que no tenía uno en mi poder...",
                            "Sigamos avanzando, detrás de ti...",
                        };
                    }
                    break;
                    case "19": // Hermano perdido
                        fishingDialogue = new string[] {
                            "¿Holaaa? ¿Hay alguien ahí?",
                            "Ugh, alguien me encontró por fin...",
                            "Pensé que moriría aquí, tiré una botella al agua...",
                            "Llegaste aquí gracias a que la encontraste, ¿no?",
                            "Mi hermana debe estar preocupada, normal.",
                            "Mi barco está destruido por completo, pero lo arreglaré.",
                            "Gracias por las provisiones."
                        };
                        break;
                    case "20": // Ladrón
                        fishingDialogue = new string[] {
                            "¿Ves lo que te dije?",
                            "Tu me ayudaste, y ahora me toca a mi.",
                            "Aunque en verdad, no sé como ayudarte...",
                            "Nunca estuve en este sitio antes, pero he conseguido mucha información.",
                            "Estuve observando a alguien que estaba en el barco morado.",
                            "Murmuraba mucho, hablaba de un corazón y de su salvación.",
                            "No debes fiarte de él, pero bueno, soy un ladrón, capaz no debas hacerme caso.",
                            "Me quedaré por aquí, esos Esqueletos tienen una gran fortuna."
                        };
                        break;
                    case "21":
                        fishingDialogue = new string[] {
                            "Buen trabajo superando al Muro del Vacío",
                            "Hmm, ¿por qué me miras así?",
                            "Veo que sospechas de mi, ¿verdad?",
                            "Tienes tus motivos, pero yo tengo los míos.",
                            "Necesitaba a alguien para lograr este objetivo.",
                            "Yo fui quien te incitó a venir al Oeste.",
                            "Y yo te impedía huir.",
                            "El Muro del Vacío lo invoqué yo también.",
                            "Necesitaba saber que eras el elegido.",
                            "Y ahora estoy en lo cierto, eres al que estaba esperando.",
                            "Hazme un último favor, y pesca mi cuerpo.",
                            "Yo fallecí hace mucho tiempo, en estos mares.",
                            "Mi muerte provocó que el Vacío naciese.",
                            "Solo tú puedes acabar con él.",
                        };
                        break;
                default:
                    fishingDialogue = new string[] { "Hola, soy un NPC genérico." };
                    break;
            }
        }
        else // Inglés --------------------------------------------------------------------------------------
        {
            switch (npcID)
            {
                case "0": // Faro
                    fishingDialogue = new string[] {
                        "Get out of here while you have the option.",
                        "The West does not forgive anyone, if you continue, you will never be the same.",
                        "But who do I want to fool, nobody will believe an old man like me.",
                        "At least don't say I didn't warn you.",
                        "If only I could have stopped that person, I would have a clean conscience.",
                        "Get lost, your presence reminds me of him."
                    };
                    break;
                case "1": // Isla central
                    fishingDialogue = new string[] {
                        "Good morning, traveler.",
                        "It's been a while since I saw a new face.",
                        "Life here is not so bad.",
                        "Although I warn you that if you continue that will change.",
                        "I never saw anyone come back, that scares me a bit.",
                        "Also, there are some cracks that appear every so often.",
                        "Call me crazy, but the last time I got close to one, I heard a voice..."
                    };
                    break;
                case "2": // Pescador
                    fishingDialogue = new string[] {
                        "Oh, a new face.",
                        "And you're not a pirate, right?",
                        "Lately many thieves come here looking for a fortune..",
                        "As a fisherman, this place is a wonder.",
                        "I never understood why they prohibited it, it's as dangerous as any ocean.",
                        "And you what are you doing here? Are you a fisherman too?",
                        "I will give you a tip, there's a chance of catching a fish with a different color.",
                        "Having one is a sign of your perseverance!"
                    };
                    break;
                case "3": // Cazador de tiburones
                    fishingDialogue = new string[] {
                        "Hey you, the fisherman's boat.",
                        "Have you found any shark yet?",
                        "All these sunken ships, what do you think?",
                        "I'm sure those sharks are the ones responsible.",
                        "I'm always watching, when you see the water turn grey, that means there's one shark nearby.",
                        "It's weird, because they don't seem to be pursuing anyone, but they will destroy your boat if you get too close.",
                        "I'm trying to recover a key in that area.",
                        "But my ship is damaged, damn it!"
                    };
                    break;
                case "4": // Traidor
                    if (chestInteraction.CheckIfOpen(0))
                    {
                        fishingDialogue = new string[] {
                            "Good job, you're good at this.",
                            "A promise is a promise, if you need anything in the next area, I'll be there..",
                            "But I'm not a fan of getting too far into this ocean.",
                            "I can detect when something is wrong, and I notice that if I go too far, I won't get anything, you get it?",
                            "So yeah, good luck.",
                        };
                    }
                    else
                    {
                        fishingDialogue = new string[] {
                            "Hide, don't let them see you...",
                            "You're not from the city, right?",
                            "I hate the city, it's occupied by a pirate band I don't like.",
                            "Although they look nice, they have control of the area.",
                            "Also, who is so crazy to build a city in this place?",
                            "I'll tell you a secret, I stole a key from those thieves.",
                            "Open a chest in this sea, if you give me what's inside, I can help you to continue.",
                            "No one is able to cross the next area for their own good, and I have experience, believe me."
                        };
                    }
                    break;

                case "5": // Ciudad 1
                    fishingDialogue = new string[] {
                        "You can pass.",
                    };
                    break;
                case "6": // Ciudad 2
                    if (HUDManager.Instance.hasBottle && bottlemessage)
                    {
                        fishingDialogue = new string[] {
                            "Fascinating, you're a good fisherman.",
                            "Let's see what the message says...",
                            "'I'm trapped in a zone full of bones, I fell into the sea and a current brought me he-'",
                            "The message is abruptly cut, a zone of bones...",
                            "It must be crossing this zone, it's too dangerous for us.",
                            "I know I'm asking a lot for being an unknown, but my brother is in trouble.",
                            "If you find it, try to save it.",
                            "A promise is a promise, here you have the key to the city's chest."
                        };
                    }
                    else
                    {
                        fishingDialogue = new string[] {
                            "Ey you, the fisherman.",
                            "I know this ocean is not the ideal place to make friends.",
                            "But I need help, and there's not many people here.",
                            "My brother went to the sea a while ago and he disappeared...",
                            "I fear the worst, because this place doesn't forgive anyone.",
                            "You could try to recover something, my brother used to throw bottles at the sea in case he was in danger.",
                            "It's not the first time, but this time I'm not able to find it, my ship got damaged.",
                            "Try to catch the bottle, if you get it, I'll give you the key to the city's chest."
                        };
                        bottlemessage = true;
                    }

                    break;
                case "7": // Grieta
                    fishingDialogue = new string[] {
                        "Get close, mortal...",
                        "You have to advance...",
                        "Until the end of the West...",
                        "I'll be waiting for you...",
                    };
                    break;
                case "9": // Barco void
                    if (!voidmessage)
                    {
                        voidmessage = true;
                        fishingDialogue = new string[] {
                            "Another poor soul lost in my waters...",
                            "You want to advance, I can feel it...",
                            "Before you leave, I need you to prove that you can survive...",
                            "I need you to bring me 10 fish...",
                            "Only then I'll let you continue...",
                            "Don't forget to complete this zone, once you pass, you can't go back...",
                            "Come back when you're ready...",
                        };
                    }
                    else if (HUDManager.Instance.fishCount < 10)
                    {
                        fishingDialogue = new string[] {
                            "You're back, do you bring what you asked?",
                            "I see you don't, you're missing something...",
                            "I can't let you pass any fanatic, this is not a game...",
                            "Come back when you have the 10 fish...",
                            "Only then I'll let you continue...",
                        };
                    }
                    else
                    {
                        fishingDialogue = new string[] {
                            "You're back, do you bring what you asked?",
                            "Good job, it seems you take this seriously...",
                            "Still, you need to know that this zone is nothing compared to what's coming.",
                            "I don't take responsibility for your death...",
                            "We will meet again later...",
                        };
                    }
                    break;
                    case "10": // Traumado
                    fishingDialogue = new string[] {
                            "Go away from me!",
                            "Everything here wants to kill me, how do I know that you won't do the same?",
                            "I came looking for a fortune and only found a skeleton army...",
                            "They didn't hesitate in shooting me, my ship got destroyed.",
                            "I managed to build this house, but I don't think I'll survive here much...",
                        };
                        break;
                    case "11": // Torre
                    fishingDialogue = new string[] {
                            "Hey, you!",
                            "This area is the territory of the Skeleton Army.",
                            "If you enter this area, we will attack you.",
                            "If you're so stupid to follow, you'll be punished.",
                            "Nobody is able to cross these seas!!!",
                        };
                        break;
                    case "12": // Cazador
                    fishingDialogue = new string[] {
                            "Careful, watch out.",
                            "There's a terrible beast around here.",
                            "I have never seen anything like this, it went underneath my ship and broke it with one bite.",
                            "I managed to escape, but look at this area, there's no ship in condition.",
                            "Be really careful, if you get too close, you'll be dead...",
                        };
                        break;
                    case "13": // Void
                    fishingDialogue = new string[] {
                            "SHIP DETECTED, SHIP DETECTED!",
                            "IDENTIFYING, IDENTIFYING!",
                            "!!!!!!!",
                            "IMPOSSIBLE, IMPOSSIBLE!",
                            "IMPOSSIBLE, IMPOSSIBLE!",
                        };
                        break;
                    case "14": // Esqueleto Army
                    fishingDialogue = new string[] {
                            "Yarrrr, who dare invade us!",
                            "I'm the leader of the Skeleton Army.",
                            "I doubt your ship is strong enough to defeat us.",
                            "We are an invincible army.",
                            "Even after death, we will still fight!",
                            "I see you managed to cross my cannons, but that's nothing!",
                            "You better leave, I don't wanna negotiate with a human like you."
                        };
                        break;
                    case "15": // Ultimo bastion
                    fishingDialogue = new string[] {
                            "A human, it's incredible!",
                            "How did you get here?",
                            "We are the last bastion of this ocean.",
                            "We were humans like you, but the void changes people.",
                            "Staying in this ocean for so long is leaving humanity behind.",
                            "I understand your mission is to continue, but I have to warn you.",
                            "This being in the purple ship is taking you to death.",
                            "Nothing good will come if you keep going, it's the only thing I can tell you..."
                        };
                        break;
                    case "16": // Locked door
                        if (!HUDManager.Instance.hasKey4) {
                            fishingDialogue = new string[] {
                                "Locked door.",
                                "You need a key."
                            };
                        }
                        else {
                            fishingDialogue = new string[] {
                                "Opening door.",
                            };
                        }
                        break;
                    case "17": // Heart void
                        fishingDialogue = new string[] {
                                "Obtuviste el corazón del vacío.",
                            };
                            break;
                    case "18": // Barco void 2
                    if (!voidmessage2)
                    {
                        voidmessage2 = true;
                        fishingDialogue = new string[] {
                            "We meet again...",
                            "What's your thoughts about this area?",
                            "Terrible, I know, I can't stop staring it...",
                            "Are you ready to continue?",
                            "You're close to the end, at least to the area I know.",
                            "I know you to do a last quest for me to pass.",
                            "The Heart of the Void, I want it, and I'm not in condition to get it.",
                            "If I'm not mistaken, it's crossing a closed area, the key should be in this area.",
                            "Good luck, come back when you have it.",
                        };
                    }
                    else if (!HUDManager.Instance.hasVoidHeart)
                    {
                        fishingDialogue = new string[] {
                            "You're back, do you bring what I asked?",
                            "I see you don't, you're missing something...",
                            "I really need it to continue...",
                            "Come back when you have the Heart of the Void...",
                            "Only then I'll let you continue...",
                        };
                    }
                    else
                    {
                        fishingDialogue = new string[] {
                            "You're back, do you bring what I asked?",
                            "Astonishing, and it's a great one...",
                            "The void hearts are formed over time from the soul of the human.",
                            "It's been a long time since I had one in my power...",
                            "Let's keep going, after you...",
                        };
                    }
                    break;
                    case "19": // Hermano perdido
                        fishingDialogue = new string[] {
                            "Hello? Is anyone here?",
                            "Ugh, finally someone found me...",
                            "I thought I would die here, I threw a bottle at the water...",
                            "You're here thanks to the bottle right?",
                            "My sister must be worried, that's normal.",
                            "My ship is destroyed completely, but I'll fix it.",
                            "Thanks for the supplies."
                        };
                        break;
                    case "20": // Ladrón
                        fishingDialogue = new string[] {
                            "See what I said?",
                            "You helped me, and now I'm helping you.",
                            "But actually, I don't know how to help you...",
                            "I have never been in this place before, but I managed to gather a lot of information.",
                            "I have been watching someone who was in the purple ship.",
                            "He was mumbling a lot, talking about a heart and its salvation.",
                            "You shouldn't trust him, but well, I'm a thief, you shouldn't trust me either.",
                            "I will stay here, those skeletons have a great fortune."
                        };
                        break;
                    case "21":
                        fishingDialogue = new string[] {
                            "Good job defeating the Void Wall",
                            "Hmm, why are you looking at me like that?",
                            "I see you doubting me, right?",
                            "You have your reasons, but I have mine.",
                            "I needed someone to achieve my goal.",
                            "I was the one who incited you to come to the West.",
                            "And I stopped you from leaving.",
                            "The Wall of Void was invoked by me too.",
                            "I had to know that you were the chosen one.",
                            "And now I'm right, you're the one I was waiting for.",
                            "Do me a last favor and fish my body.",
                            "I died a long time ago, in these seas.",
                            "My death caused the Void to rise.",
                            "Only you can end it.",
                        };
                        break;
                default:
                    fishingDialogue = new string[] { "Hi, I'm a generic NPC." };
                    break;
            }

        }


        await dialogueSystem.StartDialogue(fishingDialogue, () => { isDialogueRunning = false; });
        if (npcID == "4" && !HUDManager.Instance.IsKeyUsed(0) && !isDialogueRunning)
        {
            HUDManager.Instance.CollectKey(0);
        }
        if (npcID == "6" && !HUDManager.Instance.HasKey(1) && HUDManager.Instance.hasBottle && !isDialogueRunning)
        {
            HUDManager.Instance.CollectKey(1);
        }
        if (npcID == "9" && HUDManager.Instance.fishCount >= 10 && voidmessage && !isDialogueRunning)
        {
            StartCoroutine(TransitionToNextLevel());
        }
        if (npcID == "16" && HUDManager.Instance.hasKey4 && !isDialogueRunning) {
            Destroy(gameObject);
        }
        if (npcID == "17" && !isDialogueRunning) {
            HUDManager.Instance.CollectVoidHeart();
            Destroy(gameObject);
        }
        if (npcID == "18" && HUDManager.Instance.hasVoidHeart && voidmessage2 && !isDialogueRunning)
        {
            StartCoroutine(TransitionToNextLevel2());
        }
        if (npcID == "19" && !isDialogueRunning) {
            HUDManager.Instance.SetBrotherQuestComplete();
        }
        if (npcID == "20" && !isDialogueRunning) {
            HUDManager.Instance.SetTheftQuestComplete();
        }
    }

    private IEnumerator TransitionToNextLevel()
    {
        yield return new WaitForSeconds(2f); // Tiempo para la animación
        SceneManager.LoadScene("BoneyWaters"); // Cargar la siguiente escena
        Debug.Log("Transition to BoneyWaters");
    }

    private IEnumerator TransitionToNextLevel2()
    {
        yield return new WaitForSeconds(2f); // Tiempo para la animación
        SceneManager.LoadScene("Void"); // Cargar la siguiente escena
        Debug.Log("Transition to Void");
    }
}
