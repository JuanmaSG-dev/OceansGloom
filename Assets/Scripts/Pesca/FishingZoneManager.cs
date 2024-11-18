using System.Collections.Generic;
using UnityEngine;

public class FishingZoneManager : MonoBehaviour
{
    public GameObject fishingZonePrefab; // Prefab de la zona de pesca
    public int maxFishingZones = 25;     // N�mero m�ximo de zonas de pesca activas
    public float minDistanceBetweenZones = 15f; // Distancia m�nima entre zonas
    public LayerMask islandLayer;        // Capa para detectar colisiones (islas, obst�culos)
    public Vector2 mapBoundsMin;         // L�mites m�nimos del mapa (eje X, Y)
    public Vector2 mapBoundsMax;         // L�mites m�ximos del mapa (eje X, Y)
    public Transform ZonasPesca; // Empty que contendr� todas las zonas de pesca
    [SerializeField] private FishingMinigame fishingMinigame;

    private List<Vector2> activeFishingZones = new List<Vector2>(); // Coordenadas de las zonas activas

    void Start()
    {
        GenerateInitialFishingZones();
    }

    private void GenerateInitialFishingZones()
    {
        for (int i = 0; i < maxFishingZones; i++)
        {
            SpawnNewFishingZone();
        }
    }

    public void SpawnNewFishingZone()
    {
        for (int attempts = 0; attempts < 10; attempts++) // Intenta hasta 10 veces encontrar una posici�n v�lida
        {
            // Genera una posici�n aleatoria dentro de los l�mites del mapa
            Vector2 randomPosition = new Vector2(
                Random.Range(mapBoundsMin.x, mapBoundsMax.x),
                Random.Range(mapBoundsMin.y, mapBoundsMax.y)
            );

            // Comprueba si la posici�n es v�lida
            if (IsValidFishingZone(randomPosition))
            {
                // Genera la zona de pesca
                GameObject newZone = Instantiate(fishingZonePrefab, randomPosition, Quaternion.identity);

                if (ZonasPesca != null)
                {
                    newZone.transform.SetParent(ZonasPesca); // Asignar como hijo
                }
                // Configura las referencias necesarias
                ZonaPesca zonaPescaScript = newZone.GetComponent<ZonaPesca>();
                if (zonaPescaScript != null)
                {
                    zonaPescaScript.dialogueSystem = FindObjectOfType<DialogueSystem>();
                    zonaPescaScript.decisionSystem = FindObjectOfType<DecisionSystem>();
                    zonaPescaScript.fishingMinigame = fishingMinigame;
                }

                // A�ade la posici�n a la lista de zonas activas
                activeFishingZones.Add(randomPosition);
                return; // Sale del bucle si encuentra una posici�n v�lida
            }
        }
    }


    private bool IsValidFishingZone(Vector2 position)
    {
        // Comprueba la distancia m�nima con otras zonas
        foreach (Vector2 activeZone in activeFishingZones)
        {
            if (Vector2.Distance(activeZone, position) < minDistanceBetweenZones)
            {
                return false; // Est� demasiado cerca de otra zona
            }
        }

        // Comprueba que no est� en una isla
        Collider2D hitCollider = Physics2D.OverlapCircle(position, 1f, islandLayer);
        if (hitCollider != null)
        {
            return false; // La posici�n colisiona con una isla
        }

        return true; // La posici�n es v�lida
    }

    public void RemoveFishingZone(Vector2 position)
    {
        // Encuentra y elimina la zona de pesca en esa posici�n
        activeFishingZones.Remove(position);

        // Genera una nueva zona de pesca
        SpawnNewFishingZone();
    }
}
