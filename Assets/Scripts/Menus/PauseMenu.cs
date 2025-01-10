using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Net;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance;
    public GameObject pausePanel;  
    private bool isPaused = false;
    public TMP_Text pausarText, continueText, optionsPauseText, salirMenuText;
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

    private void Start()
    {
        currentLanguage = PlayerPrefs.GetInt("Language", 0); // 0 = Español, 1 = Inglés
        if (currentLanguage == 0)
        {
            UpdatePauseText("Juego pausado");
            UpdateContinueText("Continuar");
            UpdateOptionText("Opciones");
            UpdateSalirText("Salir al menú");
        } // Español 

        else
        {
            UpdatePauseText("Game paused");
            UpdateContinueText("Continue");
            UpdateOptionText("Options");
            UpdateSalirText("Back to main menu");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void OpenOptions()
    {
        OptionsMenu.Instance.ToggleOptionsMenu();
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void UpdatePauseText(string pause)
    {
        pausarText.text = pause;
    }

    public void UpdateContinueText(string continuar)
    {
        continueText.text = continuar;
    }

    public void UpdateOptionText(string opcion)
    {
        optionsPauseText.text = opcion;
    }

    public void UpdateSalirText(string salir)
    {
        salirMenuText.text = salir;
    }
}
