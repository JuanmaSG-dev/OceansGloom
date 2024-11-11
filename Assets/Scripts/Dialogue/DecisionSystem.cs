using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class DecisionSystem : MonoBehaviour
{
    public GameObject decisionPanel;  // Panel de decisiones
    public Button[] optionButtons;    // Botones para las opciones
    private Action<int> onDecisionMade;  // Acción que se ejecutará cuando se tome una decisión

    public ShipController shipController;

    private void Start()
    {
        decisionPanel.SetActive(false);  // Se asegura de que el panel esté oculto al principio
    }

    // Inicia el sistema de decisiones
    public void StartDecision(string[] options, Action<int> callback)
    {
        onDecisionMade = callback;
        decisionPanel.SetActive(true);  // Activa el panel de decisiones

        // Configura las opciones en los botones
        for (int i = 0; i < optionButtons.Length; i++)
        {
            if (i < options.Length)
            {
                optionButtons[i].gameObject.SetActive(true);
                optionButtons[i].GetComponentInChildren<TMP_Text>().text = options[i];
                int optionIndex = i;  // Evita problemas de capturas de variables en lambda
                optionButtons[i].onClick.AddListener(() => ChooseOption(optionIndex));
            }
            else
            {
                optionButtons[i].gameObject.SetActive(false);  // Oculta botones no utilizados
            }
        }

        shipController.SetControlEnabled(false); // Desactiva el control del barco mientras se toman decisiones
    }

    // Selecciona la opción elegida
    private void ChooseOption(int optionIndex)
    {
        onDecisionMade?.Invoke(optionIndex);  // Llama al callback con la opción elegida
        decisionPanel.SetActive(false);  // Desactiva el panel de decisiones
        shipController.SetControlEnabled(true);  // Reactiva el control del barco
    }
}
