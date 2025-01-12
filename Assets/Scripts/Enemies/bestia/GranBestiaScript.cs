using UnityEngine;
using System.Collections;

public class GranBestiaScript : MonoBehaviour
{
    public float speed = 3f;
    public float chaseDuration = 5f;
    public Collider2D killZone;
    public SpriteRenderer spriteRenderer;
    public Sprite normalSprite;
    public Sprite biteSprite; // Sprite para el ataque

    private Transform player;
    private Vector3 startPosition;
    private float chaseTimer;
    private bool isChasing = false;
    private bool isRedFlashing = false;

    private enum BeastState { Dormido, Persiguiendo, Mordisco }
    private BeastState currentState = BeastState.Dormido;

    void Start()
    {
        startPosition = transform.position;
        player = GameObject.FindWithTag("Player").transform;
        killZone.enabled = false;  // Desactivar la zona letal al inicio
        spriteRenderer.sprite = normalSprite; // Sprite normal al inicio
    }

    void Update()
    {
        switch (currentState)
        {
            case BeastState.Dormido:
                if (Vector3.Distance(player.position, transform.position) < 12f)
                {
                    currentState = BeastState.Persiguiendo;
                    isChasing = true;
                    chaseTimer = chaseDuration;
                }
                break;

            case BeastState.Persiguiendo:
                if (isChasing)
                {
                    transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
                    chaseTimer -= Time.deltaTime;

                    if (chaseTimer <= 0 && !isRedFlashing)
                    {
                        StartCoroutine(FlashRedBeforeBite());
                    }
                }
                break;

            case BeastState.Mordisco:
                // Nada especial, el daño ya ocurre con la kill zone
                break;
        }
    }

    // Efecto visual antes del mordisco (parpadeo rojo)
    private System.Collections.IEnumerator FlashRedBeforeBite()
    {
        isRedFlashing = true;
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.3f);
        spriteRenderer.color = Color.white;
        PerformBiteAttack();
    }

    // Activar el mordisco letal
    private void PerformBiteAttack()
    {
        currentState = BeastState.Mordisco;
        killZone.enabled = true;
        spriteRenderer.sprite = biteSprite; // Cambiar sprite al de mordisco
        Invoke("ResetToStart", 1.5f); // Volver a dormirse después de morder
    }

    void ResetToStart()
{
    killZone.enabled = false;
    spriteRenderer.sprite = normalSprite; // Volver al sprite normal
    spriteRenderer.color = Color.white; 
    currentState = BeastState.Dormido;
    isChasing = false;
    isRedFlashing = false;

    // Iniciar el movimiento de regreso al punto de inicio
    StartCoroutine(MoveToStartPosition());
}

// Corrutina para mover la bestia suavemente al punto de inicio
private IEnumerator MoveToStartPosition()
{
    float speed = 10f; // Velocidad de regreso
    while (Vector2.Distance(transform.position, startPosition) > 0.1f)
    {
        transform.position = Vector2.MoveTowards(transform.position, startPosition, speed * Time.deltaTime);
        yield return null;
    }

    // Asegurarse de que se coloque exactamente en la posición al terminar
    transform.position = startPosition;
}

}
