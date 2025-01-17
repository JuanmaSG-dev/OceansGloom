using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class FishingMinigame : MonoBehaviour
{
    public Canvas fishingCanvas;             // Canvas para activar o desactivar el minijuego de pesca
    public RectTransform hook;               // El anzuelo controlado por el jugador
    public Image hookImage;
    public Sprite hookDefaultImage;
    public Sprite hookUpgradeImage;
    public RectTransform greenZone;          // Zona verde movil
    public RectTransform redBar;             // Barra roja de fondo
    public TMP_Text puntosText;              // Texto para mostrar la puntuacion
    public TMP_Text livesText;               // Texto para mostrar las vidas restantes

    //public float hookSpeed = 200f;           // Velocidad del anzuelo
    public int targetPoints = 10;            // Puntos necesarios para ganar
    public int lives;                    // Vidas maximas

    private int currentPoints = 0;           // Puntos actuales
    public bool isFishingActive = false;    // Estado del minijuego
    private Vector2 greenZoneDirection;      // Direccion de movimiento del area verde
    [SerializeField] float greenZoneBaseSpeed = 200f;      // Velocidad de la zona verde
    [SerializeField] float greenZoneSpeed;

    public ShipController shipController;   // Control del barco (se desactivara)

    //private float hookUpperLimit;
    //private float hookLowerLimit;

    public FishPool fishPool;
    private Fish selectedFish;

    public Image caughtFishImage;
    private bool isShinyCaught;
    ZonaPesca currentFishingZone;

    int currentLanguage;
    public bool Upgraded = false;
    public AudioClip successSound;
    public AudioClip failSound;


    private void Start()
    { 
        fishingCanvas.gameObject.SetActive(true);
        caughtFishImage.gameObject.SetActive(false);
        //hookUpperLimit = redBar.rect.height;
        //hookLowerLimit = -redBar.rect.height;
        hook.anchoredPosition = new Vector2(hook.anchoredPosition.x, 278);
        shipController.SetControlEnabled(false);
        currentLanguage = PlayerPrefs.GetInt("Language", 0); // 0 = Español, 1 = Inglés
        UpdateUI();
    }


    public void PlayFishingSound(bool success)
    {
        AudioSource audioSource = Camera.main.GetComponent<AudioSource>();
        if (success)
            audioSource.PlayOneShot(successSound);
        else
            audioSource.PlayOneShot(failSound);
    }

    public void StartFishing(ZonaPesca fishingZone)
    {
        currentFishingZone = fishingZone;
        selectedFish = fishPool.GetRandomFish();
        Debug.Log($"Has enganchado un {selectedFish.fishName}");

        greenZoneSpeed = greenZoneBaseSpeed * selectedFish.difficulty;

        isFishingActive = true;
        fishingCanvas.gameObject.SetActive(true);

        hook.gameObject.SetActive(true);
        greenZone.gameObject.SetActive(true);
        redBar.gameObject.SetActive(true);

        // Inicializa la posicion y los puntos del minijuego
        currentPoints = 0;
        lives = 3;
        UpdateUI();

        // Inicializa el movimiento aleatorio de la zona verde
        greenZoneDirection = Vector2.up; // Comienza moviendose hacia arriba
    }

    private void Update()
    {
        if (HUDManager.Instance != null && HUDManager.Instance.isKey3Used)
        {
            Upgraded = true;
        }


        if (Upgraded)
        hookImage.sprite = hookUpgradeImage;
        else
        hookImage.sprite = hookDefaultImage;
        if (!isFishingActive) return;

        //HandleHookMovement();
        MoveGreenZone();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckScore();
        }
    }

    private void UpdateUI()
    {
        if (currentLanguage == 0) // Español
        {
            puntosText.text = "Puntos: " + currentPoints + "/" + targetPoints;
            livesText.text = "Vidas: " + lives;
        } else {
            puntosText.text = "Points: " + currentPoints + "/" + targetPoints;
            livesText.text = "Lives: " + lives;
        }
        
    }

    /*private void HandleHookMovement()
    {
        float move = 0f;

        if (Input.GetKey(KeyCode.W))
            move = hookSpeed * Time.deltaTime;
        else if (Input.GetKey(KeyCode.S))
            move = -hookSpeed * Time.deltaTime;

        // Calcula la nueva posici�n en Y y limita el movimiento dentro de los l�mites
        float newYPosition = Mathf.Clamp(hook.anchoredPosition.y + move, hookLowerLimit, hookUpperLimit);

        // Actualiza la posici�n del anzuelo solo en el eje Y
        hook.anchoredPosition = new Vector2(hook.anchoredPosition.x, newYPosition);
    }*/

    private void MoveGreenZone()
    {
        greenZone.anchoredPosition += greenZoneDirection * greenZoneSpeed * Time.deltaTime;

        // Invierte la direcci�n de movimiento cuando llega a los bordes
        float minY = redBar.rect.yMin + greenZone.rect.height / 2;
        float maxY = redBar.rect.yMax - greenZone.rect.height / 2;

        if (greenZone.anchoredPosition.y >= maxY || greenZone.anchoredPosition.y <= minY)
        {
            greenZoneDirection *= -1;
        }
    }

    private void CheckScore()
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(greenZone, hook.position))
        {
            PlayFishingSound(true);
            if (Upgraded)
                currentPoints += 2;
            else 
                currentPoints++; // Solo suma puntos en zona verde
            if (currentLanguage == 0) // Español
            {
                puntosText.text = "Puntos: " + currentPoints + "/" + targetPoints;
            } else {
                puntosText.text = "Points: " + currentPoints + "/" + targetPoints;
            }

            // Comprueba la victoria
            if (currentPoints >= targetPoints)
            {
                EndGame(true);
            }
        }
        else
        {
            PlayFishingSound(false);
            lives--; // Pierde una vida si est� fuera de la zona verde
            if (currentLanguage == 0) // Español
            {
                livesText.text = "Vidas: " + lives;
            } else {
                livesText.text = "Lives: " + lives;
            }
            if (lives <= 0)
            {
                EndGame(false);
            }
        }
    }

    public void EndGame(bool success)
    {
        isFishingActive = false;

        if (success)
        {
            Debug.Log("¡Pesca exitosa!");
            if (selectedFish.isSpecial == true) {
                Debug.Log("Has capturado un pez especial");
                selectedFish.wasCaught = true;
                if (HUDManager.Instance != null)
                {
                    HUDManager.Instance.CollectBottle();
                }
            }
            if (selectedFish.isSpecial == false) {
                if (HUDManager.Instance != null)
                {
                    HUDManager.Instance.IncrementFish();
                }
            }
            // Determina si el pez es shiny
            isShinyCaught = IsShiny(selectedFish);
            if (isShinyCaught)
            {
                Debug.Log("SHINYYYY");
            }

            // Asigna el sprite adecuado (shiny o normal)
            caughtFishImage.sprite = isShinyCaught ? selectedFish.shinySprite : selectedFish.normalSprite;

            // Activa la imagen del pez y la oculta despu�s de unos segundos
            StartCoroutine(ShowCaughtFish());
        }
        else
        {
            Debug.Log("Pesca fallida. Has perdido todas tus vidas.");
            fishingCanvas.gameObject.SetActive(false);
            shipController.SetControlEnabled(true);
        }
    }

    // Corrutina para mostrar el sprite del pez capturado
    private IEnumerator ShowCaughtFish()
    {
        caughtFishImage.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f); // Muestra la imagen por 3 segundos
        caughtFishImage.gameObject.SetActive(false);
        fishingCanvas.gameObject.SetActive(false);
        shipController.SetControlEnabled(true);
        currentFishingZone.DeactivateZone();
        FindObjectOfType<FishingZoneManager>().SpawnNewFishingZone();
    }


    private bool IsShiny(Fish fish)
    {
        if (HUDManager.Instance != null && HUDManager.Instance.isKey2Used)
            fish.shinyProbability = fish.shinyProbability * 1.5f;
        float chance = Random.value;
        return chance <= fish.shinyProbability;
    }
}
