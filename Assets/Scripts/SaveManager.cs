using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameData
{
    public bool isDead;
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

        GameData gameData = new GameData();
        gameData.isDead = player.IsDead;
        gameData.playerName = player.name;
        gameData.healthPoints = player.InitHealthPoints;
        gameData.actionPoints = player.InitActionPoints;
        gameData.movementPoints = player.InitMovementPoints;
        gameData.damagePoints = player.InitDamagePoints;
        gameData.resistancePerc = player.InitResistancePerc;
        gameData.critsPerc = player.InitCritsPerc;
        gameData.numCompletedLevels = 0;

        string saveData = JsonUtility.ToJson(gameData);
        File.WriteAllText(saveFilePath, saveData);
        /*
                saveData = File.ReadAllText(saveFilePath);
                gameData = JsonUtility.FromJson<GameData>(saveData);*/
        return Application.persistentDataPath;
    }


    public static string SaveProgress(Player player, int level)
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
