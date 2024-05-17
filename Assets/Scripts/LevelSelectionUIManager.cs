using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectionUIManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject levelOneButton;
    [SerializeField] private GameObject levelTwoButton;
    [SerializeField] private GameObject levelThreeButton;
    private GameData gameData;


    private void Start()
    {
        levelOneButton.GetComponent<Button>().onClick.AddListener(() => OnLevelSelectionButtonListener(1));
        levelTwoButton.GetComponent<Button>().onClick.AddListener(() => OnLevelSelectionButtonListener(2));
        levelThreeButton.GetComponent<Button>().onClick.AddListener(() => OnLevelSelectionButtonListener(3));
        gameData = SaveManager.Load();
    }

    private void OnLevelSelectionButtonListener(int level)
    {
        switch (level)
        {
            case 1:
                SceneManager.LoadScene("BattleOneScene");
                break;
            case 2:
                if (gameData.numCompletedLevels >= 1) SceneManager.LoadScene("BattleOneScene"); //TODO BattleTwoScene
                else print("You need to finish level 1"); //TODO warning message level 2
                break;
            case 3:
                if (gameData.numCompletedLevels >= 2) SceneManager.LoadScene("BattleOneScene"); //TODO BattleThreeScene
                else print("You need to finish level 2"); //TODO warning message level 3
                break;
        }

    }
}
