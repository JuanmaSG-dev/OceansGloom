using UnityEngine;

public class ShipCollision : MonoBehaviour
{
    public float criticalImpactSpeed = 8f;         // Velocidad crítica del impacto para destruir el barco
    public Vector3 respawnPosition;                // Posición de reaparición
    public GameObject explosionEffectPrefab;       // Prefab de explosión, si quieres un efecto visual

    private Rigidbody2D rb;
    public ShipController shipController;

    private void Start()
    {
        gameObject.SetActive(true);
        rb = GetComponent<Rigidbody2D>();
        shipController = GetComponent<ShipController>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Obtener la velocidad relativa del impacto en el punto de colisión
        float impactSpeed = collision.relativeVelocity.magnitude;

        // Verificar si la velocidad relativa supera la velocidad crítica
        if (impactSpeed >= criticalImpactSpeed)
        {
            DestroyBoat();
        }
    }

    void DestroyBoat()
    {
        // Crear un efecto de explosión en la posición actual del barco
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }

        // Desactivar temporalmente el barco para "destruirlo"
        gameObject.SetActive(false);

        // Llamar a la función de reaparición después de un breve retraso
        Invoke("Respawn", 1f); // Ajusta el tiempo de reaparición según prefieras
    }

    void Respawn()
    {
        // Colocar el barco en la posición de reaparición y reiniciar su velocidad
        transform.position = respawnPosition;
        shipController.currentSpeed = 0f;

        // Reactivar el barco
        gameObject.SetActive(true);
    }
}
