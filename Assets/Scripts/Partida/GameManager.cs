using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private string file;
    public GameData datosJuego = new GameData();

    private void Awake()
    {
        file = Application.persistentDataPath + "/GameData.json";
    }

    public void SaveGame(GameData datosJuego)
    {
        string json = JsonUtility.ToJson(datosJuego);
        File.WriteAllText(file, json);
        Debug.Log(json);
    }

    public GameData LoadData()
    {
        if (File.Exists(file))
        {
            string contents = File.ReadAllText(file);
            datosJuego = JsonUtility.FromJson<GameData>(contents);
            return datosJuego;
        }
        datosJuego.x = 0;
        datosJuego.y = 0;
        return datosJuego;
    }
}
