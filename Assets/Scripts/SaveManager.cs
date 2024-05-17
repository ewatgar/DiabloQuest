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

        GameData gameData = new()
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
        return Application.persistentDataPath;
    }

    public static bool SaveProgress(
        int healthPoints,
        int actionPoints,
        int movementPoints,
        int damagePoints,
        int resistancePerc,
        int critsPerc)
    {
        GameData oldGameData = Load();
        GameData newGameData = new()
        {
            playerName = oldGameData.playerName,
            healthPoints = healthPoints,
            actionPoints = actionPoints,
            movementPoints = movementPoints,
            damagePoints = damagePoints,
            resistancePerc = resistancePerc,
            critsPerc = critsPerc,
            numCompletedLevels = ++oldGameData.numCompletedLevels
        };

        if (File.Exists(saveFilePath))
        {
            string saveData = JsonUtility.ToJson(newGameData);
            File.WriteAllText(saveFilePath, saveData);
            return true;
        }
        return false;
    }

    public static GameData Load()
    {
        if (File.Exists(saveFilePath))
        {
            string loadData = File.ReadAllText(saveFilePath);
            return JsonUtility.FromJson<GameData>(loadData);
        }
        return null;
    }

    // public static methods -----------------------------
    // loadCompletedScenes() -> int
    // loadPlayerStats()
}
