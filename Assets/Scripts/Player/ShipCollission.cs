using UnityEngine;

public class ShipCollision : MonoBehaviour
{
    public float criticalImpactSpeed = 8f;         // Velocidad cr�tica del impacto para destruir el barco
    public Vector3 respawnPosition;                // Posici�n de reaparici�n
    public GameObject explosionEffectPrefab;       // Prefab de explosi�n, si quieres un efecto visual

    private Rigidbody2D rb;
    public ShipController shipController;
    public FishingMinigame fishingMinigame;

    private void Start()
    {
        gameObject.SetActive(true);
        rb = GetComponent<Rigidbody2D>();
        shipController = GetComponent<ShipController>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Obtener la velocidad relativa del impacto en el punto de colision
        float impactSpeed = collision.relativeVelocity.magnitude;

        // Verificar si la velocidad relativa supera la velocidad critica
        if (impactSpeed >= criticalImpactSpeed)
        {
            DestroyBoat();
        }
    }

    public void DestroyBoat()
    {
        fishingMinigame.EndGame(false);
        // Crear un efecto de explosion en la posicion actual del barco
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }
        if (HUDManager.Instance != null)
        {
            HUDManager.Instance.IncrementDeaths();
        }
        // Desactivar temporalmente el barco para "destruirlo"
        gameObject.SetActive(false);

        // Llamar a la funci�n de reaparici�n despu�s de un breve retraso
        Invoke("Respawn", 1f); // Ajusta el tiempo de reaparici�n seg�n prefieras
    }

    void Respawn()
    {
        // Colocar el barco en la posici�n de reaparici�n y reiniciar su velocidad
        transform.position = respawnPosition;
        shipController.currentSpeed = 0f;

        // Reactivar el barco
        gameObject.SetActive(true);
    }
}
