using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class FishingMinigame : MonoBehaviour
{
    public Canvas fishingCanvas;             // Canvas para activar o desactivar el minijuego de pesca
    public RectTransform hook;               // El anzuelo controlado por el jugador
    public RectTransform greenZone;          // Zona verde m�vil
    public RectTransform redBar;             // Barra roja de fondo
    public TMP_Text puntosText;              // Texto para mostrar la puntuaci�n
    public TMP_Text livesText;               // Texto para mostrar las vidas restantes

    //public float hookSpeed = 200f;           // Velocidad del anzuelo
    public int targetPoints = 10;            // Puntos necesarios para ganar
    public int lives = 3;                    // Vidas m�ximas

    private int currentPoints = 0;           // Puntos actuales
    public bool isFishingActive = false;    // Estado del minijuego
    private Vector2 greenZoneDirection;      // Direcci�n de movimiento del �rea verde
    [SerializeField] float greenZoneBaseSpeed = 200f;      // Velocidad de la zona verde
    [SerializeField] float greenZoneSpeed;

    public ShipController shipController;   // Control del barco (se desactivar�)

    //private float hookUpperLimit;
    //private float hookLowerLimit;

    public FishPool fishPool;
    private Fish selectedFish;

    public Image caughtFishImage;
    private bool isShinyCaught;
    ZonaPesca currentFishingZone;



    private void Start()
    { 
        fishingCanvas.gameObject.SetActive(true);
        caughtFishImage.gameObject.SetActive(false);
        //hookUpperLimit = redBar.rect.height;
        //hookLowerLimit = -redBar.rect.height;
        hook.anchoredPosition = new Vector2(hook.anchoredPosition.x, 278);
        shipController.SetControlEnabled(false);
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

        // Inicializa la posici�n y los puntos del minijuego
        currentPoints = 0;
        lives = 3;

        UpdateUI();

        // Inicializa el movimiento aleatorio de la zona verde
        greenZoneDirection = Vector2.up; // Comienza movi�ndose hacia arriba
    }

    private void Update()
    {
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
        puntosText.text = "Puntos: " + currentPoints + "/" + targetPoints;
        livesText.text = "Vidas: " + lives;
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
            currentPoints++; // Solo suma puntos en zona verde
            puntosText.text = "Puntos: " + currentPoints + "/" + targetPoints;
            

            // Comprueba la victoria
            if (currentPoints >= targetPoints)
            {
                EndGame(true);
            }
        }
        else
        {
            lives--; // Pierde una vida si est� fuera de la zona verde
            livesText.text = "Vidas: " + lives;
            if (lives <= 0)
            {
                EndGame(false);
            }
        }
    }

    private void EndGame(bool success)
    {
        isFishingActive = false;

        if (success)
        {
            Debug.Log("�Pesca exitosa!");
            if (selectedFish.isSpecial == true) {
                Debug.Log("Has capturado un pez especial");
                selectedFish.wasCaught = true;
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
        float chance = Random.value;
        return chance <= fish.shinyProbability;
    }
}
