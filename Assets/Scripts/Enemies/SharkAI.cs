using Unity.VisualScripting;
using UnityEngine;

public class SharkAI : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocidad del tibur�n
    public Transform player;    // Referencia al transform del jugador
    public float rotationSpeed = 180f; // Velocidad de rotaci�n del tibur�n (grados por segundo)
    public float turnInterval = 2f; // Intervalo para cambiar de direcci�n (en segundos)

    private float timeToTurn; // Temporizador para cambiar de direcci�n
    public PolygonCollider2D visionCollider; // Rango de visi�n del tibur�n

    private bool isChasing = false; // El tibur�n est� persiguiendo al jugador
    public EyeControl Eye;

    private void Start()
    {
        // Inicializa el temporizador para el giro
        timeToTurn = turnInterval;

        if (Eye == null)
        {
            Debug.LogError("EyeControl no encontrado");
        }
    }

    private void Update()
    {
        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            PatrolArea();
        }
    }

    private void PatrolArea()
    {
        // Mueve al tibur�n hacia adelante en la direcci�n actual
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

        // Reduce el temporizador de giro
        timeToTurn -= Time.deltaTime;

        // Cambia la direcci�n del tibur�n aleatoriamente despu�s del intervalo
        if (timeToTurn <= 0f)
        {
            // Realiza una rotaci�n aleatoria, pero m�s suave
            float randomTurn = Random.Range(-90f, 90f); // Giros m�s controlados
            transform.Rotate(0f, 0f, randomTurn);

            // Restablece el temporizador
            timeToTurn = turnInterval;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Cuando el jugador entra en el rango de visi�n
            isChasing = true; // Activa la persecuci�n
            Eye.SetEyeActive(true);
            Eye.SetEyeState(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Cuando el jugador sale del rango de visi�n
            isChasing = false; // Deja de perseguir
            Eye.SetEyeActive(false);
        }
    }

    private void ChasePlayer()
    {
        // Direccion hacia el jugador
        Vector2 direction = (player.position - transform.position).normalized;

        // Mover al tibur�n
        transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;

        // Rotar el tibur�n hacia la direcci�n en la que se est� moviendo
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f; // Ajuste para sprites orientados hacia arriba
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
