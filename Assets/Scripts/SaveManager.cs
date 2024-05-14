using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameData
{
    public string playerName;
    public int healthPoints;
    public int actionPoints;
    public int movementPoints;
    public int damagePoints;
    public int resistancePerc;
    public int critsPerc;
    public int numCompletedLevels;
}

public static class SaveManager
{
    public static string saveFilePath = Application.persistentDataPath + "/save.json";
    public static GameObject playerPrefab;

    public static string SaveNewGame()
    {
        GameObject playerPrefab = Resources.Load("Prefabs/Characters/Player") as GameObject;
        Player player = playerPrefab.GetComponent<Player>();

        GameData gameData = new GameData
        {
            playerName = player.name,
            healthPoints = player.InitHealthPoints,
            actionPoints = player.InitActionPoints,
            movementPoints = player.InitMovementPoints,
            damagePoints = player.InitDamagePoints,
            resistancePerc = player.InitResistancePerc,
            critsPerc = player.InitCritsPerc,
            numCompletedLevels = 0
        };

        string saveData = JsonUtility.ToJson(gameData);
        File.WriteAllText(saveFilePath, saveData);
        /*
                saveData = File.ReadAllText(saveFilePath);
                gameData = JsonUtility.FromJson<GameData>(saveData);*/
        return Application.persistentDataPath;
    }


    public static string SaveProgress(GameData gameData)
    {
        return "SaveProgress";
    }

    public static string Load()
    {
        return "Load";
    }

    // public static methods -----------------------------
    // loadCompletedScenes() -> int
    // loadPlayerStats()
}
