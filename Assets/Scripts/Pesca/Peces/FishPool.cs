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

        // Paso 1: Filtrar peces no disponibles (fueron pescados)
        List<Fish> availableFishes = allFishes.FindAll(fish => !fish.wasCaught);

        if (availableFishes.Count == 0)
        {
            Debug.LogWarning("No quedan peces disponibles para pescar.");
            return null;
        }

        // Paso 2: Calcular el total de rareza de los peces disponibles
        int totalRarity = 0;
        foreach (Fish fish in availableFishes)
        {
            totalRarity += fish.rarity;
        }

        // Paso 3: Generar un número aleatorio basado en la rareza total
        int randomValue = Random.Range(0, totalRarity);

        // Paso 4: Seleccionar un pez según la rareza
        int currentRaritySum = 0;
        foreach (Fish fish in availableFishes)
        {
            currentRaritySum += fish.rarity;
            if (randomValue < currentRaritySum)
            {
                return fish;
            }
        }

        // Esto no debería ocurrir, pero devolvemos el primero como seguridad
        return availableFishes[0];
    }
}
