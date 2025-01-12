using UnityEngine;

public class KillZoneScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<ShipCollision>().DestroyBoat(); // Muerte instantánea
        }
    }
}
