using UnityEngine;

public class SharkBite : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("¡Mordisco! El barco fue destruido.");
            other.GetComponent<ShipCollision>().DestroyBoat();
        }
    }

}
