using UnityEngine;

public class SharkAI : MonoBehaviour
{
    public float moveSpeed = 5f;          // Velocidad del tiburón
    public float rotationSpeed = 180f;    // Velocidad de rotación del tiburón (grados por segundo)
    public float turnInterval = 2f;       // Intervalo para cambiar de dirección (en segundos)

    private float timeToTurn;             // Temporizador para cambiar de dirección

    private void Start()
    {
        // Inicializa el temporizador para el giro
        timeToTurn = turnInterval;
    }

    private void Update()
    {
        // Mueve al tiburón hacia adelante en la dirección actual
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

        // Reduce el temporizador de giro
        timeToTurn -= Time.deltaTime;

        // Cambia la dirección del tiburón aleatoriamente después del intervalo
        if (timeToTurn <= 0f)
        {
            // Realiza una rotación aleatoria, permitiendo giros grandes, entre -180 y 180 grados
            float randomTurn = Random.Range(-500f, 500f);  // Puede girar tanto a la izquierda como a la derecha
            float rotationAmount = randomTurn * rotationSpeed * Time.deltaTime; // Rotación en grados

            // Aplica la rotación sobre el eje Z
            transform.Rotate(0f, 0f, rotationAmount);

            // Restablece el temporizador
            timeToTurn = turnInterval;
        }
    }
}
