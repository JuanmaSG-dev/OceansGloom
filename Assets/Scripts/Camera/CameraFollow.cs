using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;       // El barco que queremos seguir
    public float followSpeed = 2f; // Velocidad de seguimiento
    public Vector3 offset;         // Distancia de la c�mara respecto al barco

    void LateUpdate()
    {
        // Calculamos la nueva posici�n deseada, sin rotaci�n
        Vector3 desiredPosition = target.position + offset;
        // Movemos la c�mara suavemente hacia la posici�n deseada
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
        
        // Para evitar la rotaci�n, podemos bloquear la rotaci�n en este script (por si acaso)
        transform.rotation = Quaternion.identity;
    }
}

