using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;       // El barco que queremos seguir
    public float followSpeed = 2f; // Velocidad de seguimiento
    public Vector3 offset;         // Distancia de la cámara respecto al barco

    void LateUpdate()
    {
        // Calculamos la nueva posición deseada, sin rotación
        Vector3 desiredPosition = target.position + offset;
        // Movemos la cámara suavemente hacia la posición deseada
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
        
        // Para evitar la rotación, podemos bloquear la rotación en este script (por si acaso)
        transform.rotation = Quaternion.identity;
    }
}

