using UnityEngine;

public class SharkAI : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocidad del tiburón
    public Transform[] waypoints; // Ruta de waypoints
    private int currentWaypointIndex = 0; // Índice del waypoint actual

    private void Update()
    {
        PatrolRoute();
    }

    private void PatrolRoute()
    {
        if (waypoints.Length == 0) return;

        // Mover hacia el waypoint actual
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        MoveTowards(targetWaypoint.position);

        // Verificar si hemos llegado al waypoint
        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length; // Siguiente waypoint
        }
    }

    private void MoveTowards(Vector3 targetPosition)
    {
        // Mover hacia el objetivo
        Vector3 direction = (targetPosition - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;

        // Rotar hacia la dirección del movimiento
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f; // Ajuste para sprites orientados hacia arriba
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
