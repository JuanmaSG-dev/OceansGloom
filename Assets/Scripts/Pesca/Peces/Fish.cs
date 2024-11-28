using UnityEngine;
[System.Serializable]
public class Fish
{
    public string fishName;             // Nombre del pez
    public float difficulty;            // Nivel de dificultad del pez (afecta el minijuego)
    public int rarity;                  // Rareza del pez (afecta probabilidad de selección)
    public float shinyProbability;      // Probabilidad de que sea shiny (porcentaje entre 0 y 1)
    public bool isSpecial;              // Marca si el pez es especial
    public bool wasCaught;              // Marca si el pez ya fue pescado (solo para peces especiales)

    public Sprite normalSprite;         // Sprite normal del pez
    public Sprite shinySprite;          // Sprite shiny del pez

    public Fish(string name, int difficulty, int rarity, float shinyProbability, bool isSpecial, Sprite normalSprite, Sprite shinySprite)
    {
        this.fishName = name;
        this.difficulty = difficulty;
        this.rarity = rarity;
        this.shinyProbability = shinyProbability;
        this.isSpecial = isSpecial;
        this.wasCaught = false;         // Por defecto, no ha sido pescado
        this.normalSprite = normalSprite;
        this.shinySprite = shinySprite;
    }
}
