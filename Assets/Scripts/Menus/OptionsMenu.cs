using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsMenu : MonoBehaviour
{
    public static OptionsMenu Instance;
    public GameObject optionsPanel; // Panel del popup de opciones
    public Dropdown languageDropdown;
    public Slider musicVolumeSlider;
    public Slider soundVolumeSlider;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Evitar duplicados
        }
    }
    private void Start()
    {
        // Cargar configuración guardada
        LoadSettings();

        // Asignar eventos
        languageDropdown.onValueChanged.AddListener(ChangeLanguage);
        musicVolumeSlider.onValueChanged.AddListener(ChangeMusicVolume);
        soundVolumeSlider.onValueChanged.AddListener(ChangeSoundVolume);

        // Iniciar oculto
        optionsPanel.SetActive(false);
    }

    public void ToggleOptionsMenu()
    {
        optionsPanel.SetActive(!optionsPanel.activeSelf);
    }

    private void ChangeLanguage(int index)
    {
        PlayerPrefs.SetInt("Language", index); // Guardar el idioma seleccionado
        PlayerPrefs.Save();
        ChangeTextLanguage(index); // Cambiar textos al idioma seleccionado
    }

    private void ChangeMusicVolume(float volume)
    {
        PlayerPrefs.SetFloat("MusicVolume", volume); // Guardar el volumen de música
        AudioManager.Instance.SetMusicVolume(volume); // Actualizar AudioManager
    }

    private void ChangeSoundVolume(float volume)
    {
        PlayerPrefs.SetFloat("SoundVolume", volume); // Guardar el volumen de sonidos
        AudioManager.Instance.SetSoundVolume(volume); // Actualizar AudioManager
    }

    private void LoadSettings()
    {
        if (PlayerPrefs.HasKey("Language")) 
        {
            int savedLanguage = PlayerPrefs.GetInt("Language"); 
            languageDropdown.value = savedLanguage; // Sincronizar Dropdown
            languageDropdown.RefreshShownValue();
            ChangeTextLanguage(savedLanguage);
        }

        // Mantener las opciones de volumen como estaban
        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1.0f); 
        soundVolumeSlider.value = PlayerPrefs.GetFloat("SoundVolume", 1.0f); 
    }


    public void ChangeTextLanguage(int language)
    {
        TMP_Text[] allTexts = FindObjectsOfType<TMP_Text>();

        foreach (TMP_Text text in allTexts)
        {
            // Cambia el texto según el idioma
            if (language == 0) // Español
            {
                switch (text.name)
                {
                    case "JugarText":
                        text.text = "Jugar";
                        break;
                    case "OpcionesMenuText":
                        text.text = "Opciones";
                        break;
                    case "SalirText":
                        text.text = "Salir";
                        break;
                    case "OpcionesText":
                        text.text = "Opciones";
                        break;
                    case "IdiomaText":
                        text.text = "Idioma";
                        break;
                    case "MusicaText":
                        text.text = "Volumen Música";
                        break;
                    case "SonidoText":
                        text.text = "Volumen Sonido";
                        break;
                    case "LocationLv1":
                        text.text = "El Oeste";
                        if (HUDManager.Instance != null)
                        HUDManager.Instance.UpdateLocationName("El Oeste");
                        break;
                }
            }
            else // Inglés
            {
                switch (text.name)
                {
                    case "JugarText":
                        text.text = "Play";
                        break;
                    case "OpcionesMenuText":
                        text.text = "Settings";
                        break;
                    case "SalirText":
                        text.text = "Exit";
                        break;
                    case "OpcionesText":
                        text.text = "Settings";
                        break;
                    case "IdiomaText":
                        text.text = "Language";
                        break;
                    case "MusicaText":
                        text.text = "Music Volume";
                        break;
                    case "SonidoText":
                        text.text = "Sound Volume";
                        break;
                    case "LocationLv1":
                        text.text = "The West";
                        if (HUDManager.Instance != null)
                        HUDManager.Instance.UpdateLocationName("The West");
                        break;
                }
            }
        }
    }

    public void ApplyLanguageOnSceneLoad()
    {
        int savedLanguage = PlayerPrefs.GetInt("Language", 0);
        ChangeTextLanguage(savedLanguage);
    }
}
