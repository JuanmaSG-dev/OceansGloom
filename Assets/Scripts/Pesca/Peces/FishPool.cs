using System.Collections.Generic;
using UnityEngine;

public class FishPool : MonoBehaviour
{
    public List<Fish> allFishes; // Pool completa de peces disponible en el juego

    // Selecciona un pez aleatorio de la pool tomando en cuenta la rareza
    public Fish GetRandomFish()
    {
        if (allFishes == null || allFishes.Count == 0)
        {
            Debug.LogWarning("La pool de peces está vacía. No se puede seleccionar un pez.");
            return null;
        }

        // Paso 1: Calcular el total de rareza de todos los peces
        int totalRarity = 0;
        foreach (Fish fish in allFishes)
        {
            totalRarity += fish.rarity;
        }

        // Paso 2: Generar un número aleatorio en el rango total de rareza
        int randomValue = Random.Range(0, totalRarity);

        // Paso 3: Encontrar el pez seleccionado basándose en la rareza
        int currentRaritySum = 0;
        foreach (Fish fish in allFishes)
        {
            currentRaritySum += fish.rarity;
            if (randomValue < currentRaritySum)
            {
                return fish;
            }
        }

        return allFishes[0]; // Esto no debería ocurrir, pero por seguridad devolvemos el primer pez
    }
}
