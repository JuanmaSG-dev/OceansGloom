using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FishingMinigame : MonoBehaviour
{
    public Canvas fishingCanvas;
    public RectTransform hook;
    public TMP_Text puntos;

    public Image[] scoreZones; // Array de zonas de puntuación
    public float hookSpeed = 100f;

    public int targetPoints = 10;
    public int currentPoints = 0;
    public int failThreshold = -5;

    private bool isFishingActive = false;
    private float hookPosition;
    private float barWidth;
    private float leftBoundary;
    private float rightBoundary;

    private void Start()
    {
        fishingCanvas.gameObject.SetActive(false);
        barWidth = scoreZones[0].rectTransform.rect.width * scoreZones.Length; // Calcular ancho total

        leftBoundary = -barWidth / 2 + hook.rect.width / 2;
        rightBoundary = barWidth / 2 - hook.rect.width / 2;

        hookPosition = 0f;
        UpdateHookPosition();
    }

    public void StartFishing()
    {
        fishingCanvas.gameObject.SetActive(true);
        isFishingActive = true;

        currentPoints = 0;
        hookPosition = 0;
        UpdateHookPosition();
    }

    private void Update()
    {
        if (isFishingActive)
        {
            HandleHookMovement();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                CheckScore();
            }
        }
    }

    private void HandleHookMovement()
    {
        float move = 0f;

        if (Input.GetKey(KeyCode.A))
            move = -hookSpeed * Time.deltaTime;
        else if (Input.GetKey(KeyCode.D))
            move = hookSpeed * Time.deltaTime;

        hookPosition = Mathf.Clamp(hookPosition + move, leftBoundary, rightBoundary);
        UpdateHookPosition();
    }

    private void UpdateHookPosition()
    {
        hook.anchoredPosition = new Vector2(hookPosition, hook.anchoredPosition.y);
    }

    private void CheckScore()
    {
        puntos.text = "Puntos: " + currentPoints + "/" + targetPoints;

        // Comprobamos qué sprite está tocando el anzuelo
        for (int i = 0; i < scoreZones.Length; i++)
        {
            // Obtenemos los límites de cada zona de puntuación (sprite)
            RectTransform zoneRect = scoreZones[i].rectTransform;

            // Comprobamos si el hook está dentro de los límites de la zona
            if (hookPosition >= zoneRect.position.x && hookPosition <= zoneRect.position.x + zoneRect.rect.width)
            {
                // Asignamos los puntos según la zona
                switch (i)
                {
                    case 4:
                        currentPoints += 2; // Zona verde
                        break;
                    case 3:
                    case 5:
                        currentPoints += 1; // Zona verde claro
                        break;
                    case 2:
                    case 6:
                        currentPoints -= 1; // Zona amarilla
                        break;
                    case 1:
                    case 7:
                        currentPoints -= 2; // Zona naranja
                        break;
                    case 0:
                    case 8:
                    default:
                        currentPoints = failThreshold;
                        EndGame(false);
                        return;
                }

                break; // Ya hemos encontrado la zona, no hace falta seguir buscando
            }
        }

        // Comprobar si se ha alcanzado el puntaje objetivo
        if (currentPoints >= targetPoints)
        {
            EndGame(true);
        }
        else if (currentPoints <= failThreshold)
        {
            EndGame(false);
        }
    }


    private void EndGame(bool success)
    {
        fishingCanvas.gameObject.SetActive(false);
        isFishingActive = false;

        if (success)
        {
            Debug.Log("¡Pesca exitosa! Has atrapado el pez.");
        }
        else
        {
            Debug.Log("Fallaste en la pesca.");
        }
    }
}
