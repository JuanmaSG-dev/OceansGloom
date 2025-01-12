using UnityEngine;

public class WallofVoid : MonoBehaviour
{
    public float speed = 5f; // Velocidad de ascenso
    public bool isMoving = false; // Controla si el muro se mueve o no
    [SerializeField] Vector3 startPosition;

    void Update()
    {
        if (isMoving)
        {
            // Mueve el muro verticalmente hacia arriba
            transform.Translate(Vector3.up * speed * Time.deltaTime);
        }
    }

    // Método público para detener el movimiento
    public void StopWallMovement()
    {
        isMoving = false;
    }

    // Método público para reiniciar el movimiento (opcional)
    public void StartWallMovement()
    {
        isMoving = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<ShipCollision>().DestroyBoat();
            ResetPosition();
        }
        
    }

    public void ResetPosition() {
        transform.position = startPosition;
        StopWallMovement();
        Activate(false);
    }

    public void Activate(bool active) {
        gameObject.SetActive(active);
    }
}
