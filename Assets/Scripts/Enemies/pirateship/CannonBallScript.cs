using UnityEngine;

public class CannonBallScript : MonoBehaviour
{
    public float speed = 5f;

    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = transform.right * speed;
        Destroy(gameObject, 5f); // Evitar balas infinitas
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<ShipCollision>().DestroyBoat();
            
        }
        Destroy(gameObject);
    }
}
