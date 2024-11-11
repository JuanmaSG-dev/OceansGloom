using UnityEngine;

public class ShipCollision : MonoBehaviour
{
    public float criticalImpactSpeed = 8f;         // Velocidad cr�tica del impacto para destruir el barco
    public Vector3 respawnPosition;                // Posici�n de reaparici�n
    public GameObject explosionEffectPrefab;       // Prefab de explosi�n, si quieres un efecto visual

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
        // Obtener la velocidad relativa del impacto en el punto de colisi�n
        float impactSpeed = collision.relativeVelocity.magnitude;

        // Verificar si la velocidad relativa supera la velocidad cr�tica
        if (impactSpeed >= criticalImpactSpeed)
        {
            DestroyBoat();
        }
    }

    void DestroyBoat()
    {
        // Crear un efecto de explosi�n en la posici�n actual del barco
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
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
