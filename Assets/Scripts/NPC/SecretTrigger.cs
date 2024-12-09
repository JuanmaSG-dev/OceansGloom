using UnityEngine;

public class SecretTrigger : MonoBehaviour
{
    public int keyIndex = 2; // Índice de la llave que quieres activar
    public GameObject secretObject; // El objeto que representa el secreto (se eliminará al activarlo)

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Verifica que sea el jugador quien activa el trigger
        {
            // Activar la llave en el HUDManager
            if (HUDManager.Instance != null)
            {
                HUDManager.Instance.CollectKey(keyIndex);
            }

            // Destruir o desactivar el objeto del secreto
            if (secretObject != null)
            {
                Destroy(secretObject);
            }
        }
    }
}
