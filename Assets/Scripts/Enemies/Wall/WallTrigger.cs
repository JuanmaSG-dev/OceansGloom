using UnityEngine;

public class WallTrigger : MonoBehaviour
{
    public bool activateWall; // Si es true, activa el muro. Si es false, lo detiene.
    public WallofVoid wall; // Puntero al muro

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Asegúrate de que el jugador tenga la tag "Player"
        {

            if (wall != null)
            {
                if (activateWall)
                {
                    wall.gameObject.SetActive(true); // Activa el muro
                    wall.StartWallMovement(); // Activa el muro
                }
                else
                {
                    wall.StopWallMovement(); // Detiene el muro
                }
            }
        }
    }
}
