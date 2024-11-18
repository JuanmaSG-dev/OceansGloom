using UnityEngine;

public class EyeControl : MonoBehaviour
{
    public Sprite warningSprite; // Ojo verde
    public Sprite dangerSprite;  // Ojo rojo

    private SpriteRenderer spriteRenderer;
    public bool isActiveNow = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetEyeActive(false); // Desactiva el ojo al inicio
    }

    public void SetEyeState(bool isDanger)
    {
        spriteRenderer.sprite = isDanger ? dangerSprite : warningSprite; // Cambia el sprite
        SetEyeActive(true); // Asegúrate de que el ojo esté visible
    }

    public void SetEyeActive(bool isActive)
    {
        isActiveNow = isActive;
        gameObject.SetActive(isActive); // Activa o desactiva el objeto completo
    }
}
