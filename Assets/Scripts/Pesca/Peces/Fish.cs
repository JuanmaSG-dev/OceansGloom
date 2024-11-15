using UnityEngine;

[System.Serializable]
public class Fish
{
    public string fishName;             // Nombre del pez
    public float difficulty;              // Nivel de dificultad del pez (afecta el minijuego)
    public int rarity;                  // Rareza del pez (afecta probabilidad de selección)
    public float shinyProbability;      // Probabilidad de que sea shiny (porcentaje entre 0 y 1)

    public Sprite normalSprite;         // Sprite normal del pez
    public Sprite shinySprite;          // Sprite shiny del pez

    // Constructor para inicializar valores si se quiere desde código en vez de en el inspector
    public Fish(string name, int difficulty, int rarity, float shinyProbability, Sprite normalSprite, Sprite shinySprite)
    {
        this.fishName = name;
        this.difficulty = difficulty;
        this.rarity = rarity;
        this.shinyProbability = shinyProbability;
        this.normalSprite = normalSprite;
        this.shinySprite = shinySprite;
    }
}
