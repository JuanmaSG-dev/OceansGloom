using UnityEngine;
using UnityEngine.UI;
using TMPro; // Importar TextMeshPro

public class HUDManager : MonoBehaviour
{
    // Singleton para hacer persistente el HUDManager
    public static HUDManager Instance;

    // Variables persistentes
    public TMP_Text LocationNameText;
    public int deathCount = 0;
    public int fishCount = 0;

    // Variables no persistentes
    public bool hasBottle = false;

    // Llaves recogidas
    public bool hasKey1 = false;
    public bool hasKey2 = false;
    public bool hasKey3 = false;

    // Llaves usadas (asociadas a cofres)
    public bool isKey1Used = false;
    public bool isKey2Used = false;
    public bool isKey3Used = false;

    // Referencias a los elementos del HUD
    public TMP_Text deathCounterText; 
    public TMP_Text fishCounterText;  
    public Image bottleImage;
    public Image key1Image;
    public Image key2Image;
    public Image key3Image;

    // Colores para representar si están activados o no
    public Color collectedColor = Color.white;
    public Color notCollectedColor = Color.black;

    // Referencia al jugador
    public Rigidbody2D playerRigidbody;
    public float maxSpeedForHUD = 6f;

    public Canvas hudCanvas;
    public ChestInteraction[] allChests;
    int currentLanguage;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    void Start() {
        currentLanguage = PlayerPrefs.GetInt("Language", 0); // 0 = Español, 1 = Inglés
        allChests = FindObjectsOfType<ChestInteraction>();
        if (currentLanguage == 0) // Español
        UpdateLocationName("El Oeste");
        else 
        UpdateLocationName("The West");
    }

    void Update()
    {
        float playerSpeed = playerRigidbody.velocity.magnitude;
        hudCanvas.enabled = playerSpeed <= maxSpeedForHUD;
        UpdateHUD();
    }

    public void UpdateHUD()
    {
        deathCounterText.text = $"x {deathCount}";
        fishCounterText.text = $"x {fishCount}";

        // Actualizar estados de las llaves y botella
        bottleImage.color = hasBottle ? collectedColor : notCollectedColor;
        key1Image.color = hasKey1 && !isKey1Used ? collectedColor : notCollectedColor;
        key2Image.color = hasKey2 && !isKey2Used ? collectedColor : notCollectedColor;
        key3Image.color = hasKey3 && !isKey3Used ? collectedColor : notCollectedColor;
    }

    // Métodos para las llaves
    public void CollectKey(int keyNumber)
    {
        switch (keyNumber)
        {
            case 0: hasKey1 = true; break;
            case 1: hasKey2 = true; break;
            case 2: hasKey3 = true; break;
        }
    }

    public void UseKey(int keyNumber)
{
    // Verifica si el cofre correspondiente está abierto
    if (CheckIfChestIsOpen(keyNumber))
    {
        // Marca la llave como usada
        switch (keyNumber)
        {
            case 0: isKey1Used = true; break;
            case 1: isKey2Used = true; break;
            case 2: isKey3Used = true; break;
        }

        Debug.Log($"La llave {keyNumber + 1} ha sido usada en su cofre correspondiente.");
    }
}

    public bool CheckIfChestIsOpen(int keyNumber)
    {
        // Verificar si el cofre con la keyID correspondiente ya ha sido abierto
        foreach (ChestInteraction chest in allChests)
        {
            if (chest.keyID == keyNumber)
            {
                return chest.CheckIfOpen(keyNumber);
            }
        }
        return false;  // Si no se encuentra el cofre o no está abierto
    }

    // Método para verificar si una llave fue recogida
    public bool HasKey(int keyNumber)
    {
        switch (keyNumber)
        {
            case 0: return hasKey1;
            case 1: return hasKey2;
            case 2: return hasKey3;
            default: return false;
        }
    }

    public bool IsKeyUsed(int keyNumber)
    {
        switch (keyNumber)
        {
            case 0: return isKey1Used;
            case 1: return isKey2Used;
            case 2: return isKey3Used;
            default: return false;
        }
    }

    public void IncrementDeaths()
    {
        deathCount++;
    }

    public void IncrementFish()
    {
        fishCount++;
    }

    public void CollectBottle()
    {
        hasBottle = true;
    }

    public void UpdateLocationName(string locationName)
    {
        LocationNameText.text = locationName;
    }
}
