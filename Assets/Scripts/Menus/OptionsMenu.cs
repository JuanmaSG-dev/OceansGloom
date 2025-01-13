using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour
{
    public static OptionsMenu Instance;
    public GameObject optionsPanel; // Panel del popup de opciones
    public Dropdown languageDropdown;
    public Slider musicVolumeSlider;
    public Slider soundVolumeSlider;
    private PauseMenu pauseMenu;


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
        pauseMenu = FindObjectOfType<PauseMenu>();

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
        string sceneName = SceneManager.GetActiveScene().name;

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
                    case "PauseText":
                        text.text = "Juego pausado";
                        if (pauseMenu != null)
                            pauseMenu.UpdatePauseText("Juego pausado");
                        break;
                    case "ContinueText":
                        text.text = "Continuar";
                        if (pauseMenu != null)
                            pauseMenu.UpdateContinueText("Continuar");
                        break;
                    case "OptionsPauseText":
                        text.text = "Opciones";
                        if (pauseMenu != null)
                            pauseMenu.UpdateOptionText("Opciones");
                        break;
                    case "SalirMenuText":
                        text.text = "Salir al menú";
                        if (pauseMenu != null)
                            pauseMenu.UpdateSalirText("Salir al menú");
                        break;
                    case "CreditsText":
                        text.text = "Creditos";
                        break;
                    case "VolverMenuText":
                        text.text = "Fin";
                        break;
                    case "LocationLv1":
                        Debug.Log(text.name);
                        switch (sceneName)
                        {
                            case "TheWest": // Nivel 1
                                text.text = "El Oeste";
                                Debug.Log("The West");
                                break;

                            case "BoneyWaters": // Nivel 2
                                text.text = "Huesocéano";
                                Debug.Log("Boney Waters");
                                break;
                            case "Void": // Nivel 2
                                text.text = "El Vacío";
                                Debug.Log("The Void");
                                break;
                        }
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
                    case "PauseText":
                        text.text = "Game Paused";
                        if (pauseMenu != null)
                            pauseMenu.UpdatePauseText("Game Paused");
                        break;
                    case "ContinueText":
                        text.text = "Continue";
                        if (pauseMenu != null)
                            pauseMenu.UpdateContinueText("Continue");
                        break;
                    case "OptionsPauseText":
                        text.text = "Options";
                        if (pauseMenu != null)
                            pauseMenu.UpdateOptionText("Options");
                        break;
                    case "SalirMenuText":
                        text.text = "Back to main menu";
                        if (pauseMenu != null)
                            pauseMenu.UpdateSalirText("Back to main menu");
                        break;
                    case "CreditsText":
                        text.text = "Credits";
                        break;
                    case "VolverMenuText":
                        text.text = "The end";
                        break;
                    case "LocationLv1":
                        switch (sceneName)
                        {
                            case "TheWest": // Nivel 1
                                text.text = "The West";
                                Debug.Log("The West2");
                                break;
                            case "BoneyWaters": // Nivel 2
                                text.text = "Boney Waters";
                                Debug.Log("Boney Waters2");
                                break;
                            case "Void": // Nivel 2
                                text.text = "The Void";
                                Debug.Log("The Void2");
                                break;
                        }
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
