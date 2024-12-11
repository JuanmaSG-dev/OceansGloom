using UnityEngine;
using UnityEngine.UI;
using TMPro; // Importar TextMeshPro

public class HUDManager : MonoBehaviour
{
    // Singleton para hacer persistente el HUDManager
    public static HUDManager Instance;

    // Variables persistentes
    public int deathCount = 0;
    public int fishCount = 0;

    // Variables no persistentes
    public bool hasBottle = false;
    public bool hasKey1 = false;
    public bool hasKey2 = false;
    public bool hasKey3 = false;

    // Referencias a los elementos del HUD
    public TMP_Text deathCounterText; // TextMeshPro para mostrar muertes
    public TMP_Text fishCounterText;  // TextMeshPro para mostrar peces
    public Image bottleImage;
    public Image key1Image;
    public Image key2Image;
    public Image key3Image;

    // Colores para representar si están activados o no
    public Color collectedColor = Color.white;
    public Color notCollectedColor = Color.black;

    // Referencia al jugador para obtener su velocidad
    public Rigidbody2D playerRigidbody;

    // Velocidad límite para mostrar el HUD
    public float maxSpeedForHUD = 6f;

    // Referencia al Canvas del HUD
    public Canvas hudCanvas;

    private void Awake()
    {
        // Configurar el Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Hace que el objeto persista entre escenas
        }
        else
        {
            Destroy(gameObject); // Destruye instancias adicionales del HUDManager
        }
    }

    void Update()
    {
        // Mostrar u ocultar el HUD según la velocidad
        float playerSpeed = playerRigidbody.velocity.magnitude;
        hudCanvas.enabled = playerSpeed <= maxSpeedForHUD;

        // Actualizar los textos y sprites del HUD
        UpdateHUD();
    }

    public void UpdateHUD()
    {
        // Actualizar contadores
        deathCounterText.text = $"x {deathCount}";
        fishCounterText.text = $"x {fishCount}";

        // Actualizar estados de coleccionables
        bottleImage.color = hasBottle ? collectedColor : notCollectedColor;
        key1Image.color = hasKey1 ? collectedColor : notCollectedColor;
        key2Image.color = hasKey2 ? collectedColor : notCollectedColor;
        key3Image.color = hasKey3 ? collectedColor : notCollectedColor;
    }

    // Métodos públicos para modificar las variables persistentes
    public void IncrementDeaths()
    {
        deathCount++;
    }

    public void IncrementFish()
    {
        fishCount++;
    }

    // Métodos para modificar los estados no persistentes
    public void CollectBottle()
    {
        hasBottle = true;
    }

    public void CollectKey(int keyNumber)
    {
        switch (keyNumber)
        {
            case 0: hasKey1 = true; break;
            case 1: hasKey2 = true; break;
            case 2: hasKey3 = true; break;
        }
    }

    // Método para obtener el estado de una llave
    public bool GetKeyState(int keyIndex)
    {
        switch (keyIndex)
        {
            case 0: return hasKey1;
            case 1: return hasKey2;
            case 2: return hasKey3;
            default: return false;
        }
    }
}
