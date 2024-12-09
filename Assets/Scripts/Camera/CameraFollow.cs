using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;       // El barco que queremos seguir
    public float followSpeed = 2f; // Velocidad de seguimiento
    public Vector3 offset;         // Distancia de la cámara respecto al barco

    // Límites del mapa
    public float minX, maxX; // Límites horizontales
    public float minY, maxY; // Límites verticales

    void LateUpdate()
    {
        // Calculamos la nueva posición deseada
        Vector3 desiredPosition = target.position + offset;

        // Restringimos la posición de la cámara dentro de los límites
        float clampedX = Mathf.Clamp(desiredPosition.x, minX, maxX);
        float clampedY = Mathf.Clamp(desiredPosition.y, minY, maxY);

        // Aplicamos la posición ajustada
        Vector3 clampedPosition = new Vector3(clampedX, clampedY, desiredPosition.z);

        // Movemos la cámara suavemente hacia la posición ajustada
        transform.position = Vector3.Lerp(transform.position, clampedPosition, followSpeed * Time.deltaTime);
    }
}
