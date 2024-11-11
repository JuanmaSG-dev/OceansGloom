using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    void PlayButton()
    {
        SceneManager.LoadScene("Tutorial");
    }

    void OpcionesButton()
    {
        
    }

    void SalirButton()
    {
        Application.Quit();
    }

    void VolverButton()
    {
        SceneManager.LoadScene("Menu");
    }
}
