using Unity.VisualScripting;
using UnityEngine;

public class SharkAI : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocidad del tiburón
    public Transform player;    // Referencia al transform del jugador
    public float rotationSpeed = 180f; // Velocidad de rotación del tiburón (grados por segundo)
    public float turnInterval = 2f; // Intervalo para cambiar de dirección (en segundos)

    private float timeToTurn; // Temporizador para cambiar de dirección
    public PolygonCollider2D visionCollider; // Rango de visión del tiburón

    private bool isChasing = false; // El tiburón está persiguiendo al jugador
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
        // Mueve al tiburón hacia adelante en la dirección actual
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

        // Reduce el temporizador de giro
        timeToTurn -= Time.deltaTime;

        // Cambia la dirección del tiburón aleatoriamente después del intervalo
        if (timeToTurn <= 0f)
        {
            // Realiza una rotación aleatoria, pero más suave
            float randomTurn = Random.Range(-90f, 90f); // Giros más controlados
            transform.Rotate(0f, 0f, randomTurn);

            // Restablece el temporizador
            timeToTurn = turnInterval;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Cuando el jugador entra en el rango de visión
            isChasing = true; // Activa la persecución
            Eye.SetEyeActive(true);
            Eye.SetEyeState(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Cuando el jugador sale del rango de visión
            isChasing = false; // Deja de perseguir
            Eye.SetEyeActive(false);
        }
    }

    private void ChasePlayer()
    {
        // Direccion hacia el jugador
        Vector2 direction = (player.position - transform.position).normalized;

        // Mover al tiburón
        transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;

        // Rotar el tiburón hacia la dirección en la que se está moviendo
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f; // Ajuste para sprites orientados hacia arriba
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
