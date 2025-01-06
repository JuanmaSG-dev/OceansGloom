using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public OptionsMenu optionsMenu;

    public void PlayButton()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void OpcionesButton()
    {
        optionsMenu.ToggleOptionsMenu();
    }

    public void SalirButton()
    {
        Application.Quit();
    }

    public void VolverButton()
    {
        SceneManager.LoadScene("Menu");
    }
}
