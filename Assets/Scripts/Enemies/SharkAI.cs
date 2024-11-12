using UnityEngine;

public class SharkAI : MonoBehaviour
{
    public float moveSpeed = 5f;          // Velocidad del tibur�n
    public float rotationSpeed = 180f;    // Velocidad de rotaci�n del tibur�n (grados por segundo)
    public float turnInterval = 2f;       // Intervalo para cambiar de direcci�n (en segundos)

    private float timeToTurn;             // Temporizador para cambiar de direcci�n

    private void Start()
    {
        // Inicializa el temporizador para el giro
        timeToTurn = turnInterval;
    }

    private void Update()
    {
        // Mueve al tibur�n hacia adelante en la direcci�n actual
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

        // Reduce el temporizador de giro
        timeToTurn -= Time.deltaTime;

        // Cambia la direcci�n del tibur�n aleatoriamente despu�s del intervalo
        if (timeToTurn <= 0f)
        {
            // Realiza una rotaci�n aleatoria, permitiendo giros grandes, entre -180 y 180 grados
            float randomTurn = Random.Range(-500f, 500f);  // Puede girar tanto a la izquierda como a la derecha
            float rotationAmount = randomTurn * rotationSpeed * Time.deltaTime; // Rotaci�n en grados

            // Aplica la rotaci�n sobre el eje Z
            transform.Rotate(0f, 0f, rotationAmount);

            // Restablece el temporizador
            timeToTurn = turnInterval;
        }
    }
}
