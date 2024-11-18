using System.Collections.Generic;
using UnityEngine;

public class FishingZoneManager : MonoBehaviour
{
    public GameObject fishingZonePrefab; // Prefab de la zona de pesca
    public int maxFishingZones = 25;     // Número máximo de zonas de pesca activas
    public float minDistanceBetweenZones = 15f; // Distancia mínima entre zonas
    public LayerMask islandLayer;        // Capa para detectar colisiones (islas, obstáculos)
    public Vector2 mapBoundsMin;         // Límites mínimos del mapa (eje X, Y)
    public Vector2 mapBoundsMax;         // Límites máximos del mapa (eje X, Y)
    public Transform ZonasPesca; // Empty que contendrá todas las zonas de pesca
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
        for (int attempts = 0; attempts < 10; attempts++) // Intenta hasta 10 veces encontrar una posición válida
        {
            // Genera una posición aleatoria dentro de los límites del mapa
            Vector2 randomPosition = new Vector2(
                Random.Range(mapBoundsMin.x, mapBoundsMax.x),
                Random.Range(mapBoundsMin.y, mapBoundsMax.y)
            );

            // Comprueba si la posición es válida
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

                // Añade la posición a la lista de zonas activas
                activeFishingZones.Add(randomPosition);
                return; // Sale del bucle si encuentra una posición válida
            }
        }
    }


    private bool IsValidFishingZone(Vector2 position)
    {
        // Comprueba la distancia mínima con otras zonas
        foreach (Vector2 activeZone in activeFishingZones)
        {
            if (Vector2.Distance(activeZone, position) < minDistanceBetweenZones)
            {
                return false; // Está demasiado cerca de otra zona
            }
        }

        // Comprueba que no esté en una isla
        Collider2D hitCollider = Physics2D.OverlapCircle(position, 1f, islandLayer);
        if (hitCollider != null)
        {
            return false; // La posición colisiona con una isla
        }

        return true; // La posición es válida
    }

    public void RemoveFishingZone(Vector2 position)
    {
        // Encuentra y elimina la zona de pesca en esa posición
        activeFishingZones.Remove(position);

        // Genera una nueva zona de pesca
        SpawnNewFishingZone();
    }
}
