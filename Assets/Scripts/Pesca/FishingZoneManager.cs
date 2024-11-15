using System.Collections.Generic;
using UnityEngine;

public class FishingZoneManager : MonoBehaviour
{
    public GameObject ZonaPescaPrefab; // Prefab de la zona de pesca
    public Transform[] spawnPoints; // Puntos donde aparecerán las zonas de pesca
    private List<GameObject> activeZones = new List<GameObject>(); // Lista de zonas activas

    void Start()
    {
        // Genera las primeras 25 zonas de pesca en posiciones aleatorias
        for (int i = 0; i < 25; i++)
        {
            SpawnNewFishingZone();
        }
    }

    // Método para generar una nueva zona de pesca
    public void SpawnNewFishingZone()
    {
        if (spawnPoints.Length > 0)
        {
            // Selecciona un punto aleatorio para generar la nueva zona
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject newZonaPesca = Instantiate(ZonaPescaPrefab, spawnPoint.position, Quaternion.identity);

            // Añadir a la lista de zonas activas
            activeZones.Add(newZonaPesca);

            // Desactivar la zona después de ser utilizada (en el script de la zona de pesca)
            ZonaPesca fishingZoneScript = newZonaPesca.GetComponent<ZonaPesca>();
            fishingZoneScript.onFishingCompleted += OnFishingCompleted; // Enlazar evento de pesca completada
        }
    }

    // Método que se ejecuta cuando se ha pescado en una zona (se puede desactivar o reutilizar la zona)
    private void OnFishingCompleted(GameObject fishingZone)
    {
        // Elimina la zona de la lista
        activeZones.Remove(fishingZone);

        // Desactiva la zona de pesca y destrúyela (se podría guardar para reutilizarla más tarde si es necesario)
        fishingZone.SetActive(false);

        // Genera una nueva zona de pesca en otro punto
        SpawnNewFishingZone();
    }
}
